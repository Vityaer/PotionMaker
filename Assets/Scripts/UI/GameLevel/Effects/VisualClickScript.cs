using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class VisualClickScript : MonoBehaviour{
    public RectTransform rect;
	private Vector2 startSize = new Vector2(1f, 1f);
	private float minSize = 0.95f;
	void Start(){
		if(rect == null) rect = GetComponent<RectTransform>();
	}
	Tween sequenceScalePulse = null;
	public void Squezze(){
		if(sequenceScalePulse != null) sequenceScalePulse.Kill();
		sequenceScalePulse = DOTween.Sequence().Append(rect.DOScale(startSize * minSize, 0.1f));
						
	}
	public void Expansion(){
		if(sequenceScalePulse != null) sequenceScalePulse.Kill();
		sequenceScalePulse = DOTween.Sequence().Append(rect.DOScale(startSize, 0.25f));
	}
}
