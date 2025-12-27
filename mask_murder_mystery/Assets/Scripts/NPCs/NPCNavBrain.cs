using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(NpcDensity))]
public class NpcNavBrain : MonoBehaviour
{
    public enum Mode { StandStill, Wander, DensityDriven }

    public Mode mode = Mode.DensityDriven;

    public float wanderRadius = 10f;
    public float minWanderDelay = 1.5f;
    public float maxWanderDelay = 4.0f;

    public float minIdleTime = 1.0f;
    public float maxIdleTime = 3.0f;

    public float groupHangMin = 2.0f;
    public float groupHangMax = 6.0f;

    public float crowdSpeedMultiplier = 0.65f;

    public float crowdWanderRadiusMultiplier = 0.5f;

    public bool faceSomeoneInGroup = true;
    public float faceTurnSpeed = 6f;

    public float startSnapToNavMeshDistance = 5f;

    NavMeshAgent _agent;
    NpcDensity _sensor;

    float _baseSpeed;
    float _nextActionTime;
    Transform _faceTarget;

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _sensor = GetComponent<NpcDensity>();
        _baseSpeed = _agent.speed;

        // reduces clump dumping
        _agent.avoidancePriority = UnityEngine.Random.Range(30, 70);
        _agent.stoppingDistance = Mathf.Max(_agent.stoppingDistance, 0.2f);

    }

    void Start()
    {
        EnsureAgentOnNavMesh();

        if (!IsAgentReady())
            return;

        // small randomized start so they don’t all move on the same frame
        _nextActionTime = Time.time + UnityEngine.Random.Range(0.1f, 0.6f);
        SafeSetStopped(false);

        // prevents “idle forever” on first frame.
        if (mode != Mode.StandStill)
        {
            // tiny delay then pick a destination via normal loop
            ScheduleNextAction(UnityEngine.Random.Range(0.1f, 0.4f));
        }
    }

    void Update()
    {
        if (!IsAgentReady())
            return;

        if (mode == Mode.StandStill)
        {
            StopAgent(true);
            return;
        }

        if (mode == Mode.Wander)
        {
            DensityAgnosticWander();
            return;
        }

        DensityDriven();
        HandleFacing();
    }

    void EnsureAgentOnNavMesh()
    {
        if (_agent == null) return;

        if (_agent.isOnNavMesh)
            return;

        if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, startSnapToNavMeshDistance, NavMesh.AllAreas))
        {
            _agent.Warp(hit.position);
        }
    }

    bool IsAgentReady()
    {
        return _agent != null && _agent.enabled && _agent.isOnNavMesh;
    }

    void DensityAgnosticWander()
    {
        _agent.speed = _baseSpeed;

        if (Time.time < _nextActionTime) return;

        // if bro is lost, he pick a destination (not idle forever).
        if (!_agent.hasPath || _agent.pathStatus != NavMeshPathStatus.PathComplete)
        {
            PickWanderDestination(wanderRadius);
            ScheduleNextAction(UnityEngine.Random.Range(0.4f, 1.2f)); // short cooldown to avoid spam
            return;
        }

        bool arrived = !_agent.pathPending && (_agent.remainingDistance <= _agent.stoppingDistance + 0.05f);

        if (arrived)
        {
            // idle then gets bored and goes bye bye or gets drunk who knows ;)
            StopAgent(false);
            ScheduleNextAction(UnityEngine.Random.Range(minWanderDelay, maxWanderDelay));
        }
    }

    void DensityDriven()
    {
        var state = _sensor.CurrentState;

        switch (state)
        {
            case NpcDensity.DensityState.Alone:
                _agent.speed = _baseSpeed;
                _faceTarget = null;
                WanderLoop(wanderRadius, minWanderDelay, maxWanderDelay);
                break;

            case NpcDensity.DensityState.NearOne:
                // near one person: slight tendency to pause more
                _agent.speed = _baseSpeed * 0.9f;
                _faceTarget = FindNearestOtherNpc();
                WanderLoop(wanderRadius * 0.85f, minIdleTime, maxIdleTime);
                break;

            case NpcDensity.DensityState.Group:
                // group: hang around longer; move less frequently
                _agent.speed = _baseSpeed * 0.85f;
                _faceTarget = faceSomeoneInGroup ? FindNearestOtherNpc() : null;
                WanderLoop(wanderRadius * 0.7f, groupHangMin, groupHangMax);
                break;

            case NpcDensity.DensityState.Crowd:
                // crowd: slower, smaller stops; actual bots they wouldnt stand a chance against me B/
                _agent.speed = _baseSpeed * crowdSpeedMultiplier;
                _faceTarget = null;
                WanderLoop(wanderRadius * crowdWanderRadiusMultiplier, minIdleTime, maxIdleTime);
                break;
        }
    }

    void WanderLoop(float radius, float minDelay, float maxDelay)
    {
        if (Time.time < _nextActionTime) return;

        // if bro is dumb af, (or bad path), choose a destination
        if (!_agent.hasPath || _agent.pathStatus != NavMeshPathStatus.PathComplete)
        {
            PickWanderDestination(radius);
            ScheduleNextAction(UnityEngine.Random.Range(0.4f, 1.2f)); // short cooldown
            return;
        }

        bool arrived = !_agent.pathPending && (_agent.remainingDistance <= _agent.stoppingDistance + 0.05f);

        if (arrived)
        {
            StopAgent(false);
            ScheduleNextAction(UnityEngine.Random.Range(minDelay, maxDelay));
        }
    }

    void PickWanderDestination(float radius)
    {
        if (!IsAgentReady()) return;

        if (!TryGetRandomNavmeshPoint(transform.position, radius, out Vector3 target))
        {
            // fallback: try smaller radius
            if (!TryGetRandomNavmeshPoint(transform.position, Mathf.Max(2f, radius * 0.5f), out target))
            {
                StopAgent(false);
                return;
            }
        }

        SafeSetStopped(false);
        _agent.SetDestination(target);
    }

    bool TryGetRandomNavmeshPoint(Vector3 origin, float dist, out Vector3 result)
    {
        // random point in sphere then sample nearest navmesh
        for (int i = 0; i < 12; i++)
        {
            Vector3 random = origin + UnityEngine.Random.insideUnitSphere * dist;
            if (NavMesh.SamplePosition(random, out NavMeshHit hit, 2.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }

        result = origin;
        return false;
    }

    void StopAgent(bool hardStop)
    {
        if (!IsAgentReady()) return;

        if (hardStop)
        {
            _agent.isStopped = true;
            _agent.ResetPath();
        }
        else
        {
            _agent.isStopped = true;
            _agent.velocity = Vector3.zero;
            _agent.ResetPath(); // helps avoid weirdness
        }
    }

    void ScheduleNextAction(float delay)
    {
        _nextActionTime = Time.time + delay;

        // only "resume" if agent is actually on the NavMesh
        SafeSetStopped(false);
    }

    void SafeSetStopped(bool stopped)
    {
        if (IsAgentReady())
            _agent.isStopped = stopped;
    }

    Transform FindNearestOtherNpc()
    {
        // nearest search using the sensor’s radius
        Collider[] hits = Physics.OverlapSphere(transform.position, _sensor.radius, _sensor.npcLayer, QueryTriggerInteraction.Ignore);

        Transform best = null;
        float bestDist = float.PositiveInfinity;

        for (int i = 0; i < hits.Length; i++)
        {
            if (!hits[i]) continue;
            if (hits[i].transform == transform) continue;

            float d = (hits[i].transform.position - transform.position).sqrMagnitude;
            if (d < bestDist)
            {
                bestDist = d;
                best = hits[i].transform;
            }
        }

        return best;
    }

    void HandleFacing()
    {
        if (!faceSomeoneInGroup || _faceTarget == null) return;
        if (!IsAgentReady()) return;

        // only face when basically idle/not traveling
        // (hasPath false is a cleaner "idle" signal than remainingDistance when no path)
        if (_agent.hasPath) return;

        Vector3 to = _faceTarget.position - transform.position;
        to.y = 0f;
        if (to.sqrMagnitude < 0.05f) return;

        Quaternion targetRot = Quaternion.LookRotation(to.normalized, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * faceTurnSpeed);
    }
}
