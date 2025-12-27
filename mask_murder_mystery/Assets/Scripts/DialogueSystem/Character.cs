using UnityEngine;
using System.Collections.Generic;
using System.Security.Principal;
public class Character : MonoBehaviour
{
    public string nm;
    public char gender;
    public Temperament per;
    public bool surprised = false;
    public List<Impression> impressions = new List<Impression>();
    #region Emotional State
    public Emotion Determine_Emotional_State(string condition = "default")
    {
        switch (condition)
        {
            case "default": return Default_Emotion();
            case "conversation": return Conversation_Emotion();
            case "death": return Death_Emotion();
            case "lights": return Lights_Out_Emotion();
        }

        return Emotion.joy;
    }
    public Emotion Default_Emotion()
    {
        Emotion emotion = Emotion.joy;

        if(per.sociability > 50 && per.attitude < 50) {emotion = Emotion.sadness; }

        if(per.sociability < 50 && per.attitude < 50) {emotion = Emotion.fear; }

        return emotion;
    }
    public Emotion Conversation_Emotion()
    {
        Emotion emotion = Emotion.joy;
        return emotion;
    }
    public Emotion Death_Emotion()
    {
        Emotion emotion = Emotion.fear;

        if(per.integrity > 50 && per.attitude > 50) {return Emotion.anger; }

        if(per.integrity > 50 && per.attitude < 50) {return Emotion.anger; }

        if(per.integrity < 50 && per.attitude > 50) {return Emotion.anger; }

        return emotion;
    }
    public Emotion Lights_Out_Emotion()
    {
        Emotion emotion = Emotion.fear;

        if(per.attitude > 50) {emotion = Emotion.surprise;}

        return emotion;
    }
    #endregion
}
