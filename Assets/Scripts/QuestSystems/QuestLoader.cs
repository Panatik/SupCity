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
                id = 3,
                title = "Allumer un feu de camp",
                description = "Construis ton premier feu de camp",
                rewards = "Récompenses : abris",
                //icon = campfireIcon,
                goal = 1
            },
            new QuestData
            {
                id = 4,
                title = "Construire huit maisons",
                description = "Érige tes premières maisons",
                rewards = "Récompenses : routes",
                //icon = houseIcon,
                goal = 1
            },

            new QuestData
            {
                id=5,
                title = "Place des routes !",
                description = "Placer 4 routes pour réussir la mission",
                rewards = "Récompenses : buisson de baies, bâtiment cueilleur de baies (angar rouge), graines",
                //icon = berryIcon,
                goal = 1
            },

            new QuestData
            {
                id=6,
                title = "Construire un cueilleur de baies",
                description = "Oh ces adorables berries, apparemment elles nous rendraient plus rapides...",
                rewards = "Récompenses : bois, fôrets, cabane pour bûcherons (angar bleu)",
                //icon = berryIcon,
                goal = 1
            },

            new QuestData
            {
                id=7,
                title = "Construire un bâtiment pour bûcheron",
                description = "Le bois est une ressource si utile nous devrions commencer à en produire!",
                rewards = "bâtiment pour outils (angar vert)",
                //icon = berryIcon,
                goal = 1
            },

            new QuestData
            {
                id=8,
                title = "Construire un bâtiment pour les outils",
                description = "Un bâtiment pour construire des outils seraient pratique",
                rewards = "bâtiment vêtements", //parchemins
                //icon = berryIcon,
                goal = 1
            },

            new QuestData
            {
                id=9,
                title = "Construire le bâtiment pour vêtements",
                description = "Parce que même à l'âge de pierre faut bien être fashion!",
                rewards = "les roches",
                //icon = berryIcon,
                goal = 1
            },
        };

        foreach (var quest in allQuests)
            QuestManager.Instance.allQuests.Add(quest);
        QuestManager.Instance.ShowNextQuests();
    }
}
