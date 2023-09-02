using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    using System.Linq;
    using UnityEngine;
   
   [CreateAssetMenu(menuName = "RPG Quests and Dialogs/Quest")]
   public class Quest : ScriptableObject
   {
      [SerializeField] string[] objectives;
      
      public string GetTitle() => name;
      public int GetObjectivesLength() => objectives.Length;
      public IEnumerable<string> GetObjectives() => objectives;
      public bool HasObjective(string objective) => objectives.Contains(objective);
   }
}
