using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Settings : MonoBehaviour{
	public Toggle musicToggle, soundsToggle;
	public Dropdown languageDropdown;
	public bool isDelevopmentState = false;
	public void MusicOnOff(Toggle toggle){
		MainGameController.Instance.MusicOnOff(toggle.isOn);
	} 
	public void SoundsOnOff(Toggle toggle){
		MainGameController.Instance.SoundsOnOff(toggle.isOn);
	} 
	public void LoadSettings(GameSave save){
		musicToggle.isOn    = save.musicPlay;
		soundsToggle.isOn   = save.soundsPlay;
		AudioControllerScript.Instance.ChangeMode(save.soundsPlay);
		MusicControllerScript.Instance.ChangeMode(save.musicPlay);
		languageDropdown.value = save.numLanguage;
        LanguageControllerScript.Instance.ChangeLanguage(save.numLanguage);
	}
	public void ChangeDevelopmentState(Toggle toggle){
		Console.Instance?.ChangeState(toggle.isOn);
	}
	void Awake(){
		if(instance == null) instance = this;
	}
	void Start(){
		SceneManager.sceneLoaded += OpenCloseButtonExitInMainGame;
		if(SceneManager.GetActiveScene().buildIndex != 0)
			panelExitInMainMenu.SetActive(true);
	}
	public GameObject panelExitInMainMenu;
	private void OpenCloseButtonExitInMainGame(Scene scene, LoadSceneMode mode){
		panelExitInMainMenu.SetActive(scene.buildIndex != 0);
	}
	public void GoToMainMenu(){
		int myID = MainGameController.Instance.GetMyActorNumber();
		CommandCenterScript.Instance.SendCommand(new NetworkCommand(TypeCommand.PlayerLeftScene, new ObjectID(myID) ) );
		MainGameController.Instance.RemoveAllPlayer();
		CloseSettings();
		PanelResult.Instance.GoToMainMenu();
	}
	public GameObject panelSettings;
	public  void CloseSettings(){
		panelSettings.SetActive(false);
	}
	private static Settings instance;
	public static Settings Instance{get => instance;}
	void OnDestroy(){
		SceneManager.sceneLoaded -= OpenCloseButtonExitInMainGame;
	}
}