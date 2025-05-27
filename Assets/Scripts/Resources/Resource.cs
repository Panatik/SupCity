using UnityEngine;

public class Resource
{
    private int quantity = 0;
    private int capacity = 0;
    private string name;

    public void set_name(string n) {
        name = n;
    }
    public void set_capacity(int c) {
        capacity = c;
    }

    public void increase_quantity(int quant) {
        Debug.Assert(quantity + quant <= capacity);
        quantity += quant;
    }

    public void decrese_quantity(int quant) {
        Debug.Assert(quantity - quant >= 0);
        quantity -= quant;
    }

    public int get_quantity() {
        return quantity;
    }

    public string getName() {
        return name;
    }
}
