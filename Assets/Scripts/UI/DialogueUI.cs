using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Dialogue;
using TMPro;
using UnityEngine.UI;
using System;
using Extentions;

namespace RPG.UI
{
   public class DialogueUI : MonoBehaviour
   {
      [SerializeField] TextMeshProUGUI AIText;
      [SerializeField] Button nextButton;
      [SerializeField] Button quitButton;
      [SerializeField] GameObject AIResponce;
      [SerializeField] Transform playerAnswerOptionsRoot;
      [SerializeField] GameObject answerOptionButtonPrefab;
      [SerializeField] TextMeshProUGUI conversantName;

      PlayerConversant playerConversant;

      void Start()
      {
         playerConversant = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConversant>();
         playerConversant.OnConversationUpdated += UpdateUI;
         nextButton.onClick.AddListener(playerConversant.Next);
         quitButton.onClick.AddListener(playerConversant.Quit);

         UpdateUI();
      }

      private void UpdateUI()
      {
         gameObject.SetActive(playerConversant.IsActive());

         if(!playerConversant.IsActive()) return;

         conversantName.text = playerConversant.GetCurrentConversantName();
         AIResponce.SetActive(!playerConversant.IsChoosing());
         playerAnswerOptionsRoot.gameObject.SetActive(playerConversant.IsChoosing());

         if (playerConversant.IsChoosing())
         {
            playerAnswerOptionsRoot.DestroyChildren();
            SetUpNewChildren();
         }
         else
         {
            AIText.text = playerConversant.GetText();
            nextButton.gameObject.SetActive(playerConversant.HasNext());
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