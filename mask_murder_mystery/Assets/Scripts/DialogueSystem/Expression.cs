using UnityEngine;
using System.Collections.Generic;
[System.Serializable]
public class Expression
{
    public List<Token> temperament_tokens = new();
    public List<Token> context_tokens = new();
    public string template;
    public string Formatted()
    {
        return null;
    }
    public Temperament Personality()
    {
        Temperament t = new Temperament();

        for(int i = 0; i < temperament_tokens.Count; i++)
        {
            t += temperament_tokens[i].temperament;
        }

        return t;
    }
}
