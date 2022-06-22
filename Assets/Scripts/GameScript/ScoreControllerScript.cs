using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScoreControllerScript : MonoBehaviour{

	private static ScoreControllerScript instance;
	public static ScoreControllerScript Instance{get => instance;}
	List<PlayerController> listAssistance = new List<PlayerController>();
	public void AddPointToMe(int points = 1){
		MainGameController.Instance.GetMyPlayerController()?.AddPoints(points);
		if(listAssistance.Count > 0){
			for(int i = 0; i < listAssistance.Count; i++){
				listAssistance[i]?.AddPoints(points/2);
			}
		}
		Refresh();
	}
	public void AddPointToOpponent(int points = 1){
		GameControllerScript.Instance.Contoller?.AddPoints(points);
		if(listAssistance.Count > 0){
			for(int i = 0; i < listAssistance.Count; i++){
				listAssistance[i]?.AddPoints(points/2);
			}
		}
		Refresh();
	}
	public void AddAssist(PlayerController player){ listAssistance.Add(player); }
	void Awake(){ instance = this; }
	void Start(){
    	GameControllerScript.Instance.RegisterOnStartStep(Refresh); 
	}
	public void Refresh(){
		listAssistance.Clear();
	}
}