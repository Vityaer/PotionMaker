using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
public class MoveCard : MonoBehaviour{
	private Action simpleActionFinish;
	private Transform tr;
	Card data;
	public bool isReady = false;
	private bool finish = false;
	[SerializeField]private OpponentCardController controller;
	void Awake(){
		tr = base.transform;
	}
	public void SetData(Card data, Vector3 start, Vector3 finish, Action action, bool open = false){
        GameControllerScript.Instance.AddAction();
        AudioControllerScript.Instance.PlaySoundOnCardMove();
		simpleActionFinish = action;
		this.data = data;
		tr.position = start;
		if(open) {
			controller?.SetData(data);
			controller?.Open();
		}else{
			controller?.Close();
		}
		this.finish = false;
		tr.DOMove(finish, 1f, true).OnComplete(SimpleActionOnFinish);
	}
	public void Move(Vector3 finish, Action finishAction, float time = 1f){
		simpleActionFinish += finishAction;
		tr.DOMove(finish, time, false).OnComplete(FinishMove);
	}
	private void FinishMove(){
		if(simpleActionFinish != null) simpleActionFinish();
		simpleActionFinish = null;
	}
	void SimpleActionOnFinish(){ 
		if(finish == false){
			finish = true;
			if(simpleActionFinish != null)
				simpleActionFinish();
	        GameControllerScript.Instance.SubAction();
		}
		DisableCard();	
	}
	private Action<Card> actionWithCard;
	public void SetData(Card data, Vector3 start, Vector3 finish, Action<Card> action, bool open = false){
        GameControllerScript.Instance.AddAction();
        AudioControllerScript.Instance.PlaySoundOnCardMove();
		actionWithCard = action;
		this.data = data;
		tr.position = start;
		if(open) {
			controller?.SetData(data);
			controller?.Open();
		}else{
			controller?.Close();
		}
		this.finish = false;
		tr.DOMove(finish, 1f, true).OnComplete(ActionWithCardOnFinish);
	}
	void ActionWithCardOnFinish(){ 
		if(finish == false){
			finish = true;
			if(actionWithCard != null)
				actionWithCard(data);
	        GameControllerScript.Instance.SubAction();
			DisableCard();	
		}
	}
	Vector3 disablePos = new Vector3(1000, 1000, 0);
	private void DisableCard(){
		controller?.Close();
		data = null;
		tr.position = disablePos;
		gameObject.SetActive(false);
		isReady = true; 
	}
	public void EnableCard(){
		gameObject.SetActive(true);
		isReady = false;
		finish = false;
	}
}

public enum PointMove{
	Deck = 0,
	Table = 1,
	Closet = 2,
	LeftOpponent = 3,
	AgainstOpponent = 4,
	RightOpponent = 5,
	MyHand = 6
}