using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bush_Controller : MonoBehaviour
{
    private bool is_harvestable;
    public Sprite empty_sprite;
    public Sprite[] hable_sprites;
    public Tick_Controller tick_logic;
    private int tick_stamp;
    void Start()
    {
        refill_bush();
        tick_logic = GameObject.FindGameObjectWithTag("Tick").GetComponent<Tick_Controller>();
    }

    public void Update()
    {
        if (is_harvestable)
        {
            return;
        }
        int curr_tick = tick_logic.get_tick();
        if (curr_tick >= tick_stamp + 270)        // 270 seconds
        {
            refill_bush();
        }
        
    }

    public void refill_bush()
    {
        is_harvestable = true;
        int spr_num = Random.Range(0, 3);
        GetComponent<SpriteRenderer>().sprite = hable_sprites[spr_num];
    }
    [ContextMenu("harvest bush")]
    public void harvest_bush()
    {
        is_harvestable = false;
        GetComponent<SpriteRenderer>().sprite = empty_sprite;
        tick_stamp = tick_logic.get_tick();
    }
}
