using System.Diagnostics.Contracts;
using UnityEngine;
[System.Serializable]
public class Impression
{
    public Character ch;
    public int familiarity; //how many exchanges have occured
    public Temperament assumption;
    public int Stance() {return 50;}
}
