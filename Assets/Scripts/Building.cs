using static UnityEditor.Progress;
using System.Collections.Generic;
using UnityEngine;

public class Batiment : MonoBehaviour
{
    public string nomBatiment;
    public int maxOuvriers = 3;
    public List<PNJ> ouvriers = new List<PNJ>();


    //public List<PNJ> GetPNJ() { /* TO DO */ }
    //public List<Item> GetTools() { /* TO DO */ }
    //public List<Item> GetRessourcesInput() { /* TO DO */ }
    //public List<Item> GetRessourcesOutput() { /* TO DO */ }

    public bool PeutAccepterOuvrier()
    {
        return ouvriers.Count < maxOuvriers;
    }

    public void AjouterOuvrier(PNJ pnj)
    {
        if (!ouvriers.Contains(pnj) && PeutAccepterOuvrier())
        {
            ouvriers.Add(pnj);
            Debug.Log($"{pnj.name} assigné à {name}");
        }
    }

    public void RetirerOuvrier(PNJ pnj)
    {
        if (ouvriers.Contains(pnj))
        {
            ouvriers.Remove(pnj);
        }
    }

    private void OnMouseDown()
    {
        BuildingUI.Instance.AfficherInfosBatiment(this);
    }
}
