using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasMessage : MonoBehaviour{

	public Canvas canvas;
	void Awake(){
		instance = this;
	}
	public void SwitchCanvas(bool flag){canvas.enabled = flag;}
	private static CanvasMessage instance;
	public static CanvasMessage Instance{get => instance;}
}