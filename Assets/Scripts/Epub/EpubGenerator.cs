using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VersOne.Epub;

public class EpubGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EpubBook epubBook = EpubReader.ReadBook("Assets/Books/Epub/Fresko_Vse-luchshee-chto-ne-kupish-za-dengi.375942.fb2.epub");


        foreach (EpubTextContentFile textContentFile in epubBook.ReadingOrder)
        {
            // HTML of current text content file
            string htmlContent = textContentFile.Content;
            print(htmlContent);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
