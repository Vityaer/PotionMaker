using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PanelResult : MonoBehaviour{
	public Canvas panel;
	public Text textMainResult;
	public GameObject prefabPlayerResult;
	public Transform content;
	private static bool open = false;
	public static void ShowResults(){
		if(open == false){
			open = true;
			MainGameController.Instance.AddCountGame();
			instance.UpdateUI();
		}
	}
	private void UpdateUI(){
		List<PlayerController> players = MainGameController.Instance.players;
		players.Sort(new PlayerScoreComparer()); 
		int score = 0, place = 1;
		score = players[0].cardController.Score;		
		for(int i = 0; i < players.Count; i++){
			if(score > players[i].cardController.Score){
				place += 1;
				score = players[i].cardController.Score;
			}
	        Instantiate(prefabPlayerResult, content).GetComponent<PlayerResultPanel>().SetData(place, players[i]);
		}
		CheckResult(players);

		panel.enabled = true;
	}
	public void GoToMainMenu(){
		FadeInOut.Instance.EndScene("MainMenu");
	}
	void Awake(){
		instance = this;
	}
	private static PanelResult instance;
	public static PanelResult Instance{get => instance;}
	private void CheckResult(List<PlayerController> players){
		string result = string.Empty;
		if(players.Count > 1){
			if(players[0].side == Side.Me){
				if(players[0].cardController.Score == players[1].cardController.Score){
					result = LanguageControllerScript.GetWord(TypeWord.resultDraw);
				}else{
					result = LanguageControllerScript.GetWord(TypeWord.resultWin);
					MainGameController.Instance.AddWin();
				}
			}else{
				if(players.Find(x => x.side == Side.Me).cardController.Score == players[0].cardController.Score){
					result = LanguageControllerScript.GetWord(TypeWord.resultDraw);
				}else{
					result = LanguageControllerScript.GetWord(TypeWord.resultDefeat);
				}
			}
		}
		textMainResult.text = result;
	}
}

