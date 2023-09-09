using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
   [Serializable]
   public class Condition
   {
      [SerializeField] Disjunction[] and;

      public bool Check(IEnumerable<IPredicateEvaluator> evaluators)
      {
         foreach (var disjunction in and)
            if (!disjunction.Check(evaluators)) return false;

         return true;
      }

      [Serializable]
      class Disjunction
      {
         [SerializeField] Predicate[] or;

         public bool Check(IEnumerable<IPredicateEvaluator> evaluators)
         {
            foreach (var predicate in or)
               if (predicate.Check(evaluators)) return true;

            return false;
         }
      }

      [Serializable]
      class Predicate
      {
         [SerializeField] string predicate;
         [SerializeField] bool isNegated;
         [SerializeField] string[] parameters;

         public bool Check(IEnumerable<IPredicateEvaluator> evaluators)
         {
            foreach (var evaluator in evaluators)
            {
               bool? result = evaluator.Evaluate(predicate, parameters);
               if (result == null) continue;
               if (result == isNegated) return false;
            }
            return true;
         }
      }
   }
}
