using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class MoveControllerCard : MonoBehaviour{
	public Transform mainPanel;
	public GameObject prefabMoveCard;
	public Transform Deck, Table, Closet, LeftOpponent, AgainstOpponent, RightOpponent, MyHand;
	private List<MoveCard> moveCards = new List<MoveCard>();  
	public void CreateMoveCard(Card data, PointMove start, PointMove finish, Action action, bool open = false){
        if(start == PointMove.Deck) open = false;
        GetFreeMoveCard().SetData(data, GetPosition(start), GetPosition(finish), action, open);
	}
	public void CreateMoveCard(Card data, PointMove start, PointMove finish, Action<Card> action, bool open = false){
        if(start == PointMove.Deck) open = false;
        GetFreeMoveCard().SetData(data, GetPosition(start), GetPosition(finish), action, open);
	}
	public void CreateMoveCard(Card data, PointMove start, Vector3 finish, Action<Card> action, bool open = false){
        if(start == PointMove.Deck) open = false;
        GetFreeMoveCard().SetData(data, GetPosition(start), finish, action, open);
	}
	private MoveCard GetFreeMoveCard(){
		MoveCard result = moveCards.Find(x => (x.isReady));
		if(result == null){
		 	result = Instantiate(prefabMoveCard, mainPanel).GetComponent<MoveCard>();
			moveCards.Add(result);
		}else{
			result.EnableCard();
		}
		return result;
	}
	private static MoveControllerCard instance;
	public static MoveControllerCard Instance{get => instance;}
	void Awake(){
		instance = this;
	}
	public Vector3 GetPosition(PointMove point){
		Transform result = null;
		switch(point){
			case PointMove.Deck:
				result = Deck;
				break;
			case PointMove.Table:
				result = Table;
				break;
			case PointMove.Closet:
				result = Closet;
				break;
			case PointMove.LeftOpponent:
				result = LeftOpponent;
				break;
			case PointMove.AgainstOpponent:
				result = AgainstOpponent;
				break;
			case PointMove.RightOpponent:
				result = RightOpponent;
				break;
			case PointMove.MyHand:
				result = MyHand;
				break;					
		}
		return result.position; 
	}
}
