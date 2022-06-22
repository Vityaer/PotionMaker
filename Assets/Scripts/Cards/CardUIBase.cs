using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CardUIBase : MonoBehaviour{
	public GameObject panel;
	public virtual void UpdateUI(Card data){}
	public virtual void Hide(){ panel.SetActive(false); }
	public virtual void Open(){panel.SetActive(true);}
}