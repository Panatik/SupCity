using UnityEngine;
using System.Collections.Generic;

public class QuestLoader : MonoBehaviour
{
    /*public Sprite campfireIcon;
    public Sprite treeIcon;
    public Sprite houseIcon;*/

    void Start()
    {
        QuestManager.Instance.ClearQuests();

        List<QuestData> allQuests = new List<QuestData>
        {
            new QuestData
            {
                id = "build_campfire",
                title = "Allumer un feu de camp",
                description = "Construis ton premier feu de camp",
                rewards = "abris",
                //icon = campfireIcon,
                goal = 1
            },
            new QuestData
            {
                id = "cut_tree",
                title = "Couper un arbre",
                description = "Abats ton premier arbre",
                rewards = "routes",
                //icon = treeIcon,
                goal = 1
            },
            new QuestData
            {
                id = "build_house",
                title = "Construire une maison",
                description = "�rige ta premi�re maison",
                rewards = "baies, buissons de baies, cueilleurs de baies",
                //icon = houseIcon,
                goal = 1
            },

            new QuestData
            {
                id="build_berry_picker",
                title = "Construire un cueilleur de baies",
                description = "Oh ces adorables berries, apparemment elles nous rendraient plus rapides...",
                rewards = "eau et cabane de porteur d'eau",
                //icon = berryIcon,
                goal = 1
            },

            new QuestData
            {
                id="build_waterkworker_hut",
                title = "Construire une cabane � eau",
                description = "Boire est essentiel pour survivre ! Il est donc naturel de rendre l'acc�s � l'eau ais�.",
                rewards = "bois, f�rets, cabane pour b�cherons",
                //icon = berryIcon,
                goal = 1
            },

            new QuestData
            {
                id="build_wood_cutter",
                title = "Construire un b�timent pour b�cheron",
                description = "Le bois est une ressource si utile nous devrions commencer � en produire!",
                rewards = "recherche sur l�eau, b�timent laboratoire, recherche agricole, recherche en menuiserie",
                //icon = berryIcon,
                goal = 1
            },

            new QuestData
            {
                id="build_thinking_range",
                title = "Construire un b�timent de recherche",
                description = "Un b�timent de recherche permettrait de faire des recherches sur les ressources qui nous entourent",
                rewards = "recherche, artisannat du bois", //parchemins
                //icon = berryIcon,
                goal = 1
            },

            new QuestData
            {
                id="research_stone",
                title = "Faire des recherches sur la pierre",
                description = "L'�ge de pierre a commenc�!",
                rewards = "stones",
                //icon = berryIcon,
                goal = 1
            },
            new QuestData
            {
                id="research_masonery",
                title = "Faires des recherches sur la ma�onnerie",
                description = "Faisons des recherches sur la ma�onnerie afin d'acc�der � de nouvelles �les!",
                rewards = "briques",
                //icon = berryIcon,
                goal = 1
            },
            new QuestData
            {
                id="store_bread",
                title = "Avoir 50 pains stock�s",
                description = "Un bon stock de nourriture permettrait d'avoir de la stabilit�",
                rewards = "am�lioration upgrades",
                //icon = berryIcon,
                goal = 1
            },
            new QuestData
            {
                id="store_fish",
                title = "Avoir 50 poissons en stock",
                description = "Un bon stock de nourriture permettrait d'avoir de la stabilit�",
                rewards = "am�lioration pour la p�che",
                //icon = berryIcon,
                goal = 1
            },
            new QuestData
            {
                id="store_5_tools",
                title = "Stock� 5 outils en pierre",
                description = "Produire des outils nous aiderait � nous am�liorer",
                rewards = "am�liorations pour le tailleur de pierre et le fabricant d�outils en pierre",
                //icon = berryIcon,
                goal = 1
            },
        };

        foreach (var quest in allQuests)
            QuestManager.Instance.allQuests.Add(quest);
        QuestManager.Instance.ShowNextQuests();
    }
}
