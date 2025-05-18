using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanNeeds
{
    public float hunger;
    public float maxHunger;

    public HumanNeeds(float maxHunger)
    {
        this.maxHunger = maxHunger;
        this.hunger = maxHunger;
    }

    public void UpdateHunger(float rate)
    {
        hunger -= rate * Time.deltaTime;
        hunger = Mathf.Clamp(hunger, 0f, maxHunger);
    }
    public void Eat(float amount)
    {
        hunger += amount;
        hunger = Mathf.Clamp(hunger, 0f, maxHunger);
    }
}
