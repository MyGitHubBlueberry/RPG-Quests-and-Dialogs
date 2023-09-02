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

      QuestList questList;

      void Awake()
      {
         questList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();
      }
      void Start()
      {
         questList.OnQuestListUpdated += UpdateUI;
         UpdateUI();
      }

      private void UpdateUI()
      {
         transform.DestroyChildren();
         foreach (QuestStatus status in questList.GetStatuses())
         {
            Instantiate(questPrefab, transform).SetUp(status);
         }
      }
   }
}
