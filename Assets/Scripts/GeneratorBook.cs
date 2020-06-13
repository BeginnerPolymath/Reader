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


[System.Serializable]
[MessagePackObject]
public class SpritzStep
{
    [Key(0)]
    public ushort First = 1;

    [Key(1)]
    public ushort Last = 0;

    [Key(2)]
    public byte ColorID = 0;
}

[System.Serializable]
[MessagePackObject]
public class Page
{
    [Key(0)]
    public List<SpritzStep> Steps = new List<SpritzStep>();
}

[System.Serializable]
[MessagePackObject]
public class PageTextC
{
    [Key(0)]
    public string PageText = string.Empty;

    [Key(1)]
    public List<string> NameImages = new List<string>();
}

public class GeneratorBook : MonoBehaviour
{
    public delegate void Method(XmlNode xmlNode);

    public Dictionary<string, Method> Methods = new Dictionary<string, Method>();

    public List<string> TextParts;

    public TextMeshProUGUI ViweportText;


    public List<Page> Pages = new List<Page>(1);

    public int PageID = 0;
    
    public List<PageTextC> PagesText = new List<PageTextC>();

    public int CountWordsInPage = 200;

    

    int OnWord;

    void Awake()
    {
        MethodsInit ();

        //print(Application.persistentDataPath);

        Application.targetFrameRate = 60;
    }

    void LoadColorSpritzStep ()
    {
        for (int stepIDa = 0; stepIDa < Pages[PageID].Steps.Count; stepIDa++)
        {
            //wHTMP.ChangeColorChars(PageID, stepIDa, Pages[PageID].Steps[stepIDa].Color);
        }
    }



    void MakeBook ()
    {
        int pageID = -1;

        int GetCountPages = Mathf.CeilToInt((float)TextParts.Count / (float)CountWordsInPage);

        for (int z = 0; z < GetCountPages; z++)
        {
            PagesText.Add(new PageTextC());
            pageID++;

            for (int i = 0; i < CountWordsInPage; i++)
            {
                if(i + OnWord < TextParts.Count)
                {
                    if(TextParts[i + OnWord].IndexOf("image=") == 0)
                    {
                        PagesText[pageID].NameImages.Add(TextParts[i + OnWord].Remove(0, 6));

                        PagesText[pageID].PageText += "<b>(Рис." + PagesText[pageID].NameImages.Count + ")</b> ";
                    }
                    else
                    {
                        PagesText[pageID].PageText += TextParts[i + OnWord];
                    }
                }
                else
                {
                    break;
                }
            }

            char lastPageChar = PagesText[PagesText.Count - 1].PageText[PagesText[PagesText.Count - 1].PageText.Length-1];

            if(lastPageChar == ' ' || lastPageChar == (char)160 ||  lastPageChar == '\n')
            {
                PagesText[PagesText.Count - 1].PageText = PagesText[PagesText.Count - 1].PageText.Remove(PagesText[PagesText.Count - 1].PageText.Length-1);
            }

            int lengths = 1;

            for (int i = 0; i < lengths; i++)
            {
                char firstPageChar = PagesText[PagesText.Count - 1].PageText[0];

                if(firstPageChar == ' ' || firstPageChar == (char)160 ||  firstPageChar == '\n')
                {
                    PagesText[PagesText.Count - 1].PageText = PagesText[PagesText.Count - 1].PageText.Remove(0, 1);
                    lengths++;
                }
            }

            PagesText[pageID].PageText = PagesText[pageID].PageText.Replace("\r", string.Empty);

            

            OnWord += CountWordsInPage;
        }
        
        TextParts.Clear();

        MakeSpritzPages ();
    }

    void MakeSpritzPages ()
    {
        for (int pageID = 0; pageID < PagesText.Count; pageID++)
        {
            Pages.Add( new Page() );

            ViweportText.SetText(PagesText[pageID].PageText);
            ViweportText.ForceMeshUpdate();

            Pages[pageID].Steps.Add( new SpritzStep() );

            List<int> CharsID = new List<int>();



            for (int charID = 0; charID < ViweportText.textInfo.characterCount; charID++)
            {
                char splitChar = ViweportText.textInfo.characterInfo[charID].character;
                

                if(char.IsWhiteSpace(splitChar))
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

                    if(charID == ViweportText.textInfo.characterCount - 1)
                    {
                        AddSpritzStep(pageID, ref CharsID, charID);
                    }
                }

                if(splitChar == '\n')
                {
                    Pages[pageID].Steps.Add( new SpritzStep() {First = 1, Last = 0} );
                    Pages[pageID].Steps[Pages[pageID].Steps.Count-1].First = 1;
                    Pages[pageID].Steps[Pages[pageID].Steps.Count-1].Last = 0;
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

    
    void AlternativBook (string bookFolder)
    {
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
    }



    //Парсинг и методы парсинга//

    void MethodsInit ()
    {
        Methods.Add("body", Body);

        Methods.Add("title", Title);
        Methods.Add("p", P);
        Methods.Add("#text", SpalshText);
        Methods.Add("strong", StrongNew);
        Methods.Add("emphasis", EmphasisNew);
        Methods.Add("epigraph", Epigraph);
        Methods.Add("text-author", TextAuthor);

        Methods.Add("section", Section);

        Methods.Add("empty-line", EmptyLine);

        Methods.Add("cite", Cite);

        Methods.Add("image", Image);

        Methods.Add("em", Em); 

        Methods.Add("a", A);

        Methods.Add("sub", Sub);

        Methods.Add("sup", Sup);

        Methods.Add("poem", Poem);

        Methods.Add("stanza", Stanza);

        Methods.Add("v", V);

        Methods.Add("date", Date);

        Methods.Add("strikethrough", Strikethrough);

        Methods.Add("code", Code);

        Methods.Add("annotation", Annotation);

        Methods.Add("subtitle", Subtitle);
        
        
    }

    public DescriptionGenerator _DescriptionGenerator;

    XmlDocument bookFile;

    string bookFolder;

    public string ParsingStart (string path)
    {
        bookFile = new XmlDocument();

        //xmlDoc.Load("Assets/Books/FB2/Уйти красиво. Удивительные похоронные обряды разных стран - (Кейтлин Даути) - 2018.fb2");
        bookFile.Load(path);
        //xmlDoc.Load("/storage/emulated/0/NEWFolder/Fresko.fb2");
        //xmlDoc.Load("jar:file://" + Application.dataPath + "!/assets" + "/Fresko_Vse-luchshee-chto-ne-kupish-za-dengi.KH_9ew.375942.fb2");
        
        //xmlDoc.Load("Assets/Books/FB2/Федин С.Н. - Математики тоже шутят - 2009.fb2");
        //xmlDoc.Load("Assets/Books/FB2/Лопатин Владимир. Русский орфографический словарь - royallib.ru.fb2");

        bookFolder = Library.pathToBooksFolder + '/' + Path.GetFileNameWithoutExtension(path);

        XmlNodeList BookNodes = bookFile.GetElementsByTagName("FictionBook")[0].ChildNodes;

        foreach (XmlNode childNode in BookNodes)
        {
            if(childNode.Name == "body")
            {
                Methods[childNode.Name].Invoke(childNode);
            }
            
            //Methods[childNode.Name].Invoke(childNode);
        }

        MakeBook ();

        AlternativBook(bookFolder);
        _DescriptionGenerator.CreateDescriptionFile(bookFile, bookFolder, PagesText.Count);

        Pages = new List<Page>(1);;
        PagesText = new List<PageTextC>();
        OnWord = 0;
        ViweportText.text = string.Empty;

        return bookFolder;
    }

    
    void EmptyLine (XmlNode node)
    {

    }

    void Image(XmlNode node)
    {
        if(!Directory.Exists(bookFolder))
        {
            Directory.CreateDirectory(bookFolder);
            Directory.CreateDirectory(bookFolder + "/Images");
        }

        Texture2D tex = new Texture2D(0, 0);

        foreach (XmlAttribute attribute in node.Attributes)
        {
            if(attribute.Name == "l:href")
            {
                string a = node.Attributes["l:href"].Value;
                a = a.Remove(0, 1);

                byte[] z = Convert.FromBase64String(bookFile.SelectSingleNode($"//*[@id = '{a}']").InnerText);
                


                tex.LoadImage(z);

                if(tex.width * tex.height > 60000)
                {
                    File.WriteAllBytes(bookFolder + '/' + "/Images/" + "hight" + a, tex.EncodeToJPG());

                    //TextureScale.Bilinear(tex, tex.width/2, tex.height/2);
                    TextureScale.Bilinear(tex, 200, 300);

                    File.WriteAllBytes(bookFolder + '/' + "/Images/" + a, tex.EncodeToJPG());
                }
                else
                {
                    File.WriteAllBytes(bookFolder + '/' + "/Images/" + a, tex.EncodeToJPG());
                }

                TextParts.Add("image=" + a);
            }
        }

        
    }

    public void Em (XmlNode node)
    {
        TextParts.Add("<align=" + "center" + ">");
        TextParts.Add("<b><i>");

        foreach (XmlNode childNode in node.ChildNodes)
        {
            Methods[childNode.Name].Invoke(childNode);
        }

        TextParts[TextParts.Count-1] += "</align></i></b>";
    }

    void Epigraph (XmlNode node)
    {
        TextParts.Add("<width=70%>");
        foreach (XmlNode childNode in node.ChildNodes)
        {
            Methods[childNode.Name].Invoke(childNode);
        }
        TextParts.Add("</width>");
    }

    void TextAuthor (XmlNode node)
    {
        TextParts.Add("<margin=3em>");
        foreach (XmlNode childNode in node.ChildNodes)
        {
            Methods[childNode.Name].Invoke(childNode);
        }
        TextParts.Add("</margin>");
        TextParts.Add("\n");
    }

    void Cite (XmlNode node)
    {
        TextParts.Add("\n");
        
        TextParts.Add("<color=#565656>");
        //TextParts.Add("<b>Цитата:</b>" + '\n');

        foreach (XmlNode childNode in node.ChildNodes)
        {
            Methods[childNode.Name].Invoke(childNode);
        }

        TextParts.Add("</color>");
        TextParts.Add("\n");
    }

    void A (XmlNode node)
    {
        TextParts.Add(" <link><color=#0044a3>");

        foreach (XmlNode childNode in node.ChildNodes)
        {
            Methods[childNode.Name].Invoke(childNode);
        }
        
        TextParts.Add("</color></link> ");

    }

    void Body (XmlNode node)
    {
        foreach (XmlNode childNode in node.ChildNodes)
        {
            Methods[childNode.Name].Invoke(childNode);
        }
    }

    void Section (XmlNode node)
    {
        TextParts.Add("\n");
        foreach (XmlNode childNode in node.ChildNodes)
        {
            Methods[childNode.Name].Invoke(childNode);
        }
        //Texttt.Add("\n");
    }

    void Title (XmlNode node)
    {
        TextParts.Add("<align=" + "center" + ">");
        foreach (XmlNode childNode in node.ChildNodes)
        {
            Methods[childNode.Name].Invoke(childNode);
        }
        TextParts.Add("</align>");
        TextParts.Add("\n");
    }

    void P (XmlNode node)
    {
        if(node.ParentNode.Name == "section")
            TextParts.Add("<space=1em>");

        foreach (XmlNode childNode in node.ChildNodes)
        {
            Methods[childNode.Name].Invoke(childNode);
        }
        TextParts.Add("\n");
    }

    public int WordLength = 15;

    public int CountParts = 2;

    void SpalshText (XmlNode node)
    {
        string[] a = (node.InnerText).Split(' ');

        foreach (var item in a)
        {
            TextParts.Add(item + ' ');

            // if(item.Length < WordLength)
            // {
                
            // }
            // else if(item.Length >= WordLength)
            // {
            //     string it = item.Replace("-", "");

            //     // print(item + " | " + item.Length);

            //     int z = (int)Mathf.Floor(it.Length / CountParts);

            //     // print("Количество частей " + z);


            //     List<int> Parts = new List<int>();

            //     for (int i = 0; i < CountParts; i++)
            //     {
            //         Parts.Add(z);
            //     }

            //     int ost = it.Length % CountParts;

            //     // print("Остаток " + ost);

            //     // print("_");

            //     int x = 0;

            //     for (int i = 0; i < ost; i++)
            //     {
            //         Parts[x]++;
            //         x++;
            //         if(x == Parts.Count)
            //         {
            //             x = 0;
            //         }
            //     }

            //     // foreach (var xax in Parts)
            //     // {
            //     //     print(xax);
            //     // }

            //     // print(" ");
                
            //     string items = it;

            //     for (int i = 0; i < Parts.Count; i++)
            //     {
            //         if(i == 0)
            //             TextParts.Add(items.Substring(0, Parts[i]) + '-');
            //         else if(i != Parts.Count - 1)
            //             TextParts.Add(items.Substring(0, Parts[i]) + '-');
            //         else
            //             TextParts.Add(items.Substring(0, Parts[i]) + ' ');
                    
            //         items = items.Remove(0, Parts[i]);
            //     }

            //     // foreach (var xax in Parts)
            //     // {
                    
            //     // }

            //     // int lastzWord = TextParts.Count - 1;
            //     // int lastzChar = TextParts[lastzWord].Length - 1;

            //     // TextParts[lastzWord] = TextParts[lastzWord].Remove(lastzChar);
            //     // TextParts[lastzWord] = TextParts[lastzWord].Insert(lastzChar, " ");

            //     //TextParts.Add(item + '-');
            // }
                
        }

        int lastWord = TextParts.Count - 1;
        int lastChar = TextParts[lastWord].Length - 1;

        TextParts[lastWord] = TextParts[lastWord].Remove(lastChar);
        

        foreach (XmlNode childNode in node.ChildNodes)
        {
            //Methods[childNode.Name].Invoke(childNode);
        }
    }

    void EmphasisNew (XmlNode node)
    {
        TextParts.Add("<i>");

        foreach (XmlNode childNode in node.ChildNodes)
        {
            Methods[childNode.Name].Invoke(childNode);
        }

        TextParts.Add("</i>");
    }

    void StrongNew (XmlNode node)
    {
        TextParts.Add("<b>");

        foreach (XmlNode childNode in node.ChildNodes)
        {
            Methods[childNode.Name].Invoke(childNode);
        }

        TextParts.Add("</b>");
    }

    void Poem (XmlNode node)
    {
        TextParts.Add("\n");
        
        TextParts.Add("<color=#565656>");
        //TextParts.Add("<b>Стихотворение:</b>");
        TextParts.Add("<margin=3em>");
        TextParts.Add("<i>");

        foreach (XmlNode childNode in node.ChildNodes)
        {
            Methods[childNode.Name].Invoke(childNode);
        }

        TextParts.Add("</color>");
        TextParts.Add("</margin>");
        TextParts.Add("</i>");
        TextParts.Add("\n");
    }

    void Stanza (XmlNode node)
    {
        foreach (XmlNode childNode in node.ChildNodes)
        {
            Methods[childNode.Name].Invoke(childNode);
        }
    }

    
    void V (XmlNode node)
    {
        
        

        foreach (XmlNode childNode in node.ChildNodes)
        {
            Methods[childNode.Name].Invoke(childNode);
        }

        
        

        TextParts.Add("\n");
    }

    void Date (XmlNode node)
    {
        TextParts.Add("\n");

        foreach (XmlNode childNode in node.ChildNodes)
        {
            Methods[childNode.Name].Invoke(childNode);
        }

        TextParts.Add("\n");
    }

    void Strikethrough (XmlNode node)
    {
        TextParts.Add("<s>");

        foreach (XmlNode childNode in node.ChildNodes)
        {
            Methods[childNode.Name].Invoke(childNode);
        }

        TextParts.Add("</s>");
    }

    void Sub (XmlNode node)
    {
        TextParts.Add("<sub>");

        foreach (XmlNode childNode in node.ChildNodes)
        {
            Methods[childNode.Name].Invoke(childNode);
        }

        TextParts.Add("</sub>");
    }

    void Sup (XmlNode node)
    {
        TextParts.Add("<sup>");

        foreach (XmlNode childNode in node.ChildNodes)
        {
            Methods[childNode.Name].Invoke(childNode);
        }

        TextParts.Add("</sup>");
    }

    void Code (XmlNode node)
    {
        TextParts.Add("<mark=#7a7a7a66>");

        foreach (XmlNode childNode in node.ChildNodes)
        {
            Methods[childNode.Name].Invoke(childNode);
        }

        TextParts.Add("</mark>");
    }

    void Annotation (XmlNode node)
    {
        TextParts.Add("<size=95%><margin=1.5em>");

        foreach (XmlNode childNode in node.ChildNodes)
        {
            Methods[childNode.Name].Invoke(childNode);
        }

        TextParts.Add("</margin></size>");
    }

    void Subtitle (XmlNode node)
    {
        TextParts.Add("\n");
        TextParts.Add("<align=" + "center" + ">");

        foreach (XmlNode childNode in node.ChildNodes)
        {
            Methods[childNode.Name].Invoke(childNode);
        }

        TextParts.Add("</align>");
        TextParts.Add("\n");
    }

    


    
}
