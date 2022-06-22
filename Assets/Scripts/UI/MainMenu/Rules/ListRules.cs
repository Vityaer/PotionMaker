using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class ListRules : MonoBehaviour{
	 private Vector3 startPosition;
	private Vector3 endPosition;
	private float dragDistance;
	void Start(){
		dragDistance = Screen.width*30/100;
		// curSheet = ListCitySheet.Count / 2;
		curSheet = 0;
		SetStartPosition();
	}
	void Update(){
		if (Input.GetMouseButtonDown(0)){
			startPosition  = Input.mousePosition;
		}
		if (Input.GetMouseButtonUp(0)){
			endPosition  = Input.mousePosition;
			if(Mathf.Abs(endPosition.y - startPosition.y) < dragDistance){
				if(endPosition.x - startPosition.x > dragDistance){
					SwipeLeft();
				}else if(startPosition.x - endPosition.x > dragDistance){
					SwipeRight();
				}
			}
		}
	}
	private int curSheet = 0;
	public List<RectTransform> ListCitySheet = new List<RectTransform>();
	public void SwipeLeft(){
			ListCitySheet[curSheet].DOAnchorPos(new Vector2(Screen.width * 2, 0f), 0.25f);
			if(curSheet > 0) curSheet--;
			ListCitySheet[curSheet].DOAnchorPos(Vector2.zero, 0.25f);
	}
	public void SwipeRight(){
			ListCitySheet[curSheet].DOAnchorPos(new Vector2(-Screen.width * 2, 0f), 0.25f);
			if(curSheet < ListCitySheet.Count - 1) curSheet++;
			ListCitySheet[curSheet].DOAnchorPos(Vector2.zero, 0.25f);
	}
	private void SetStartPosition(){
		for(int i=0; i < curSheet; i++){
			ListCitySheet[i].DOAnchorPos(new Vector2(-Screen.width * 2, 0f), 0f);
		}
		for(int i=curSheet + 1; i < ListCitySheet.Count; i++){
			ListCitySheet[i].DOAnchorPos(new Vector2(Screen.width  * 2, 0f), 0f);
		}
	}
}
