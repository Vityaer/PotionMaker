using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OpenClosePanelScript : MonoBehaviour{
	private Transform tr;
	public Transform closePoint, openPoint;
	public TypeState typeStartState;
	Vector3 baseDelta = Vector3.zero;
	public float timeAnimation = 0.6f;
	void Awake(){
		tr = base.transform;
	}
	void Start(){
		tr.position = (typeStartState == TypeState.Open) ? openPoint.position : closePoint.position;
		baseDelta = Vector3.Normalize(openPoint.position - closePoint.position) * 20f;
	}
	Tween sequenceMove;
	Vector3 delta, openPosition;
	public void Open(Transform openPoint = null, bool inertia = true){
		if(openPoint != null){
			openPosition = openPoint.position;
			delta = inertia ? Vector3.Normalize(openPoint.position - closePoint.position) * 20f : Vector3.zero;
		}else{
			openPosition = this.openPoint.position;
			delta = baseDelta;
		}
		gameObject.SetActive(true);
		sequenceMove = DOTween.Sequence()
						.Append(tr.DOMove(openPosition, timeAnimation, false))
						.Append(tr.DOMove(openPosition + delta, 0.15f, false))
						.Append(tr.DOMove(openPosition, 0.1f, false));
	}
	public void Close(){
		tr.DOMove(closePoint.position, timeAnimation, false).OnComplete(() => {gameObject.SetActive(false);});
	}
	public enum TypeState{
		Open,
		Close
	}
}
