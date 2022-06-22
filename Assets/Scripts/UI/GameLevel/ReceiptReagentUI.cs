using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class ReceiptReagentUI : MonoBehaviour{
	[SerializeField] private Image image, background;
	public void SetData(ReagentName name, Color color){
		Reagent reagent = ReagentsManager.FindReagentFromName(name);
		image.sprite = reagent.image;
	}
	public void SetData(ReagentName name){
		Reagent reagent = ReagentsManager.FindReagentFromName(name);
		image.sprite = reagent.image;
	}
	public void SetData(Sprite specialSprite){
		image.sprite = specialSprite;
	}
	public void SwitchOff(){gameObject.SetActive(false);}
	public void SwitchOn(){gameObject.SetActive(true);}
	public Color colorWrongSelect;
	private Color defaultColor = Color.white;
	Sequence sequenceWrongSelect;
	public void ShowWrongSelect(){
		if(sequenceWrongSelect != null) sequenceWrongSelect.Kill();
		sequenceWrongSelect = DOTween.Sequence();
		sequenceWrongSelect.Append(background.DOColor(colorWrongSelect, 0.03f));
		sequenceWrongSelect.Append(background.DOColor(defaultColor, 0.04f));
		sequenceWrongSelect.Append(background.DOColor(colorWrongSelect, 0.05f));
		sequenceWrongSelect.Append(background.DOColor(defaultColor, 0.07f));
		sequenceWrongSelect.Append(background.DOColor(colorWrongSelect, 0.06f));
		sequenceWrongSelect.Append(background.DOColor(defaultColor, 0.05f));
	}
	public Color colorAddCard;
	Sequence sequenceShowAddCard;
	public void ShowAddCard(){
			if(sequenceShowAddCard != null) sequenceWrongSelect.Kill();
			sequenceShowAddCard = DOTween.Sequence();
			sequenceShowAddCard.Append(background.DOColor(colorAddCard, 0.25f));
			sequenceShowAddCard.Append(background.DOColor(defaultColor, 0.25f));
			sequenceShowAddCard.Append(background.DOColor(colorAddCard, 0.25f));
			sequenceShowAddCard.Append(background.DOColor(defaultColor, 0.25f));
			sequenceShowAddCard.Append(background.DOColor(colorAddCard, 0.25f));
			sequenceShowAddCard.Append(background.DOColor(defaultColor, 0.25f));
	}
}