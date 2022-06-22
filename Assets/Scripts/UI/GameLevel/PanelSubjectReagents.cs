using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelSubjectReagents : MonoBehaviour{
	public GameObject prefabCard;
	public List<Transform> panels = new List<Transform>();
	public int maxCountReagentsOnPanel = 8;
	int countCreated = 0;
	public bool isMyCard;
	public void CreateSubject(Card card, List<Card> ingredients){
		SubjectScript subject = Instantiate(prefabCard, panels[countCreated / maxCountReagentsOnPanel]).GetComponent<SubjectScript>();
		subject.SetData(card, ingredients, isMyCard);
		TableScript.Instance.AddInListSubject(subject);
		countCreated += 1;
	}
}