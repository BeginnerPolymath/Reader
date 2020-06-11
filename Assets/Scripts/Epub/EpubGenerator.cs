using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Compression;
using System.Xml;
using System.IO;
using EpubSharp.Format;

public class EpubGenerator : MonoBehaviour
{
    private ZipArchive Open(Stream stream, bool leaveOpen)
    {
        return new ZipArchive(stream, ZipArchiveMode.Read, leaveOpen, Constants.DefaultEncoding);
    }

    // Start is called before the first frame update
    void Start()
    {
        FileStream x = new FileStream("Assets/Books/Epub/Fresko_Vse-luchshee-chto-ne-kupish-za-dengi.375942.fb2.epub", FileMode.Open, FileAccess.Read);

        ZipArchive z = new ZipArchive(x);

        XmlDocument bookFile = new XmlDocument();

        foreach (ZipArchiveEntry item in z.Entries)
        {
            print(item.FullName);
        }

        bookFile.Load(z.GetEntry("OPS/ch1.xhtml").Open());

        print(bookFile.InnerText);
    }

    // Update is called once per frames
    void Update()
    {
        
    }
}
