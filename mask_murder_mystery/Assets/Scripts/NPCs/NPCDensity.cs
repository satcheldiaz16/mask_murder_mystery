using System;
using UnityEngine;

public class NpcDensity : MonoBehaviour
{
    public enum DensityState { Alone, NearOne, Group, Crowd }
    public LayerMask npcLayer;

    public float radius = 3.0f;

    public float refreshInterval = 0.35f;

    public int nearOneMax = 1;     // 0 = alone, 1 = near one
    public int groupMax = 4;       // 2-4 = group
    public int crowdMin = 5;       // 5+ = crowd

    public DensityState CurrentState { get; private set; } = DensityState.Alone;
    public int NearbyCount { get; private set; }

    Collider[] _hits;

    void Awake()
    {
        _hits = new Collider[32];
        InvokeRepeating(nameof(Refresh), UnityEngine.Random.Range(0f, refreshInterval), refreshInterval);
    }

    void Refresh()
    {
        int count = Physics.OverlapSphereNonAlloc(transform.position, radius, _hits, npcLayer, QueryTriggerInteraction.Ignore);

        int selfRemoved = 0;
        for (int i = 0; i < count; i++)
        {
            if (_hits[i] != null && _hits[i].transform == transform)
            {
                selfRemoved = 1;
                break;
            }
        }

        NearbyCount = Mathf.Max(0, count - selfRemoved);

        if (NearbyCount <= 0) CurrentState = DensityState.Alone;
        else if (NearbyCount <= nearOneMax) CurrentState = DensityState.NearOne;
        else if (NearbyCount <= groupMax) CurrentState = DensityState.Group;
        else if (NearbyCount >= crowdMin) CurrentState = DensityState.Crowd;
        else CurrentState = DensityState.Group; // fallback
    }

}
