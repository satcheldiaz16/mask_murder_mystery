using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Resources;

public class CharacterManager : MonoBehaviour
{
    public GameObject character_prefab;
    public List<Character> characters = new();
    public int num_characters;
    void Start()
    {
        Generate_Character();
    }
    public void Generate_Characters()
    {
        List<string> character_presets = Build_Character_List();

        for(int i = 0; i < num_characters; i++)
        {
            int preset_index = Random.Range(0, character_presets.Count);

            string preset = character_presets[preset_index];
            character_presets.RemoveAt(preset_index);

            Generate_Character(preset);
        }
    }
    public void Generate_Character(string nm = "Large Johnson, M")
    {
        //instantiate
        Character temp = Instantiate(character_prefab).GetComponent<Character>();
        
        characters.Add(temp);

        temp.Build_Character(nm);
    }
    public List<string> Build_Character_List(string file_path = "")
    {
        if(file_path.Equals("")) {file_path = "Tokens/character_presets.txt";}

        if(!File.Exists(file_path)) return null;

        List<string> temp = new();

        using StreamReader reader = new StreamReader(file_path);
        string line;
        while((line = reader.ReadLine()) != null) {temp.Add(line);}

        return temp;
    }
}
