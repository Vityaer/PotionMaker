using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeLanguageBigText : MonoBehaviour{
    public Text componentText;
	public TypeBigText typeBigText;
	public string beforeSymbols, afterSymbols;
	void Start(){
		if(componentText == null) 
			componentText = GetComponent<Text>();
		LanguageControllerScript.Instance.RegisterOnChangeLanguage(ChangeLanguage);
		ChangeLanguage();
	}
   	
   	public void ChangeLanguage(){
		componentText.text = string.Concat(beforeSymbols, LanguageControllerScript.GetBigText(typeBigText), afterSymbols);
   	}
   	void OnDestroy(){
		LanguageControllerScript.Instance.UnRegisterOnChangeLanguage(ChangeLanguage);
   	}
}
