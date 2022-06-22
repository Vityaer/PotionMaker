using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameSave{
	public string playerName = "Player";
	public int countGame = 0, countWin = 0;
	public bool musicPlay = true, soundsPlay = true;
	public int numLanguage = 0;
	public GameSave(){
		numLanguage = FunctionHelp.GetDefaultLanguage();
	}
}