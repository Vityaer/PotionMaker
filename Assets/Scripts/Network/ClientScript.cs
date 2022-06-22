using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using ExitGames.Client.Photon; 

/// <summary>
/// This Client inheritated class acts like Client using UI elements like buttons and input fields.
/// </summary>
public class ClientScript : MonoBehaviourPunCallbacks, IOnEventCallback{
    
    public void SendCommand(NetworkCommand command){
        RaiseEventOptions options = new RaiseEventOptions{Receivers = ReceiverGroup.Others}; 
        SendOptions sendOptions = new SendOptions{ Reliability = true};
        if(PhotonNetwork.CurrentRoom != null){
            command.PrepareID();
            Console.Instance?.Print(command.info);
            PhotonNetwork.RaiseEvent((byte) command.type, command.info, options, sendOptions);
        }
    }
    public void SendCommand(NetworkCommand command, Player player){
        RaiseEventOptions options = new RaiseEventOptions{TargetActors = new []{player.ActorNumber}}; 
        SendOptions sendOptions = new SendOptions{ Reliability = true};
        if(PhotonNetwork.CurrentRoom != null){
            Console.Instance?.Print(command.ToString());
            PhotonNetwork.RaiseEvent((byte) command.type, command.info, options, sendOptions);
        }
    }
    public void OnEvent(EventData photonEvent){
        if(photonEvent.Code < 100){
            NetworkCommand command = new NetworkCommand( (TypeCommand) photonEvent.Code, photonEvent.CustomData.ToString());
            CommandCenterScript.Instance?.AddCommand(command);
        }
    } 
    void OnEnable(){
        PhotonNetwork.AddCallbackTarget(this);
    }
    void OnDisable(){
        PhotonNetwork.RemoveCallbackTarget(this);
    }
    public override void OnPlayerLeftRoom(Player otherPlayer){
        MainGameController.Instance.WaitPlayer(otherPlayer);
    }
    public override void OnPlayerEnteredRoom(Player otherPlayer){
        MainGameController.Instance.StopWaitPlayer(otherPlayer);
    }
}