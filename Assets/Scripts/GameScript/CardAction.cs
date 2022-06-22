using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAction{
	public virtual void ClickDown(Card card){}
	public virtual void ClickUp(){} 
}
public class ClosetReagentAction : CardAction{
	public override void ClickDown(Card card){
		PanelBigCardInfoScript.ShowCard(card);
	}
	public override void ClickUp(){
		PanelBigCardInfoScript.Close();
	}
}
public class WorkbenchReagentAction : CardAction{
	public ReagentController reagentController;
	public override void ClickDown(Card card){
		reagentController.AnswerFromWorkbench( WorkBench.Instance.CheckIngredient(card) );
	}
}
public class BigClosetReagentAction : CardAction{
	public override void ClickDown(Card card){
		PanelBigCloset.Instance.SelectCard(card);
	}
}
public class ReagentActionSwapSubjectAndReagent : CardAction{
	public override void ClickDown(Card card){
		PanelSwapSubjetsWithReagents.Instance.SelectCard(card);
	}
}
public class ReagentIntoSubject : CardAction{
	public override void ClickDown(Card card){
		PanelBigSubject.Instance.SelectCard(card);
	}
} 


public class SubjectAction{
	public virtual void ClickDown(SubjectScript subject, Card card, List<Card> ingredients){}
}
public class WorkbenchSubjectAction : SubjectAction{
	public SubjectScript subjectController;
	public override void ClickDown(SubjectScript subject, Card card, List<Card> cards){
		subjectController.AnswerFromWorkbench( WorkBench.Instance.CheckIngredient(subject.Data, false) );
	}
}
public class TableSubjectReagentAction : SubjectAction{
	public override void ClickDown(SubjectScript subject, Card card, List<Card> ingredients){
		PanelBigSubject.Instance.ShowSubject(subject, card, ingredients);
	}
}
public class SubjectActionSwapSubjectAndReagent : SubjectAction{
	public override void ClickDown(SubjectScript subject, Card card, List<Card> ingredients){
		PanelSwapSubjetsWithReagents.Instance.SelectSubject(card);
	}
}
