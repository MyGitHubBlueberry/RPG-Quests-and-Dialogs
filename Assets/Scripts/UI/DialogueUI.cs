using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Dialogue;
using TMPro;
using UnityEngine.UI;
using System;

namespace RPG.UI
{
   public class DialogueUI : MonoBehaviour
   {
      [SerializeField] TextMeshProUGUI AIText;
      [SerializeField] Button nextButton;
      [SerializeField] Transform playerAnswerOptionsRoot;
      [SerializeField] GameObject answerOptionButtonPrefab;

      PlayerConversant playerConversant;

      void Start()
      {
         playerConversant = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConversant>();
         nextButton.onClick.AddListener(Next);

         UpdateUI();
      }

      private void Next()
      {
         playerConversant.Next();
         UpdateUI();
      }

      private void UpdateUI()
      {
         AIText.text = playerConversant.GetText();
         nextButton.gameObject.SetActive(playerConversant.HasNext());
         DestroyOldChildren();
         SetUpNewChildren();
      }

      private void DestroyOldChildren()
      {
         foreach (Transform child in playerAnswerOptionsRoot)
         {
            Destroy(child.gameObject);
         }
      }

      private void SetUpNewChildren()
      {
         foreach (string choise in playerConversant.GetAnswerChoises())
         {
            Instantiate(answerOptionButtonPrefab, playerAnswerOptionsRoot)
               .GetComponentInChildren<TextMeshProUGUI>().text = choise;
         }
      }
   }
}