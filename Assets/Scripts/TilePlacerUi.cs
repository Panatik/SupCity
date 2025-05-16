using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class TilePlacerUi : MonoBehaviour
{
    public Tilemap targetTilemap; // Tilemap sur laquelle le joueur place les tuiles
    public List<TileBase> placeableTiles; // Tiles disponibles à placer (ex: herbe, sol décoratif, etc.)
    public List<TileBase> wallTiles; // Tiles considérées comme mur (à ne pas écraser)

    private TileBase selectedTile = null;
    private GameObject selectedObject = null;

    public GameObject[] placeableObjects;

    public GameObject[] previewObjects; // Les versions preview (une par object)
    private GameObject currentPreview;  // L’instance actuellement affichée

    public void SelectObject(int objectIndex)
    {
        Debug.Log("Objet : " + selectedObject);
        selectedTile = null; // On désélectionne la tuile si une était choisie

        // Détruire l’ancienne preview si elle existe
        if (currentPreview != null)
            Destroy(currentPreview);

        if (objectIndex >= 0 && objectIndex < placeableObjects.Length)
        {
            selectedObject = placeableObjects[objectIndex];

            // Créer une nouvelle preview
            currentPreview = Instantiate(previewObjects[objectIndex]);
        }
    }
    public void SelectTile(int index)
    {
        selectedObject = null;
        if (index >= 0 && index < placeableTiles.Count)
        {
            selectedTile = placeableTiles[index];
        }
    }

    bool CanPlaceAt(Vector3Int cellPos)
    {
        BoundsInt bounds = new BoundsInt(cellPos.x, cellPos.y, 0, 2, 2, 1); // 2x2 si maison 2x2

        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            TileBase tile = targetTilemap.GetTile(pos);
            Debug.Log($"Tile at {pos} = {tile}");
            if (tile == null || wallTiles.Contains(tile))
            {
                return false;
            }
        }

        return true;
    }


    // Update is called once per frame
    void Update()
    {
        // Calcule la position de l'intersection la plus proche
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        float cellSize = targetTilemap.cellSize.x; // Supposé carré

        // Snap la position à l’intersection la plus proche
        float snappedX = Mathf.Round(mouseWorldPos.x / cellSize) * cellSize;
        float snappedY = Mathf.Round(mouseWorldPos.y / cellSize) * cellSize;
        Vector3 intersectionPos = new Vector3(snappedX, snappedY, 0f);

        // Preview qui suit la souris
        if (currentPreview != null)
        {
            currentPreview.transform.position = intersectionPos;
            // Décalage vertical pour centrer visuellement (adapter selon le sprite)
            currentPreview.transform.position -= new Vector3(0f, cellSize / 2f, 0f);
        }

        if (Input.GetMouseButtonDown(0) && selectedTile != null)
        {
            //Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPos = targetTilemap.WorldToCell(mouseWorldPos);

            TileBase currentTile = targetTilemap.GetTile(cellPos);

            // Vérifie qu’on ne place pas sur une walltile
            if (!wallTiles.Contains(currentTile) && currentTile != null && !placeableTiles.Contains(currentTile))
            {
                targetTilemap.SetTile(cellPos, selectedTile);
            }
        }

        if (selectedObject != null && Input.GetMouseButtonDown(0))
        {
            //Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0f;

            Vector3Int cellPos = targetTilemap.WorldToCell(mouseWorldPos);

            //Vector3Int baseCell = new Vector3Int(cellPos.x, cellPos.y, 0);
            Vector3Int baseCell = targetTilemap.WorldToCell(intersectionPos);
            baseCell.x -= 1;
            baseCell.y -= 1;

            // Convertir en coin inférieur gauche du bloc de 4 tuiles
            //Vector3 intersectionPos = targetTilemap.CellToWorld(baseCell) + new Vector3(1f, 1f, 0f);


            //Vector3Int cellPos = targetTilemap.WorldToCell(mouseWorldPos);
            //Vector3 worldSnapPos = targetTilemap.GetCellCenterWorld(cellPos); // aligné à la tile

            if (CanPlaceAt(baseCell))
            {
                Instantiate(selectedObject, intersectionPos, Quaternion.identity);
                // Optional: hide preview while placed
                currentPreview.SetActive(false);
            }
            else
            {
                Debug.Log("Tu ne peux pas placer ici !");
            }
        }
    }
}
