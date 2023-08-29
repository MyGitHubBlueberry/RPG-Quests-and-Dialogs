using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.Dialogue
{
   public class PlayerConversant : MonoBehaviour
   {
      [SerializeField] Dialogue currentDialogue;

      DialogueNode currentNode;
      bool isChoosing;

      private void Awake()
      {
         currentNode = currentDialogue.GetRootNode();
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
         if(numPlayerResponces > 0)
         {
            isChoosing = true;
            return;
         }
         DialogueNode[] children = currentDialogue.GetAIChildren(currentNode).ToArray();
         int childIndex = Random.Range(0, children.Length);
         currentNode = children[childIndex];
      }

      public bool HasNext()
      {
         return currentDialogue.GetAllChildren(currentNode).Count() > 0;
      }
   }
}