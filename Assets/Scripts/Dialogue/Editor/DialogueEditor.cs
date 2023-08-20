using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System;

namespace RPG.Dialogue.Editor
{
   public class DialogueEditor : EditorWindow
   {
      Dialogue selectedDialogue;
      [NonSerialized] DialogueNode draggingNode;
      [NonSerialized] GUIStyle nodeStyle;
      [NonSerialized] Vector2 draggingOffset;
      [NonSerialized] DialogueNode creatingNode = null;

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
               DrawConnections(node);
            }
            foreach (var node in selectedDialogue.GetAllNodes())
            {
               DrawNode(node);
            }
            if (creatingNode != null)
            {
               Undo.RecordObject(selectedDialogue, "Added Dialogue Node");
               selectedDialogue.CreateNode(creatingNode);
               creatingNode = null;
            }
            selectedDialogue.CreateDefaultNode();
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
            if (draggingNode != null)
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
         foreach (var node in selectedDialogue.GetAllNodes())
         {
            if (node.rect.Contains(point)) foundNode = node;
         }
         return foundNode;
      }

      private void DrawNode(DialogueNode node)
      {
         GUILayout.BeginArea(node.rect, nodeStyle);
         EditorGUI.BeginChangeCheck();

         string newNodeText = EditorGUILayout.TextField(node.text);

         if (EditorGUI.EndChangeCheck())
         {
            Undo.RecordObject(selectedDialogue, "Update Dialogue Text");
            node.text = newNodeText;
         }

         if (GUILayout.Button("+"))
         {
            creatingNode = node;
         }

         GUILayout.EndArea();
      }

      private void DrawConnections(DialogueNode node)
      {
         Vector3 startPosition = new Vector2(node.rect.xMax, node.rect.center.y);
         foreach (DialogueNode childNode in selectedDialogue.GetAllChildren(node))
         {
            Vector3 endPosition = new Vector2(childNode.rect.xMin, childNode.rect.center.y);
            Vector3 controlPointOffset = endPosition - startPosition;
            controlPointOffset.y = 0;
            controlPointOffset.x *= 0.8f;
            Handles.DrawBezier(
               startPosition, endPosition,
               startPosition + controlPointOffset,
               endPosition - controlPointOffset,
               Color.white, null, 4f);
         }
      }
   }
}