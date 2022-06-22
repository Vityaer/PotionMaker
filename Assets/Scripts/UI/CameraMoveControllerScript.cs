using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class CameraMoveControllerScript : MonoBehaviour{
	private Transform tr;
	private static CameraMoveControllerScript instance;
	public static CameraMoveControllerScript Instance{get => instance;}
	void Awake(){
		instance = this;
		tr = GetComponent<Transform>();
	}
	RaycastHit2D hit;
	void Update(){
		if (Input.GetMouseButtonDown(0)){
	        Vector2 CurMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        	hit = Physics2D.Raycast(CurMousePos, Vector2.zero);
			if (hit != null){
				if(hit.transform != null){
	            	Debug.Log("попали в объект " + hit.transform.name);
				}else{
	            	Debug.Log("нет transform");
				}
            }else{
	            	Debug.Log("нет transform");
            }
		}
	}
}