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
      public void SetUp(Quest quest)
      {
         title.text = quest.GetTitle();
         progress.text = "0/" + quest.GetObjectivesLength().ToString();
      }
   }
}
