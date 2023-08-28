using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPG.Dialogue
{

   [CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue")]
   public class Dialogue : ScriptableObject, ISerializationCallbackReceiver
   {
      [SerializeField] List<DialogueNode> nodes = new List<DialogueNode>();
      [SerializeField] Vector2 newNodeOffset = new Vector2(250, 0);
      Dictionary<string, DialogueNode> nodeLookup = new Dictionary<string, DialogueNode>();


      public DialogueNode GetRootNode()
      {
         return nodes[0];
      }

      public IEnumerable<DialogueNode> GetPlayerChildren(DialogueNode currentNode)
      {
         foreach(DialogueNode node in GetAllChildren(currentNode))
         {
            if(node.IsPlayerSpeaking()) yield return node;
         }         
      }

      public IEnumerable<DialogueNode> GetAIChildren(DialogueNode currentNode)
      {
         foreach (DialogueNode node in GetAllChildren(currentNode))
         {
            if (!node.IsPlayerSpeaking()) yield return node;
         }
      }

      public IEnumerable<DialogueNode> GetAllChildren(DialogueNode parentNode)
      {
         foreach (string childID in parentNode.GetChildren())
         {
            if (nodeLookup.ContainsKey(childID))
            {
               yield return nodeLookup[childID];
            }
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


      public void OnBeforeSerialize()
      {
#if UNITY_EDITOR
         CreateDefaultNode();
         if (AssetDatabase.GetAssetPath(this) != "")
         {
            foreach (DialogueNode node in GetAllNodes())
            {
               if (AssetDatabase.GetAssetPath(node) == "")
               {
                  AssetDatabase.AddObjectToAsset(node, this);
               }
            }
         }
#endif
      }

      public void OnAfterDeserialize()
      {
      }

#if UNITY_EDITOR
      public void CreateDefaultNode()
      {
         if (nodes.Count == 0)
         {
            DialogueNode node = MakeNode(null);
            AddNode(node);
         }
      }

      private DialogueNode MakeNode(DialogueNode parentNode)
      {
         DialogueNode node = CreateInstance<DialogueNode>();
         node.name = Guid.NewGuid().ToString();

         if (parentNode != null)
         {
            parentNode.AddChild(node.name);
            node.SetPlayerSpeaking(!parentNode.IsPlayerSpeaking());
            Vector2 parentPosition = parentNode.GetRect().position;
            node.SetPosition(parentPosition + newNodeOffset);
         }

         return node;
      }

      private void AddNode(DialogueNode node)
      {
         nodes.Add(node);
         OnValidate();
      }
      
      public void CreateNode(DialogueNode parentNode)
      {
         DialogueNode node = MakeNode(parentNode);

         Undo.RegisterCreatedObjectUndo(node, "Created Dialogue Node");
         Undo.RecordObject(this, "Added Dialogue Node");

         AddNode(node);
      }

      public void DeleteNode(DialogueNode nodeToDelete)
      {
         Undo.RecordObject(this, "Delete Dialogue Node");
         nodes.Remove(nodeToDelete);
         OnValidate();
         CleanDanglingChildren(nodeToDelete);
         Undo.DestroyObjectImmediate(nodeToDelete);
      }

      private void CleanDanglingChildren(DialogueNode nodeToDelete)
      {
         foreach (DialogueNode node in GetAllNodes())
         {
            node.RemoveChild(nodeToDelete.name);
         }
      }
#endif
   }
}