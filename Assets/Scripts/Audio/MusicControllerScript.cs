using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MusicControllerScript : MonoBehaviour{
	[SerializeField] private AudioSource audio; 
	[Header("Main sounds")]
	[SerializeField] List<AudioClip> listClipBackgroundMusic = new List<AudioClip>();
	void Start(){
		DontDestroyOnLoad(this.gameObject);
	}
	Coroutine coroutineLoopMusic;
	private void StartMusic(){
		if(coroutineLoopMusic != null){
			StopCoroutine(coroutineLoopMusic);
			coroutineLoopMusic = null;
		}
		coroutineLoopMusic = StartCoroutine(ILoopMusic());
	}
	int indexCurrentMusicClip = 0;
	private bool playing = true; 
	IEnumerator ILoopMusic(){
		int newIndex = Random.Range(0, listClipBackgroundMusic.Count);
		while(playing){
			while(newIndex == indexCurrentMusicClip)
				newIndex = Random.Range(0, listClipBackgroundMusic.Count);
			indexCurrentMusicClip = newIndex;
			Play(listClipBackgroundMusic[indexCurrentMusicClip]);
			yield return new WaitForSeconds(listClipBackgroundMusic[indexCurrentMusicClip].length + 2f);
		}
	}
	private void Play(AudioClip clip){
		Debug.Log("Play music");
		audio.Stop();
		audio.PlayOneShot(clip);
	}
	private void StopMusic(){
		if(coroutineLoopMusic != null){
			StopCoroutine(coroutineLoopMusic);
			coroutineLoopMusic = null;
		}
		audio.Stop();
	} 
	public void ChangeMode(Toggle toggle){
		ChangeMode(toggle.isOn);
	}
	public void ChangeMode(bool flag){
		playing = flag;
		if(playing == true){
			StartMusic();
		}else{
			StopMusic();
		}
	}
	void Awake(){
		if(instance != null){
			Destroy(gameObject);
		}else{
			instance = this;
		}
	}
	private static MusicControllerScript instance;
	public static MusicControllerScript Instance{get => instance;}
}
