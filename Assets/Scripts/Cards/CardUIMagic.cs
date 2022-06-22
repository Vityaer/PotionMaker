using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardUIMagic : CardUIBase{
	public Text textName, textDescription;
	public ReceiptReagentUI reagentStore; 
	public override void UpdateUI(Card data){
		textName.text = data.name;
		textDescription.text = data.description;
		reagentStore.SetData(data.reagentStore[0]);
		panel.SetActive(true);
	}
}