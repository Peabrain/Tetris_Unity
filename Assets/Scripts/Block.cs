using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public Sprite [] Tex;
    int stone = 0;
    int newstone = 0;

    // Start is called before the first frame update
    void Start()
    {
        Renderer r = GetComponent<Renderer>();
        gameObject.GetComponent<SpriteRenderer>().sprite = Tex[stone];
    }

    // Update is called once per frame
    void Update()
    {
        SpriteRenderer r = gameObject.GetComponent<SpriteRenderer>();
        if (stone != newstone)
        {
            stone = newstone;
            r.sprite = Tex[stone];
        }
    }
    public void SetColor(int stone)
    {
        newstone = stone;
    }
}
