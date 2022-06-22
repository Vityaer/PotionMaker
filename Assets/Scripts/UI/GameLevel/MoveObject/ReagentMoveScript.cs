using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class ReagentMoveScript : MonoBehaviour{
	public bool isReady = false;
	private bool finish = false;
	public ReceiptReagentUI mainReagent;
	private Action<Card> actionOnFinish;
	public Transform tr;
	public float timeMove = 0.1f;
	private Card data;
	Tween sequenceMove;
	bool createCard = false;
	public void SetData(Card card, Vector3 start, Vector3 finish, Action<Card> action, bool createCard = true){ 
		this.data = card;
		mainReagent.SetData(createCard ? card.reagentStore[0] : card.reagentName, card.colorCard);
		this.createCard = createCard;
		tr.position = start;
		GameControllerScript.Instance.AddAction();
		actionOnFinish = action;
		sequenceMove = DOTween.Sequence().Append(tr.DOMove(finish, timeMove, true).OnComplete(SimpleActionOnFinish));
		WorkBench.Instance.RegisterOnOpenClose(OnCloseWorkBench);
	}
	void SimpleActionOnFinish(){ 
		if(finish == false){
			finish = true;
			if(actionOnFinish != null){
				WorkBench.Instance.createCard = this.createCard;
				actionOnFinish(data);
			}
			WorkBench.Instance.UnregisterOnOpenClose(OnCloseWorkBench);
			DisableReagent();	
		}
	}
	Vector3 disablePos = new Vector3(1000, 1000, 0);
	private void DisableReagent(){
	    GameControllerScript.Instance.SubAction();
		data = null;
		tr.position = disablePos;
		gameObject.SetActive(false);
		isReady = true; 
	}
	public void EnableReagent(){
		gameObject.SetActive(true);
		isReady = false;
		finish = false;
	}
	public void OnCloseWorkBench(bool flag){
		if(flag == false){
			if(finish == false){
				actionOnFinish = null;
				sequenceMove.Kill();
				sequenceMove = null;
				Vector3 finishPosition = ClosetScript.Instance.GetPosition(data);
			    GameControllerScript.Instance.SubAction();
				SetData(data, tr.position, finishPosition, ClosetScript.Instance.RevertIngredient);
			}

		}
	}

}
