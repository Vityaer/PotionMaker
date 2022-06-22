using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicSystem : MonoBehaviour{

	void Awake(){
		instance = this;
	}
	public void MagicAction(Card card, bool isMyCraft){
		switch(card.reagentName){
			case ReagentName.GetUpCardFromCloset:
				ClosetScript.Instance.OpenBigCloset();
				PanelBigCloset.Instance.SetAction(GetUpCardFromCloset);
				break;
			case ReagentName.SwapCardInSubject:
				PanelSubjects.Instance.Open();
				PanelBigSubject.Instance.SetAction(SwapCardInSubject);
				break;
			case ReagentName.SwapCardClosetAndSubject:
				PanelSwapSubjetsWithReagents.Instance.Open();
				break;		
		}
	}

	public void SwapCardInSubject(SubjectScript subject, int numNewCard){
		NetworkCommand commandCard = new NetworkCommand(TypeCommand.MagicSwapSubjectAndIngedient, new SwapSubjectAndIngredient(subject.Data.ID, numNewCard) );
		CommandCenterScript.Instance?.Sender?.SendCommand(commandCard);
		TableScript.Instance.subjects.Find(x => x.Data.reagentName == subject.Data.reagentName).SwapCard(numNewCard);
		WorkBench.Instance.ClearWorckbench();
	}
	public void GetUpCardFromCloset(Card card){
		NetworkCommand commandCard = new NetworkCommand(TypeCommand.MagicGetCardFromCloset,  new ObjectID (card.ID) );
		CommandCenterScript.Instance?.Sender?.SendCommand(commandCard);
		WorkBench.Instance.ClearWorckbench();
	}

	private static MagicSystem instance;
	public static MagicSystem Instance{get => instance;}
}