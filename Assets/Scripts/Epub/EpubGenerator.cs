using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Compression;
using System.Xml;
using System.IO;
using EpubSharp;

public class EpubGenerator : MonoBehaviour
{
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

    // Start is called before the first frame update
    void Start()
    {
        EpubBook book = EpubReader.Read(PathBooks[BookID]);

        ICollection<EpubChapter> chapters = book.TableOfContents;

        print(book.Title);

        Books.Add(new FileStream("Assets/Books/Epub/igp-twss.epub", FileMode.Open, FileAccess.Read));
        Books.Add(new FileStream("Assets/Books/Epub/Fresko_Vse-luchshee-chto-ne-kupish-za-dengi.375942.fb2.epub", FileMode.Open, FileAccess.Read));

        EpubFile = new ZipArchive(Books[BookID]);

        foreach (var item in chapters)
        {
            Chapters.Add(new XmlDocument());
            Chapters[Chapters.Count-1].Load(EpubFile.GetEntry(item.AbsolutePath.Remove(0, 1)).Open());

            foreach (var itemz in item.SubChapters)
            {
                Chapters.Add(new XmlDocument());
                Chapters[Chapters.Count-1].Load(EpubFile.GetEntry(itemz.AbsolutePath.Remove(0, 1)).Open());
            }
        }


        //EpubFile.GetEntry($"OPS/ch{chID}.xhtml");

        XmlDocument bookFile = new XmlDocument();

        foreach (XmlDocument item in Chapters)
        {
            //print(item.InnerXml);
        }
    }

    // Update is called once per frames
    void Update()
    {
        
    }
}
