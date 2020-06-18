using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using TMPro.Examples;

public class PositionHi : MonoBehaviour
{
    public TextMeshProUGUI txt;

    public RectTransform textTras;

    void Start()
    {
        
    }

    public int CharID;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1))
        {
            txt.ForceMeshUpdate();
 
            TMP_CharacterInfo lastCharInfo = txt.textInfo.characterInfo[CharID];

            print(lastCharInfo.character);

            Vector3 br = lastCharInfo.vertex_BR.position;
            Vector3 tl = lastCharInfo.vertex_TL.position;

            Vector3 center = Vector3.Lerp(br, tl, 0.5f);

            Vector3 position = txt.transform.localToWorldMatrix.MultiplyPoint3x4(center);

            textTras.position = position;
        }
    }
}
