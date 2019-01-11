using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextFigure : MonoBehaviour
{
    int next = 0;
    int color = 0;
    int rot = 0;
    bool update = false;

    public Figure figures = null;
    public InGame ingame = null;

    List<GameObject> blocks = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        randomize();
    }

    void randomize()
    {
        next = Random.Range(0, 8957403) % figures.getMax();
        color = (Random.Range(0, 58295932) % 7) + 1;
        int[,,] f = figures.getFigure(next);
        rot = (Random.Range(0, 58295932) % f.GetLength(0));
    }

    // Update is called once per frame
    void Update()
    {
        if(update)
        {
            clear();

            int[,,] figure = figures.getFigure(next);

            for (int y = 0; y < figure.GetLength(1); y++)
            {
                for (int x = 0; x < figure.GetLength(2); x++)
                {
                    if(figure[rot,y,x] != 0)
                    {
                        GameObject go = Instantiate(ingame.BlockPrefab, transform.position, Quaternion.identity) as GameObject;
                        go.transform.SetParent(this.transform);
                        go.transform.localPosition = new Vector3(-(0.1f * ((figure.GetLength(2) + 1) & 1) + 0.2f * (-figure.GetLength(2) / 2 + x)),
                                        0.1f * ((figure.GetLength(1) + 1) & 1) + 0.2f * (-figure.GetLength(1) / 2 + y),
                                        0);
                        go.transform.localRotation = Quaternion.identity;
                        Block s = go.GetComponent<Block>();
                        s.SetColor(color);
                        blocks.Add(go);
                        //                    PlayfieldObj[y * PlayfieldWidth + x] = go;
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
        return figures.getFigure(next);
    }
    public int getColor()
    {
        return color;
    }
    public int getRotation()
    {
        return rot;
    }
}
