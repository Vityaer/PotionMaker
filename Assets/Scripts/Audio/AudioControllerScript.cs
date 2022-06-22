using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AudioControllerScript : MonoBehaviour{
	[SerializeField] private AudioSource audio; 

	[Header("Main sounds")]
	[SerializeField] List<AudioClip> listClipClickOnButton = new List<AudioClip>();
	[SerializeField] List<AudioClip> listClipCardMove = new List<AudioClip>();
	[SerializeField] List<AudioClip> listClipSuccess = new List<AudioClip>();
	[SerializeField] AudioClip clipError, clipWin, clipDefeat,clipCreateMagic, clipYourTurn, clipClosetSuccess;

	public void PlaySoundClickOnButton(){ if(playing) audio.PlayOneShot(listClipClickOnButton[Random.Range(0, listClipClickOnButton.Count)]); }
	public void PlaySoundOnCardMove(){if(playing) audio.PlayOneShot(listClipCardMove[Random.Range(0, listClipCardMove.Count)]); }
	public void PlayError(){if(playing) audio.PlayOneShot(clipError);}
	public void PlaySuccess(int level){level = Mathf.Min(level, listClipSuccess.Count - 1); if(playing) audio.PlayOneShot(listClipSuccess[level]);}
	public void PlayWin(){if(playing) audio.PlayOneShot(clipWin);}
	public void PlayDefeat(){if(playing) audio.PlayOneShot(clipDefeat);}
	public void PlaySoundCreateMagic(){if (playing) audio.PlayOneShot(clipCreateMagic);}
	public void PlaySoundYourTurn(){if(playing) audio.PlayOneShot(clipYourTurn);}
	public void PlayClosetSuccess(){if(playing) audio.PlayOneShot(clipClosetSuccess);}
	private bool playing = true;
	public void ChangeMode(Toggle toggle){
		playing = toggle.isOn;
	}
	public void ChangeMode(bool flag){
		playing = flag;
	}
	void Awake(){ if(instance == null) {instance = this;} else{Destroy(this);} }
	private static AudioControllerScript instance;
	public static AudioControllerScript Instance{get => instance;}
}