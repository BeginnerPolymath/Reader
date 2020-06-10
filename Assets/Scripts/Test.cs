using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public string Tezz;

    void Start()
    {
        Agaga(ref Tezz);
        print(Tezz);
    }

    void Agaga (ref string Aga)
    {
        Aga = "Hay";
    }
}
