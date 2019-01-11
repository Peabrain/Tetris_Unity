using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Figure : MonoBehaviour
{
    static int[,,] figure_z = new int[,,]
    {
        {
            { 0,0,0 },
            { 1,1,0 },
            { 0,1,1 },
        },
        {
            { 0,1,0 },
            { 1,1,0 },
            { 1,0,0 },
        }
    };

    static int[,,] figure_s = new int[,,]
    {
            {
                { 0,0,0 },
                { 0,1,1 },
                { 1,1,0 },
            },
            {
                { 0,1,0 },
                { 0,1,1 },
                { 0,0,1 },
            }
    };

    static int[,,] figure_L = new int[,,]
    {
            {
                { 0,1,0 },
                { 0,1,0 },
                { 0,1,1 },
            },
            {
                { 0,0,1 },
                { 1,1,1 },
                { 0,0,0 },
            },
            {
                { 1,1,0 },
                { 0,1,0 },
                { 0,1,0 },
            },
            {
                { 0,0,0 },
                { 1,1,1 },
                { 1,0,0 },
            }
    };

    static int[,,] figure_l = new int[,,]
    {
            {
                { 0,1,0 },
                { 0,1,0 },
                { 1,1,0 },
            },
            {
                { 0,0,0 },
                { 1,1,1 },
                { 0,0,1 },
            },
            {
                { 0,1,1 },
                { 0,1,0 },
                { 0,1,0 },
            },
            {
                { 1,0,0 },
                { 1,1,1 },
                { 0,0,0 },
            }
    };

    static int[,,] figure_T = new int[,,]
    {
            {
                { 0,0,0 },
                { 1,1,1 },
                { 0,1,0 },
            },
            {
                { 0,1,0 },
                { 0,1,1 },
                { 0,1,0 },
            },
            {
                { 0,1,0 },
                { 1,1,1 },
                { 0,0,0 },
            },
            {
                { 0,1,0 },
                { 1,1,0 },
                { 0,1,0 },
            }
    };

    static int[,,] figure_I = new int[,,]
    {
            {
                { 0,0,1,0 },
                { 0,0,1,0 },
                { 0,0,1,0 },
                { 0,0,1,0 },
            },
            {
                { 0,0,0,0 },
                { 0,0,0,0 },
                { 1,1,1,1 },
                { 0,0,0,0 },
            },
    };
    static int[,,] figure_O = new int[,,]
    {
            {
                { 1,1 },
                { 1,1 },
            },
    };

    int[][,,] tab = new int[][,,]
    {
        figure_z,
        figure_s,
        figure_l,
        figure_L,
        figure_I,
        figure_T,
        figure_O
    };

    public int[,,] getFigure(int i)
    {
        return tab[i];
    }
    public int getMax()
    {
        return tab.Length;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}