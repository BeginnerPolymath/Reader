using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.IO;
using MessagePack;

public class ColorStyle : MonoBehaviour
{
    public SpritzScript SpritzScript;

    public TMP_ColorGradient TextMaterial;
    public Color32[] TextColors;

    public Material BackgroundMaterial;
    public Color32[] BackgroundColors;

    public Material ElementBackgroundMaterial;
    public Color32[] ElementBackgroundColors;

    public Material BlockMaterial;
    public Color32[] BlockColors;

    public Material LinesMaterial;
    public Color32[] LinesColors;

    public RectTransform BookViwer;

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
        
        
        TextMaterial.bottomLeft = TextColors[ID];
        TextMaterial.bottomRight = TextColors[ID];

        TextMaterial.topLeft = TextColors[ID];
        TextMaterial.topRight = TextColors[ID];

        BookViwer.gameObject.SetActive(false);
        BookViwer.gameObject.SetActive(true);

        BackgroundMaterial.color = BackgroundColors[ID];

        ElementBackgroundMaterial.color = ElementBackgroundColors[ID];

        BlockMaterial.color = BlockColors[ID];

        LinesMaterial.color = LinesColors[ID];
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
}
