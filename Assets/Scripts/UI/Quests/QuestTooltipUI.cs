using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
      [SerializeField] TextMeshProUGUI reward;

      public void SetUp(QuestStatus status)
      {
         Quest quest = status.GetQuest();
         title.text = quest.GetTitle();
         reward.text = GetRewardText(quest.GetRewards());

         objectives.DestroyChildren();
         foreach (Quest.Objective objective in quest.GetObjectives())
         {
            GameObject prefabToInstantiate = status
            .IsObjectiveCompleted(objective.reference)
            ? objectivePrefab
            : incompletedObjectivePrefab;

            GameObject objectiveInstance = Instantiate(prefabToInstantiate, objectives);
            TextMeshProUGUI objectiveText = objectiveInstance
               .GetComponentInChildren<TextMeshProUGUI>();
            objectiveText.text = objective.description;
         }
      }

      private string GetRewardText(IEnumerable<Quest.Reward> questRewards)
      {
         StringBuilder stringBuilder = new StringBuilder();
         foreach (Quest.Reward reward in questRewards)
         {
            if(stringBuilder.Length > 0)
            {
               stringBuilder.Append(", ");
            }
            if (reward.number > 1)
            {
               stringBuilder.Append(reward.number + " ");
            }
            stringBuilder.Append(reward.item.GetDisplayName());
         }
         if (stringBuilder.Length == 0)
         {
            stringBuilder.Append("No reward");
         }
         stringBuilder.Append(".");
         return stringBuilder.ToString();
      }
   }
}
