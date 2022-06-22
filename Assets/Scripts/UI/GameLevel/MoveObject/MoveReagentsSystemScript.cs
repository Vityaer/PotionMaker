using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class MoveReagentsSystemScript : MonoBehaviour{
	private List<ReagentMoveScript> moveReagents = new List<ReagentMoveScript>();  
	public Transform mainPanel;
	public GameObject prefabReagentMove;
	public void CreateReagentMove(Card data, Vector3 start, Vector3 finish, Action<Card> action, bool createCard = true){
        GetFreeReagentMove().SetData(data, start, finish, action, createCard);
	}
	private ReagentMoveScript GetFreeReagentMove(){
		ReagentMoveScript result = moveReagents.Find(x => (x.isReady));
		if(result == null){
		 	result = Instantiate(prefabReagentMove, mainPanel).GetComponent<ReagentMoveScript>();
			moveReagents.Add(result);
		}else{
			result.EnableReagent();
		}
		return result;
	}
	private static MoveReagentsSystemScript instance;
	public static MoveReagentsSystemScript Instance{get => instance;}
	void Awake(){
		instance = this;
	}
}
