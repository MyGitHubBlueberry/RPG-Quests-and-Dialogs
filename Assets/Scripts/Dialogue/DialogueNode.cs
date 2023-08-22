using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPG.Dialogue
{

   public class DialogueNode : ScriptableObject
   {
      [SerializeField] bool isPlayerSpeaking;
      [SerializeField] string text;
      [SerializeField] List<string> children = new List<string>();
      [SerializeField] Rect rect = new Rect(0, 0, 200, 100);

      public Rect GetRect()
      {
         return rect;
      }

      public string GetText()
      {
         return text;
      }

      public List<string> GetChildren()
      {
         return children;
      }

      public bool IsPlayerSpeaking()
      {
         return isPlayerSpeaking;
      }

#if UNITY_EDITOR
      public void SetPosition(Vector2 position)
      {
         Undo.RecordObject(this, "Update Dialogue Position");
         rect.position = position;
         EditorUtility.SetDirty(this);
      }

      public void SetText(string text)
      {
         if (this.text == text) return;

         Undo.RecordObject(this, "Update Dialogue Text");
         this.text = text;

         EditorUtility.SetDirty(this);
      }

      public void AddChild(string childID)
      {
         Undo.RecordObject(this, "Add Dialogue Node link");
         children.Add(childID);
         EditorUtility.SetDirty(this);
      }

      public void RemoveChild(string childID)
      {
         Undo.RecordObject(this, "Remove Dialogue Node link");
         children.Remove(childID);
         EditorUtility.SetDirty(this);
      }

      public void SetPlayerSpeaking(bool isPlayerSpeaking)
      {
         Undo.RecordObject(this, "Change Dialogue Speaker");
         this.isPlayerSpeaking = isPlayerSpeaking;
         EditorUtility.SetDirty(this);
      }

#endif
   }
}