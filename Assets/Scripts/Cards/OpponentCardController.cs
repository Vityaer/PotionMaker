using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HelpFuction;
using System;
using DG.Tweening;
public class OpponentCardController : MonoBehaviour{
	[Header("UI")]
	public MoveCard moveCotroller;
	public RectTransform rect;
	public GameObject otherSide, outLight;
	public CardUIBase PotionUI, MagicUI, ReagentUI;
	
	public Image BackGroundCard;

	private Card data;
	public Card Data{get => data;}
	void Start(){
		LanguageControllerScript.Instance.RegisterOnChangeLanguage(ChangeLanguage);
		if(data != null){
			data.PrerareCard();
			UpdateUI();
		}
	}
	void DeleteCard(){
		Destroy(gameObject);
	}
	public void SetData(Card data){
		if(this.data != null) HideUI();
		this.data = data;
		UpdateUI();
		HideUI();
	}
	public void SetData(int ID){
		Card card = ReagentsManager.FindCardFromID(ID);
		SetData(card); 
	}
	private void HideUI(){
		switch(data.typeCard){
			case TypeCard.Special:
			case TypeCard.Potion:
				PotionUI?.Hide();
				break;
			case TypeCard.Magic:
				MagicUI?.Hide();
				break;
			case TypeCard.Reagent:
				ReagentUI?.Hide();
				break;					 	
		}
	}
	private void OpenUI(){
		switch(data.typeCard){
			case TypeCard.Special:
			case TypeCard.Potion:
				PotionUI?.Open();
				break;
			case TypeCard.Magic:
				MagicUI?.Open();
				break;
			case TypeCard.Reagent:
				ReagentUI?.Open();
				break;					 	
		}
	}
	private void UpdateUI(){
		BackGroundCard.color = data.colorCard;
		switch(data.typeCard){
			case TypeCard.Special:
			case TypeCard.Potion:
				PotionUI?.UpdateUI(data);
				break;
			case TypeCard.Magic:
				MagicUI?.UpdateUI(data);
				break;
			case TypeCard.Reagent:
				ReagentUI?.UpdateUI(data);
				break;					 	
		}
	}
	public void MoveToCloset(){
		transform.SetParent(CardOpponentHandScript.Instance.mainBackground);
		Vector3 start = MoveControllerCard.Instance.GetPosition(CardOpponentHandScript.Instance.pointMove);
		Vector3 finish = MoveControllerCard.Instance.GetPosition(PointMove.Closet);
		moveCotroller.SetData(data, start, finish, ActionToCloset, open: true);
	}
	private void ActionToCloset(){
		ClosetScript.Instance.PutInCard(data, isMyCard: false);
		Delete();
	}
	public void MoveToTable(){
		transform.SetParent(CardOpponentHandScript.Instance.mainBackground);
		Vector3 start = MoveControllerCard.Instance.GetPosition(CardOpponentHandScript.Instance.pointMove);
		Vector3 finish = MoveControllerCard.Instance.GetPosition(PointMove.Table);
		moveCotroller.SetData(data, start, finish, ActionToTable, open: true);
	}
	private void ActionToTable(){
		TableScript.Instance.PutDownCard(data, isMyCraft: false);
		Delete();
	}
	public void Delete(){
		CardOpponentHandScript.Instance.DeleteCard(this, data);
		Destroy(gameObject);
	}
	public void Open(){
		if(data != null) OpenUI();
		otherSide.SetActive(false);
	}
	public void Close(){
		if(data != null) HideUI();
		otherSide.SetActive(true);
	}
	void OnDestroy(){
		LanguageControllerScript.Instance.UnRegisterOnChangeLanguage(ChangeLanguage);
	}
	private void ChangeLanguage(){
		data.UpdateLocalization();
		UpdateUI();
	}
}