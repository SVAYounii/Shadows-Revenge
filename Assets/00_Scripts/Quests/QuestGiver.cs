using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class QuestGiver : MonoBehaviour
{
    public Quest quest;

    public Player player;

    public GameObject QuestWindow;
    public TextMeshPro QuestTitle;
    public TextMeshPro QuestDescription;
    public TextMeshPro QuestGoldText;

    public void OpenQuestWindow()
    {
        QuestWindow.SetActive(true);
        QuestTitle.text = quest.title;
        QuestDescription.text = quest.description;
        QuestGoldText.text = quest.goldreward.ToString();
    }

}
