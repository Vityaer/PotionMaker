using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MessageWaitConnectPlayer : PanelMessageScript{
	public Text textTimeWait;
	Coroutine waitCoroutine; 
	private const string StringStartTime = "0:00";
	void Awake(){
		instance = this;
	}
	public void StartWait(){
		if(waitCoroutine == null){
			base.Show();
			waitCoroutine = StartCoroutine(IWaitConnectPlayers());
		}
	}
	IEnumerator IWaitConnectPlayers(){
		textTimeWait.text = StringStartTime;
		int secondsWait = 0;
		while(true){
			yield return new WaitForSeconds(1f);
			secondsWait += 1;
			textTimeWait.text = FunctionHelp.TimerText(secondsWait);
		}
	}
	public void StopWait(){
		if(waitCoroutine != null){
			StopCoroutine(waitCoroutine);
			waitCoroutine = null;
		}
	}
	private static MessageWaitConnectPlayer instance;
	public static MessageWaitConnectPlayer Instance{get => instance;}
}
