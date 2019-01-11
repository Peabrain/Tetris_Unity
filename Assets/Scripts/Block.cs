using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* this class is for the blocks.
 * i build this in an extra class
 * because of maybe animations or so
 */

public class Block : MonoBehaviour
{
    public Sprite [] blocks;       // blocks that are possibly visible
    int block = 0;              // actual block
    int newblock = 0;           // new block if you want to change it

    void Start()
    {
        Renderer r = GetComponent<Renderer>();
        gameObject.GetComponent<SpriteRenderer>().sprite = blocks[block];      // first init the default block
    }

    void Update()
    {
        // update to the new block if necessary
        SpriteRenderer r = gameObject.GetComponent<SpriteRenderer>();
        if (block != newblock)
        {
            block = newblock;
            r.sprite = blocks[block];
        }
    }
    public void SetColor(int stone)
    {
        newblock = stone;
    }
}
