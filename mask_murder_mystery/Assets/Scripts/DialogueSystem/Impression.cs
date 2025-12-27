using System.Diagnostics.Contracts;
using UnityEngine;
using System.Collections.Generic;
[System.Serializable]
public class Impression
{
    public Character host;
    public string character_name;
    public int familiarity; //how many exchanges have occured
    public List<Temperament> interactions = new List<Temperament>();
    public Temperament assumption;
    public float Stance() 
    {
        return Mathf.Clamp(0, 1, Temperament.Distance(assumption, host.Personality_State()) / Temperament.maximum_distance);
    }
    public void Recieve_Interaction(Temperament t)
    {
        interactions.Add(t);

        familiarity++;

        Temperament sum = new Temperament();

        foreach (Temperament te in interactions)
        {
            sum += te;
        }

        assumption = sum / interactions.Count;
    }
}
