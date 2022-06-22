using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class ReagentController : MonoBehaviour{
	public ReceiptReagentUI mainReagent;
	public Image background;
	public Text textCountReagents;
	private int countReagents = 0;
	public RectTransform rect;
	private Card card;
	public Card Data{get => card;}
	public CardAction action;
	public void SetData(Card card){ 
		this.card = card;
		WorkBench.Instance.RegisterOnOpenClose(GetAction);
		if(background != null) CardController.RegisterOnDrag(IWork);
		action = new ClosetReagentAction();
		mainReagent.SetData(card.reagentStore[0], card.colorCard);
		mainReagent.ShowAddCard();
	}
	public void SetData(ReagentName name){ mainReagent.SetData(name); }
	public void OnClickDown(){
		action.ClickDown(this.card);
	}
	public void IncreaseCountCard(){
		countReagents += 1;
		UpdateUI();
	}
	public void DecreaseCountCard(){
		countReagents -= 1;
		UpdateUI();
	}
	public void OnClickUp(){ action.ClickUp(); }
	public void AnswerFromWorkbench(bool flag){
		if(flag == true){
			AudioControllerScript.Instance.PlaySoundClickOnButton();
			commandCard.SetData( new ObjectID(card.ID) );
			CommandCenterScript.Instance?.Sender?.SendCommand(commandCard);
			ClosetScript.Instance.RemoveReagent(card);
			MoveToWorkBench();
		}
		else{
			AudioControllerScript.Instance.PlayError();
			ShowWrongSelect();
		}
	}
	public void MoveToWorkBench(){
		MoveReagentsSystemScript.Instance.CreateReagentMove(card, transform.position, WorkBench.Instance.GetPositonEmptyPlace(), WorkBench.Instance.AddIngredient, true);
	}
	void GetAction(bool WorkbenchOpen){
		if(WorkbenchOpen){
			action = new WorkbenchReagentAction();
			(action as WorkbenchReagentAction).reagentController = this;
		}else{
			action = new ClosetReagentAction();
		}
	}
	public void Clear(){
		Destroy(gameObject);
	}
	void OnDestroy(){
		WorkBench.Instance.UnregisterOnOpenClose(GetAction);
		CardController.UnregisterOnDrag(IWork);
	}
	private static NetworkCommand commandCard = new NetworkCommand(TypeCommand.GetReagentFromCloset);
	private void IWork(bool dragCard){if(background != null) background.raycastTarget = !dragCard; }
	Vector2 pinPong = new Vector2(0, 10);
	void UpdateUI(){
		textCountReagents.text = (countReagents > 1) ? countReagents.ToString() : string.Empty; 
	}

	Sequence sequenceWrongSelect;
	private void ShowWrongSelect(){
		if(sequenceWrongSelect != null){
			sequenceWrongSelect.Kill();
			sequenceWrongSelect = null;
		}
		mainReagent.ShowWrongSelect();
		sequenceWrongSelect = DOTween.Sequence();
		sequenceWrongSelect.Append(rect.DOAnchorPos(pinPong, 0.03f));
		sequenceWrongSelect.Append(rect.DOAnchorPos(-pinPong, 0.04f));
		sequenceWrongSelect.Append(rect.DOAnchorPos(pinPong/2, 0.05f));
		sequenceWrongSelect.Append(rect.DOAnchorPos(-pinPong/2, 0.06f));
		sequenceWrongSelect.Append(rect.DOAnchorPos(-pinPong/2, 0.06f));
		sequenceWrongSelect.Append(rect.DOAnchorPos(Vector2.zero, 0.07f));
	}
}