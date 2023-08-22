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
               selectedDialogue.CreateNode(creatingNode);
               creatingNode = null;
            }
            if (deletingNode != null)
            {
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
               draggingOffset = draggingNode.GetRect().position - Event.current.mousePosition;
               Selection.activeObject = draggingNode;
            }
            else
            {
               draggingCanvas = true;
               draggingCanvasOffset = Event.current.mousePosition + scrollPosition;
               Selection.activeObject = selectedDialogue;
            }
         }
         else if (draggingNodeIsInProcess)
         {
            draggingNode.SetPosition(Event.current.mousePosition + draggingOffset);

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
         else if (draggingCanvasIsStopped)
         {
            draggingCanvas = false;
         }
      }

      private DialogueNode GetNodeAtPoint(Vector2 point)
      {
         DialogueNode foundNode = null;
         foreach (var node in selectedDialogue.GetAllNodes())
         {
            if (node.GetRect().Contains(point)) foundNode = node;
         }
         return foundNode;
      }

      private void DrawNode(DialogueNode node)
      {
         GUILayout.BeginArea(node.GetRect(), nodeStyle);

         node.SetText(EditorGUILayout.TextField(node.GetText()));

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
         else if (linkingParentNode.GetChildren().Contains(node.name))
         {
            if (GUILayout.Button("unlink"))
            {
               linkingParentNode.RemoveChild(linkingParentNode.name);
               linkingParentNode = null;
            }
         }
         else if (node.GetChildren().Contains(linkingParentNode.name))
         {
            if (GUILayout.Button("unlink"))
            {
               node.RemoveChild(linkingParentNode.name);
               linkingParentNode = null;
            }
         }
         else
         {
            if (GUILayout.Button("child"))
            {
               linkingParentNode.AddChild(node.name);
               linkingParentNode = null;
            }
         }
      }

      private void DrawConnections(DialogueNode node)
      {
         Vector3 startPosition = new Vector2(node.GetRect().xMax, node.GetRect().center.y);
         foreach (DialogueNode childNode in selectedDialogue.GetAllChildren(node))
         {
            Vector3 endPosition = new Vector2(childNode.GetRect().xMin, childNode.GetRect().center.y);
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