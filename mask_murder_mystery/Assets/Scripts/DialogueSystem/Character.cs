using UnityEngine;
using System.Collections.Generic;
public class Character : MonoBehaviour
{
    public string nm;
    public Temperament per;
    public float[] emotions = new float[5];
    public float[] emotional_threshold = new float[5];
    public bool surprised = false;
    public List<Impression> impressions = new List<Impression>();
    public Temperament Emotional_Offset(float emotional_weight = .2f)
    {
        return Temperament.Emotions_To_Temperament(emotions) * emotional_weight;
    }
    public Temperament Personality_State()
    {
        return per + Emotional_Offset();
    }
    public Emotion Emotional_State()
    {
        int strongest_emotion = -1;

        for(int i = 0; i < emotions.Length; i++)
        {
            if(emotions[i] >= emotional_threshold[i])
            {
                if (strongest_emotion == -1)
                {
                    strongest_emotion = i;
                }
                else if(emotions[i] > emotions[strongest_emotion])
                {
                    strongest_emotion = i;
                }
            }
        }

        return strongest_emotion == -1 ? Emotion.neutral : (Emotion)strongest_emotion;
    }
}
