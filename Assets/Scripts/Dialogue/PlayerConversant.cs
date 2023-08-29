using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.Dialogue
{
   public class PlayerConversant : MonoBehaviour
   {
      Dialogue currentDialogue;
      DialogueNode currentNode;
      bool isChoosing;

      public event Action OnConversationUpdated;

      public void StartDialogue(Dialogue newDialogue)
      {
         currentDialogue = newDialogue;
         currentNode = currentDialogue.GetRootNode();
         OnConversationUpdated?.Invoke();
      }

      public void Quit()
      {
         currentDialogue = null;
         currentNode = null;
         isChoosing = false;
         OnConversationUpdated?.Invoke();
      }

      public bool IsActive()
      {
         return currentDialogue != null;  
      }
      public bool IsChoosing()
      {
         return isChoosing;
      }

      public string GetText()
      {
         if (currentNode == null) return "";
         return currentNode.GetText();
      }

      public IEnumerable<DialogueNode> GetAnswerOptions()
      {
         return currentDialogue.GetPlayerChildren(currentNode);
      }

      public void SelectAnswer(DialogueNode chosenNode)
      {
         currentNode = chosenNode;
         isChoosing = false;
         Next();
      }

      public void Next()
      {
         int numPlayerResponces = currentDialogue.GetPlayerChildren(currentNode).Count();
         if (numPlayerResponces > 0)
         {
            isChoosing = true;
            OnConversationUpdated?.Invoke();
            return;
         }
         DialogueNode[] children = currentDialogue.GetAIChildren(currentNode).ToArray();
         int childIndex = UnityEngine.Random.Range(0, children.Length);
         currentNode = children[childIndex];

         OnConversationUpdated?.Invoke();
      }

      public bool HasNext()
      {
         return currentDialogue.GetAllChildren(currentNode).Count() > 0;
      }
   }
}