using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System;

namespace RPG.Dialogue.Editor
{
   public class DialogueEditor : EditorWindow
   {
      Dialogue selectedDialogue;

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
      }

      private void OnSelectionChanged()
      {
         Dialogue newDialogue = Selection.activeObject as Dialogue;
         if(newDialogue != null)
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
            foreach (var node in selectedDialogue.GetAllNodes())
            {
               EditorGUI.BeginChangeCheck();

               EditorGUILayout.LabelField("Node: ");
               string newNodeText = EditorGUILayout.TextField(node.text);
               string newNodeId = EditorGUILayout.TextField(node.uniqueID);

               if(EditorGUI.EndChangeCheck())
               {
                  Undo.RecordObject(selectedDialogue, "Update Dialogue Text");
                  node.text = newNodeText;
                  node.uniqueID = newNodeId;
               }
            }
         }
      }
   }
}