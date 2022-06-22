using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class SubjectScript : MonoBehaviour{

	private Vector2 startSize;

	public Image image, background;
	// public Text name;
	public GameObject panelCards;
	private Card data;
	public Card Data{get => data;}
	public List<Card> ingredients = new List<Card>();
	public List<Card> Ingredients{get => ingredients;}
	private SubjectAction action;
	private Side side;
	public Side GetSide{get => side;}
	private PlayerController controller;
	public PlayerController Controller{get => controller;}
	public Image outLight;
	void Awake(){
		startSize = rect.localScale;
	}
	public void SetData(Card data, List<Card> ingredients, bool isMyCraft){
		for(int i = 0; i < ingredients.Count; i++){
			this.ingredients.Add(ingredients[i].Clone());
		}
		WorkBench.Instance.RegisterOnOpenClose(GetAction);
		action = new TableSubjectReagentAction();
		CardController.RegisterOnDrag(IWork);
		side = isMyCraft ? Side.Me : Side.Opponent;
		this.data = data;
		controller = GameControllerScript.Instance.Contoller;
		// defaultColor = isMyCraft ? image.color : controller.color;
		defaultColor = image.color;
		ShowAddCard();
		UpdateUI();
	} 
	public void SetData(Card data, List<Card> ingredients, Side side, PlayerController controller){
		for(int i = 0; i < ingredients.Count; i++){
			this.ingredients.Add(ingredients[i].Clone());
		}
		action = new WorkbenchSubjectAction();
		(action as WorkbenchSubjectAction).subjectController = this;
		this.side = side;
		this.data = data;
		this.controller = controller;
		defaultColor = (side == Side.Me) ? image.color : controller.color;
		WorkbenchOpen = true;
		UpdateUI(withSound: false);
	}
	private void UpdateUI(bool withSound = true){
		if(withSound){ AudioControllerScript.Instance.PlaySuccess(data.amountScore / 5); }
		image.sprite = data.image; 
		// name.text = data.name;
		panelCards.SetActive(ingredients.Count > 0);
	}
	public void OnClick(){
		if(WorkbenchOpen){
			if(GameControllerScript.Instance.MyStep){
				commandSubject.SetData(new ObjectID( data.ID) );
				CommandCenterScript.Instance?.Sender?.SendCommand(commandSubject);
			}
		}
		action?.ClickDown(this, this.data, this.ingredients);
	}
	public void SendToWorkbench(){
		SubjectAction actionOpponent = new WorkbenchSubjectAction();
		(actionOpponent as WorkbenchSubjectAction).subjectController = this;
		actionOpponent.ClickDown(this, this.data, this.ingredients);
	}
	public void AnswerFromWorkbench(bool flag){
		if(flag == true){
			AudioControllerScript.Instance.PlaySoundClickOnButton();
			MoveReagentsSystemScript.Instance.CreateReagentMove(data, transform.position, WorkBench.Instance.GetPositonEmptyPlace(), WorkBench.Instance.AddIngredient, false);
			SelectSubject();
			if(controller != GameControllerScript.Instance.Contoller){
				ScoreControllerScript.Instance.AddAssist(controller);
			}
		}
		else{
			AudioControllerScript.Instance.PlayError();
			ShowWrongSelect();
		}
	}
	public bool WorkbenchOpen = false;
	void GetAction(bool WorkbenchOpen){
		this.WorkbenchOpen = WorkbenchOpen;
		DiselectSubject();
		if(WorkbenchOpen){
			action = new WorkbenchSubjectAction();
			(action as WorkbenchSubjectAction).subjectController = this;
		}else{
			action = new TableSubjectReagentAction();
		}
	}
	public void	SelectSubject(){ 
		rect.DOScale(startSize * 1.1f, 0.25f);
		outLight.enabled = true;
	}
	public void DiselectSubject(){
		rect.DOScale(startSize, 0.25f);
		outLight.enabled = false;
	}
	public void SetAction(SubjectAction newAction){ action = newAction; }
	public void SwapCard(int num){
		if(ingredients[num].typeCard != TypeCard.Magic){
			Card t = ingredients[num];
			ingredients[num] = this.data;
			this.data = t;
			UpdateUI();
		}else{
			ShowWrongSelect();
		}
	}
	public void ChangeData(Card newData, List<Card> newIngredients){
		if(ingredients.Count > 0){
			for(int i = 0; i < ingredients.Count; i++){
				MoveControllerCard.Instance.CreateMoveCard(ingredients[i], PointMove.Table, PointMove.Closet, ClosetScript.Instance.AddCardFromDeck, open: true);
			}
		}
		this.ingredients.Clear();
		SetData(newData, newIngredients, (this.side == Side.Me));
	}
	void OnDestroy(){ WorkBench.Instance.UnregisterOnOpenClose(GetAction); }
	private void IWork(bool dragCard){ if(background != null) background.raycastTarget = !dragCard; }
	public RectTransform rect;

	public Color colorWrongSelect;
	private Color defaultColor;
	Vector2 pinPong = new Vector2(5, 0);
	Sequence sequenceWrongSelect;
	private void ShowWrongSelect(){
		if(sequenceWrongSelect != null) sequenceWrongSelect.Kill();
		sequenceWrongSelect = DOTween.Sequence();
		ShowColorWrongSelect();
		sequenceWrongSelect.Append(rect.DOAnchorPos(pinPong, 0.03f));
		sequenceWrongSelect.Append(rect.DOAnchorPos(-pinPong, 0.04f));
		sequenceWrongSelect.Append(rect.DOAnchorPos(pinPong/2, 0.05f));
		sequenceWrongSelect.Append(rect.DOAnchorPos(-pinPong/2, 0.06f));
		sequenceWrongSelect.Append(rect.DOAnchorPos(Vector2.zero, 0.07f));
	}
	Sequence sequenceColorWrongSelect;
	public void ShowColorWrongSelect(){
		if(sequenceColorWrongSelect != null) sequenceColorWrongSelect.Kill();
		sequenceColorWrongSelect = DOTween.Sequence();
		Color defaultColor = image.color;
		sequenceColorWrongSelect.Append(background.DOColor(colorWrongSelect, 0.03f));
		sequenceColorWrongSelect.Append(background.DOColor(defaultColor, 0.04f));
		sequenceColorWrongSelect.Append(background.DOColor(colorWrongSelect, 0.05f));
		sequenceColorWrongSelect.Append(background.DOColor(colorWrongSelect, 0.06f));
		sequenceColorWrongSelect.Append(background.DOColor(defaultColor, 0.07f));
	}
	public Color colorAddCard;
	Sequence sequenceAddCard;
	public void ShowAddCard(){
		if(sequenceAddCard != null) sequenceAddCard.Kill();
		sequenceAddCard = DOTween.Sequence();
		sequenceAddCard.Append(background.DOColor(colorAddCard, 0.25f));
		sequenceAddCard.Append(background.DOColor(defaultColor, 0.25f));
		sequenceAddCard.Append(background.DOColor(colorAddCard, 0.25f));
		sequenceAddCard.Append(background.DOColor(defaultColor, 0.25f).OnComplete(() => {GameControllerScript.Instance.CheckGame();}));
	}

	private static NetworkCommand commandSubject = new NetworkCommand(TypeCommand.GetSubjectFromTable);
}
public enum Side{
	Me = 0,
	Opponent = 1
}