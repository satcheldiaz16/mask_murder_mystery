using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Guests : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Image profile;

    public void SetData(Character guest)
    {
        text.text = guest.nm;
        profile.sprite = guest.character_icon;
    }
}
