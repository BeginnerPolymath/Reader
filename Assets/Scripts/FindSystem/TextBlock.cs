using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextBlock : MonoBehaviour
{
    public int PageNumber;
    public int PartNumber;

    public TextMeshProUGUI Text;
    public TextMeshProUGUI PageNumberText;

    public FindTextScript FindTextScript;

    public void SetPage ()
    {
        FindTextScript.gameObject.SetActive(false);
        FindTextScript.ClearAll();
        FindTextScript._SpritzScript.SetPage(PageNumber);
        FindTextScript.Selected(PartNumber);
        FindTextScript._SpritzScript.SetSpritzWord(0, SpritzScript.WordInTextColor);
    }
}
