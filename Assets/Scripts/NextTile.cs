using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this class is to manage the random tiles selection

public class NextTile : MonoBehaviour
{
    int next = 0;
    int color = 0;
    int rotation = 0;
    bool update = false;

    public Tiles tiles = null;
    public InGame ingame = null;

    List<GameObject> blocks = new List<GameObject>(); // save sprites for the next tile for visibility

    void Start()
    {
        randomize();
    }

    void randomize()
    {
        next = Random.Range(0, 8957403) % tiles.getMax();
        color = (Random.Range(0, 58295932) % 7) + 1;
        int[,,] f = tiles.getTile(next);
        rotation = (Random.Range(0, 58295932) % f.GetLength(0));
    }

    // update the next tile graphic
    void Update()
    {
        if(update)
        {
            clear();

            int[,,] tile = tiles.getTile(next);

            for (int y = 0; y < tile.GetLength(1); y++)
            {
                for (int x = 0; x < tile.GetLength(2); x++)
                {
                    if(tile[rotation, y,x] != 0)
                    {
                        GameObject go = Instantiate(ingame.BlockPrefab, transform.position, Quaternion.identity) as GameObject;
                        go.transform.SetParent(this.transform);
                        go.transform.localPosition = new Vector3(-(0.1f * ((tile.GetLength(2) + 1) & 1) + 0.2f * (-tile.GetLength(2) / 2 + x)),
                                        0.1f * ((tile.GetLength(1) + 1) & 1) + 0.2f * (-tile.GetLength(1) / 2 + y),
                                        0);
                        go.transform.localRotation = Quaternion.identity;
                        Block s = go.GetComponent<Block>();
                        s.SetColor(color);
                        blocks.Add(go);
                    }
                }
            }


            update = false;
        }
    }

    void clear()
    {
        foreach (GameObject g in blocks)
            Destroy(g);
        blocks.Clear();
    }

    public void newNext()
    {
        randomize();
        update = true;
    }

    public int[,,] getNext()
    {
        return tiles.getTile(next);
    }
    public int getColor()
    {
        return color;
    }
    public int getRotation()
    {
        return rotation;
    }
}
