using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelSwapSubjetsWithReagents : MonoBehaviour{

	public Transform contentSubjects, contentReagent;
	public GameObject prefabCard, prefabSubject;
	public GameObject mainPanel;
	public GameObject btnAgree;
	List<SubjectScript> subjects = new List<SubjectScript>();
	List<CardController> reagents = new List<CardController>();
	[ContextMenu("Open swap window")]
	public void Open(){
		List<SubjectScript> listSubject = TableScript.Instance.subjects.FindAll(x => x.GetSide == Side.Me);
		SubjectScript subject = null;
		for(int i = 0; i < listSubject.Count; i++){
			subject = Instantiate(prefabSubject, contentSubjects).GetComponent<SubjectScript>();
			subject.SetData(listSubject[i].Data, listSubject[i].Ingredients, Side.Me, listSubject[i].Controller);
			subjects.Add(subject);
		}
		List<Card> listReagents = ClosetScript.Instance.GetTopReagents(); 
		CardController card = null;
		for(int i = 0; i < listReagents.Count; i++){
			if(listReagents[i].typeCard != TypeCard.Magic){
				card = Instantiate(prefabCard, contentReagent).GetComponent<CardController>();
				card.SetData(listReagents[i], CanDrag: false);
				reagents.Add(card);
			}
		}
		if(listSubject.Count == 0){
			GameControllerScript.Instance.PrintError(LanguageControllerScript.GetMessage(TypeMessage.SubjectsNotCreated), PointMove.Table);
		}else if(listReagents.Count == 0){
			GameControllerScript.Instance.PrintError(LanguageControllerScript.GetMessage(TypeMessage.ClosetEmpty), PointMove.Table);
		}else{
			mainPanel.SetActive(true);
			SetActionForCards();
		}
	}

	private CardController selectedReagent;
	private SubjectScript selectedSubject;
	private void SetActionForCards(){
		for(int i = 0; i < subjects.Count; i++)
			subjects[i].SetAction(new SubjectActionSwapSubjectAndReagent());
		for(int i = 0; i < reagents.Count; i++)
			reagents[i].SetAction(new ReagentActionSwapSubjectAndReagent());

	} 

	public void SelectCard(Card card){
		selectedReagent?.Disable();
		selectedReagent = reagents.Find(x => x.data.reagentName == card.reagentName);
		if((selectedSubject != null) && (selectedReagent != null))
			btnAgree.SetActive(true);
	}
	public void SelectSubject(Card card){
		selectedSubject?.DiselectSubject();
		selectedSubject = subjects.Find(x => x.Data.reagentName == card.reagentName);
		selectedSubject?.SelectSubject();
		if((selectedSubject != null) && (selectedReagent != null))
			btnAgree.SetActive(true);
	}

	public void SwapCard(){
		NetworkCommand commandCard = new NetworkCommand(TypeCommand.MagicSwapSubjectAndReagent,  new SwapSubjectAndReagent(selectedSubject.Data.ID, selectedReagent.data.ID) );
		CommandCenterScript.Instance?.Sender?.SendCommand(commandCard);
		TableScript.Instance.ChangeSubjectMagicSwap(selectedSubject.Data, selectedReagent.data);
		ClosetScript.Instance.ChangeReagentMagicSwap(selectedReagent.data, selectedSubject.Data);
		WorkBench.Instance.ClearWorckbench();
		Close();
	}
	public void Close(){
		foreach(Transform child in contentSubjects) Destroy(child.gameObject);
		foreach(Transform child in contentReagent) Destroy(child.gameObject);
		selectedSubject?.DiselectSubject();
		selectedSubject = null;
		selectedReagent?.Disable();
		selectedReagent = null;
		subjects.Clear();
		reagents.Clear();
		btnAgree.SetActive(false);
		mainPanel.SetActive(false);
	}
	void Awake(){ instance = this; }
	private static PanelSwapSubjetsWithReagents instance;
	public static PanelSwapSubjetsWithReagents Instance{get => instance;}
}
