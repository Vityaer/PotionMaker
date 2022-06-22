using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
public class WorkBench : MonoBehaviour{
	[Header("UI")]
	public GameObject prefabCard, prefabIngredient;
	public Transform mainPlace;
	public GameObject panelTwoReagent, panelTreeReagent, workbench;
	public List<Transform> twoReagent = new List<Transform>(), treeReagent = new List<Transform>(); 
	public Button buttonClose;
	[Header("data")]
	private List<GameObject> CreatedIngredient = new List<GameObject>();
	private Card card;
	List<ReagentName> receipt = new List<ReagentName>();
	List<Card> cardInReceipt = new List<Card>();
	private bool isMyCraft = true;
	public Side SideCraft{get => (isMyCraft) ? Side.Me : Side.Opponent; }
	public void StartCrafting(Card card, bool isMyCraft){
		isCreate = false;
		ScoreControllerScript.Instance.Refresh();
		this.isMyCraft = isMyCraft;
		numPlace = 0;
		numIngredient = 0;
		GameObject gameObjectCard = Instantiate(prefabCard, mainPlace);
		gameObjectCard.GetComponent<CardController>().SetData(card, CanDrag: false);
		CreatedIngredient.Add(gameObjectCard);
		this.card = card;
		this.card.PrerareCard();
		if(isMyCraft) {
			PanelBigWorkbench.Instance.Open();
			OnOpenClose(true);
		}
		workbench.SetActive(true);
		if(card.countRequireIngredient == 2){ panelTwoReagent.SetActive(true); }else{ panelTreeReagent.SetActive(true); }
	}
	public void StartCancelCraft(){
		if(this.card != null){
			if(GameControllerScript.Instance.MyStep){
				CommandCenterScript.Instance?.Sender?.SendCommand(commandWorkbench);
				CancelCraft();
			}
		}
	}
	public void StartMagicAction(Card card, bool isMyCraft){
		this.isMyCraft = isMyCraft;
		this.card = card;
	}
	public void CancelCraft(){
		if(this.card != null){
			if(isCreate == false){
				for(int i = 0; i <  cardInReceipt.Count; i++){
					Vector3 startPosition = ((card.countRequireIngredient == 2) ? twoReagent[i] : treeReagent[i]).position;
					MoveReagentsSystemScript.Instance.CreateReagentMove(cardInReceipt[i], startPosition, ClosetScript.Instance.GetPosition(cardInReceipt[i]), ClosetScript.Instance.RevertIngredient);
				}
				if(isMyCraft){
					CardInHandScript.Instance.RevertCard(this.card);
				}else{
					CardOpponentHandScript.Instance.CreateCard(this.card);
				}
			}
		}
		Close();
	}
	public void ClearWorckbench(){
		this.card = null;
	}
	private void Close(){
		isCreate = false;
		numPlace = 0;
		numIngredient = 0;
		if(isMyCraft) PanelBigWorkbench.Instance.Close();
		this.card = null;
		receipt.Clear();
		cardInReceipt.Clear();
		ScoreControllerScript.Instance.Refresh();
		foreach(GameObject ingredient in CreatedIngredient){ Destroy(ingredient); }
		panelTreeReagent.SetActive(false);
		panelTwoReagent.SetActive(false);
		workbench.SetActive(false);
		OnOpenClose(false);
	}

	int numIngredient = 0;
	public bool CheckIngredient(Card newCard, bool createCard = true){
		List<ReagentName> ingredients = createCard ? ingredients = newCard.reagentStore : new List<ReagentName>{newCard.reagentName}; 
		bool result = false;
		for(int i = 0; i < ingredients.Count; i++){
			if( (card.receipt.Contains(ingredients[i]) == true) && (receipt.Contains(ingredients[i]) == false) ){
				result = true;
				break;
			}
		}
		return result;
	}
	public bool createCard = true;
	public void AddIngredient(Card newCard){
		AddIngredient(newCard, this.createCard);
	}
	public void AddIngredient(Card newCard, bool createCard = true){
		List<ReagentName> ingredients = createCard ? ingredients = newCard.reagentStore : new List<ReagentName>{newCard.reagentName}; 
		int num = 0;
		for(int i = 0; i < ingredients.Count; i++){
			if( (card.receipt.Contains(ingredients[i]) == true) && (receipt.Contains(ingredients[i]) == false) ){
				receipt.Add(ingredients[i]);
				num = i;
				break;
			}
		}	
		if(createCard) cardInReceipt.Add(newCard);		
		GameObject ingredient = Instantiate(prefabIngredient, (card.countRequireIngredient == 2) ? twoReagent[numIngredient] : treeReagent[numIngredient]);
		ingredient.GetComponent<ReagentController>().SetData(ingredients[num]);
		CreatedIngredient.Add(ingredient);
		numIngredient++;
		if(receipt.Count == card.countRequireIngredient){
			StartCoroutine(ICoroutineCreateSubject());
		}
	}
	public bool isCreate = false;
	IEnumerator ICoroutineCreateSubject(){
		buttonClose.interactable = false;
		isCreate = true;
		yield return new WaitForSeconds(0.1f);
		if(isMyCraft){
			ScoreControllerScript.Instance.AddPointToMe(card.amountScore);
		}else{
			ScoreControllerScript.Instance.AddPointToOpponent(card.amountScore);
		}
		TableScript.Instance.CreateSubjects(card, cardInReceipt, isMyCraft: isMyCraft);
		Close();
		buttonClose.interactable = true;
	}
	int numPlace = 0;
	public Vector3 GetPositonEmptyPlace(){ return ((card.countRequireIngredient == 2) ? twoReagent[numPlace++] : treeReagent[numPlace++]).position;}
	private static NetworkCommand commandWorkbench = new NetworkCommand(TypeCommand.CancelCrafting);
	public Action<bool> observerOpenClose;
	public void RegisterOnOpenClose(Action<bool> d) { observerOpenClose += d;} 
	public void UnregisterOnOpenClose(Action<bool> d) { observerOpenClose -= d;} 
	private void OnOpenClose(bool isOpen){ if(observerOpenClose != null) observerOpenClose(isOpen); }

	void Awake(){
		instance = this;
	}
	private static WorkBench instance;
	public static WorkBench Instance{get => instance;}
}