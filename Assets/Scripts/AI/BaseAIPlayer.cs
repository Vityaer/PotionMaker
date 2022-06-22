using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAIPlayer : MonoBehaviour{
	public AILevel aiLevel;
	public CardOpponentHandScript cardsController;
	Coroutine corouineWaitCard;
	public void DoStep(PlayerPlaceController cardsController){
		this.cardsController = cardsController as CardOpponentHandScript;
		if(this.cardsController != null){
			(this.cardsController as CardOpponentHandScript).GetCards();
			GameControllerScript.Instance.RegisterOnActionsFinish(IWaitCards);
		}
	}
	public List<Idea> ideasForCreate    = new List<Idea>();
	List<Card> cardInHand               = new List<Card>();
	bool created = false;
	bool putInCloset = false;
	public void IWaitCards(){
		GameControllerScript.Instance.UnregisterOnActionsFinish(IWaitCards);
		created = false;
		putInCloset = false;
		FindIdeas();
	}
	void Awake(){
		instance = this;
	}
	private static BaseAIPlayer instance;
	public static BaseAIPlayer Instance{get => instance;}

	private void TryFindReagents(){
//Magic
		// if(cardInHand.Find(x => x.typeCard == TypeCard.Magic)){

		// }
//
		// List<Card> listReagents = new List<Card>();
		// for(int i = 0; i < ideasForCreate.Count; i++){
		// 	List<ReagentName> listNotEnough = ideasForCreate[i].GetNotEnoughReagent();
		// 	if(listNotEnough.Count == 1){
		// 		Card card = cardInHand.Find(x => x.reagentStore[0] == listNotEnough[0]);
		// 		if(card != null){
		// 			if(listReagents.Find(x => x.reagentStore[0] == listNotEnough[j]) == null)
		// 				listReagents.Add(card);
		// 		}
		// 	}
		// }
		// for(int i = 0; i < cardInHand.Count; i++){
		// 	if(cardInHand[i].TypeCard.Reagent || cardInHand[i].TypeCard.Magic){
		// 		listReagents.Add(cardInHand[i]);
		// 	}
		// }
		if(putInCloset == false){
			if(cardInHand.Find(x => x.typeCard == TypeCard.Magic)){
				cardsController.SendCardToCloset(cardInHand.Find(x => x.typeCard == TypeCard.Magic));
			}else if(ideasForCreate.Count > 0){
				if(ideasForCreate.FindLast(x => x.IsReady == false) != null)
					cardsController.SendCardToCloset(ideasForCreate.FindLast(x => x.IsReady == false).card);
			}else{
				if(cardInHand.Count > 0)
					cardsController.SendCardToCloset(cardInHand[UnityEngine.Random.Range(0, cardInHand.Count)]);
			}
			putInCloset = true;
		}
		if(created == false){
			FindIdeas();
		}else{
			GameControllerScript.Instance.RegisterOnActionsFinish(FinishTurn);
		}
	}

	Idea idea = null;
	List<Card> cardInCloset = new List<Card>();
	List<SubjectScript> subjectInTable = new List<SubjectScript>();
	private void FindIdeas(){
		ideasForCreate.Clear();
		idea = null;
		cardInCloset   = ClosetScript.Instance.GetTopReagents();
		subjectInTable = TableScript.Instance.subjects;
		cardInHand     = cardsController.listCard;
		for(int i = 0; i < cardInHand.Count; i++){
			if((cardInHand[i].typeCard == TypeCard.Potion) || (cardInHand[i].typeCard == TypeCard.Special)){
				idea = new Idea(cardInHand[i]);
				ideasForCreate.Add( idea );
				for(int j = 0; j < cardInHand[i].receipt.Count; j++){
					if((cardInCloset.Find(x => x.reagentStore[0] == cardInHand[i].receipt[j]) != null) || (subjectInTable.Find(x => x.Data.reagentName == cardInHand[i].receipt[j]) != null)){
						idea.Add(cardInHand[i].receipt[j]);
					}
				}
			}
		}
		ideasForCreate.Sort(new IdeaComparer());
		StartCoroutine(CreateIdea());
	}
	IEnumerator CreateIdea(){
		idea = ideasForCreate.Find(x => (x.IsReady == true) );
		if((idea != null) && (created == false)) {
			cardsController.SendCardToTable(idea.card);
			yield return new WaitForSeconds(UnityEngine.Random.Range(1.1f, 1.3f));
			for(int i = 0; i < idea.findedReagents.Count; i++){
				if(cardInCloset.Find(x => x.reagentStore[0] == idea.findedReagents[i]) != null){
					ClosetScript.Instance.SendReagentToWorkBench(idea.findedReagents[i]);
				}else{
					TableScript.Instance.ClickOnSubject(idea.findedReagents[i]);		
				}
				yield return new WaitForSeconds(UnityEngine.Random.Range(0.2f, 0.5f));
			}
			created = true;
		}
		yield return new WaitForSeconds(1.2f);
		if(putInCloset == false){
			TryFindReagents();
		}else{
			GameControllerScript.Instance.RegisterOnActionsFinish(FinishTurn);
		}
	}
	private void FinishTurn(){
		GameControllerScript.Instance.UnregisterOnActionsFinish(FinishTurn);
		GameControllerScript.Instance.FinishStep();
	}
}
[System.Serializable]
public class Idea{
	public Card card;
	public int CountFindReagent{get => findedReagents.Count;}
	public bool IsReady{get => CountFindReagent == card.countRequireIngredient;}
	public List<ReagentName> findedReagents = new List<ReagentName>();
	public void Add(ReagentName reagent){
		if(findedReagents.Count < card.countRequireIngredient){
			findedReagents.Add(reagent);
		}
	}
	public List<ReagentName> GetNotEnoughReagent(){
		List<ReagentName> result = new List<ReagentName>();
		for(int i = 0; i < card.receipt.Count; i++){
			if(findedReagents.Find(x => x == card.receipt[i] ) == null ){
				result.Add(card.receipt[i]);
			}
		}
		return result;
	}
	public Idea(Card card){
		this.card = card;
	}
	public override string ToString(){
		return string.Concat(card.reagentName.ToString(), ": ", "countFindReagent = " , CountFindReagent.ToString(), " , IsReady = ", IsReady.ToString());
	}
}

public class IdeaComparer : IComparer<Idea>{
    public int Compare(Idea idea1, Idea idea2){
    	float weightCountReagent = 3f, weightAmount = 0.8f; 
    	int countReagent1 = idea1.CountFindReagent;
    	int countReagent2 = idea2.CountFindReagent;
    	int amount1       = idea1.card.amountScore;
    	int amount2       = idea2.card.amountScore;
        if (countReagent1 < countReagent2)
            return 1;
        else if (countReagent1 > countReagent2)
            return -1;
        else if((amount1 * weightAmount + countReagent1 * weightCountReagent) < (amount2 * weightAmount + countReagent2 * weightCountReagent))
        	return 1;
        else if((amount1 * weightAmount + countReagent1 * weightCountReagent) > (amount2 * weightAmount + countReagent2 * weightCountReagent))	
            return -1;
        else 
         return 0;    
    }
}  