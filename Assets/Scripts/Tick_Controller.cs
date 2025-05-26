using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tick_Controller : MonoBehaviour
{
    public int current_tick = 0;
    private float curr_time;
    private int tick_multiplier = 10;

    void Start()
    {
        curr_time = Time.time;
    }

    void Update()
    {
        if (Time.time > curr_time + 1)
        {
            current_tick += tick_multiplier;
            curr_time = Time.time;
            print(current_tick);
        }
    }

    public void pause_time()
    {
        tick_multiplier = 0;
    }
    public void set_time_speed(int multi)
    {
        Debug.Assert(multi > 0 && multi < 4);
        tick_multiplier = multi;
    }

    public int get_tick()
    {
        return current_tick;
    }
}
