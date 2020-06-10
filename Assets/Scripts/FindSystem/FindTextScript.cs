using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class FindTextScript : MonoBehaviour
{
    public SpritzScript _SpritzScript;

    public RectTransform ContentTextBlock;

    public GameObject TextBlockPrefab;

    public string StripAga(string input)
    {
        return Regex.Replace(input, "<.*?>", string.Empty);
    }

    int InstCount;

    int RemPageFind = 0;

    public GameObject NextFinds;

    public void ClearAll()
    {
        ClearFinds ();

        NextFinds.SetActive(false);

        RemPageFind = 0;
        InstCount = 0;
    }

    string RemFind;

    public void NextFind ()
    {
        FindStart(RemFind);
    }

    public List<TextBlock> Blocks;

    public int CharsCount = 70;

    public void FindStart (string value)
    {
        if(value.Length == 0)
        {
            ClearAll();
            return;
        }


        if(value != RemFind)
        {
            ClearAll();
        }

        NextFinds.SetActive(true);

        RemFind = value;

        ClearFinds ();

        for (int i = RemPageFind; i < _SpritzScript.PagesText.Count; i++)
        {
            string zaz = StripAga(_SpritzScript.PagesText[i].PageText)/*.Replace("-", string.Empty)*/;

            Regex regex = new Regex(value);
            MatchCollection math = regex.Matches(zaz);

            int coef = 0;

            int x = -1;

            foreach (Match item in math)
            {
                coef++;
                x++;

                InstCount++;
                if(InstCount == 25)
                {
                    RemPageFind = i;
                    InstCount = 0;

                    NextFinds.transform.SetAsLastSibling();

                    ContentTextBlock.anchoredPosition = Vector2.zero;

                    return;
                }

                TextBlock textBlock = Instantiate(TextBlockPrefab, ContentTextBlock).GetComponent<TextBlock>();
                Blocks.Add(textBlock);

                textBlock.PartNumber = x;

                textBlock.FindTextScript = this;

                textBlock.PageNumber = i;
                textBlock.PageNumberText.text = (i+1).ToString();

                int index = item.Index;

                string z = zaz.Insert(index, "<i><b><color=#8dbafc>");
                z = z.Insert(index+value.Length+21, "</color></b></i>");

                int Start;

                int End;

                if(index + CharsCount + value.Length + 37 < z.Length)
                {
                    End = index + CharsCount + value.Length + 37;
                }
                else
                {
                    End = z.Length;
                }

                if(index - CharsCount >= 0)
                {
                    Start = index - CharsCount;
                }
                else
                {
                    Start = 0;
                }

                textBlock.Text.text = "..." + z.Substring(Start, End - Start) + "...";

                LayoutRebuilder.ForceRebuildLayoutImmediate(ContentTextBlock);
            }
        }

        NextFinds.SetActive(false);

        ContentTextBlock.anchoredPosition = Vector2.zero;

        RemPageFind = 0;
        InstCount = 0;
    }

    public void ClearFinds ()
    {
        for (int i = 0; i < Blocks.Count; i++)
        {
            Destroy(Blocks[i].gameObject);
        }
        Blocks.Clear();
    }

    public void Selected (int partNumber)
    {
        Regex regex = new Regex(RemFind);
        MatchCollection math = regex.Matches(_SpritzScript.ViweportText.text);

        _SpritzScript.ViweportText.text = _SpritzScript.ViweportText.text.Insert(math[partNumber].Index, "<b><color=#66a3ff>");
        _SpritzScript.ViweportText.text = _SpritzScript.ViweportText.text.Insert(math[partNumber].Index+math[partNumber].Length+18, "</color></b>");
    }
}
