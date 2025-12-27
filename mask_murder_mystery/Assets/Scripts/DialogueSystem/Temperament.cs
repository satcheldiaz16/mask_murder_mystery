using UnityEngine;

[System.Serializable]
public class Temperament
{
    //each float ranges 1-100.
    public float sociability = 1; //introvert - extrovert
    public float integrity = 1; //superficiality - realness
    public float attitude = 1; //depressive - positive
    public const float maximum_distance = 173.21f;
    public Temperament(){}
    public Temperament(bool generate_random_stats)
    {
        if (generate_random_stats)
        {
            sociability = Random.Range(0f, 100f);
            integrity = Random.Range(0f, 100f);
            attitude = Random.Range(0f, 100f);
        }
    }
    public Temperament(float s, float i, float a)
    {
        sociability = s;
        integrity = i;
        attitude = a;
    }
    public static Temperament Emotions_To_Temperament(float[] emotions)
    {
        float sociability = emotions[(int)Emotion.joy] - emotions[(int)Emotion.fear];

        float integrity = emotions[(int)Emotion.anger] - emotions[(int)Emotion.disgust];

        float attitude = emotions[(int)Emotion.joy] - emotions[(int)Emotion.sadness];

        Temperament temp = new Temperament(sociability, integrity, attitude);

        return temp;
    }
    public float[] Temperament_To_Emotions()
    {
        float[] temp = new float[5];
        //joy
        temp[0] = Mathf.clamp(0f, 100f, (sociability + attitude) / 2);
        //sadness
        temp[1] = attitude < 0 ? attitude + 100 : attitude;
        //anger
        temp[2] = integrity < 0 ? Mathf.Abs(integrity) : 100 - integrity;
        //fear
        temp[3] = sociability < 0 ? 100 + sociability : sociability;
    }
    public static Temperament operator +(Temperament a, Temperament b)
    {
        return new Temperament(a.sociability + b.sociability, a.integrity + b.integrity, a.attitude + b.attitude);
    }
    public static Temperament operator *(Temperament a, float scalar)
    {
        return new Temperament(a.sociability * scalar, a.integrity * scalar, a.attitude * scalar);
    }
    public static Temperament operator *(float scalar, Temperament a)
    {
        return a * scalar;
    }
    public static Temperament operator *(Temperament a, Temperament b)
    {
        return new Temperament(a.sociability * b.sociability, a.integrity * b.integrity, a.attitude * b.attitude);
    }
    public static Temperament operator /(Temperament a, float scalar)
    {
        return a * (1/scalar);
    }
    public static float Distance(Temperament a, Temperament b)
    {
        float dx = a.sociability - b.sociability;
        float dy = a.integrity - b.integrity;
        float dz = a.attitude - b.attitude;
        
        return Mathf.Sqrt(dx * dx + dy * dy + dz * dz);
    }
}
