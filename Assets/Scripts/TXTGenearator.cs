using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;
using TMPro;
using System.IO;
using System;
using MsgPack;
using MessagePack;

using System.Runtime.Serialization.Formatters.Binary;

public class TXTGenearator : MonoBehaviour
{
    public TextMeshProUGUI TextViewport;

    
    public List<string> TextParts = new List<string>();


    public List<Page> Pages = new List<Page>(1);

    public int PageID = 0;

    public List<PageTextC> PagesText = new List<PageTextC>();

    public int CountWordsInPage = 300;

    public static char[] FindChars = new char[] { ' ', (char)160, '\n', '\0'};

    public void StartParse (string path)
    {
        TextPartsGenerate(path);

        CreatePagesText ();

        MakeSpritzPages ();

        AlternativBook(path);

        ClearAll();
    }

    void ClearAll ()
    {
        TextViewport.text = string.Empty;

        TextParts = new List<string>();

        Pages = new List<Page>(1);

        PagesText = new List<PageTextC>();
    }

    void AlternativBook (string path)
    {
        string bookFolder = Library.pathToBooksFolder + '/' + Path.GetFileNameWithoutExtension(path);

        if(!Directory.Exists(bookFolder))
        {
            Directory.CreateDirectory(bookFolder);
            Directory.CreateDirectory(bookFolder + "/Images");
        }

        FileStream steama = new FileStream(bookFolder + "/pages.packNew", FileMode.Create, FileAccess.Write);
        MessagePackSerializer.Serialize(steama, Pages);
        steama.Close();

        steama = new FileStream(bookFolder + "/pagesText.packNew", FileMode.Create, FileAccess.Write);
        MessagePackSerializer.Serialize(steama, PagesText);
        steama.Close();

        steama = new FileStream(bookFolder + "/descript.packNew", FileMode.Create, FileAccess.Write);
        MessagePackSerializer.Serialize(steama, new BookDescription() {BookTitle = Path.GetFileNameWithoutExtension(path), PagesCount = PagesText.Count});
        steama.Close();
    }

    void TextPartsGenerate (string path)
    {
        string text = File.ReadAllText(path);

        text = text.Replace("\r", string.Empty);

        string[] a = text.Split(' ');

        foreach (var item in a)
        {
            TextParts.Add(item + ' ');
        }

        int lastWord = TextParts.Count - 1;
        int lastChar = TextParts[lastWord].Length - 1;

        TextParts[lastWord] = TextParts[lastWord].Remove(lastChar);
    }

    void CreatePagesText ()
    {
        int steps = 0;

        int GetCountPages = Mathf.CeilToInt((float)TextParts.Count / (float)CountWordsInPage);

        for (int i = 0; i < GetCountPages; i++)
        {
            PagesText.Add(new PageTextC());

            for (int z = 0; z < CountWordsInPage; z++)
            {
                if(z + steps < TextParts.Count)
                {
                    PagesText[PagesText.Count-1].PageText += TextParts[z + steps];
                }
                else
                {
                    break;
                }
            }

            steps += CountWordsInPage;

            int lengths = 1;

            for (int a = 0; a < lengths; a++)
            {
                char lastPageChar = PagesText[PagesText.Count - 1].PageText[PagesText[PagesText.Count - 1].PageText.Length-1];

                foreach (char chars in FindChars)
                {
                    if(lastPageChar == chars)
                    {
                        PagesText[PagesText.Count - 1].PageText = PagesText[PagesText.Count - 1].PageText.Remove(PagesText[PagesText.Count - 1].PageText.Length-1);
                        lengths++;
                    }
                }
            }

            lengths = 1;

            for (int a = 0; a < lengths; a++)
            {
                char firstPageChar = PagesText[PagesText.Count - 1].PageText[0];

                foreach (char chars in FindChars)
                {
                    if(firstPageChar == chars)
                    {
                        PagesText[PagesText.Count - 1].PageText = PagesText[PagesText.Count - 1].PageText.Remove(0, 1);
                        lengths++;
                    }
                }
            }
        }
    }

    void MakeSpritzPages ()
    {
        for (int pageID = 0; pageID < PagesText.Count; pageID++)
        {
            Pages.Add( new Page() );

            TextViewport.text = PagesText[pageID].PageText;
            TextViewport.ForceMeshUpdate();

            Pages[pageID].Steps.Add( new SpritzStep() );

            List<int> CharsID = new List<int>();


            for (int charID = 0; charID < TextViewport.textInfo.characterCount; charID++)
            {
                char splitChar = TextViewport.textInfo.characterInfo[charID].character;
                
                if(splitChar == ' ' || splitChar == '\n' || splitChar == (char)160)
                {
                    AddSpritzStep(pageID, ref CharsID, charID);

                    Pages[pageID].Steps.Add( new SpritzStep() );
                }
                else if(splitChar == '-')
                {
                    CharsID.Add(charID);

                    AddSpritzStep(pageID, ref CharsID, charID);

                    Pages[pageID].Steps.Add( new SpritzStep() );
                }
                else
                {
                    CharsID.Add(charID);

                    if(charID == TextViewport.textInfo.characterCount - 1)
                    {
                        AddSpritzStep(pageID, ref CharsID, charID);
                    }
                }

                if(splitChar == '\n')
                {
                    Pages[pageID].Steps.Add( new SpritzStep());
                }

                

                if(splitChar == '\u00A0')
                {
                    print("aaaa");
                }
            }
        }
    }

    void AddSpritzStep (int pageID, ref List<int> CharsID, int charID)
    {
        if(CharsID.Count > 0)
        {
            Pages[pageID].Steps[Pages[pageID].Steps.Count-1].First = (ushort)CharsID[0];
            Pages[pageID].Steps[Pages[pageID].Steps.Count-1].Last = (ushort)CharsID[CharsID.Count-1];
        }
        else
        {
            Pages[pageID].Steps[Pages[pageID].Steps.Count-1].First = (ushort)charID;
            if(charID > 0)
                Pages[pageID].Steps[Pages[pageID].Steps.Count-1].Last = (ushort)(charID-1);
            else if(charID == 0)
                Pages[pageID].Steps[Pages[pageID].Steps.Count-1].Last = (ushort)(0);
        }
        CharsID.Clear();
    }
}
