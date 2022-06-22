using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun.Demo.Asteroids;
public class RoomVersusAIScript : MonoBehaviour{
    public Transform listPlayers;
    public GameObject PlayerListEntryPrefab;
    public Button StartGameButton, buttonAddPlayer;
    private List<string> NicknamesAI = new List<string>(){"Mellisa_AI", "Noxyn_AI", "Rooster_AI"};
    public int numPlayer = 0;
    public void AddPlayer(){
        numPlayer++;
    	GameObject entry = Instantiate(PlayerListEntryPrefab);
    	entry.transform.SetParent(listPlayers);
        entry.transform.localScale = Vector3.one;
        string name = NicknamesAI[UnityEngine.Random.Range(0, NicknamesAI.Count)];
        MainGameController.Instance.AddPlayer(numPlayer, Side.Opponent, name);
        entry.GetComponent<PlayerListEntry>().Initialize(numPlayer, name, Owner.AI);
        if(numPlayer == 2){
            buttonAddPlayer.interactable = false; 
        }
    }
    public void OpenGame(){
    	FadeInOut.Instance.EndScene("PotionLevel");
    }
}
