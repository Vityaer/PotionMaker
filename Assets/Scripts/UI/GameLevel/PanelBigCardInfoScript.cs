using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelBigCardInfoScript : MonoBehaviour{
	public CardController cardController;
	public GameObject mainPanel;
	public static void ShowCard(Card card){
		Instance?.cardController.SetData(card, CanDrag: false);
		Instance?.mainPanel.SetActive(true);
	} 
	public static void Close(){
		Instance?.mainPanel.SetActive(false);
	}
	void Awake(){ instance = this;}
	private static PanelBigCardInfoScript instance;
	public static PanelBigCardInfoScript Instance{get => instance;}

}
