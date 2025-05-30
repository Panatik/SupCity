using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tree_Controller : Harvestable_Controller {

    const int GROW_TIME = 200;        // value to change
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
        if (curr_tick >= tick_stamp + GROW_TIME)
        {
            grow_tree();
        }
    }
    public void grow_tree()
    {
        is_harvestable = true;
        GetComponentInChildren<SpriteRenderer>().sprite = tree_spr;
    }
    public void cut_tree()
    {
        is_harvestable = false;
        GetComponentInChildren<SpriteRenderer>().sprite = trunk_spr;
        tick_stamp = tick_logic.get_tick();
    }
    [ContextMenu("Cut tree !")]
    public override Dictionary<string, int> harvest() {
        cut_tree();
        return base.harvest();
    }
}
