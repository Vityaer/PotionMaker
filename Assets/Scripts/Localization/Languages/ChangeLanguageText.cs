using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ChangeLanguageText : MonoBehaviour{

	public Text componentText;
	public TypeWord typeWord;
	public string beforeSymbols, afterSymbols;
	void Start(){
		if(componentText == null) 
			componentText = GetComponent<Text>();
		if(LanguageControllerScript.Instance != null){
			LanguageControllerScript.Instance.RegisterOnChangeLanguage(ChangeLanguage);
			ChangeLanguage();
		}
	}
   	
   	public void ChangeLanguage(){
		componentText.text = string.Concat(beforeSymbols, LanguageControllerScript.GetWord(typeWord), afterSymbols);
   	}
   	void OnDestroy(){
		LanguageControllerScript.Instance?.UnRegisterOnChangeLanguage(ChangeLanguage);
   	}
}
