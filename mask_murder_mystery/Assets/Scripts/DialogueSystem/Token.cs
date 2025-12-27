using UnityEngine;
[System.Serializable]
public enum Token_Type{ greeting, chirp, surprise, adjective, farewell, context }
[System.Serializable]
public class Token
{
    public string content;
    public bool has_temperament;
    public Token_Type type;
    public Temperament temperament;
    public Token(string c, bool h, Token_Type ty, Temperament t)
    {
        content = c;
        has_temperament = h;
        type = ty;
        temperament = has_temperament ? t : new Temperament();
    }
}
