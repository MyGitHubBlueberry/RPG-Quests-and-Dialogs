using RPG.Control;
using UnityEngine;

namespace RPG.Dialogue
{
   public class AIConversant : MonoBehaviour, IRaycastable
   {
      [SerializeField] Dialogue dialogue;

      public CursorType GetCursorType()
      {
         return CursorType.Dialogue;
      }

      public bool HandleRaycast(PlayerController callingController)
      {
         if(dialogue == null) return false;

         if (Input.GetMouseButton(0))
         {
            callingController.GetComponent<PlayerConversant>().StartDialogue(this,dialogue);
         }
         return true;
      }
   }
}