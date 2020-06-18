using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tests : MonoBehaviour
{
    public int[][] Narray = new int[10][];

    public int[,] aza = new int[10,11];

    void Start()
    {

        for (int i = 0; i < Narray.Length; i++)
        {
            Narray[i] = new int[10] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10};
        }

        // for (int up = 0; up < Narray.Length; up++)
        // {
        //     for (int right = 0; right < Narray[0].Length; right++)
        //     {
        //         print(Narray[up][right]);
        //     }
        // }

        // print("  ");

        TransposArray(ref Narray);
    }

    void TransposArray (ref int[][] array)
    {
        int upL = Narray.Length;
        int rightL = Narray[0].Length;

        int[][] x = new int[rightL][];

        for (int i = 0; i < rightL; i++)
        {
            x[i] = new int[upL];
        }

        for (int up = 0; up < upL; up++)
        {
            for (int right = 0; right < rightL; right++)
            {
                x[right][up] = array[up][right];
            }
        }

        Narray = x;

        // for (int up = 0; up < x.Length; up++)
        // {
        //     for (int right = 0; right < x[0].Length; right++)
        //     {
        //         print(x[up][right]);
        //     }
        // }
    }
}
