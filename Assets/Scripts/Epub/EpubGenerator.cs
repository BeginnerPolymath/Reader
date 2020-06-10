using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EpubSharp;

public class EpubGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EpubBook book = EpubReader.Read("my.epub");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
