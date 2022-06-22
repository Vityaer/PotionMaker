using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DeckControllerScript : MonoBehaviour{

	public Text textCountCard;
	public GameObject deckVisual;
	[SerializeField] private List<Card> listCard = new List<Card>();
	public bool NotEmpty{get => (listCard.Count > 0);}
	public Card GetCard(){
		int rand = UnityEngine.Random.Range(0, listCard.Count);
		Card result = listCard[rand];
		result.PrerareCard();
		listCard.RemoveAt(rand);
		UpdateUI();
		return result;
	}
	public Card GetNotMagicCard(){
		int rand = UnityEngine.Random.Range(0, listCard.Count);
		Card result = listCard[rand];
		while(result.typeCard == TypeCard.Magic){
			rand = UnityEngine.Random.Range(0, listCard.Count);
			result = listCard[rand];
		}
		result.PrerareCard();
		listCard.RemoveAt(rand);
		if(result != null)
		UpdateUI();
		return result;
	}

	public void DeleteCard(int ID){
		Card card = listCard.Find(x => x.ID == ID);
		if(card != null) listCard.Remove(card);
		UpdateUI();
	}
	private void UpdateUI(){
		textCountCard.text = listCard.Count.ToString();
		if(listCard.Count == 0) deckVisual.SetActive(false);
	}
	void Awake(){
		instance = this;
		UpdateUI();
	}
	public void PutCardInCloset(Card card){
		MoveControllerCard.Instance.CreateMoveCard(card, PointMove.Deck, PointMove.Closet, ClosetScript.Instance.AddCardFromDeck, open: true);
	}
	public void GiveCardInHand(GiveCard giveCard){
		Card card = ReagentsManager.FindCardFromID(giveCard.ID);
		Card deleteCard = listCard.Find(x => (x.reagentName == card.reagentName));
		if(deleteCard != null) listCard.Remove(deleteCard);
		card?.PrerareCard();
		if(giveCard.side == SideGive.Me){
			MoveControllerCard.Instance.CreateMoveCard(card, PointMove.Deck, PointMove.MyHand, CardInHandScript.Instance.RevertCard);
		}else{
			MainGameController.Instance.AddCardOpponent(card, giveCard.ActorNumber);
		}
		UpdateUI();
	}
	public void GiveCardWithoutDelete(GiveCard giveCard){
		Card card = ReagentsManager.FindCardFromID(giveCard.ID);
		if(giveCard.side == SideGive.Me){
			MoveControllerCard.Instance.CreateMoveCard(card, PointMove.Deck, PointMove.MyHand, CardInHandScript.Instance.RevertCard);
		}else{
			MainGameController.Instance.AddCardOpponent(card, giveCard.ActorNumber);
		}
		UpdateUI();
	}
	private static DeckControllerScript instance;
	public static DeckControllerScript Instance{get => instance;}
}