using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using TMPro;
using System.Collections.Generic;

public class Journal : MonoBehaviour
{
    public Pages image;
    public TextMeshProUGUI guests;
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

    public void GuestList(List<string> names)
    {
        names = new List<string> { "John Pork", "Craig Bobbin", "Susan Pork", "Lebron", "Bob", "Joe Dirt" };
        String list = string.Join("\n", names);
        guests.text = list;
    }

    public void Inventory()
    {

    }

    public void GuestTab()
    {
        
    }
}
