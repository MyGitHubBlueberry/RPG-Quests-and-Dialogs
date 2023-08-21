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
      [NonSerialized] DialogueNode deletingNode = null;
      [NonSerialized] DialogueNode linkingParentNode = null;
      Vector2 scrollPosition;
      [NonSerialized] bool draggingCanvas;
      [NonSerialized] Vector2 draggingCanvasOffset;

      const float CANVAS_SIZE = 4000;
      const float BACKGROUND_SIZE = 50;

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

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            Rect canvas = GUILayoutUtility.GetRect(CANVAS_SIZE, CANVAS_SIZE);
            Texture2D backgroundTex = Resources.Load("background") as Texture2D;
            int textureRepeatNumber = (int)(CANVAS_SIZE / BACKGROUND_SIZE);
            Rect texCoords = new Rect(0, 0, textureRepeatNumber, textureRepeatNumber);

            GUI.DrawTextureWithTexCoords(canvas, backgroundTex, texCoords);    

            foreach (var node in selectedDialogue.GetAllNodes())
            {
               DrawConnections(node);
            }
            foreach (var node in selectedDialogue.GetAllNodes())
            {
               DrawNode(node);
            }

            EditorGUILayout.EndScrollView();

            if (creatingNode != null)
            {
               Undo.RecordObject(selectedDialogue, "Added Dialogue Node");
               selectedDialogue.CreateNode(creatingNode);
               creatingNode = null;
            }
            if (deletingNode != null)
            {
               Undo.RecordObject(selectedDialogue, "Delete Dialogue Node");
               selectedDialogue.DeleteNode(deletingNode);
               deletingNode = null;
            }
            selectedDialogue.CreateDefaultNode();
         }
      }

      private void ProcessEvents()
      {
         bool draggingStarted = Event.current.type == EventType.MouseDown && draggingNode == null;
         bool draggingNodeIsInProcess = Event.current.type == EventType.MouseDrag && draggingNode != null;
         bool draggingCanvasIsInProcess = Event.current.type == EventType.MouseDrag && draggingCanvas;
         bool draggingNodeIsStopped = Event.current.type == EventType.MouseUp && draggingNode != null;
         bool draggingCanvasIsStopped = Event.current.type == EventType.MouseUp && draggingCanvas;

         if (draggingStarted)
         {
            draggingNode = GetNodeAtPoint(Event.current.mousePosition + scrollPosition);
            if (draggingNode != null)
            {
               draggingOffset = draggingNode.rect.position - Event.current.mousePosition;
            }
            else
            {
               draggingCanvas = true;
               draggingCanvasOffset = Event.current.mousePosition + scrollPosition;
            }
         }
         else if (draggingNodeIsInProcess)
         {
            Undo.RecordObject(selectedDialogue, "Update Dialogue position");
            draggingNode.rect.position = Event.current.mousePosition + draggingOffset;

            GUI.changed = true;
         }
         else if (draggingCanvasIsInProcess)
         {
            scrollPosition = draggingCanvasOffset - Event.current.mousePosition;

            GUI.changed = true;
         }
         else if (draggingNodeIsStopped)
         {
            draggingNode = null;
         }
         else if(draggingCanvasIsStopped)
         {
            draggingCanvas = false;
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

         GUILayout.BeginHorizontal();

         if (GUILayout.Button("-"))
         {
            deletingNode = node;
         }

         DrawLinkButtons(node);

         if (GUILayout.Button("+"))
         {
            creatingNode = node;
         }

         GUILayout.EndHorizontal();

         GUILayout.EndArea();
      }

      private void DrawLinkButtons(DialogueNode node)
      {
         if (linkingParentNode == null)
         {
            if (GUILayout.Button("link"))
            {
               linkingParentNode = node;
            }
         }
         else if (node == linkingParentNode)
         {
            if (GUILayout.Button("cancel"))
            {
               linkingParentNode = null;
            }
         }
         else if (linkingParentNode.children.Contains(node.uniqueID))
         {
            if (GUILayout.Button("unlink"))
            {
               Undo.RecordObject(selectedDialogue, "Remove Dialogue Node link");
               linkingParentNode.children.Remove(node.uniqueID);
               linkingParentNode = null;
            }
         }
         else if (node.children.Contains(linkingParentNode.uniqueID))
         {
            if (GUILayout.Button("unlink"))
            {
               Undo.RecordObject(selectedDialogue, "Remove Dialogue Node link");
               node.children.Remove(linkingParentNode.uniqueID);
               linkingParentNode = null;
            }
         }
         else
         {
            if (GUILayout.Button("child"))
            {
               Undo.RecordObject(selectedDialogue, "Add Dialogue Node link");
               linkingParentNode.children.Add(node.uniqueID);
               linkingParentNode = null;
            }
         }
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