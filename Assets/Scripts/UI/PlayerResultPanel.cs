using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerResultPanel : MonoBehaviour{
	public Text textName, textScore, textPlace;
	public Image imagePlace;
	public List<Sprite> spritePlaces = new List<Sprite>();
	public void SetData(int place, PlayerController player){
		if(place <= 3){
			imagePlace.sprite = spritePlaces[place - 1]; 
			imagePlace.enabled = true;
		}else{
			textPlace.text = place.ToString();
			textPlace.enabled = true;
		}
		textName.text  = player.NickName;
		textScore.text = player.cardController.Score.ToString();
	}
}
