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
         TriggerEnterAction();
         OnConversationUpdated?.Invoke();
      }

      public void Quit()
      {
         currentDialogue = null;
         TriggerExitAction();
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
         TriggerEnterAction();
         isChoosing = false;
         Next();
      }

      public void Next()
      {
         int numPlayerResponces = currentDialogue.GetPlayerChildren(currentNode).Count();
         if (numPlayerResponces > 0)
         {
            isChoosing = true;
            TriggerExitAction();
            OnConversationUpdated?.Invoke();
            return;
         }
         DialogueNode[] children = currentDialogue.GetAIChildren(currentNode).ToArray();
         int childIndex = UnityEngine.Random.Range(0, children.Length);
         TriggerExitAction();
         currentNode = children[childIndex];
         TriggerEnterAction();

         OnConversationUpdated?.Invoke();
      }

      public bool HasNext()
      {
         return currentDialogue.GetAllChildren(currentNode).Count() > 0;
      }

      private void TriggerEnterAction()
      {
         if (currentNode != null && currentNode.GetOnEnterAction() != "")
         {
            Debug.Log(currentNode.GetOnEnterAction());
         }
      }

      private void TriggerExitAction()
      {
         if (currentNode != null && currentNode.GetOnEnterAction() != "")
         {
            Debug.Log(currentNode.GetOnExitAction());
         }
      }
   }
}