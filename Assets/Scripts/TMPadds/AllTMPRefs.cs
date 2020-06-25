using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AllTMPRefs : MonoBehaviour
{
    public List<TextMeshProUGUI> Texts = new List<TextMeshProUGUI>();

    public List<TMP_InputField> InputFields = new List<TMP_InputField>();

    public GameObject BookPrefab;

    [Space(10)]

    public TMP_FontAsset MainFont;
}