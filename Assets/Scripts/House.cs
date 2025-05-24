using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    [Header("Configuration")]
    public int maxOccupants = 2; // Nombre de PNJ maximum selon le niveau de la maison

    [Header("Entr�e de la maison")]
    public Transform entrancePoint; // Point vers lequel les PNJ se d�placent (placer un empty object devant la porte)

    private List<PNJ> occupants = new List<PNJ>();

    // V�rifie s'il reste de la place pour un nouveau PNJ
    public bool HasSpace()
    {
        return occupants.Count < maxOccupants;
    }

    // Ajoute un occupant (appel� quand un PNJ entre dans la maison)
    public void AddOccupant(PNJ pnj)
    {
        if (!occupants.Contains(pnj) && HasSpace())
        {
            occupants.Add(pnj);
        }
    }

    // Retourne la position vers laquelle le PNJ doit aller
    public Vector3 GetEntrancePosition()
    {
        if (entrancePoint != null)
            return entrancePoint.position;

        // Par d�faut, retourne la position de la maison elle-m�me
        return transform.position;
    }

    // (Optionnel) Supprimer un PNJ si jamais il part
    public void RemoveOccupant(PNJ pnj)
    {
        if (occupants.Contains(pnj))
        {
            occupants.Remove(pnj);
        }
    }

    // (Optionnel) Retourne la liste des occupants
    public List<PNJ> GetOccupants()
    {
        return occupants;
    }
}
