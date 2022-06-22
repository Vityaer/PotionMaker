using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardUIReagent : CardUIBase{
	public Image mainImage;
	public Text textName;
	public override void UpdateUI(Card data){
		mainImage.sprite = data.image;
		textName.text = data.name;
		panel.SetActive(true);
	}
}
