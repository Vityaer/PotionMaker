using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class TableScript : MonoBehaviour{
	public PanelSubjectReagents opponentParentListSubjects, myParentListSubjects;
	public WorkBench workBench;
	public GameObject prefabSubject;
	private static TableScript instance;
	public static TableScript Instance{get => instance;}
	public bool canCraft = true;
	public bool manyAction = true;
	public List<SubjectScript> subjects = new List<SubjectScript>(); 
	void Awake(){ instance = this; }
		
	void Start(){ GameControllerScript.Instance.RegisterOnStartStep(Refresh); }
	public void PutDownCard(Card card, bool isMyCraft = true){
		if(card.typeCard != TypeCard.Magic){
			if(canCraft){
		        AudioControllerScript.Instance.PlaySoundClickOnButton();
				workBench.StartCrafting(card, isMyCraft);
				GameControllerScript.Instance.UseCard = null;
			}else{
				if(GameControllerScript.Instance.MyStep){
					CardInHandScript.Instance.RevertCard(card);
				}else{
			    	GameControllerScript.Instance.Contoller.AddCard(card, PointMove.Table);
				}
			}
		}else{
	        AudioControllerScript.Instance.PlaySoundCreateMagic();
	        workBench.StartMagicAction(card, isMyCraft);
			if(GameControllerScript.Instance.MyStep){
				MagicSystem.Instance.MagicAction(card, isMyCraft);
			}
		}
	}
	public void CreateSubjects(Card card, List<Card> ingredients, bool isMyCraft){
		if(manyAction == false) canCraft = false;
		OnFinishCraft();
		((isMyCraft) ? myParentListSubjects : opponentParentListSubjects ).CreateSubject(card, ingredients);
	}
	public void ClickOnTable(){ OnClickTable(); }
	public void Refresh(){canCraft = true; }

	private Action observerCraft;
	public void RegisterOnFinishCraft(Action d){observerCraft += d;}
	public void UnregisterOnFinishCraft(Action d){observerCraft -= d;}
	private void OnFinishCraft(){
		if(observerCraft != null) observerCraft();
	}

	private Action observerClick;
	public void RegisterOnClickTable(Action d){observerClick += d;}
	public void UnregisterOnClickTable(Action d){observerClick -= d;}
	private void OnClickTable(){
		if(observerClick != null) observerClick();
	}
	public void AddInListSubject(SubjectScript subject){ subjects.Add(subject); }
	public void ClickOnSubject(int ID){subjects.Find(x => x.Data.ID == ID).SendToWorkbench(); }
	public void ClickOnSubject(ReagentName name){subjects.Find(x => x.Data.reagentName == name).SendToWorkbench();}
	public void ChangeSubjectMagicSwap(Card oldSubject, Card newSubject){
		subjects.Find(x => x.Data.reagentName == oldSubject.reagentName)?.ChangeData(newSubject, new List<Card>());
	}
}
