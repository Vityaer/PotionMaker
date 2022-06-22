using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class UIPanelControllerScript : MonoBehaviour{
	public Side side;
	[SerializeField] private RectTransform panel;
	public void Open(){
		if(side == Side.Me){
			opponentCardPanel.Close();
		}else{
			myCardPanel.Close();
		}
		gameObject.SetActive(true);
		panel.DOAnchorPos(new Vector2(0f, 0f), 0.25f);
	}
	public void Close(){
		panel.DOAnchorPos(new Vector2(0f, -Screen.height * 0.25f), 0.05f).OnComplete(OnFinishClose);
	}
	private void OnFinishClose(){
		gameObject.SetActive(false);
	}
	void Awake(){
		if(side == Side.Me){
			myCardPanel = this;
		}else{
			opponentCardPanel = this;
		}
	}
	private static UIPanelControllerScript myCardPanel, opponentCardPanel;
	public static UIPanelControllerScript MyCardPanel{get => myCardPanel;}
	public static UIPanelControllerScript OpponentCardPanel{get => opponentCardPanel;}
}