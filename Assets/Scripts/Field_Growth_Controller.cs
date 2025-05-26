using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Field_Growth_Controller : MonoBehaviour
{
    const int GROW_TIME = 200;
    private bool is_harvestable;
    public Sprite[] sprites;
    public Tick_Controller tick_controller;
    private int tick_stamp;
    private int grow_state = 0;

    void Start()
    {
        tick_controller = GameObject.FindGameObjectWithTag("Tick").GetComponent<Tick_Controller>();
    }

    void Update()
    {
        if (is_harvestable)
        {
            return;
        }
        int curr_tick = tick_controller.get_tick();
        print(sprites[0]);
        print(GROW_TIME / sprites.Length);
        if (curr_tick > tick_stamp + GROW_TIME/sprites.Length)
        {
            grow_field();
        }
    }
    public void grow_field()
    {
        tick_stamp = tick_controller.get_tick();
        grow_state++;
        GetComponent<SpriteRenderer>().sprite = sprites[grow_state];
        if (grow_state == sprites.Length - 1)
        {
            is_harvestable = true;
        }

    }
    [ContextMenu("Harvest field !")]
    public void harvest_field()
    {
        is_harvestable = false;
        GetComponent<SpriteRenderer>().sprite = sprites[0];
        tick_stamp = tick_controller.get_tick();
    }
}
