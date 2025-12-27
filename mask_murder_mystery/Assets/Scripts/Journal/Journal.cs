using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using TMPro;
using System.Collections.Generic;
using Unity.VisualScripting;

public class Journal : MonoBehaviour
{
    public Pages image;
    public GameObject GuestsPrefab;
    public Transform content;
    public List<Guests> characterList = new List<Guests>();
    public void Update()
    {
        if (Keyboard.current.tabKey.wasPressedThisFrame)
        {
            image.LoadPage();
        }
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            image.pages[image.index].gameObject.SetActive(false);
        }

    }

    public void GuestList(List<Character> characters)
    {
        foreach (var guest in characterList)
        {
            Destroy(guest.gameObject);
        }
        characterList.Clear();
        foreach (var character in characters)
        {
            GameObject entry = Instantiate(GuestsPrefab, content);
            Guests components = entry.GetComponent<Guests>();
            components.SetData(character);
            characterList.Add(components);
        }
    }

    public void Inventory()
    {

    }

    public void GuestTab()
    {
        
    }
}
