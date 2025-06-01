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
                rewards = "R�compenses : abris",
                //icon = campfireIcon,
                goal = 1
            },
            new QuestData
            {
                id = 4,
                title = "Construire huit maisons",
                description = "�rige tes premi�res maisons",
                rewards = "R�compenses : routes",
                //icon = houseIcon,
                goal = 1
            },

            new QuestData
            {
                id=5,
                title = "Place des routes !",
                description = "Placer 4 routes pour r�ussir la mission",
                rewards = "R�compenses : buisson de baies, b�timent cueilleur de baies (angar rouge), graines",
                //icon = berryIcon,
                goal = 1
            },

            new QuestData
            {
                id=6,
                title = "Construire un cueilleur de baies",
                description = "Oh ces adorables berries, apparemment elles nous rendraient plus rapides...",
                rewards = "R�compenses : bois, f�rets, cabane pour b�cherons (angar bleu)",
                //icon = berryIcon,
                goal = 1
            },

            new QuestData
            {
                id=7,
                title = "Construire un b�timent pour b�cheron",
                description = "Le bois est une ressource si utile nous devrions commencer � en produire!",
                rewards = "b�timent pour outils (angar vert)",
                //icon = berryIcon,
                goal = 1
            },

            new QuestData
            {
                id=8,
                title = "Construire un b�timent pour les outils",
                description = "Un b�timent pour construire des outils seraient pratique",
                rewards = "b�timent v�tements", //parchemins
                //icon = berryIcon,
                goal = 1
            },

            new QuestData
            {
                id=9,
                title = "Construire le b�timent pour v�tements",
                description = "Parce que m�me � l'�ge de pierre faut bien �tre fashion!",
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
