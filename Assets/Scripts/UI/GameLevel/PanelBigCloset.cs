using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
public class PanelBigCloset : MonoBehaviour{
   	public GameObject mainPanel;
   	public List<CardController> controllers = new List<CardController>();
   	private bool isOpen = false;
   	public GameObject btnAgree, btnClose;
	public void ShowCards(List<Card> cards){
		for(int i = 0; i < cards.Count; i++){
			controllers[i].gameObject.SetActive(true);
			controllers[i].SetData(cards[i], CanDrag: false, side: Side.Opponent);
		}
		for(int i = cards.Count; i < controllers.Count; i++)
			controllers[i].gameObject.SetActive(false);
		if(cards.Count == 0){ 
			GameControllerScript.Instance.PrintError(LanguageControllerScript.GetMessage(TypeMessage.ClosetEmpty), PointMove.Closet);
			Close();
		}else{
			btnAgree.SetActive(false);
			mainPanel.SetActive(true);
			SetActionForCards(null);
		}
	} 
	public void Close(){ mainPanel.SetActive(false); }
	public Action<Card> action;
	public void SetAction(Action<Card> d){
		SetActionForCards(new BigClosetReagentAction());
		action = d; 
	}
	CardController selectedController;
	Card selectedCard;
	public void AgreeWithSelect(){
		if(action != null){
			action(selectedCard);
			action -= action;
			ClosetScript.Instance.GetUpReagent(selectedCard);
			ClosetScript.Instance.CloseBigCloset();
			btnClose.SetActive(true);
		}
	}
	public void SelectCard(Card card){
		selectedController?.Disable();
		selectedController = controllers.Find(x => x.data.reagentName == card.reagentName);
		btnAgree.SetActive(true);
		selectedCard = card;
	}
	private void SetActionForCards(CardAction newAction){
		for(int i = 0; i < controllers.Count; i++){
			controllers[i].SetAction(newAction);
		}
	}
	public Button btnCloseComponentAction;  
    void Awake(){ instance = this;}
	private static PanelBigCloset instance;
	public static PanelBigCloset Instance{get => instance;}
}
