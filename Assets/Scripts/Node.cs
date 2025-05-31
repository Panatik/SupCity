using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Node cameFrom;
    public List<Node> connections = new List<Node>();

    public float gScore;
    public float hScore;

    public bool isRoute = false; // coche cette case dans l'inspecteur pour les routes
    [HideInInspector]
    public List<Node> voisins = new List<Node>();

    // Ajout d'un booléen pour bloquer le node
    //public bool isBlocked = false;
    public bool IsBlocked => blockCount > 0;
    public int blockCount = 0;

    public void Start()
    {
        TrouverVoisins();
    }

    public float FScore() 
    {
        return gScore + hScore;
    }

    public void AddBlocker()
    {
        blockCount++;
    }

    public void RemoveBlocker()
    {
        blockCount = Mathf.Max(0, blockCount - 1);
    }

    public void TrouverVoisins()
    {
        voisins.Clear();
        Vector2[] directions = {
            Vector2.up, Vector2.down, Vector2.left, Vector2.right
        };

        foreach (var dir in directions)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position + (Vector3)dir, Vector2.zero, 0.1f);
            if (hit.collider != null)
            {
                Node voisin = hit.collider.GetComponent<Node>();
                if (voisin != null && voisin.isRoute)
                {
                    voisins.Add(voisin);
                }
            }
        }
    }

    public List<Node> GetVoisins()
    {
        return voisins;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = IsBlocked ? Color.red : Color.blue;

        if (connections != null)
        {
            foreach (var conn in connections)
            {
                Gizmos.DrawLine(transform.position, conn.transform.position);
            }
        }

        Gizmos.DrawCube(transform.position, Vector3.one * 0.2f);
    }
}
