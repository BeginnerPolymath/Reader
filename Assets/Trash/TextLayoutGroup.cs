using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class TextLayoutGroup : MonoBehaviour
{
    public List<Text> Words;

    [Space(10)]

    public RectTransform TextPanel;

    public RectTransform TextViwer;

    public Vector2 Padding = new Vector2(20, 20);

    public TextMeshProUGUI TextMesh;


    void Start ()
    {

        for (int i = 0; i < TextViwer.childCount; i++)
        {
            Words.Add(TextViwer.GetChild(i).GetComponent<Text>());
        }

        
    }

    void Update()
    {
        SrotingBegin ();
    }

    Vector2 PanelSize;

    public Vector2 TextViwerSize;

    void SrotingBegin ()
    {
        PanelSize = TextPanel.rect.size;

        PanelSize.x -= Padding.x;
        PanelSize.y -= Padding.y;

        TextViwer.sizeDelta = PanelSize;

        TextUpdate ();
    }

    void TextUpdate ()
    {
        TextViwerSize = TextViwer.rect.size;

        Vector2 shift = new Vector2(0,0);

        foreach (Text word in Words)
        {
            if(word.text != "/n")
            {
                Vector2 sizeWord = new Vector2(word.preferredWidth / 2, word.preferredHeight / 2);

                word.rectTransform.anchoredPosition = new Vector3(sizeWord.x + shift.x, -sizeWord.y + shift.y);

                
                shift.x += word.preferredWidth + 11;
                
                if(shift.x - 11 >= TextViwerSize.x)
                {
                    shift.y -= word.preferredHeight + 11;
                    word.rectTransform.anchoredPosition = new Vector3(sizeWord.x, -sizeWord.y + shift.y);
                    shift.x = word.preferredWidth + 11;
                }
            }
            else if(word.text == "/n")
            {
                word.gameObject.SetActive(false);
                shift.y -= word.preferredHeight;
                shift.x = 0;
            }
        }
    }
}
