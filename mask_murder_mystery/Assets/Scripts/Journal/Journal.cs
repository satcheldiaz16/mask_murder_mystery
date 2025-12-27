using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using TMPro;
using System.Collections.Generic;

public class Journal : MonoBehaviour
{
    public Pages image;
    public GameObject GuestsPrefab;
    public Transform content;
    public void Update()
    {
        if (Keyboard.current.tabKey.wasPressedThisFrame)
        {
            image.pages[image.index].gameObject.SetActive(true);
        }
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            image.pages[image.index].gameObject.SetActive(false);
        }

    }

    public void GuestList(List<Character> characters)
    {
        foreach (var character in characters)
        {
            GameObject entry = Instantiate(GuestsPrefab, content);
            Guests components = entry.GetComponent<Guests>();
            components.SetData(character);
        }
    }

    public void Inventory()
    {

    }

    public void GuestTab()
    {
        
    }
}
