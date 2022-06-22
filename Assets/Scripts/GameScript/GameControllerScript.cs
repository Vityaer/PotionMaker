using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;

using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using AndroidPlugin;
public class GameControllerScript : MonoBehaviour{
	// private int CountFinishStep = 10;
    public int CurrentQueue = 1;
	public Side side = Side.Me;
	public bool MyStep{get => side == Side.Me;}
	public Text queueStepText;
	public void StartStep(){
		this.controller = MainGameController.Instance.GetPlayerControllerFromQueueNum(CurrentQueue);
		if(this.controller != null){
			side = this.controller.side;
			CheckGame();
			OnItIsMyStep(controller.side == Side.Me);
			controller.StartStep(CurrentQueue);
			OnStartStep();
			UpdateUI();
		}
	}
	Coroutine coroutineCheckGame;
	public void CheckGame(){
		// if(DeckControllerScript.Instance.NotEmpty == false) CountFinishStep -= 1;
		// if(CountFinishStep == 0) FinishGame();
		if( (DeckControllerScript.Instance.NotEmpty == false) && MainGameController.Instance.IsEmptyCardInHands()){
			coroutineCheckGame = StartCoroutine(ICanCheckFinishGame());
		} 

	}
	public FinishTurnScript finishTurnScript;
	Coroutine corouniteFinish;
	public void FinishStep(){
		corouniteFinish = StartCoroutine(ICanFinish());
	}
	private bool isGame = true;
	public bool IsGame{get => isGame;}
	private void FinishGame(){
		isGame = false;
		PanelResult.ShowResults();
	}
	private PlayerController controller = null;
	public PlayerController Contoller{get => controller;}
	public void UpdateUI(){
		if(isGame){
			if(controller.side == Side.Me) {
				finishTurnScript.SetTurn(true);
			}
			queueStepText.text = (controller.side == Side.Me) ? LanguageControllerScript.GetWord(TypeWord.MyStep) : string.Concat(LanguageControllerScript.GetWord(TypeWord.OpponentStep), " " , (controller == null) ? string.Empty : controller.NickName);
		}else{
			queueStepText.text = LanguageControllerScript.GetWord(TypeWord.FinishGame);	
			finishTurnScript.SetTurn(false);
		}
	}
	private static GameControllerScript instance;
	public static GameControllerScript Instance{get => instance;}

	private Action observerStartStep;
	public void RegisterOnStartStep(Action d){observerStartStep += d;}
	public void UnregisterOnStartStep(Action d){observerStartStep -= d;}
	private void OnStartStep(){if(observerStartStep != null) observerStartStep();}

	Vector2 posHideButtonFinishStep = new Vector2(Screen.width, Screen.height);

	private void GiveCardToOpponent(Card card, int ActorNumber){
        GiveCard giveCard = new GiveCard(card.ID, SideGive.Deck, ActorNumber, isOrderFromServer: true);
        commandGiveCard.SetData(giveCard);
        CommandCenterScript.Instance?.SendCommand(commandGiveCard);
        DeckControllerScript.Instance.GiveCardWithoutDelete(giveCard);
    }
    NetworkCommand commandGiveCard = new NetworkCommand(TypeCommand.GiveCard);
    NetworkCommand commandCardPutInCloset = new NetworkCommand(TypeCommand.PuInCloset);
    NetworkCommand commandStartStep = new NetworkCommand(TypeCommand.StartStep);
	

	Coroutine coroutineCreateGame;
    IEnumerator ICreateGame(){
    	DefineQueue();
		yield return new WaitForSeconds(0.5f);

    	queueStepText.text = LanguageControllerScript.GetMessage(TypeMessage.GiveCardsAllPlayers);
    	for(int i = 0; i < 4; i++){
			CardInHandScript.Instance.GetUpCardFromDeck(DeckControllerScript.Instance.GetCard());
			yield return new WaitForSeconds(0.2f);
			if(PhotonNetwork.CurrentRoom != null){
				for(int j = 1; j < PhotonNetwork.CurrentRoom.PlayerCount; j++){
					GiveCardToOpponent(DeckControllerScript.Instance.GetCard(), PhotonNetwork.PlayerList[j].ActorNumber);
					yield return new WaitForSeconds(0.2f);
				}
			}else{
				for(int j = 1; j < MainGameController.Instance.CountPlayers; j++){
					GiveCardToOpponent(DeckControllerScript.Instance.GetCard(), MainGameController.Instance.players[j].ActorNumber);
					yield return new WaitForSeconds(0.2f);
				}
			}
		}
		Card card = null;
		for(int i = 0; i < 4; i++){
			card = DeckControllerScript.Instance.GetNotMagicCard();
			DeckControllerScript.Instance.PutCardInCloset(card);
			CardPutInCloset cardPutInCloset = new CardPutInCloset(card.ID, SideGive.Deck, false);
			commandCardPutInCloset.SetData(cardPutInCloset);
	        CommandCenterScript.Instance.SendCommand(commandCardPutInCloset);
			yield return new WaitForSeconds(0.2f);
		}
		while(currentAction > 0){ yield return null; }
		yield return new WaitForSeconds(0.5f);
        CommandCenterScript.Instance.SendCommand(commandStartStep);
    	StartStep();
    }

    private Action<bool> observerItIsMyStep;
    public void RegisterOnMyStep(Action<bool> d){ observerItIsMyStep += d; }
    public void UnregisterOnMyStep(Action<bool> d){ observerItIsMyStep -= d; }
    private void OnItIsMyStep(bool flag){ if(observerItIsMyStep != null) observerItIsMyStep(flag);}


#region UNITY
	void Awake(){
		instance = this;
		if(LanguageControllerScript.Instance != null)
		queueStepText.text = LanguageControllerScript.GetMessage(TypeMessage.WaitPlayersLoaded);
	}
	void Start(){
		if((MainGameController.Instance != null) && (MainGameController.Instance.GameStatusOnline)){
			if(PhotonNetwork.IsMasterClient) {
				AddPlayerCountLoadedGame();
				SendRequestLoaded();
			}	
			CurrentQueue = 1;
		}else{
			CreateGameVersusAI();
		}

	}
	private void CheckCountPlayers(){
		if(PhotonNetwork.IsMasterClient){
			if(countPlayersLoadedGame == PhotonNetwork.CurrentRoom.PlayerCount){
				StopCoroutine(requestLoaded);
				requestLoaded = null;
	    		coroutineCreateGame = StartCoroutine(ICreateGame());
			}
    	}
	}
	private int countPlayersLoadedGame = 0;
	public void AddPlayerCountLoadedGame(){
		if(PhotonNetwork.IsMasterClient){
			countPlayersLoadedGame++;
			CheckCountPlayers();
		}
	}
#endregion
	private void AddQueue(){
		CurrentQueue += 1;
		if(CurrentQueue > MainGameController.Instance.CountPlayers){
			CurrentQueue = 1;
		}
	}
	public int CountPlayer{ 
		get{
			if(PhotonNetwork.CurrentRoom != null) {return PhotonNetwork.CurrentRoom.PlayerCount;}
			else{return MainGameController.Instance.CountPlayers;}
		}
	}
	private void DefineQueue(){
    	queueStepText.text = LanguageControllerScript.GetMessage(TypeMessage.DefineQueuePlayers);


		List<int> listNum = new List<int>();
		for(int i = 0; i < MainGameController.Instance.CountPlayers; i++) listNum.Add(i + 1);
		QueuePlayers queuePlayers = new QueuePlayers();
		int random = 0, count = listNum.Count;
		if(PhotonNetwork.CurrentRoom != null){
			for(int i = 0; i < count; i++){
				random = UnityEngine.Random.Range(0, listNum.Count);
				queuePlayers.queue.Add(new QueueStep(PhotonNetwork.PlayerList[i].ActorNumber, listNum[random]));
				listNum.RemoveAt(random);
			}
	        CommandCenterScript.Instance.SendCommand(new NetworkCommand(TypeCommand.SetQueuePlayer, queuePlayers ));
		}else{
			for(int i = 0; i < count; i++){
				random = UnityEngine.Random.Range(0, listNum.Count);
				queuePlayers.queue.Add(new QueueStep(MainGameController.Instance.players[i].ActorNumber, listNum[random]));
				listNum.RemoveAt(random);
			}
		}
		MainGameController.Instance.SetQueuePlayers(queuePlayers.queue);
	}
	public void ShowMessagePrepareGame(){
    	queueStepText.text = LanguageControllerScript.GetMessage(TypeMessage.GiveCardsAllPlayers);
	}

//API
	[Header("API")]
	public Card UseCard;
	public void PrintError(string error, PointMove pointStart){
		AudioControllerScript.Instance.PlayError();
		AndroidPlugin.PluginControllerScript.ToastPlugin.Show(error, isLong: true);
		queueStepText.text = error;
		if(UseCard != null){
			controller.AddCard(UseCard, pointStart);
			UseCard = null;
		}
	}
	public void PrintError(string error){
		AudioControllerScript.Instance.PlayError();
		AndroidPlugin.PluginControllerScript.ToastPlugin.Show(error, isLong: true);
		queueStepText.text = error;
	}

	public int currentAction = 0;
	public Action observerActionsFinish;
	public void RegisterOnActionsFinish(Action d){
		observerActionsFinish += d;
		if(currentAction == 0) d();
	}
	public void UnregisterOnActionsFinish(Action d){observerActionsFinish -= d;}
	public void AddAction(){currentAction += 1;}
	public void SubAction(){currentAction -= 1; if((currentAction == 0) &&(observerActionsFinish != null)) observerActionsFinish(); }
	IEnumerator ICanFinish(){
		while(currentAction > 0){ yield return null; }
		UseCard = null;
		WorkBench.Instance.CancelCraft();
		if((PhotonNetwork.CurrentRoom != null) && (PhotonNetwork.CurrentRoom.PlayerCount > 1)){
			if(controller.side == Side.Me){
				CommandCenterScript.Instance?.Sender?.SendCommand(new NetworkCommand(TypeCommand.FinishStep));
			}
		}
		AddQueue();
		StartStep();
	}
	IEnumerator ICanCheckFinishGame(){
		while(currentAction > 0){ yield return null; }
		yield return new WaitForSeconds(0.2f);
		if( (DeckControllerScript.Instance.NotEmpty == false) && MainGameController.Instance.IsEmptyCardInHands()){
			FinishGame();
		}
	}
	public bool IsMasterClient{get => PhotonNetwork.IsMasterClient;}

//Check loaded player
	Coroutine requestLoaded = null;
	private void SendRequestLoaded(){
		requestLoaded = StartCoroutine(ISendRequestLoaded());
	}

	IEnumerator ISendRequestLoaded(){
		if(PhotonNetwork.IsMasterClient){
			while(countPlayersLoadedGame < PhotonNetwork.CurrentRoom.PlayerCount){
			    CommandCenterScript.Instance.SendCommand(new NetworkCommand(TypeCommand.RequestLoaded));
			    // CommandCenterScript.Instance.SendCommand(new NetworkCommand(TypeCommand.LoadedGame));
				yield return new WaitForSeconds(1f);
			}
		}
	}

	private bool sendedLoadGame = false;
	public void GetRequestOnLoaded(){
		if(sendedLoadGame == false){
			sendedLoadGame = true;
		    CommandCenterScript.Instance.SendCommand(new NetworkCommand(TypeCommand.LoadedGame));
		}
	}
// Game versus AI
	public void CreateGameVersusAI(){
		if(MainGameController.Instance == null){
			MainGameController mc = gameObject.AddComponent(typeof(MainGameController)) as MainGameController;
		}
		if(MainGameController.Instance.CountPlayers == 0){
			string myName = "You";
			myName = MainGameController.Instance.GetMyName();
			MainGameController.Instance.RemoveAllPlayer();
	        MainGameController.Instance.AddPlayer(0, Side.Me, myName);
	        MainGameController.Instance.AddPlayer(1, Side.Opponent);
	        coroutineCreateGame = StartCoroutine(ICreateGame());
		}
	}	
}