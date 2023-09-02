using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    using System.Linq;
    using System.Runtime.CompilerServices;
    using UnityEngine;
   
   [CreateAssetMenu(menuName = "RPG Quests and Dialogs/Quest")]
   public class Quest : ScriptableObject
   {
      [SerializeField] string[] objectives;
      
      public string GetTitle() => name;
      public int GetObjectivesLength() => objectives.Length;
      public IEnumerable<string> GetObjectives() => objectives;
      public bool HasObjective(string objective) => objectives.Contains(objective);
      public static Quest GetByName(string questName)
      {
         foreach(Quest quest in Resources.LoadAll<Quest>(""))
         {
            if(quest.name == questName) return quest;
         }
         return null;
      }
   }
}
