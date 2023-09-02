using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
   public class QuestCompletion : MonoBehaviour
   {
      [SerializeField] Quest quest;
      [SerializeField] string objective;
      QuestList questList;
      
      void Awake()
      {
         questList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();
      }

      public void CompleteObjective()
      {
         if(!quest.HasObjective(objective)) return;
         questList.CompleteObjective(quest ,objective);
      }
   }
}
