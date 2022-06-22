using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardUIPotion : CardUIBase{
	public Image mainImage;
	public Text textScore, textName;
	public List<ReceiptReagentUI> receiptUI = new List<ReceiptReagentUI>(); 
	public ReceiptReagentUI reagentStore; 
	public override void UpdateUI(Card data){

		if(data.countRequireIngredient > receiptUI.Count) Debug.Log(data.reagentName.ToString());
		if(data.typeCard == TypeCard.Potion){
			for(int i = 0; i < data.countRequireIngredient; i++)
				receiptUI[i].SetData(data.receipt[i]);
		}else if(data.typeCard == TypeCard.Special){
			for(int i = 0; i < data.countRequireIngredient; i++)
				receiptUI[i].SetData(data.specialImageIngredient);
		}
		if(data.countRequireIngredient == 2){ 
			receiptUI[2].SwitchOff();
		}else{
			receiptUI[2].SwitchOn();		
		}
		reagentStore.SetData(data.reagentStore[0]);
		mainImage.sprite = data.image;
		textScore.text = data.amountScore.ToString();
		textName.text = data.name;
		panel.SetActive(true);
	}
}
