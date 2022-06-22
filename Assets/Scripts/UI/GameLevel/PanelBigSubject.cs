using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class PanelBigSubject : MonoBehaviour{

	public GameObject mainPanel;
	public CardController mainCardController;
	public List<CardController> ingredientControllers = new List<CardController>();
   	public GameObject btnAgree;
	public void ShowSubject(SubjectScript subject, Card mainCard, List<Card> ingredients){
		selectedSubject = subject;
		mainCardController.SetData(mainCard, CanDrag: false);
		for(int i = 0; i < ingredients.Count; i++){
			ingredientControllers[i].SetData(ingredients[i], CanDrag: false);
			ingredientControllers[i].gameObject.SetActive(true);
		}
		for(int i = ingredients.Count; i < ingredientControllers.Count; i++){
			ingredientControllers[i].gameObject.SetActive(false);
		}

		btnAgree.SetActive(false);
		mainPanel.SetActive(true);
	}
	public void Close(){
		selectedReagent?.Disable();
		btnAgree.SetActive(false);
		mainPanel.SetActive(false);
	}
	public Action<SubjectScript, int> action;
	public void SetAction(Action<SubjectScript, int> d){
		selectedReagent = null;
		numSelectedIngredient = -1;
		action = d;
		for(int i = 0; i < ingredientControllers.Count; i++){
			ingredientControllers[i].SetAction(new ReagentIntoSubject());
		}
	}
	SubjectScript selectedSubject;
	int numSelectedIngredient = -1;
	public void AgreeWithSelect(){
		if(isSelect == false){
			isSelect = true;
			if(action != null){
				action(selectedSubject, numSelectedIngredient);
				action -= action;
			}
			Close();
			PanelSubjects.Instance.Close();
		}
	}
	private CardController selectedReagent;
	private bool isSelect = false;
	public void SelectCard(Card card){
		isSelect = false;
		selectedReagent?.Disable();
		numSelectedIngredient = ingredientControllers.FindIndex(x => x.data.reagentName == card.reagentName);
		selectedReagent = ingredientControllers[numSelectedIngredient];
		btnAgree.SetActive(true);
	}
    void Awake(){ instance = this;}
	private static PanelBigSubject instance;
	public static PanelBigSubject Instance{get => instance;}
}
