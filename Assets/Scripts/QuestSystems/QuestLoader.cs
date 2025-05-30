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
                description = "Érige ta première maison",
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
                title = "Construire une cabane à eau",
                description = "Boire est essentiel pour survivre ! Il est donc naturel de rendre l'accès à l'eau aisé.",
                rewards = "bois, fôrets, cabane pour bûcherons",
                //icon = berryIcon,
                goal = 1
            },

            new QuestData
            {
                id="build_wood_cutter",
                title = "Construire un bâtiment pour bûcheron",
                description = "Le bois est une ressource si utile nous devrions commencer à en produire!",
                rewards = "recherche sur l’eau, bâtiment laboratoire, recherche agricole, recherche en menuiserie",
                //icon = berryIcon,
                goal = 1
            },

            new QuestData
            {
                id="build_thinking_range",
                title = "Construire un bâtiment de recherche",
                description = "Un bâtiment de recherche permettrait de faire des recherches sur les ressources qui nous entourent",
                rewards = "recherche, artisannat du bois", //parchemins
                //icon = berryIcon,
                goal = 1
            },

            new QuestData
            {
                id="research_stone",
                title = "Faire des recherches sur la pierre",
                description = "L'âge de pierre a commencé!",
                rewards = "stones",
                //icon = berryIcon,
                goal = 1
            },
            new QuestData
            {
                id="research_masonery",
                title = "Faires des recherches sur la maçonnerie",
                description = "Faisons des recherches sur la maçonnerie afin d'accéder à de nouvelles îles!",
                rewards = "briques",
                //icon = berryIcon,
                goal = 1
            },
            new QuestData
            {
                id="store_bread",
                title = "Avoir 50 pains stockés",
                description = "Un bon stock de nourriture permettrait d'avoir de la stabilité",
                rewards = "amélioration upgrades",
                //icon = berryIcon,
                goal = 1
            },
            new QuestData
            {
                id="store_fish",
                title = "Avoir 50 poissons en stock",
                description = "Un bon stock de nourriture permettrait d'avoir de la stabilité",
                rewards = "amélioration pour la pêche",
                //icon = berryIcon,
                goal = 1
            },
            new QuestData
            {
                id="store_5_tools",
                title = "Stocké 5 outils en pierre",
                description = "Produire des outils nous aiderait à nous améliorer",
                rewards = "améliorations pour le tailleur de pierre et le fabricant d’outils en pierre",
                //icon = berryIcon,
                goal = 1
            },
        };

        foreach (var quest in allQuests)
            QuestManager.Instance.allQuests.Add(quest);
        QuestManager.Instance.ShowNextQuests();
    }
}
