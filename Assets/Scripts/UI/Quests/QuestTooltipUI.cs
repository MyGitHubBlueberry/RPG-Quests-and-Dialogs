using System.Collections;
using System.Collections.Generic;
using Extentions;
using RPG.Quests;
using TMPro;
using UnityEngine;

namespace RPG.UI.Quests
{
   public class QuestTooltipUI : MonoBehaviour
   {
      [SerializeField] TextMeshProUGUI title;
      [SerializeField] Transform objectives;
      [SerializeField] GameObject objectivePrefab;

      public void SetUp(Quest quest)
      {
         title.text = quest.GetTitle();
         objectives.DestroyChildren();
         foreach(string objective in quest.GetObjectives())
         {
            GameObject objectiveInstance = Instantiate(objectivePrefab, objectives);
            TextMeshProUGUI objectiveText = objectiveInstance.GetComponentInChildren<TextMeshProUGUI>();
            objectiveText.text = objective;
         }
      }
   }
}
