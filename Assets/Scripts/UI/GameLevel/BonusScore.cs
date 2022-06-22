using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class BonusScore : MonoBehaviour{
	public Text textComponent;
	private RectTransform tr;
	[SerializeField]private Vector3 startPosition;
	public float deltaY = 2f;
	[SerializeField]private Vector3 finishPosition, startpos;
	private bool iMove = false;
	public Color startColor, finishColor;
	Sequence mySequenceBonus;
	void Awake(){
		textComponent.text = string.Empty;
		tr = GetComponent<RectTransform>();
		startpos = tr.position;
	}
	public void Show(int point){
		if(mySequenceBonus != null){
			mySequenceBonus.Kill();
			mySequenceBonus = null;
		}
		tr.position = startPosition;
		textComponent.color = startColor;
		textComponent.text = string.Empty;
		textComponent.text = string.Concat("+" , point.ToString());
		mySequenceBonus = DOTween.Sequence();
		mySequenceBonus.Append( tr.DOMove(finishPosition, 1f , false));
		mySequenceBonus.Append(textComponent.DOColor(finishColor, 0.5f));
	}
	Side side;
	public void SetSide(Side side){
		tr = GetComponent<RectTransform>();
		this.side = side;
		startPosition = tr.position;
		finishPosition = startPosition + new Vector3(0, (side == Side.Me) ? deltaY : -deltaY, 0);
	}
}
