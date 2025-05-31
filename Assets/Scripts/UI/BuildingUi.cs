using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildingUI : MonoBehaviour
{
    public static BuildingUI Instance;

    public GameObject panel;
    public TMP_Text titre;

    public Transform sectionOuvriers;
    public Transform sectionOutils;
    public Transform sectionEntrants;
    public Transform sectionSortants;

    public GameObject infoSlotPrefab;

    private void Awake()
    {
        Instance = this;
        panel.SetActive(false);
    }

    public void AfficherInfosBatiment(Batiment bat)
    {
        panel.SetActive(true);
        titre.text = bat.nomBatiment;

        // Nettoie les anciennes infos
        ClearSection(sectionOuvriers);
        ClearSection(sectionOutils);
        ClearSection(sectionEntrants);
        ClearSection(sectionSortants);

        // Ouvriers
        foreach (var pnj in bat.GetOuvriers())
        {
            AjouterInfo(sectionOuvriers, pnj.nom, pnj.sprite);
        }

        // Outils
        foreach (var outil in bat.GetOutilsUtilisés())
        {
            AjouterInfo(sectionOutils, outil.nom, outil.sprite, outil.quantité);
        }

        // Ressources entrantes
        foreach (var res in bat.GetRessourcesEntrantes())
        {
            AjouterInfo(sectionEntrants, res.nom, res.sprite, res.quantité);
        }

        // Ressources sortantes
        foreach (var res in bat.GetRessourcesSortantes())
        {
            AjouterInfo(sectionSortants, res.nom, res.sprite, res.quantité);
        }
    }

    void ClearSection(Transform section)
    {
        foreach (Transform child in section)
        {
            Destroy(child.gameObject);
        }
    }

    void AjouterInfo(Transform parent, string nom, Sprite image, int quantité = 1)
    {
        GameObject slot = Instantiate(infoSlotPrefab, parent);
        slot.transform.GetChild(0).GetComponent<Image>().sprite = image;
        slot.transform.GetChild(1).GetComponent<TMP_Text>().text = $"{nom} x{quantité}";
    }

    public void Fermer()
    {
        panel.SetActive(false);
    }
}
