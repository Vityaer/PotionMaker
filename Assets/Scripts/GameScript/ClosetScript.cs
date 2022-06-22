using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
public class ClosetScript : MonoBehaviour{
	private static ClosetScript instance;
	public static ClosetScript Instance{get => instance;}
	private List<ListCardWithReagent> listReagents = new List<ListCardWithReagent>();
	public Transform content;
	public GameObject prefabReagent;
	public bool manyAction = true;
	public bool canPutInCard = true;
    void Awake(){ instance = this; }
    void Start(){
    	GameControllerScript.Instance.RegisterOnStartStep(Refresh); 
    }
    public void ClickCloset(){
    	OnClickCloset();
    }
    public void PutInCard(Card card, bool isMyCard = true, bool withReward = true){
        AudioControllerScript.Instance.PlaySoundClickOnButton();
    	AddNewCard(card, isMyCard, withReward);
    	if(manyAction == false) canPutInCard = false;
		OnFinishCraft();
		if(showCloset) OpenBigCloset();
    }
    public void AddNewCard(Card card, bool isMyCard, bool withReward = true){
			ListCardWithReagent list = listReagents.Find(x => x.reagentName == card.reagentStore[0]);
			if(list != null){
				list.AddCard(card);
			}else{
				if(withReward){
					AudioControllerScript.Instance.PlayClosetSuccess();
					if(isMyCard){
						ScoreControllerScript.Instance.AddPointToMe((card.typeCard == TypeCard.Special) ? 3 : 1);
			    	}else{
						ScoreControllerScript.Instance.AddPointToOpponent((card.typeCard == TypeCard.Special) ? 3 : 1); 
					}
				}
				ReagentController reagentController = Instantiate(prefabReagent, content).GetComponent<ReagentController>();
				listReagents.Add(new ListCardWithReagent(card, reagentController));
				RecalculateUI();
			}
	}
	public void RevertIngredients(List<Card> cards){
		foreach(Card card in cards){ AddNewCard(card, true, false); }
		if(showCloset) OpenBigCloset();
	}
	public void RevertIngredient(Card card){
		AddNewCard(card, true, false);
		if(showCloset) OpenBigCloset();
	}
	public Vector3 GetPosition(Card card){
		ListCardWithReagent list = listReagents.Find(x => x.reagentName == card.reagentStore[0]);
		if(list != null){
			return list.reagentController.transform.position;
		}else{
			return MoveControllerCard.Instance.GetPosition(PointMove.Closet);
		}
	}
	public void RemoveReagent(Card card){
		ListCardWithReagent list = listReagents.Find(x => x.reagentName == card.reagentStore[0]);
		list.RemoveCard();
		if(list.CountCard == 0) listReagents.Remove(list);
		if(showCloset) OpenBigCloset();
	}

	public void Refresh(){
		canPutInCard = true;
		if(waitFinishMoveCard != null){
			StopCoroutine(waitFinishMoveCard);
			waitFinishMoveCard = null;
		}
	}
	private Action observerCraft;
	public void RegisterOnFinishCraft(Action d){observerCraft += d;}
	public void UnregisterOnFinishCraft(Action d){observerCraft -= d;}
	private void OnFinishCraft(){
		if(observerCraft != null) observerCraft();
	}

	private Action observerClick;
	public void RegisterOnClickCloset(Action d){observerClick += d;}
	public void UnregisterOnClickCloset(Action d){observerClick -= d;}
	private void OnClickCloset(){
    	if(canPutInCard){
			if(observerClick != null) observerClick();
    	}else{
    		CardInHandScript.Instance.DropCard();
    	}
	}
	[SerializeField] private GridLayoutGroup gridLayoutGroup;
    private void RecalculateUI(){ gridLayoutGroup?.CalculateLayoutInputHorizontal(); }
    private bool showCloset = false;
    public void OpenBigCloset(){
    	PanelBigCloset.Instance.ShowCards(GetTopReagents());
    	showCloset = true;
    }
    public void CloseBigCloset(){
    	showCloset = false;
    	PanelBigCloset.Instance.Close();
    }
    public List<Card> GetTopReagents(){
    	List<Card> result = new List<Card>();
    	for(int i = 0; i < listReagents.Count; i++)
    		result.Add(listReagents[i].GetCard());
    	return result;	
    }
    public void GetUpReagent(Card card){
    	RemoveReagent(card);
    	GameControllerScript.Instance.Contoller.AddCard(card, PointMove.Closet);
    }
//API
    public void AddCardFromDeck(Card card){ PutInCard(card, isMyCard: false, withReward: false); }
    public void ChangeReagentMagicSwap(Card oldReagent, Card newReagent){
    	RemoveReagent(oldReagent);
    	AddNewCard(newReagent, false, false);
    }
    Coroutine waitFinishMoveCard;
    public void SendReagentToWorkBench(Card card){
    	SendReagentToWorkBench(card.reagentStore[0]);
    }
    public void SendReagentToWorkBench(ReagentName type){
	    ListCardWithReagent list = listReagents.Find(x => x.reagentName == type);
    	if((list == null) && (GameControllerScript.Instance.currentAction > 0)){
    		waitFinishMoveCard = StartCoroutine(IWaitFinishMoveCard(list.GetCard()));
    	}else{
    		list.MoveToWorkBench();
	    	RemoveReagent(list.GetCard());	
    	}	
    }
    IEnumerator IWaitFinishMoveCard(Card card){
    	while(GameControllerScript.Instance.currentAction > 0){
    		yield return null;
    	}
    	ListCardWithReagent list = listReagents.Find(x => x.reagentName == card.reagentStore[0]);
    	if(list != null){
    		list.MoveToWorkBench();
	    	RemoveReagent(card);	
    	}else{
    		GameControllerScript.Instance.PrintError(LanguageControllerScript.GetMessage(TypeMessage.ClosetNotContainsReagent));
    	}
    }
}

[System.Serializable]
public class ListCardWithReagent{
	public ReagentName reagentName;
	Stack<Card> stack = new Stack<Card>();
	public ReagentController reagentController;
	public void AddCard(Card card){
		stack.Push(card);
		reagentController.IncreaseCountCard();
		reagentController.SetData(card);
	} 
	public Card GetCard(){
		return stack.Peek();
	}
	public void RemoveCard(){
		stack.Pop();
		reagentController.DecreaseCountCard();
		if(stack.Count > 0){
			reagentController.SetData( GetCard() );
		}else{
			Clear();
		}
	}
	public void MoveToWorkBench(){
		reagentController.MoveToWorkBench();
	}
	private void Clear(){
		reagentController.Clear();
	}
	public int CountCard{get => stack.Count;}
	public ListCardWithReagent(Card card, ReagentController reagentController){
		this.reagentController = reagentController;
		reagentName = card.reagentStore[0];
		AddCard(card);
	}
}