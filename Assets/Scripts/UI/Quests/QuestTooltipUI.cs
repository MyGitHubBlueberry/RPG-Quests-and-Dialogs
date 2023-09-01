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
      [SerializeField] GameObject incompletedObjectivePrefab;

      public void SetUp(QuestStatus status)
      {
         Quest quest = status.GetQuest();
         title.text = quest.GetTitle();
         objectives.DestroyChildren();
         foreach (string objective in quest.GetObjectives())
         {
            GameObject prefabToInstantiate = status.IsObjectiveCompleted(objective) 
            ? objectivePrefab
            : incompletedObjectivePrefab;
            
            GameObject objectiveInstance = Instantiate(prefabToInstantiate, objectives);
            TextMeshProUGUI objectiveText = objectiveInstance.GetComponentInChildren<TextMeshProUGUI>();
            objectiveText.text = objective;
         }
      }
   }
}
