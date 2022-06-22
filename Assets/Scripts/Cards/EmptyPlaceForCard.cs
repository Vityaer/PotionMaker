using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;  
public class EmptyPlaceForCard : MonoBehaviour{
	public RectTransform rect;
	public float duraction = 1f;
	public void StartAnimOpen(){
		StartCoroutine(IFillPlace( rect.sizeDelta.y * 0.625f, duraction ));
	}
	public void StartAnimOpen(int siblingsIndex){
		transform.SetSiblingIndex(siblingsIndex);
		Destroy(gameObject, 3f);
		StartCoroutine(IFillPlace( rect.sizeDelta.y * 0.625f, duraction * 2, destroySelf: false ));
	}
	IEnumerator IFillPlace(float target, float duraction, bool destroySelf = false){
		Vector2 size = new Vector2(0, rect.sizeDelta.y);
		duraction = 7.5f;
		while(size.x < target){
			size.x += target/duraction;
			rect.sizeDelta = size;
			yield return null;   
		}
		rect.sizeDelta = new Vector2(rect.sizeDelta.y * 0.625f, rect.sizeDelta.y);
		if(destroySelf) Destroy(gameObject);
	}
	public void StartAnimClose(int siblingsIndex){
		transform.SetSiblingIndex(siblingsIndex);
		StartCoroutine(IClearPlace( duraction ));
	}

	IEnumerator IClearPlace(float duraction){
		float target = rect.sizeDelta.y * 0.625f;
		Vector2 size = new Vector2(target, rect.sizeDelta.y);
		while(size.x > -3f){
			size.x -= target/duraction * Time.deltaTime;
			if(size.x < -3f) size.x = -3f;
			rect.sizeDelta = size;
			yield return null;   
		}
		Destroy(gameObject);
	}
	public void DestoyPlace(){
		Destroy(gameObject);
	}
}
