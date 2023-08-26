using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Dialogue
{
   public class PlayerConversant : MonoBehaviour
   {
      [SerializeField] Dialogue currentDialogue;

      public string GetText()
      {
         if(currentDialogue == null) return "";
         foreach (DialogueNode node in currentDialogue.GetAllNodes())
         {
            return node.GetText();
         }
         return ""; 
      }
   }
}