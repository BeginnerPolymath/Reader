using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZoomWordScript : MonoBehaviour
{
    void FixedUpdate()
    {
        transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y + 150);
    }
}
