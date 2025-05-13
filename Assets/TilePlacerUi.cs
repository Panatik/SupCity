using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class TilePlacerUi : MonoBehaviour
{
    public Tilemap targetTilemap; // Tilemap sur laquelle le joueur place les tuiles
    public List<TileBase> placeableTiles; // Tiles disponibles � placer (ex: herbe, sol d�coratif, etc.)
    public List<TileBase> wallTiles; // Tiles consid�r�es comme mur (� ne pas �craser)

    private TileBase selectedTile;

    void Start()
    {
        if (placeableTiles.Count > 0)
        {
            selectedTile = placeableTiles[0]; // Tuile par d�faut
        }
    }

    public void SelectTile(int index)
    {
        if (index >= 0 && index < placeableTiles.Count)
        {
            selectedTile = placeableTiles[index];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && selectedTile != null)
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPos = targetTilemap.WorldToCell(mouseWorldPos);

            TileBase currentTile = targetTilemap.GetTile(cellPos);

            // V�rifie qu�on ne place pas sur une walltile
            if (!wallTiles.Contains(currentTile))
            {
                targetTilemap.SetTile(cellPos, selectedTile);
            }
        }
    }
}
