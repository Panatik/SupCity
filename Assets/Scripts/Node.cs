using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Node cameFrom;
    public List<Node> connections = new List<Node>();

    public float gScore;
    public float hScore;

    // Ajout d'un booléen pour bloquer le node
    //public bool isBlocked = false;
    public bool IsBlocked { get; private set; } = false;
    private int blockCount = 0;

    public float FScore() 
    {
        return gScore + hScore;
    }

    public void AddBlocker()
    {
        IsBlocked = true;
    }

    public void RemoveBlocker()
    {
        IsBlocked = false;
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
