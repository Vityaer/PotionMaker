using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class PanelMessageScript : MonoBehaviour{
	public Text textMessage;
	public RectTransform rect;
    public void Show(){
    	if(CanvasMessage.Instance != null){
			CanvasMessage.Instance.SwitchCanvas(true);
			rect.DOScale(Vector2.one * 1.1f, 0.25f);
    	}
		void StageTwo(){
			rect.DOScale(Vector2.one, 0.1f);
		}
	}
	public void FinishShow(){
		rect.DOScale(Vector2.zero, 0.25f).OnComplete(SwitchOffCanvas);
	}
	private void SwitchOffCanvas(){
		if(CanvasMessage.Instance != null)
		CanvasMessage.Instance.SwitchCanvas(false);
	}
}
