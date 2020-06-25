using UnityEngine;
using UnityEditor;
using TMPro;

[CustomEditor(typeof(AllTMPRefs))]
[CanEditMultipleObjects]
public class AllTMPRefsEditor : Editor 
{
    AllTMPRefs targetScript;

    Object[] SelectionObjects;

    void OnEnable()
    {
        targetScript = (AllTMPRefs)target;

        
    }


    public override void OnInspectorGUI()
    {
        SelectionObjects = Selection.objects;

        DrawDefaultInspector();

        if(GUILayout.Button("Set Font"))
        {
            foreach (var text in targetScript.Texts)
            {
                text.font = targetScript.MainFont;
            }

            foreach (var text in targetScript.InputFields)
            {
                text.fontAsset = targetScript.MainFont;
            }

            
        }

        if(GUILayout.Button("Add book Name and Autors tmp components"))
        {
            TextMeshProUGUI tmpcomp;

            tmpcomp = targetScript.BookPrefab.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();

            if(!targetScript.Texts.Contains(tmpcomp))
                targetScript.Texts.Add(tmpcomp);

            tmpcomp = targetScript.BookPrefab.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>();

            if(!targetScript.Texts.Contains(tmpcomp))
                targetScript.Texts.Add(tmpcomp);

            tmpcomp = targetScript.BookPrefab.transform.GetChild(1).GetChild(2).GetComponent<TextMeshProUGUI>();

            if(!targetScript.Texts.Contains(tmpcomp))
                targetScript.Texts.Add(tmpcomp);
        }
    }
}