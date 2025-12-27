using UnityEngine;
using System.Collections.Generic;

public class CharacterManager : MonoBehaviour
{
    public GameObject character_prefab;
    public List<Character> characters = new();
    public int num_characters;
    void Start()
    {
        for(int i = 0; i < num_characters; i++)
        {
            GenerateCharacter();
        }
    }
    public void GenerateCharacter()
    {
        //instantiate
        Character temp = Instantiate(character_prefab).GetComponent<Character>();
        
        characters.Add(temp);

        temp.per = new Temperament(true);
    }
}
