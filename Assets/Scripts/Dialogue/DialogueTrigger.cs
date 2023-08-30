using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Dialogue
{
   public class DialogueTrigger : MonoBehaviour
   {
      [SerializeField] string action;
      [SerializeField] UnityEvent OnTrigger;

      public void Trigger(string action)
      {
         if(this.action == action)
         {
            OnTrigger?.Invoke();
         }
      }
   }
}