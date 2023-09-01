using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Extentions;
using RPG.Quests;
using UnityEngine;

namespace RPG.UI.Quests
{
   public class QuestListUI : MonoBehaviour
   {
      [SerializeField] QuestItemUI questPrefab;

      void Start()
      {
         transform.DestroyChildren();
         QuestList questList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();
         foreach (QuestStatus status in questList.GetStatuses())
         {
            Instantiate(questPrefab, transform).SetUp(status);
         }
      }
   }
}
