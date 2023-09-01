using System.Collections;
using System.Collections.Generic;
using RPG.Quests;
using TMPro;
using UnityEngine;

namespace RPG.UI.Quests
{
   public class QuestItemUI : MonoBehaviour
   {
      [SerializeField] TextMeshProUGUI title;
      [SerializeField] TextMeshProUGUI progress;

      QuestStatus status;

      public void SetUp(QuestStatus status)
      {
         this.status = status;
         title.text = status.GetQuest().GetTitle();
         progress.text = status.GetCompletedObjectivesCount() + "/" + status.GetQuest().GetObjectivesLength().ToString();
      }

      public QuestStatus GetQuestStatus() => status;
   }
}
