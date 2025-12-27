using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public string[] Get_Tokens_From_File(string file_path)
    {
        TextAsset text_file = Resources.Load<TextAsset>(file_path);

        if(text_file == null) { return null; }

        string content = text_file.text;

        return content.Split('\n');
    }
}
