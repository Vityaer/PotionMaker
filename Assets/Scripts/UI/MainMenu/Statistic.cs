using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class Statistic : MonoBehaviour{

	public InputField playerNickName;
	public Canvas canvas;
	public Text textCountGame, textCountWin, textWinRate;
	GameSave save = null;
	public void Open(){
		if(save == null) save = MainGameController.Instance.GetSave;
		textCountGame.text  = save.countGame.ToString();
		textCountWin.text   = save.countWin.ToString();
		playerNickName.text = save.playerName;
		if(save.countGame > 0){
			textWinRate.text = Math.Round( ((float)save.countWin / (float)save.countGame) * 100f, 2).ToString() + "%" ;
		}else{
			textWinRate.text = "-";		
		}
		canvas.enabled = true;
		gameObject.SetActive(true);
	}
	public void Close(){
		canvas.enabled = false;
		gameObject.SetActive(false);
	}
	public void SaveNewPlayerNickname(){
		if(playerNickName.text.Length > 0){
			if(MainGameController.Instance.SetNewPlayerName(playerNickName.text) == true){
				btnSaveNewNickname.SetActive(false);
				AndroidPlugin.PluginControllerScript.ToastPlugin.Show(LanguageControllerScript.GetMessage(TypeMessage.SaveNewNickname), isLong: true);
			}
		}else{
			AndroidPlugin.PluginControllerScript.ToastPlugin.Show(LanguageControllerScript.GetMessage(TypeMessage.NicknameEmpty), isLong: true);
		}
	}
	public GameObject btnSaveNewNickname;
	public void CheckNewNickname(){
		if(playerNickName.text.Length > 0){
			btnSaveNewNickname.SetActive(!save.playerName.Equals(playerNickName.text));
		}else{
			btnSaveNewNickname.SetActive(false);
		}
	}
}
