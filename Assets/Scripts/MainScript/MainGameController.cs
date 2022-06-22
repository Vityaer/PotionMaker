using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun.Demo.Asteroids;
using UnityEngine.SceneManagement;
public class MainGameController : MonoBehaviour{
	public List<PlayerController> players = new List<PlayerController>();
    public int CountPlayers {get => players.Count;}
    private bool gameOnline = false;
    public bool GameStatusOnline{get => gameOnline;}
    private bool inLobby = false;
    void Awake(){
        if(instance == null){
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            SaveLoadScript.LoadGame(save);
        }else{
            Destroy(this);
        }
    }
    void Start(){
        SceneManager.sceneLoaded += ExitInMainGame;
        Settings.Instance?.LoadSettings(save);
   		Application.runInBackground = true; 
        Screen.autorotateToPortrait = false; 
        Screen.autorotateToPortraitUpsideDown = false; 
        Screen.orientation = ScreenOrientation.AutoRotation;
    }
    public void RemoveAllPlayer(){ players.Clear(); inLobby = false;}
    public void AddPlayer(Player newPlayer, Side side){
        players.Add(new PlayerController(newPlayer, side));
    }
    public void AddPlayer(int ActorNumber, Side side){ players.Add(new PlayerController(ActorNumber, side));}
    public void AddPlayer(int ActorNumber, Side side, string Name){ players.Add(new PlayerController(ActorNumber, Name, side ));}
    public void RemovePlayer(Player player){
        PlayerController controller = players.Find(x => x.player.ActorNumber == player.ActorNumber);
        if(controller != null){
            controller.DeleteAll();
            players.Remove( controller );
            CheckTable();
        }else{
            Debug.Log("error! not found num = " + player.ActorNumber.ToString());
        }
    }
    // public void RemovePlayer(int IDremove){
    //     PlayerController controller = players.Find(x => x.player.ActorNumber == IDremove);
    //     controller.DeleteAll();
    //     players.Remove( controller );
    //     CheckTable();
    // }
    public string GetNamePlayer(int num){return players[num].NickName;}
    public void SetQueuePlayers(List<QueueStep> queue){
        GameControllerScript.Instance.ShowMessagePrepareGame();
        for(int i = 0; i < queue.Count; i++){
            players.Find(x => ((x.player != null) &&  (x.player.ActorNumber == queue[i].ActorNumber)) || (x.ActorNumber == queue[i].ActorNumber) )?.SetQueueStep(queue[i].numQueue);            
        }
    }
    public void StartStep(int queue){
        for(int i = 0; i < players.Count; i++)
            players[i].StartStep(queue);
    }
    public void AddCardOpponent(Card card, int ActorNumber){ 
        players.Find(x => x.ActorNumber == ActorNumber)?.AddCard(card);
    }
    public bool IsEmptyCardInHands(){
        return (players.Find(x => x.CountCardInHand > 0) == null);
    }
    public int GetMyActorNumber(){return players.Find(x => x.side == Side.Me).ActorNumber;}
    public PlayerController GetMyPlayerController(){return players.Find(x => x.side == Side.Me); }
    public PlayerController GetPlayerControllerFromQueueNum(int num){ return players.Find(x => x.queueStep == num);}
    private static MainGameController instance;
    public static MainGameController Instance{get => instance;}


//API
    List<Player> waitConnectPlayer = new List<Player>(); 
    public void WaitPlayer(Player player){
        if(MessageWaitConnectPlayer.Instance != null){
            waitConnectPlayer.Add(player);
            MessageWaitConnectPlayer.Instance.StartWait();
        }
    }
    public void StopWaitPlayer(Player player){
        Player waitPlayer = waitConnectPlayer.Find(x => x.NickName == player.NickName);
        if(waitPlayer != null) {
            players.Find(x => x.player.NickName == player.NickName).player = player;
            waitConnectPlayer.Remove(waitPlayer);
            if(GameControllerScript.Instance.IsMasterClient){
                SendStatusGame(player);
            }
        }    
        if(waitConnectPlayer.Count == 0){
            MessageWaitConnectPlayer.Instance?.StopWait();
        }
    }
    void SendStatusGame(Player player){
        GameStatus gameStatus = new GameStatus();
        NetworkCommand commandStartGame = new NetworkCommand(TypeCommand.GameStatus, gameStatus);
        CommandCenterScript.Instance?.Sender?.SendCommand(commandStartGame, player);
    }
    void CheckTable(){
        if(GameControllerScript.Instance.IsGame){
            if(players.Count <= 1){
                SaveLoadScript.SaveGame(save);
                AndroidPlugin.PluginControllerScript.ToastPlugin.Show(LanguageControllerScript.GetMessage(TypeMessage.YouAloneInGame), isLong: true);
                PanelResult.Instance.GoToMainMenu();
            }
        }
    }
//Game Save
    private GameSave save = new GameSave();
    public GameSave GetSave{get => save;}
    public bool SetNewPlayerName(string newName){
        Debug.Log(string.Concat("save new name: ", newName));
        AndroidPlugin.PluginControllerScript.ToastPlugin.Show(string.Concat("save new name: ", newName), isLong: true);
        save.playerName = newName;
        return SaveLoadScript.SaveGame(save);
    }
    public void MusicOnOff(bool flag){
        save.musicPlay = flag;
        SaveLoadScript.SaveGame(save);
    }
    public void SoundsOnOff(bool flag){
        save.soundsPlay = flag;
        SaveLoadScript.SaveGame(save);
    }
    public void AddCountGame(){
        save.countGame += 1;
        SaveLoadScript.SaveGame(save);
    }
    public void AddWin(){
        save.countWin += 1;
        SaveLoadScript.SaveGame(save);
    }
    public void ChangeLanguage(int newNumLanguage){
        save.numLanguage = newNumLanguage;
        SaveLoadScript.SaveGame(save);
    }
    public string GetMyName(){
        return save.playerName;
    }
    public void TryLogin(){
        if(Settings.Instance != null){
            Settings.Instance.LoadSettings(save);
            if(save.playerName.Length > 0){
                LobbyMainPanel.Instance.LoginWithSave(save.playerName);
                gameOnline = true;
            }else{
                LobbyMainPanel.Instance.DefaultAutorization();
            }
        }else{
            LobbyMainPanel.Instance.DefaultAutorization();
        }
    }
    private void ExitInMainGame(Scene scene, LoadSceneMode mode){
        if((scene.buildIndex == 0) && gameOnline){
            if(inLobby == false){
                LobbyMainPanel.Instance?.OnLeaveGameButtonClicked();
                LobbyMainPanel.Instance?.OpenSelectionPanel();
            }else{
                LobbyMainPanel.Instance?.OpenRoom();
            }
        }
    }
    public void SetOnlineStatus(bool flag){
        gameOnline = flag;
    }
}
