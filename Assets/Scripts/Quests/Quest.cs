using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
   using System;
   using System.Linq;
   using System.Runtime.CompilerServices;
   using GameDevTV.Inventories;
    using UnityEditor;
    using UnityEngine;

   [CreateAssetMenu(menuName = "RPG Quests and Dialogs/Quest")]
   public class Quest : ScriptableObject
   {
      [SerializeField] Objective[] objectives;
      [SerializeField] Reward[] rewards;
      

      [Serializable]
      public class Reward
      {
         [Min(1)]
         public int number;
         public InventoryItem item;
      }

      [Serializable]
      public class Objective
      {
         public string reference;
         public string description;
      }

      public string GetTitle() => name;
      public int GetObjectivesLength() => objectives.Length;
      public IEnumerable<Objective> GetObjectives() => objectives;
      public IEnumerable<Reward> GetRewards() => rewards;
      public bool HasObjective(string reference) => objectives
         .Any(objective => objective.reference == reference);

      public static Quest GetByName(string questName)
      {
         foreach (Quest quest in Resources.LoadAll<Quest>(""))
         {
            if (quest.name == questName) return quest;
         }
         return null;
      }
   }
}
