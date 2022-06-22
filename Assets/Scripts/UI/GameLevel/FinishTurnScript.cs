using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class FinishTurnScript : MonoBehaviour{
	public GameObject buttonFinishStep;
	public Transform tr;
	public Transform openPoint, closePoint;
	private Vector3 delta = new Vector3(15, 0, 0);
	private bool myTurn = false;
	public void SetTurn(bool flag){
		myTurn = flag;
		if(flag == true){
			TableScript.Instance.RegisterOnFinishCraft(CanFinishTurn);
			ClosetScript.Instance.RegisterOnFinishCraft(CanFinishTurn);
		}else{
			AnimOpen();
		}
	}
	public void CanFinishTurn(){
		TableScript.Instance.UnregisterOnFinishCraft(CanFinishTurn);
		ClosetScript.Instance.UnregisterOnFinishCraft(CanFinishTurn);
		AnimOpen();
	}
	public void ClickButton(){
		sequenceMove.Kill();
		sequenceMove = null;
		GameControllerScript.Instance.FinishStep();
		AnimClose();
	}
	Tween sequenceMove;
	private void AnimOpen(){
		buttonFinishStep.SetActive(true);
		sequenceMove = DOTween.Sequence()
						.Append(tr.DOMove(openPoint.position, 0.35f, false))
						.Append(tr.DOMove(openPoint.position + delta, 0.15f, false))
						.Append(tr.DOMove(openPoint.position, 0.1f, false));
	}
	private void AnimClose(){
		tr.DOMove(closePoint.position, 0.5f, false).OnComplete(() => {buttonFinishStep.SetActive(false);});
	}
}