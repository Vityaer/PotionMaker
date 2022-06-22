using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelBigWorkbench : MonoBehaviour{
	public GameObject bigWorkbench;
	public Transform contentReagents, contentSubjects;
	public Transform closetParent;
	public GameObject prefabSubject;
	public Transform parentWorkbench, parentBigWorkbench;
	public RectTransform workbench, closet;
	public void Open(){
		workbench.SetParent(parentBigWorkbench);
		closet.SetParent(contentReagents);
		workbench.SetAllZero();
		closet.SetAllZero();
		List<SubjectScript> listSubject = TableScript.Instance.subjects;
		SubjectScript subject = null;
		for(int i = 0; i < listSubject.Count; i++){
			subject = Instantiate(prefabSubject, contentSubjects).GetComponent<SubjectScript>();
			subject.SetData(listSubject[i].Data, listSubject[i].Ingredients, listSubject[i].GetSide, listSubject[i].Controller);
		}
		bigWorkbench.SetActive(true);
	}
	public void Close(){
		foreach(Transform child in contentSubjects){ Destroy(child.gameObject); }
		workbench.SetParent(parentWorkbench);
		closet.SetParent(closetParent);
		workbench.SetAllZero();
		closet.SetAllZero();
		bigWorkbench.SetActive(false);
	}
	void Awake(){
		instance = this;
	}
	private static PanelBigWorkbench instance;
	public static PanelBigWorkbench Instance{get => instance;}
}