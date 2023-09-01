using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Extentions
{
   public static class Extentions
   {
      public static void DestroyChildren(this Transform t)
      {
         foreach(Transform child in t)
         {
            Object.Destroy(child.gameObject);
         }
      }
   }
}
