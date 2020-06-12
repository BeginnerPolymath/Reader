using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Compression;
using System.Xml;
using System.IO;
using EpubSharp;
using TMPro;
using MessagePack;

public class EpubGenerator : MonoBehaviour
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



    public int BookID;

    public List<XmlDocument> Chapters = new List<XmlDocument>();

    public List<string> ChaptersString = new List<string>();

    ZipArchive EpubFile;

    public List<FileStream> Books = new List<FileStream>();
    
    public List<string> PathBooks = new List<string>()
    {
        "Assets/Books/Epub/igp-twss.epub",
        "Assets/Books/Epub/Fresko_Vse-luchshee-chto-ne-kupish-za-dengi.375942.fb2.epub",
    };

    void InitializeMethods()
    {
        Methods.Add("body", Body);          //Тело документа

        Methods.Add("span", Span);          //ID главый или подглавы

        Methods.Add("div", Div);            //Алигмент строк

        Methods.Add("p", P);                //Элемент абзаца

        Methods.Add("#text", SpalshText);   //Текст

        Methods.Add("strong", StrongNew);   //Модификатор жирности текста

        Methods.Add("em", EmphasisNew);     //Модификатор наклона текста
    }

    void Start()
    {
        InitializeMethods();
    }

    string bookFolder;

    public void Generate (string path)
    {
        EpubBook book = EpubReader.Read(PathBooks[BookID]);

        ICollection<EpubChapter> chapters = book.TableOfContents;

        Books.Add(new FileStream("Assets/Books/Epub/igp-twss.epub", FileMode.Open, FileAccess.Read));
        Books.Add(new FileStream("Assets/Books/Epub/Fresko_Vse-luchshee-chto-ne-kupish-za-dengi.375942.fb2.epub", FileMode.Open, FileAccess.Read));

        EpubFile = new ZipArchive(Books[BookID]);

        foreach (var item in chapters)
        {
            Chapters.Add(new XmlDocument());
            Chapters[Chapters.Count-1].Load(EpubFile.GetEntry(item.AbsolutePath.Remove(0, 1)).Open());

            
        }

        foreach (XmlDocument books in Chapters)
        {
            print(books.Name);
            XmlNodeList BookNodes = books.GetElementsByTagName("html")[0].ChildNodes;

            foreach (XmlNode childNode in BookNodes)
            {
                if(childNode.Name == "body")
                {
                    Methods[childNode.Name].Invoke(childNode);
                }
            }
        }


        bookFolder = Library.pathToBooksFolder + '/' + Path.GetFileNameWithoutExtension(path);

        MakeBook();
        AlternativBook(bookFolder, book.Title);

        Pages = new List<Page>(1);;
        PagesText = new List<PageTextC>();
        OnWord = 0;
        ViweportText.text = string.Empty;
    }

    public void Body (XmlNode node)
    {
        foreach (XmlNode childNode in node.ChildNodes)
        {
            Methods[childNode.Name].Invoke(childNode);
        }
    }

    public void Span (XmlNode node)
    {
        foreach (XmlNode childNode in node.ChildNodes)
        {
            Methods[childNode.Name].Invoke(childNode);
        }
    }

    public void Div (XmlNode node)
    {
        foreach (XmlAttribute attribute in node.Attributes)
        {
            if(attribute.Value == "title1")
            {
                TextParts.Add("<align=" + "center" + ">");
            }
        }

        foreach (XmlNode childNode in node.ChildNodes)
        {
            Methods[childNode.Name].Invoke(childNode);
        }

        TextParts.Add("</align>");
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

    void SpalshText (XmlNode node)
    {
        string[] a = (node.InnerText).Split(' ');

        foreach (var item in a)
        {
            TextParts.Add(item + ' ');
        }

        int lastWord = TextParts.Count - 1;
        int lastChar = TextParts[lastWord].Length - 1;

        TextParts[lastWord] = TextParts[lastWord].Remove(lastChar);
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

    void AlternativBook (string bookFolder, string bookTitle)
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

        steama = new FileStream(bookFolder + "/descript.packNew", FileMode.Create, FileAccess.Write);
        MessagePackSerializer.Serialize(steama, new BookDescription() {BookTitle = bookTitle, PagesCount = PagesText.Count});
        steama.Close();
    }
}
