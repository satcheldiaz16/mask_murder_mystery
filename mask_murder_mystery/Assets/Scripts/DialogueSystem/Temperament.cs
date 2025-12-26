using UnityEngine;

[System.Serializable]
public class Temperament
{
    //each float ranges 1-100.
    public float sociability = 1; //introvert - extrovert
    public float integrity = 1; //superficiality - realness
    public float attitude = 1; //depressive - positive
    public Temperament(){}
    public Temperament(float s, float i, float a)
    {
        sociability = s;
        integrity = i;
        attitude = a;
    }
    public Temperament Emotions_To_Temperament(float[] emotions)
    {
        Temperament temp = new Temperament();

        //float sociability = emotions[joy] - emotions[fear];

        //float attitude = emotions[joy] - emotions[]

        return temp;
    }
}
