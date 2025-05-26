using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tree_Controller : MonoBehaviour
{
    private bool is_harvestable;
    public Sprite trunk_spr;
    public Sprite tree_spr;
    public Tick_Controller tick_logic;
    private int tick_stamp;
    void Start()
    {
        grow_tree();
        tick_logic = GameObject.FindGameObjectWithTag("Tick").GetComponent<Tick_Controller>();
    }

    void Update()
    {
        if (is_harvestable)
        {
            return;
        }
        int curr_tick = tick_logic.get_tick();
        if (curr_tick >= tick_stamp + 55500)  // value to change
        {
            grow_tree();
        }
    }
    public void grow_tree()
    {
        is_harvestable = true;
        GetComponent<SpriteRenderer>().sprite = tree_spr;
    }
    [ContextMenu("cut tree")]
    public void cut_tree()
    {
        is_harvestable = false;
        GetComponent<SpriteRenderer>().sprite = trunk_spr;
        tick_stamp = tick_logic.get_tick();
    }
}
