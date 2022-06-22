using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
public class LanguageControllerScript : MonoBehaviour{

	public LanguageObject currentLanguage;
	public List<LanguageObject> languages = new List<LanguageObject>();
    
    public static ReagentLocalization GetLocalizationReagent(ReagentName name){
    	return Instance.currentLanguage.GetLocalizationReagent(name);
    }
    public static string GetWord(TypeWord type){
        return Instance.currentLanguage.GetWord(type);
    }
    public static string GetMessage(TypeMessage type){
        return Instance.currentLanguage.GetMessage(type);
    }
    public static string GetBigText(TypeBigText type){
        return Instance.currentLanguage.GetBigText(type);
    }
    public void ChangeLanguage(int IDLanguage){
    	if(IDLanguage < languages.Count){
	    	currentLanguage = languages[IDLanguage];
            MainGameController.Instance.ChangeLanguage(IDLanguage);
            OnChangeLanguage();
        }
    }

    public void ChangeLanguage(Dropdown laguageDropdown){ ChangeLanguage(laguageDropdown.value); }

    public delegate void Del();
    public Del observerChangeLanguage;
    public void RegisterOnChangeLanguage(Del d){ observerChangeLanguage += d;  } 
    public void UnRegisterOnChangeLanguage(Del d){ observerChangeLanguage -= d; }
    public void OnChangeLanguage(){
    	if(observerChangeLanguage != null)
	    	observerChangeLanguage();
    }

    private static LanguageControllerScript instance;
    public static LanguageControllerScript Instance{get => instance;}
    void Awake(){
    	if(instance == null){ instance = this;} else { Destroy(this); }
        currentLanguage = languages[FunctionHelp.GetDefaultLanguage()];
    }
}
