using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelSubjects : MonoBehaviour{
	List <SubjectScript> listSubject = new List<SubjectScript>();
	public Transform content;
	public GameObject prefabSubject;
	public GameObject mainPanel;
	public void Open(){
		listSubject = TableScript.Instance.subjects.FindAll(x => x.GetSide == Side.Me);
		for(int i = 0; i < listSubject.Count; i++){
			SubjectScript subject = Instantiate(prefabSubject, content).GetComponent<SubjectScript>();
			subject.SetData(listSubject[i].Data, listSubject[i].Ingredients, true);
		}
		mainPanel.SetActive(listSubject.Count > 0);
		if(listSubject.Count == 0) GameControllerScript.Instance.PrintError(LanguageControllerScript.GetMessage(TypeMessage.SubjectsNotCreated), PointMove.Table);
	}
	public void CancelMagic(){
		WorkBench.Instance.StartCancelCraft();
		Close();
	}
	public void Close(){
		mainPanel.SetActive(false);
		foreach(Transform child in content){
			Destroy(child.gameObject);
		}
		listSubject.Clear();
	}
	void Awake(){ instance = this; }
	private static PanelSubjects instance;
	public static PanelSubjects Instance{get => instance;}
}
