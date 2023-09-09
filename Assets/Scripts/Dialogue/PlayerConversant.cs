using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RPG.Core;
using UnityEngine;

namespace RPG.Dialogue
{
   public class PlayerConversant : MonoBehaviour
   {
      [SerializeField] string playerName;
      Dialogue currentDialogue;
      DialogueNode currentNode;
      AIConversant currentConversant;
      bool isChoosing;

      public event Action OnConversationUpdated;

      public void StartDialogue(AIConversant newConversant, Dialogue newDialogue)
      {
         currentConversant = newConversant;
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
         currentConversant = null;
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

      public string GetCurrentConversantName()
      {
         return IsChoosing() ? playerName : currentConversant.GetName();
      }

      public IEnumerable<DialogueNode> GetAnswerOptions()
      {
         return FilterOnCondition(currentDialogue.GetPlayerChildren(currentNode));
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
         int numPlayerResponces = FilterOnCondition(currentDialogue.GetPlayerChildren(currentNode)).Count();
         if (numPlayerResponces > 0)
         {
            isChoosing = true;
            TriggerExitAction();
            OnConversationUpdated?.Invoke();
            return;
         }
         DialogueNode[] children = FilterOnCondition(currentDialogue.GetAIChildren(currentNode)).ToArray();
         int childIndex = UnityEngine.Random.Range(0, children.Length);
         TriggerExitAction();
         currentNode = children[childIndex];
         TriggerEnterAction();

         OnConversationUpdated?.Invoke();
      }

      public bool HasNext()
      {
         return FilterOnCondition(currentDialogue.GetAllChildren(currentNode)).Count() > 0;
      }

      private IEnumerable<DialogueNode> FilterOnCondition(IEnumerable<DialogueNode> inputNode)
      {
         foreach (var node in inputNode)
         {
            if(node.CheckCondition(GetEvaluators()))
            {
               yield return node;
            }
         }
      }

      private IEnumerable<IPredicateEvaluator> GetEvaluators()
      {
         return GetComponents<IPredicateEvaluator>();
      }

      private void TriggerEnterAction()
      {
         if (currentNode != null)
         {
            TriggerAction(currentNode.GetOnEnterAction());
         }
      }

      private void TriggerExitAction()
      {
         if (currentNode != null)
         {
            TriggerAction(currentNode.GetOnExitAction());
         }
      }

      private void TriggerAction(string action)
      {
         if (String.IsNullOrEmpty(action)) return;

         foreach (DialogueTrigger trigger in currentConversant.GetComponents<DialogueTrigger>())
         {
            trigger.Trigger(action);
         }
      }
   }
}