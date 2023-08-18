using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System;

namespace RPG.Dialogue.Editor
{
   public class DialogueEditor : EditorWindow
   {
      Dialogue selectedDialogue;
      DialogueNode draggingNode;
      GUIStyle nodeStyle;
      Vector2 draggingOffset;

      [OnOpenAsset(1)]
      public static bool OnOpenAsset(int instanceID, int line)
      {
         if (EditorUtility.InstanceIDToObject(instanceID) is Dialogue)
         {
            ShowEditorWindow();
            return true;
         }
         return false;
      }

      [MenuItem("Window/Dialogue Editor")]
      public static void ShowEditorWindow()
      {
         GetWindow(typeof(DialogueEditor), false, "Dialogue Editor");
      }

      private void OnEnable()
      {
         Selection.selectionChanged += OnSelectionChanged;

         nodeStyle = new GUIStyle();
         nodeStyle.normal.background = EditorGUIUtility.Load("node0") as Texture2D;
         nodeStyle.padding = new RectOffset(20, 20, 20, 20);
         nodeStyle.border = new RectOffset(12, 12, 12, 12);
      }

      private void OnSelectionChanged()
      {
         Dialogue newDialogue = Selection.activeObject as Dialogue;
         if (newDialogue != null)
         {
            selectedDialogue = newDialogue;
            Repaint();
         }
      }

      private void OnGUI()
      {
         if (selectedDialogue == null)
         {
            EditorGUILayout.LabelField("No Dialogue selected.");
         }
         else
         {
            ProcessEvents();
            foreach (var node in selectedDialogue.GetAllNodes())
            {
               OnGUINode(node);
            }
         }
      }

      private void ProcessEvents()
      {
         bool draggingStarted = Event.current.type == EventType.MouseDown && draggingNode == null;
         bool draggingStopped = Event.current.type == EventType.MouseUp && draggingNode != null;
         bool draggingInProcess = Event.current.type == EventType.MouseDrag && draggingNode != null;

         if (draggingStarted)
         {
            draggingNode = GetNodeAtPoint(Event.current.mousePosition);
            if(draggingNode != null)
            {
               draggingOffset = draggingNode.rect.position - Event.current.mousePosition;
            }
         }
         else if (draggingInProcess)
         {
            Undo.RecordObject(selectedDialogue, "Update Dialogue position");
            draggingNode.rect.position = Event.current.mousePosition + draggingOffset;
            GUI.changed = true;
         }
         else if (draggingStopped)
         {
            draggingNode = null;
         }
      }

      private DialogueNode GetNodeAtPoint(Vector2 point)
      {
         DialogueNode foundNode = null;
         foreach(var node in selectedDialogue.GetAllNodes())
         {
            if(node.rect.Contains(point)) foundNode = node;
         }
         return foundNode;
      }

      private void OnGUINode(DialogueNode node)
      {
         GUILayout.BeginArea(node.rect, nodeStyle);
         EditorGUI.BeginChangeCheck();

         EditorGUILayout.LabelField("Node: ");
         string newNodeText = EditorGUILayout.TextField(node.text);
         string newNodeId = EditorGUILayout.TextField(node.uniqueID);

         if (EditorGUI.EndChangeCheck())
         {
            Undo.RecordObject(selectedDialogue, "Update Dialogue Text");
            node.text = newNodeText;
            node.uniqueID = newNodeId;
         }

         GUILayout.EndArea();
      }
   }
}