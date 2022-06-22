using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPanelScript : MonoBehaviour{

	public Text textPlayersName;
	public GameObject imageWillingness;
	public bool isWillingness = false;


	public void FillInfoAboutPlayer(){
		// textPlayersName.text = account.userName;
		ChangeWillingness(false);
	}

	public void ChangeWillingness(bool isFlag){
		isWillingness = isFlag;
		imageWillingness.GetComponent<Image>().color = isWillingness ? Color.green : Color.red;
	}
}
