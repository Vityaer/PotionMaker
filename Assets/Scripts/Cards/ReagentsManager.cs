using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReagentsManager : MonoBehaviour{
	public List<Card> listCard = new List<Card>();
	[SerializeField] private List<Reagent> reagents = new List<Reagent>();
	public static Reagent FindReagentFromName(ReagentName name){
		return Instance.reagents.Find(x => x.name == name);
	}
	public static Card FindCardFromID(int ID){
		return Instance.listCard.Find(x => x.ID == ID).Clone();
	}
	void Awake(){
		instance = this;
	}
	private static ReagentsManager instance;
	public static ReagentsManager Instance{get => instance;}
}