using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCard", menuName = "Custom ScriptableObject/Card", order = 51)]
[System.Serializable]
public class Card : ScriptableObject{
	public string name;
	public int ID;
	public Color colorCard;
	public TypeCard typeCard;
	private Reagent reagent = null;
	public ReagentName reagentName;
	public Sprite sprite;
	public Sprite image{ 
		get{
			if(sprite != null) return sprite;
			if(reagent == null) PrerareCard();
			return reagent?.image; 
		}
	}
	[Min(1)]public int amountScore;
	public string description;
	public List<ReagentName> receipt = new List<ReagentName>();
	public int countRequireIngredient = 0;
	public List<ReagentName> reagentStore = new List<ReagentName>(); 
	public Sprite specialImageIngredient;
	private ReagentLocalization localization;
	public void PrerareCard(){
		if(countRequireIngredient == 0) countRequireIngredient = receipt.Count;
		UpdateLocalization();
		reagent = ReagentsManager.FindReagentFromName(reagentName); 
	}
	public void UpdateLocalization(){
		UnpackLocalization(LanguageControllerScript.GetLocalizationReagent(reagentName));
	}
	public Card Clone(){
		return new Card{name = this.name,
								ID = this.ID,
		 						colorCard = this.colorCard,
		 						typeCard = this.typeCard,
		 						reagentName = this.reagentName,
		 						amountScore = this.amountScore,
		 						description = this.description,
		 						countRequireIngredient = this.countRequireIngredient,
		 						specialImageIngredient = this.specialImageIngredient,
		 						receipt = this.receipt,
		 						reagentStore = this.reagentStore};
	}
	private void UnpackLocalization(ReagentLocalization localization){
		if(localization != null){
			this.localization = localization;
			this.name = localization.Name; 
			this.description = localization.Description; 
		}
	}
}
public enum TypeCard{
	Potion = 0,
	Special = 1,
	Magic = 2,
	Reagent = 3
}