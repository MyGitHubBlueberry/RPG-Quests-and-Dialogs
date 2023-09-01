using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
   using UnityEngine;
   
   [CreateAssetMenu(menuName = "RPG Quests and Dialogs/Quest")]
   public class Quest : ScriptableObject
   {
      [SerializeField] string[] objectives;
      
      public string GetTitle()
      {
         return name;
      }

      public int GetObjectivesLength()
      {
         return objectives.Length;
      }
   }
}
