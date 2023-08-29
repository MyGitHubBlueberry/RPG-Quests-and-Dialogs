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
      [SerializeField] GameObject AIResponce;
      [SerializeField] Transform playerAnswerOptionsRoot;
      [SerializeField] GameObject answerOptionButtonPrefab;

      PlayerConversant playerConversant;

      void Start()
      {
         playerConversant = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConversant>();
         playerConversant.OnConversationUpdated += UpdateUI;
         nextButton.onClick.AddListener(Next);

         UpdateUI();
      }

      private void Next()
      {
         playerConversant.Next();
      }

      private void UpdateUI()
      {
         if(!playerConversant.IsActive()) return;

         AIResponce.SetActive(!playerConversant.IsChoosing());
         playerAnswerOptionsRoot.gameObject.SetActive(playerConversant.IsChoosing());

         if (playerConversant.IsChoosing())
         {
            DestroyOldChildren();
            SetUpNewChildren();
         }
         else
         {
            AIText.text = playerConversant.GetText();
            nextButton.gameObject.SetActive(playerConversant.HasNext());
         }
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
         foreach (DialogueNode choise in playerConversant.GetAnswerOptions())
         {
            GameObject choiseInstance = Instantiate(answerOptionButtonPrefab, playerAnswerOptionsRoot);
            choiseInstance.GetComponentInChildren<TextMeshProUGUI>().text = choise.GetText();
            Button button = choiseInstance.GetComponentInChildren<Button>();
            button.onClick.AddListener(() =>
            {
               playerConversant.SelectAnswer(choise);
            });
         }
      }
   }
}