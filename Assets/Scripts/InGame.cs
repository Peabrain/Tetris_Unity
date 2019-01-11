using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGame : MonoBehaviour
{
    // object ingame needed from the unity engine
    public GameObject BlockPrefab;
    public NextTile NextTile = null;
    public Text ScoreText = null;
    public Text NameText = null;
    public GameObject GameOverObj = null;
    public GameObject PauseText = null;


    Random rand = new Random();

    int PlayfieldWidth = 11;
    int PlayfieldHeight = 20;

    // array for saving the temp GameObjects
    GameObject[] PlayfieldObj = null;
    // in playfield stores the actual block screen
    // 0 for empty
    // 1 .. 7 colored blocks 
    int[] Playfield = null;

    int tilePosX = 0;
    int tilePosY = 0;
    int tileRot = 0;
    int tileColor = 1;

    enum CollisionTypes {none,wall,stone};

    // actual tile
    int[,,] tile = null;

    // auto-falling-down-autofalling_timer
    float autofalling_timer = 0.0f;
    float autofalling_speed = 0.5f;

    int Score = 0;

    // states of the game
    public enum STATE {wait,running,gameover};
    STATE state = STATE.wait;

    static float keyspeed = 0.15f;
    key left = new key(KeyCode.LeftArrow, keyspeed);
    key right= new key(KeyCode.RightArrow, keyspeed);
    key down = new key(KeyCode.DownArrow, keyspeed / 2);
    key up = new key(KeyCode.UpArrow, 0);

    // defines how many tiles to set before speed up
    int speedup = 10;
    int speedup_counter = 0;
    float speedup_factor = 0.9f;

    // Start is called before the first frame update
    void Start()
    {
        GameOverObj.SetActive(false);

        // create the blocks and init the playfield
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

        // init the randomizer for next tile
        newTile(true);

        updateScore();
        NameText.text = "Player 1";
        state = STATE.running;
        PauseText.SetActive(false);
        GameOverObj.SetActive(false);
    }

    // this is for the renderupdate
    void updatePlayfield()
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

    void setTile(int value)
    {
        for (int y = 0; y < tile.GetLength(1); y++)
        {
            for (int x = 0; x < tile.GetLength(2); x++)
            {
                if (tile[tileRot, y, x] != 0)
                    Playfield[(tilePosY + y) * PlayfieldWidth + tilePosX + x] = value;
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

    void newTile(bool start)
    {
        if(!start)
            crunchPlayfield();
        tile = NextTile.getNext();
        tileColor = NextTile.getColor();
        tilePosY = PlayfieldHeight - tile.GetLength(1);
        tilePosX = PlayfieldWidth / 2 - (tile.GetLength(2)) / 2;
        tileRot = NextTile.getRotation();

        if(checkCollisions(tilePosX,tilePosY,tileRot) != CollisionTypes.none)
        {
            state = STATE.gameover;
            GameOverObj.SetActive(true);
            PauseText.SetActive(true);
        }
        setTile(tileColor);

        NextTile.newNext();
        autofalling_timer = 0;

        speedup_counter++;
        if (speedup_counter == speedup)
        {
            speedup_counter = 0;
            autofalling_speed *= speedup_factor;
        }

    }

    CollisionTypes checkCollisions(int tilePosXnew,int tilePosYnew,int tileRotnew)
    {
        for (int y = 0; y < tile.GetLength(1); y++)
        {
            for (int x = 0; x < tile.GetLength(2); x++)
            {
                if (tile[tileRotnew, y, x] != 0)
                {
                    if (tilePosYnew + y < 0)
                        return CollisionTypes.stone;
                    if (tilePosXnew + x < 0 || tilePosXnew + x >= PlayfieldWidth)
                        return CollisionTypes.wall;
                    if (Playfield[(tilePosYnew + y) * PlayfieldWidth + tilePosXnew + x] != 0)
                        if(tilePosYnew != tilePosY)
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

        // collect deletable columns
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

        // check rows to delete
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

        // crunch rows
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

        // delete deleable columns
        foreach(int x in columnsToClear)
        {
            for (int y = 0; y < PlayfieldHeight; y++)
                Playfield[y * PlayfieldWidth + x] = 0;
        }

        // count scores
        Score += (lineCount + columnsToClear.Count) * 10;
        updateScore();
    }

    void control()
    {
        // save old tilestate
        int tilePosXnew = tilePosX;
        int tilePosYnew = tilePosY;
        int tileRotnew = tileRot;

        // check the keys and get new tilestate
        if (down.checkSignal())
        {
            tilePosYnew = tilePosY - 1;
            autofalling_timer = 0;
        }
        if (left.checkSignal())
            tilePosXnew = tilePosX + 1;
        if (right.checkSignal())
            tilePosXnew = tilePosX - 1;
        if (up.checkSignal())
            tileRotnew = (tileRot + 1) % tile.GetLength(0);

        // update keys
        float delta = Time.deltaTime;
        down.update(delta);
        left.update(delta);
        right.update(delta);
        up.update(delta);

        // auto-falling-down
        if (tilePosYnew == tilePosY)
        {
            if (autofalling_timer >= autofalling_speed)
            {
                tilePosYnew = tilePosY - 1;
                autofalling_timer -= autofalling_speed;
            }
        }
        else
            autofalling_timer = 0.0f;
        autofalling_timer += Time.deltaTime;

        // if tilestate has changed, do something
        if (tilePosXnew != tilePosX || tilePosYnew != tilePosY || tileRotnew != tileRot)
        {
            setTile(0); // clear the actual tile from the playfield

            if (tileRotnew != tileRot)
            {
                if (checkCollisions(tilePosXnew, tilePosYnew, tileRotnew) == CollisionTypes.none)
                    tileRot = tileRotnew;
            }

            if (tilePosXnew != tilePosX || tilePosYnew != tilePosY)
            {
                switch (checkCollisions(tilePosXnew, tilePosYnew, tileRot))
                {
                    case CollisionTypes.none:
                        { 
                            setTile(0); // clear the actual tile from the playfield
                            tilePosX = tilePosXnew;
                            tilePosY = tilePosYnew;
                        }
                        break;
                    case CollisionTypes.wall:
                        {
                        }
                        break;
                    case CollisionTypes.stone:
                        {
                            setTile(tileColor);
                            newTile(false);
                        }
                        break;
                }
            }
            setTile(tileColor);
        }
    }

    void Update()
    {
        // control the gamestates

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
                    autofalling_timer = 0;
                    Score = 0;
                    autofalling_speed = 0.5f;
                    speedup_counter = 0;
                    newTile(true);
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
                control();
                updatePlayfield();
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
