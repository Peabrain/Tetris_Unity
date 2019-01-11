using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGame : MonoBehaviour
{
    // 
    public GameObject BlockPrefab;
    public NextFigure nextfigure = null;
    public Text ScoreText = null;
    public Text NameText = null;
    public GameObject GameOverObj = null;
    public GameObject PauseText = null;


    int PlayfieldWidth = 11;
    int PlayfieldHeight = 20;

    GameObject[] PlayfieldObj = null;
    int[] Playfield = null;

    int figurePosX = 0;
    int figurePosY = 0;
    int figureRot = 0;
    int figureColor = 1;

    Random rand = new Random();

    enum CollisionTypes {none,wall,stone};

    int[,,] figure = null;

    float timer = 0.0f;
    int Score = 0;

    public enum STATE {wait,running,gameover};
    STATE state = STATE.wait;

    float stonespeed = 0.5f;
    static float keyspeed = 0.15f;
    key left = new key(KeyCode.LeftArrow, keyspeed);
    key right= new key(KeyCode.RightArrow, keyspeed);
    key down = new key(KeyCode.DownArrow, keyspeed / 2);
    key up = new key(KeyCode.UpArrow, 0);

    int speedup = 10;
    int speedup_counter = 0;

    // Start is called before the first frame update
    void Start()
    {
        GameOverObj.SetActive(false);
        PlayfieldObj = new GameObject[PlayfieldWidth * PlayfieldHeight];
        Playfield = new int[PlayfieldWidth * PlayfieldHeight];
        for (int y = 0;y < PlayfieldHeight; y++)
        {
            for (int x = 0; x < PlayfieldWidth; x++)
            {
                GameObject go = Instantiate(BlockPrefab, transform.position, Quaternion.identity) as GameObject;
                go.transform.SetParent(this.transform);
                go.transform.localPosition = new Vector3(-(0.1f * ((PlayfieldWidth + 1) & 1) + 0.2f * (-PlayfieldWidth / 2 + x)),
                                0.1f * ((PlayfieldHeight + 1) & 1) + 0.2f * (-PlayfieldHeight / 2 + y),
                                0);
                go.transform.localRotation = Quaternion.identity;
                Block s = go.GetComponent<Block>();
                PlayfieldObj[y * PlayfieldWidth + x] = go;
                Playfield[y * PlayfieldWidth + x] = 0;
            }
        }
        newFigure(true);
        updateScore();
        NameText.text = "Player 1";
        state = STATE.running;
        PauseText.SetActive(false);
        GameOverObj.SetActive(false);
    }

    void UpdatePlayfield()
    {
        for (int y = 0; y < PlayfieldHeight; y++)
        {
            for (int x = 0; x < PlayfieldWidth; x++)
            {
                Block s = PlayfieldObj[y * PlayfieldWidth + x].GetComponent<Block>();
                s.SetColor(Playfield[y * PlayfieldWidth + x]);
            }
        }
    }

    void setFigure(int value)
    {
        for (int y = 0; y < figure.GetLength(1); y++)
        {
            for (int x = 0; x < figure.GetLength(2); x++)
            {
                if (figure[figureRot, y, x] != 0)
                    Playfield[(figurePosY + y) * PlayfieldWidth + figurePosX + x] = value;
            }
        }
    }

    void clearPlayfield()
    {
        for (int y = 0; y < PlayfieldHeight; y++)
        {
            for (int x = 0; x < PlayfieldWidth; x++)
            {
                Playfield[y * PlayfieldWidth + x] = 0;
            }
        }
    }

    void newFigure(bool start)
    {
        if(!start)
            crunchPlayfield();
        figure = nextfigure.getNext();
        figureColor = nextfigure.getColor();
        figurePosY = PlayfieldHeight - figure.GetLength(1);
        figurePosX = PlayfieldWidth / 2 - (figure.GetLength(2)) / 2;
        figureRot = nextfigure.getRotation();

        if(checkCollisions(figurePosX,figurePosY,figureRot) != CollisionTypes.none)
        {
            state = STATE.gameover;
            GameOverObj.SetActive(true);
            PauseText.SetActive(true);
        }
        setFigure(figureColor);

        nextfigure.newNext();
        timer = 0;

        speedup_counter++;
        if (speedup_counter == speedup)
        {
            speedup_counter = 0;
            stonespeed *= 0.9f;
        }

    }

    CollisionTypes checkCollisions(int figurePosXnew,int figurePosYnew,int figureRotnew)
    {
        for (int y = 0; y < figure.GetLength(1); y++)
        {
            for (int x = 0; x < figure.GetLength(2); x++)
            {
                if (figure[figureRotnew, y, x] != 0)
                {
                    if (figurePosYnew + y < 0)
                        return CollisionTypes.stone;
                    if (figurePosXnew + x < 0 || figurePosXnew + x >= PlayfieldWidth)
                        return CollisionTypes.wall;
                    if (Playfield[(figurePosYnew + y) * PlayfieldWidth + figurePosXnew + x] != 0)
                        if(figurePosYnew != figurePosY)
                            return CollisionTypes.stone;
                        else
                            return CollisionTypes.wall;
                }
            }
        }

        return CollisionTypes.none;
    }

    void updateScore()
    {
        ScoreText.text = Score.ToString();
    }

    void crunchPlayfield()
    {
        Score++;
        int lineCount = 0;

        List<int> columnsToClear = new List<int>();
        for (int x = 0; x < PlayfieldWidth; x++)
        {
            int clear_column_counter = 0;
            for (int y = 0; y < PlayfieldHeight; y++)
            {
                if (Playfield[y * PlayfieldWidth + x] > 0)
                    clear_column_counter++;
            }
            if (clear_column_counter == PlayfieldHeight)
                columnsToClear.Add(x);
        }


        for (int y = 0; y < PlayfieldHeight; y++)
        {
            int countBlocks = 0;
            for (int x = 0; x < PlayfieldWidth; x++)
            {
                if (Playfield[y * PlayfieldWidth + x] != 0)
                    countBlocks++;
            }
            if (countBlocks == PlayfieldWidth)
            {
                lineCount++;
                for (int x = 0; x < PlayfieldWidth; x++)
                    Playfield[y * PlayfieldWidth + x] = -1;
            }
        }

        for (int x = 0; x < PlayfieldWidth; x++)
        {
            int y_up = 0;
            for (int y = 0; y < PlayfieldHeight; y++)
            {
                int value = Playfield[y * PlayfieldWidth + x];
                Playfield[y * PlayfieldWidth + x] = 0;
                if (value != -1)
                {
                    Playfield[y_up * PlayfieldWidth + x] = value;
                    y_up++;
                }
            }
        }

        foreach(int x in columnsToClear)
        {
            for (int y = 0; y < PlayfieldHeight; y++)
                Playfield[y * PlayfieldWidth + x] = 0;
        }

        Score += (lineCount + columnsToClear.Count) * 10;
        updateScore();
    }

    void Control()
    {
        int figurePosXnew = figurePosX;
        int figurePosYnew = figurePosY;
        int figureRotnew = figureRot;

        if (down.checkSignal())
        {
            figurePosYnew = figurePosY - 1;
            timer = 0;
        }
        if (left.checkSignal())
            figurePosXnew = figurePosX + 1;
        if (right.checkSignal())
            figurePosXnew = figurePosX - 1;
        if (up.checkSignal())
            figureRotnew = (figureRot + 1) % figure.GetLength(0);

        float delta = Time.deltaTime;
        down.update(delta);
        left.update(delta);
        right.update(delta);
        up.update(delta);

        if (figurePosYnew == figurePosY)
        {
            if (timer >= stonespeed)
            {
                figurePosYnew = figurePosY - 1;
                timer -= stonespeed;
            }
        }
        else
            timer = 0.0f;
        timer += Time.deltaTime;


        if (figurePosXnew != figurePosX || figurePosYnew != figurePosY || figureRotnew != figureRot)
        {
            setFigure(0);

            if (figureRotnew != figureRot)
            {
                if (checkCollisions(figurePosXnew, figurePosYnew, figureRotnew) == CollisionTypes.none)
                    figureRot = figureRotnew;
            }

            if (figurePosXnew != figurePosX || figurePosYnew != figurePosY)
            {
                switch (checkCollisions(figurePosXnew, figurePosYnew, figureRot))
                {
                    case CollisionTypes.none:
                        { 
                            setFigure(0);
                            figurePosX = figurePosXnew;
                            figurePosY = figurePosYnew;
                        }
                        break;
                    case CollisionTypes.wall:
                        {
                        }
                        break;
                    case CollisionTypes.stone:
                        {
                            setFigure(figureColor);
                            newFigure(false);
                        }
                        break;
                }
            }
            setFigure(figureColor);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
        switch (state)
        {
            case STATE.gameover:
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    state = STATE.running;
                    GameOverObj.SetActive(false);
                    PauseText.SetActive(false);
                    clearPlayfield();
                    timer = 0;
                    Score = 0;
                    stonespeed = 0.5f;
                    speedup_counter = 0;
                    newFigure(true);
                    updateScore();
                    down.clear();
                    left.clear();
                    right.clear();
                    up.clear();
                }
            }break;
            case STATE.running:
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    PauseText.SetActive(true);
                    state = STATE.wait;
                    return;
                }
                Control();
                UpdatePlayfield();
            }break;
            case STATE.wait:
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    PauseText.SetActive(false);
                    state = STATE.running;
                }
            }break;
        }
    }

    public STATE getState()
    {
        return state;
    }
    public int getScore()
    {
        return Score;
    }
}
