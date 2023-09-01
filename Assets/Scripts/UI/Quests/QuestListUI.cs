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
      [SerializeField] Quest[] tempQuests;
      [SerializeField] QuestItemUI questPrefab;

      void Start()
      {
         transform.DestroyChildren();
         foreach (Quest quest in tempQuests)
         {
            Instantiate(questPrefab, transform).SetUp(quest);
         }
      }
   }
}
