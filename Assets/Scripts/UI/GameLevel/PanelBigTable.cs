using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelBigTable : MonoBehaviour{
	public GameObject bigTable;
	public RectTransform opponentParentListSubjects, myParentListSubjects;
	public Transform topPanel, bottomPanel;
	public Transform bigTopPanel, bigBottomPanel;
	public void Open(){
		opponentParentListSubjects.SetParent(bigTopPanel);
		myParentListSubjects.SetParent(bigBottomPanel);
		opponentParentListSubjects.SetAllZero();
		myParentListSubjects.SetAllZero();
		bigTable.SetActive(true);
	}
	public void Close(){
		opponentParentListSubjects.SetParent(topPanel);
		myParentListSubjects.SetParent(bottomPanel);
		opponentParentListSubjects.SetAllZero();
		myParentListSubjects.SetAllZero();
		bigTable.SetActive(false);
	}
}