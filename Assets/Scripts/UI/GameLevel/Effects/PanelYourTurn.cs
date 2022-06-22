using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using HelpFuction;
public class PanelYourTurn : MonoBehaviour{
	public RectTransform rect;

	void Awake(){
		instance = this;
	}
	public void Show(){
		AudioControllerScript.Instance?.PlaySoundYourTurn();
		rect.DOScale(Vector2.one * 1.1f, 0.25f).OnComplete(StageTwo);
		TimerScript.Timer.StartTimer(0.75f, FinishShow);
		void StageTwo(){
			rect.DOScale(Vector2.one, 0.1f);
		}
	}
	void FinishShow(){
		rect.DOScale(Vector2.zero, 0.25f);
	}
    private static PanelYourTurn instance;
    public static PanelYourTurn Instance{get => instance;}
}
