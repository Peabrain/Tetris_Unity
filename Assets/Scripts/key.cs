using UnityEngine;

class key
{
    bool pressed;
    float repeat;
    float timer;
    KeyCode code;

    public key(KeyCode code_, float repeat_)
    {
        code = code_;
        repeat = repeat_;
    }

    public void clear()
    {
        pressed = false;
        timer = 0;
    }

    public bool checkSignal()
    {
        if (Input.GetKeyDown(code))
        {
            timer = 0;
            pressed = true;
        }
        else
        if (Input.GetKeyUp(code))
            pressed = false;

        if (pressed && repeat == 0)
        {
            pressed = false;
            return true;
        }
        else
        if (pressed && ((timer >= repeat) || (timer == 0)))
        {
            if (timer > 0)
                timer -= repeat;
            return true;
        }

        return false;
    }

    public void update(float t)
    {
        timer += t;
    }
}
