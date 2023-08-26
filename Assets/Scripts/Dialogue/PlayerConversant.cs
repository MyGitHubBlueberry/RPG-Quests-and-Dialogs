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

      private void Awake()
      {
         currentNode = currentDialogue.GetRootNode();
      }

      public string GetText()
      {
         if(currentNode == null) return "";
         return currentNode.GetText();
      }

      public void Next()
      {
         DialogueNode[] children = currentDialogue.GetAllChildren(currentNode).ToArray();
         int childIndex = Random.Range(0, children.Length);
         currentNode = children[childIndex];
      }

      public bool HasNext()
      {
         return currentDialogue.GetAllChildren(currentNode).Count() > 0;
      }
   }
}