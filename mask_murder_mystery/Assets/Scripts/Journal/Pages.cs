using System;
using UnityEngine;
using UnityEngine.UI;

public class Pages : MonoBehaviour
{
    public Image[] pages;
    public int index = 0;
    public Journal journal;
    public CharacterManager characterManager;
    
    public void Next()
    {
        pages[index].gameObject.SetActive(false);
        index++;
        if (index == 1)
            {
                journal.Inventory();
            }
        if (index == 2)
            {
                journal.GuestList(characterManager.characters);
                journal.GuestTab();
            }
        pages[index].gameObject.SetActive(true);
    }

    public void prev()
    {
        pages[index].gameObject.SetActive(false);
        index--;
        if (index > 0)
        {
            if (index == 1)
            {
                journal.Inventory();
            }
            if (index == 2)
            {
                journal.GuestList(characterManager.characters);
                journal.GuestTab();
            }
            pages[index].gameObject.SetActive(true);
        }
        else pages[0].gameObject.SetActive(true);
    }

    public void LoadPage()
    {
        if (index > 0)
        {
            if (index == 1)
            {
                journal.Inventory();
            }
            if (index == 2)
            {
                journal.GuestList(characterManager.characters);
                journal.GuestTab();
            }
            pages[index].gameObject.SetActive(true);
        }
        else pages[0].gameObject.SetActive(true);
    }
      
   
}
