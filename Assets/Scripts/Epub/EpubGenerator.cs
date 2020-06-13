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



    public List<XmlDocument> Chapters = new List<XmlDocument>();

    public List<string> ChaptersPaths;

    ZipArchive EpubFile;


    void InitializeMethods()
    {
        Methods.Add("body", Body);          //Тело документа

        Methods.Add("span", Span);          //ID главый или подглавы

        Methods.Add("div", Div);            //Алигмент строк

        Methods.Add("p", P);                //Элемент абзаца

        Methods.Add("#text", SpalshText);   //Текст

        Methods.Add("strong", StrongNew);   //Модификатор жирности текста
        Methods.Add("b", StrongNew);        //Модификатор жирности текста

        Methods.Add("i", EmphasisNew);      //Модификатор наклона текста

        Methods.Add("a", A);

        Methods.Add("em", Em);              //Модификатор выделения текста

        Methods.Add("img", Image);  
        
        Methods.Add("sub", Sub);

        Methods.Add("sup", Sup);

        Methods.Add("hr", P);

        Methods.Add("blockquote", Blockquote);
        

        Methods.Add("h1", H);
        Methods.Add("h2", H);
        Methods.Add("h3", H);
        Methods.Add("h4", H);
        Methods.Add("h5", H);
        Methods.Add("h6", H);

            Methods.Add("ol", EpmtyLine);
            Methods.Add("ul", EpmtyLine);


        Methods.Add("br", Br);
        Methods.Add("section", Div);
    }

    void Start()
    {
        InitializeMethods();
    }

    string bookFolder;

    public void Generate (string path)
    {
        EpubBook book = EpubReader.Read(path);

        ICollection<EpubChapter> chapters = book.TableOfContents;

        FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);

        ICollection<EpubByteFile> images = book.Resources.Images;

        bookFolder = Library.pathToBooksFolder + '/' + Path.GetFileNameWithoutExtension(path);

        Directory.CreateDirectory(bookFolder);
        Directory.CreateDirectory(bookFolder + "/Images");

        foreach (var item in images)
        {
            Texture2D tex = new Texture2D(0, 0);

            tex.LoadImage(item.Content);

            if(tex.width * tex.height > 60000)
            {
                File.WriteAllBytes(bookFolder + '/' + "/Images/" + "hight" + Path.GetFileNameWithoutExtension(item.AbsolutePath) + ".jpg", tex.EncodeToJPG());

                TextureScale.Bilinear(tex, tex.width/2, tex.height/2);
                //TextureScale.Bilinear(tex, 200, 300);

                File.WriteAllBytes(bookFolder + '/' + "/Images/" + Path.GetFileNameWithoutExtension(item.AbsolutePath) + ".jpg", tex.EncodeToJPG());
            }
            else
            {
                File.WriteAllBytes(bookFolder + '/' + "/Images/" + Path.GetFileNameWithoutExtension(item.AbsolutePath) + ".jpg", tex.EncodeToJPG());
            }
        }

        EpubFile = new ZipArchive(stream);

        

        foreach (var item in chapters)
        {
            if(!ChaptersPaths.Contains(item.AbsolutePath))
            {
                ChaptersPaths.Add(item.AbsolutePath);
                Chapters.Add(new XmlDocument());
                Chapters[Chapters.Count-1].Load(EpubFile.GetEntry(item.AbsolutePath.Remove(0, 1)).Open());
            }

            foreach (var items in item.SubChapters)
            {
                if(!ChaptersPaths.Contains(items.AbsolutePath))
                {
                    ChaptersPaths.Add(items.AbsolutePath);
                    Chapters.Add(new XmlDocument());
                    Chapters[Chapters.Count-1].Load(EpubFile.GetEntry(items.AbsolutePath.Remove(0, 1)).Open());
                }
            }
        }

        foreach (XmlDocument books in Chapters)
        {
            XmlNodeList BookNodes = books.GetElementsByTagName("html")[0].ChildNodes;

            foreach (XmlNode childNode in BookNodes)
            {
                if(childNode.Name == "body")
                {
                    Methods[childNode.Name].Invoke(childNode);
                }
            }
        }


        

        MakeBook();
        AlternativBook(bookFolder);
        Description(book);

        CoverImage(book);

        Pages = new List<Page>(1);;
        PagesText = new List<PageTextC>();
        OnWord = 0;
        ViweportText.text = string.Empty;
        Chapters.Clear();
        ChaptersPaths.Clear();
    }

    void CoverImage(EpubBook book)
    {
        Texture2D tex = new Texture2D(0, 0);

        tex.LoadImage(book.CoverImage);

        if(tex.width * tex.height > 119286)
        {
            File.WriteAllBytes(bookFolder + "/Images/hightCoverImage.jpg", tex.EncodeToJPG());

            TextureScale.Bilinear(tex, 200, 300);

            File.WriteAllBytes(bookFolder + "/Images/CoverImage.jpg", tex.EncodeToJPG());
        }
        else
        {
            File.WriteAllBytes(bookFolder + "/Images/CoverImage.jpg", tex.EncodeToJPG());
        }

        Resources.UnloadUnusedAssets();
    }

    void Image(XmlNode node)
    {
        if(!Directory.Exists(bookFolder))
        {
            Directory.CreateDirectory(bookFolder);
            Directory.CreateDirectory(bookFolder + "/Images");
        }

        foreach (XmlAttribute attribute in node.Attributes)
        {
            if(attribute.Name == "src" && attribute.Value != null)
            {
                TextParts.Add("image=" + Path.GetFileNameWithoutExtension(attribute.Value) + ".jpg");
            }
        }
    }

    public void Br (XmlNode node)
    {
        TextParts.Add("\n");
    }
    
    bool Blockquotes;

    public void Blockquote (XmlNode node)
    {
        TextParts.Add("\n<indent=10em>");
        Blockquotes = true;

        foreach (XmlNode childNode in node.ChildNodes)
        {
            Methods[childNode.Name].Invoke(childNode);
        }

        Blockquotes = false;

        TextParts[TextParts.Count-1] += "</indent>\n";
    }

    public void Em (XmlNode node)
    {
        TextParts.Add("<align=" + "center" + "><b><i>");

        foreach (XmlNode childNode in node.ChildNodes)
        {
            Methods[childNode.Name].Invoke(childNode);
        }

        TextParts[TextParts.Count-1] += "</align></i></b>";
    }

    public void H (XmlNode node)
    {
        TextParts.Add("\n<align=" + "center" + "><b>");

        foreach (XmlNode childNode in node.ChildNodes)
        {
            Methods[childNode.Name].Invoke(childNode);
        }

        TextParts[TextParts.Count-1] += "</b></align>\n\n";
    }

    public void EpmtyLine (XmlNode node)
    {
        
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
            print(childNode.Name);
            Methods[childNode.Name].Invoke(childNode);
        }


        foreach (XmlAttribute attribute in node.Attributes)
        {
            if(attribute.Value == "title1")
            {
                TextParts[TextParts.Count-1] += "</align>";
            }
        }
    }

    void P (XmlNode node)
    {
        if(!Blockquotes)
            TextParts.Add("<space=1em>");

        foreach (XmlNode childNode in node.ChildNodes)
        {
            print(childNode.Name);
            Methods[childNode.Name].Invoke(childNode);
        }
        TextParts[TextParts.Count-1] += "\n";
    }

    void EmphasisNew (XmlNode node)
    {
        TextParts.Add("<i>");

        foreach (XmlNode childNode in node.ChildNodes)
        {
            Methods[childNode.Name].Invoke(childNode);
        }

        TextParts[TextParts.Count-1] += "</i>";
    }

    void StrongNew (XmlNode node)
    {
        TextParts.Add("<b>");

        foreach (XmlNode childNode in node.ChildNodes)
        {
            Methods[childNode.Name].Invoke(childNode);
        }

        TextParts[TextParts.Count-1] += "</b>";
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

    void A (XmlNode node)
    {
        TextParts.Add(" <link><color=#0044a3>");

        foreach (XmlNode childNode in node.ChildNodes)
        {
            Methods[childNode.Name].Invoke(childNode);
        }
        
        TextParts[TextParts.Count-1] += "</color></link> ";
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
                        int a = TextParts[i + OnWord].IndexOf(".jpg");


                        string zaz = TextParts[i + OnWord].Substring(0, a+4);

                        PagesText[pageID].NameImages.Add(zaz.Remove(0, 6));

                        PagesText[pageID].PageText += "<b>(Рис." + PagesText[pageID].NameImages.Count + ")</b>";
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

    void Description (EpubBook book)
    {
        BookDescription bookDescription = new BookDescription();
        bookDescription.BookTitle = book.Title;
        
        foreach (var item in book.Authors)
        {
            bookDescription.Authors.Add(new Autor());
            bookDescription.Authors[bookDescription.Authors.Count-1].NameParts.Add(item);
        }

        bookDescription.PagesCount = PagesText.Count;
        

        FileStream steama = new FileStream(bookFolder + "/descript.packNew", FileMode.Create, FileAccess.Write);
        MessagePackSerializer.Serialize(steama, bookDescription);
        steama.Close();
    }
}
