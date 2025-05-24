using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    [Header("Configuration")]
    public int maxOccupants = 2; // Nombre de PNJ maximum selon le niveau de la maison

    [Header("Entrée de la maison")]
    public Transform entrancePoint; // Point vers lequel les PNJ se déplacent (placer un empty object devant la porte)

    private List<PNJ> occupants = new List<PNJ>();

    // Vérifie s'il reste de la place pour un nouveau PNJ
    public bool HasSpace()
    {
        return occupants.Count < maxOccupants;
    }

    // Ajoute un occupant (appelé quand un PNJ entre dans la maison)
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

        // Par défaut, retourne la position de la maison elle-même
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
