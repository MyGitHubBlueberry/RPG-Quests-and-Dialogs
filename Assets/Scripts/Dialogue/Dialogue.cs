using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPG.Dialogue
{

   [CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue")]
   public class Dialogue : ScriptableObject
   {
      [SerializeField] List<DialogueNode> nodes = new List<DialogueNode>();
      Dictionary<string, DialogueNode> nodeLookup = new Dictionary<string, DialogueNode>();

#if UNITY_EDITOR
      private void Awake()
      {
         CreateDefaultNode();
         OnValidate();
      }
#endif

      public void CreateDefaultNode()
      {
         if (nodes.Count == 0)
         {
            CreateNode(null);
         }
      }

      private void OnValidate()
      {
         nodeLookup.Clear();
         foreach (DialogueNode node in GetAllNodes())
         {
            nodeLookup[node.name] = node;
         }
      }

      public IEnumerable<DialogueNode> GetAllNodes()
      {
         return nodes;
      }

      public IEnumerable<DialogueNode> GetAllChildren(DialogueNode parentNode)
      {
         foreach (string childID in parentNode.children)
         {
            if (nodeLookup.ContainsKey(childID))
            {
               yield return nodeLookup[childID];
            }
         }
      }

      public void CreateNode(DialogueNode parentNode)
      {
         DialogueNode node = CreateInstance<DialogueNode>();
         node.name = Guid.NewGuid().ToString();

         Undo.RegisterCreatedObjectUndo(node, "Created Dialogue Node");

         if (parentNode != null)
         {
            parentNode.children.Add(node.name);
         }
         nodes.Add(node);
         OnValidate();
      }

      public void DeleteNode(DialogueNode nodeToDelete)
      {
         nodes.Remove(nodeToDelete);
         OnValidate();
         CleanDanglingChildren(nodeToDelete);
         Undo.DestroyObjectImmediate(nodeToDelete);
      }

      private void CleanDanglingChildren(DialogueNode nodeToDelete)
      {
         foreach (DialogueNode node in GetAllNodes())
         {
            node.children.Remove(nodeToDelete.name);
         }
      }
   }
}