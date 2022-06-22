using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;
public class CardController : MonoBehaviour{

	[Header("UI")]
	public RectTransform rect;
	private Vector2 startSize;
	public CardUIBase PotionUI, MagicUI, ReagentUI;

	public GameObject otherSide, outLight;

	public Image BackGroundCard;
	public int siblingsIndex = 0;
	public MoveCard moveController; 
	[Header("Data")]
	public Card data;
	void Start(){
		startSize = rect.localScale;
		LanguageControllerScript.Instance.RegisterOnChangeLanguage(ChangeLanguage);
		CardController.RegisterOnDrag(IWork);
		if(data != null){
			data.PrerareCard();
			UpdateUI();
		}
	}
	void Update(){
		if (Input.GetMouseButtonUp(0)){
			OnClickUp();
		}
	}
	public void DeleteCard(){
		Destroy(gameObject);
	}
	public Side side;
	public void SetData(Card data, bool CanDrag = true, Side side = Side.Me){
		siblingsIndex = transform.GetSiblingIndex();
		if(this.data != null) HideUI();
		this.data = data;
		this.CanDrag = CanDrag;
		this.side = side;
		UpdateUI();
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
	public static CardController selectedCard;

	public void Disable(){
		rect.DOScale(startSize, 0.25f);
		BackGroundCard.raycastTarget = true;
		outLight.SetActive(false);
		TableScript.Instance.UnregisterOnClickTable(MoveToTable);
		ClosetScript.Instance.UnregisterOnClickCloset(MoveToCloset);
	}
	private void MoveToTable(){
		if(data.typeCard != TypeCard.Reagent){
			GameControllerScript.Instance.UseCard = data;
			TableScript.Instance.PutDownCard(data);
			commandCard.SetData(TypeCommand.PutOnTable, new GiveCard(data.ID, SideGive.Opponent, MainGameController.Instance.GetMyActorNumber(), isOrderFromServer: true) );
			CommandCenterScript.Instance?.Sender?.SendCommand(commandCard);
			CardInHandScript.Instance.DeleteCard(this);
			Disable();
			Destroy(gameObject);
		}else{
			Drop();
		}
	}
	private void MoveToCloset(){
		ClosetScript.Instance.PutInCard(data);
		commandCard.SetData(TypeCommand.PuInCloset, new CardPutInCloset(data.ID, SideGive.Opponent, true));
		CommandCenterScript.Instance?.Sender?.SendCommand(commandCard);
		CardInHandScript.Instance.DeleteCard(this);
		Disable();
		Destroy(gameObject);
	}
	void OnDestroy(){
		CardInHandScript.Instance?.AnimOpen();
		OnDrag(false);
		selectedCard = null;
		CardController.UnregisterOnDrag(IWork);
		TableScript.Instance?.UnregisterOnClickTable(MoveToTable);
		ClosetScript.Instance?.UnregisterOnClickCloset(MoveToCloset);
		LanguageControllerScript.Instance.UnRegisterOnChangeLanguage(ChangeLanguage);
	}
	public void SetActive(bool flag){ gameObject.SetActive(flag); }
	private static NetworkCommand commandCard = new NetworkCommand(TypeCommand.PutOnTable);

	public void StartDrag(){
		if(CardInHandScript.IsMyStep && side == Side.Me){
			siblingsIndex = transform.GetSiblingIndex();
			transform.SetParent(CardInHandScript.Instance.mainBackground);
			CardInHandScript.Instance.CreateEmptyPlaceClose(siblingsIndex);
			drag = true;
			StopCoroutine(coroutineShowCard);
			coroutineShowCard = null;
			OnClickUp();
			if(selectedCard != null) selectedCard.Drop();
			BackGroundCard.raycastTarget = false;
			AudioControllerScript.Instance?.PlaySoundClickOnButton();
			selectedCard = this;
			TableScript.Instance.RegisterOnClickTable(MoveToTable);
			ClosetScript.Instance.RegisterOnClickCloset(MoveToCloset);
			OnDrag(true);
			CardInHandScript.Instance.AnimClose();
		}
	}
	Vector2 posMouse;
	public bool drag = false;
	private bool CanDrag = true;
	public void Drag(){
		if(CanDrag){
			if(CardInHandScript.IsMyStep){
				if(drag == false) StartDrag();
				posMouse = Input.mousePosition;
				rect.position = posMouse;
			}
		}
	}
	EmptyPlaceForCard controllerEmptyPlace;
	public void Drop(){
		if((CardInHandScript.Instance != null) && (selectedCard != null) && (side == Side.Me)){
			drag = false;
			siblingsIndex = (rect.localPosition.y > (Screen.height * 0.35f)) ? siblingsIndex : CardInHandScript.Instance.GetSiblingsIndex(transform.position);
			controllerEmptyPlace = CardInHandScript.Instance.CreateEmptyPlaceOpen(siblingsIndex);
			moveController.Move(CardInHandScript.Instance.GetPosForCard(siblingsIndex), SetInPlace, 0.25f);
			CardInHandScript.Instance.AnimOpen();
			selectedCard = null;
		}
	}
	public void SetInPlace(){
		controllerEmptyPlace.DestoyPlace();
		transform.SetParent(CardInHandScript.Instance.content);
		transform.SetSiblingIndex(siblingsIndex);
		CardInHandScript.Instance.RecalculateUI();
		OnDrag(false);
		selectedCard = null;
		Disable();
	}
	public void OnClickDown(){
		outLight.SetActive(true);
		rect.DOScale(startSize * 1.1f, 0.25f);
		if(overrideAction == null){
			coroutineShowCard = StartCoroutine(IWaitForShowCard());
		}else{
			overrideAction.ClickDown(this.data);		
		}
	}
	public void OnClickUp(){
		if(coroutineShowCard != null){
			StopCoroutine(coroutineShowCard);
			coroutineShowCard = null;
		}
		if(overrideAction == null) outLight.SetActive(false);
		rect.DOScale(startSize, 0.25f);
		OnDrag(false);
		PanelBigCardInfoScript.Close();
	}
	private static Action<bool> observerDrag;
	public static void RegisterOnDrag(Action<bool> d){ observerDrag += d;}
	public static void UnregisterOnDrag(Action<bool> d){ observerDrag -= d;}
	private void OnDrag(bool isDrag){ if(observerDrag != null) observerDrag(isDrag); }

	Coroutine coroutineShowCard;
	IEnumerator IWaitForShowCard(){
		yield return new WaitForSeconds(0.5f);
		PanelBigCardInfoScript.ShowCard(data);
	}
	CardAction overrideAction = null; 
	public void SetAction(CardAction newAction){
		overrideAction = newAction;
	}
	private void IWork(bool dragCard){ BackGroundCard.raycastTarget = !dragCard; }
	private void ChangeLanguage(){
		data.UpdateLocalization();
		UpdateUI();
	} 
}