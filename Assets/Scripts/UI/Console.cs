using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Console : MonoBehaviour{

	public Text textComponent;
	private static Console instance;
	public static Console Instance{get => instance;}
	public bool isDevelopState = false;
	public GameObject openButton, panelConsole;
	void Awake(){
		if(instance == null){
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }else{
            Destroy(gameObject);
        }
	}
	void Start(){
		if(isDevelopState == false){
			if(Settings.Instance != null) ChangeState(Settings.Instance.isDelevopmentState);
		}else{
			ChangeState(true);
		}

	}
	public void Print(string message){
		if(isDevelopState){
			Debug.Log(message);
			textComponent.text = string.Concat(textComponent.text, "\n", message);
		}
	}
	public void ChangeState(bool flag){
		isDevelopState = flag;
		openButton.SetActive(isDevelopState);
		if(isDevelopState == false)
			panelConsole.SetActive(false);
	}
	[ContextMenu("Open console")]
	public void OpenConsoleFromEditor(){
		ChangeState(true);
	}

}
