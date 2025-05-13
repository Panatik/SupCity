using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class TilePlacerUi : MonoBehaviour
{
    public Tilemap targetTilemap; // Tilemap sur laquelle le joueur place les tuiles
    public List<TileBase> placeableTiles; // Tiles disponibles à placer (ex: herbe, sol décoratif, etc.)
    public List<TileBase> wallTiles; // Tiles considérées comme mur (à ne pas écraser)

    private TileBase selectedTile;

    void Start()
    {
        if (placeableTiles.Count > 0)
        {
            selectedTile = placeableTiles[0]; // Tuile par défaut
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

            // Vérifie qu’on ne place pas sur une walltile
            if (!wallTiles.Contains(currentTile))
            {
                targetTilemap.SetTile(cellPos, selectedTile);
            }
        }
    }
}
