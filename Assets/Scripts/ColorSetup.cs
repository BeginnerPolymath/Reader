using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.IO;
using MessagePack;

[Serializable]
public struct ImageColor
{
    public Image Image;
    public Color32[] Color;
}

[Serializable]
public struct TextColor
{
    public TextMeshProUGUI Text;
    public Color32[] Color;
}

[Serializable]
public struct LibraryElements
{
    public ImageColor Background;

    public TextMeshProUGUI TextLibraryEmpty;

    public Color32[] TextColors;

    public Book BookPrefab;
    public Color32[] BookBackgroundColors;

    public ImageColor AddBookButton;
    public TextMeshProUGUI AddBookButtonText;
}

[Serializable]
public struct SettingsElements
{
    public ImageColor Background;

    public Image[] Settings;

    public Color32[] SetsColors;

    public Image[] SettingsIF;
    public Image[] SettingsBlocks;
    public Color32[] BlocksColors;

    public TextMeshProUGUI[] TextSetsName;
    public Color32[] TextColors;
}

public class ColorSetup : MonoBehaviour
{
    public LibraryElements LibraryElements;

    [Space(10)]

    public SettingsElements SettingsElements;

    [Space(10)]

    public ImageColor SpritzBackground;
    public ImageColor BookBackground;

    [Space(10)]

    public Image[] SpritzCarret;
    public Color32[] CarretColors;

    public TextColor SpritzPrefabChar;
    public ImageColor SettingsGearButton;
    public ImageColor SearchButton;

    [Space(10)]

    public ImageColor SpritzStartBackground;

    [Space(10)]

    public TextColor BookText;

    [Space(10)]

    public ImageColor PageInfoBackground;
    public TextColor PageInfoText;


    public SpritzScript SpritzScript;

    void Start ()
    {
        LoadColor ();
    }

    void LoadColor ()
    {
        FileStream filez = File.Open(Application.persistentDataPath + "/ReadSetting.newPack", FileMode.Open);
        Settings settings = MessagePackSerializer.Deserialize<Settings>(filez);

        SetColor(settings.ColorSetupID);

        filez.Close();
    }


    public void SetColor (int ID)
    {
        SpritzBackground.Image.color = SpritzBackground.Color[ID];

        foreach (Image img in SpritzCarret)
        {
            img.color = CarretColors[ID];
        }
        
        SpritzPrefabChar.Text.color = SpritzPrefabChar.Color[ID];
        SettingsGearButton.Image.color = SettingsGearButton.Color[ID];
        SearchButton.Image.color = SearchButton.Color[ID];

        SpritzStartBackground.Image.color = SpritzStartBackground.Color[ID];

        BookBackground.Image.color = BookBackground.Color[ID];
        BookText.Text.color = BookText.Color[ID];

        PageInfoBackground.Image.color = PageInfoBackground.Color[ID];
        PageInfoText.Text.color = PageInfoText.Color[ID];


        LibraryElements.Background.Image.color = LibraryElements.Background.Color[ID];
        LibraryElements.TextLibraryEmpty.color = LibraryElements.TextColors[ID];
        LibraryElements.AddBookButton.Image.color = LibraryElements.AddBookButton.Color[ID];
        LibraryElements.AddBookButtonText.color = LibraryElements.TextColors[ID];

        LibraryElements.BookPrefab.PageCounter.color = LibraryElements.TextColors[ID];
        LibraryElements.BookPrefab.Authors.color = LibraryElements.TextColors[ID];
        LibraryElements.BookPrefab.Name.color = LibraryElements.TextColors[ID];

        LibraryElements.BookPrefab.Background.color = LibraryElements.BookBackgroundColors[ID];


        SettingsElements.Background.Image.color = SettingsElements.Background.Color[ID];   
        
        foreach (Image set in SettingsElements.Settings)
        {
            set.color = SettingsElements.SetsColors[ID];
        }

        foreach (TextMeshProUGUI set in SettingsElements.TextSetsName)
        {
            set.color = SettingsElements.TextColors[ID];
        }

        foreach (Image set in SettingsElements.SettingsBlocks)
        {
            set.color = SettingsElements.BlocksColors[ID];
        }

        foreach (Image set in SettingsElements.SettingsIF)
        {
            set.color = SettingsElements.Background.Color[ID]; 
        }



        SpritzScript._Library.LoadLibrary();
    }

    public void SaveColor (int ID)
    {
        FileStream filez = File.Open(Application.persistentDataPath + "/ReadSetting.newPack", FileMode.Open);
        Settings settings = MessagePackSerializer.Deserialize<Settings>(filez);

        settings.ColorSetupID = ID;

        filez.Close();

        FileStream steams = new FileStream(Application.persistentDataPath + "/ReadSetting.newPack", FileMode.OpenOrCreate, FileAccess.ReadWrite);
        MessagePackSerializer.Serialize(steams, settings);
        steams.Close();

        if(!SpritzScript._Library.gameObject.activeSelf)
            SpritzScript.TextForceUpdate();
    }



    public enum Colors : int
    {
        White, Black
    }
}
