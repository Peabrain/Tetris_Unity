using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this class is for the tiles (Spielsteine)

public class Tiles : MonoBehaviour
{
    static int[,,] tile_z = new int[,,]
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

    static int[,,] tile_s = new int[,,]
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

    static int[,,] tile_L = new int[,,]
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

    static int[,,] tile_l = new int[,,]
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

    static int[,,] tile_T = new int[,,]
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

    static int[,,] tile_I = new int[,,]
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
    static int[,,] tile_O = new int[,,]
    {
            {
                { 1,1 },
                { 1,1 },
            },
    };

    // tile-list
    int[][,,] tab = new int[][,,]
    {
        tile_z,
        tile_s,
        tile_l,
        tile_L,
        tile_I,
        tile_T,
        tile_O
    };

    public int[,,] getTile(int i)
    {
        return tab[i];
    }
    public int getMax()
    {
        return tab.Length;
    }

    void Start()
    {
    }

    void Update()
    {
    }
}