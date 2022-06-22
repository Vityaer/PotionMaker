using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using Photon.Pun.Demo.Asteroids;
public class CommandCenterScript : MonoBehaviour{


	[Header("Links")]
	public ClientScript Sender;
	GiveCard giveCard = null;
	Card card;
	// public List<NetworkCommand> commands = new List<NetworkCommand>();
	public void AddCommand(NetworkCommand newCommand){
		Debug.Log("add command");
		ExecuteCommand(newCommand);
	}
	// public int curentIDCommand = 0;
	// private void ExecuteNextCommand(){
	// 	for(int i = 0; i < commands.Count; i++){
	// 		if(commands[i].ID == curentIDCommand){

	// 		}
	// 		command = commands.Find(x => x.ID == curentIDCommand);
	// 		break;
	// 	}
	// 	if(command != null){
	// 		ExecuteCommand(command);
	// 		NetworkCommand.SetCurrentID(curentIDCommand);
	// 	}
	// }
	private void ExecuteCommand(NetworkCommand curentCommand){
		if(curentCommand != null){
			Debug.Log("command null = " + (curentCommand.command == null ).ToString());
			Debug.Log("ExecuteCommand");
			switch(curentCommand.type){
				case TypeCommand.PutOnTable:
					giveCard = (GiveCard)curentCommand.command as GiveCard;
					card = ReagentsManager.FindCardFromID(giveCard.ID);
					CardOpponentHandScript.Instance?.DeleteCard(card);
					TableScript.Instance.PutDownCard(card, isMyCraft: false);
					break;
				case TypeCommand.PuInCloset:
					CardPutInCloset cardPutInCloset = (CardPutInCloset)curentCommand.command as CardPutInCloset;
					if(cardPutInCloset != null){
						card = ReagentsManager.FindCardFromID(cardPutInCloset.ID);
						switch(cardPutInCloset.side){
							case SideGive.Opponent:
								CardOpponentHandScript.Instance.PutCardInCloset(card);
								break;
							case SideGive.Deck:
								DeckControllerScript.Instance.DeleteCard(card.ID);
								DeckControllerScript.Instance.PutCardInCloset(card);
								break;
							default:
								Debug.Log(cardPutInCloset.side.ToString());
								break;	
						}
					}
					break;
				case TypeCommand.MagicGetCardFromCloset:
					int IDMagicCardFromCloset = (curentCommand.command as ObjectID).objectID;	
					Card deleteFromCosetCard = ReagentsManager.FindCardFromID(IDMagicCardFromCloset);
					ClosetScript.Instance.GetUpReagent(deleteFromCosetCard);
					break;	
				case TypeCommand.MagicSwapSubjectAndIngedient:
					SwapSubjectAndIngredient swapSubjectAndIngredient = (SwapSubjectAndIngredient)curentCommand.command as SwapSubjectAndIngredient;	
					TableScript.Instance.subjects.Find(x => x.Data.ID == swapSubjectAndIngredient.subjectName).SwapCard(swapSubjectAndIngredient.numSwap);
					break;
				case TypeCommand.MagicSwapSubjectAndReagent:
					SwapSubjectAndReagent swapSubjectAndReagent = (SwapSubjectAndReagent)curentCommand.command as SwapSubjectAndReagent;
					Card subjectCard = ReagentsManager.FindCardFromID(swapSubjectAndReagent.subjectName);
					Card reagentCard = ReagentsManager.FindCardFromID(swapSubjectAndReagent.reagentName);
					TableScript.Instance.ChangeSubjectMagicSwap(subjectCard, reagentCard);
					ClosetScript.Instance.ChangeReagentMagicSwap(reagentCard, subjectCard);
					break;	
				case TypeCommand.StartStep:
					GameControllerScript.Instance.StartStep();
					break;	
				case TypeCommand.FinishStep:
					GameControllerScript.Instance.FinishStep();	
					break;
				case TypeCommand.GetReagentFromCloset:
					int ID = (curentCommand.command as ObjectID).objectID;	
					card = ReagentsManager.FindCardFromID(ID);
					ClosetScript.Instance.SendReagentToWorkBench(card);
					break;
				case TypeCommand.GetSubjectFromTable:
					int IDSubject = (curentCommand.command as ObjectID).objectID;
					TableScript.Instance.ClickOnSubject(IDSubject);
					break;
				case TypeCommand.GiveCard:
					giveCard = (GiveCard)curentCommand.command as GiveCard;
					DeckControllerScript.Instance.GiveCardInHand(giveCard);
					break;	
				case TypeCommand.CancelCrafting:
					WorkBench.Instance.CancelCraft();	
					break;	
				case TypeCommand.ChatMessage:
	                ChatControllerScript.Instance?.PrintMessage((curentCommand.command as ChatMessage).message );
					break;
				case TypeCommand.PlayerLeftScene:
					int IDLeftPlayer = (curentCommand.command as ObjectID).objectID;	
	                AndroidPlugin.PluginControllerScript.ToastPlugin.Show(LanguageControllerScript.GetMessage(TypeMessage.PlayerLeftGame), isLong: true);
					// MainGameController.Instance.RemovePlayer(IDLeftPlayer);
					break;
				case TypeCommand.SetQueuePlayer:
				    QueuePlayers queuePlayers = (QueuePlayers)curentCommand.command as QueuePlayers;	
				    MainGameController.Instance.SetQueuePlayers(queuePlayers.queue);
					break; 
				case TypeCommand.LoadedGame:
					GameControllerScript.Instance.AddPlayerCountLoadedGame();
					break;
				case TypeCommand.RequestLoaded:
					Debug.Log("send loaded");
					GameControllerScript.Instance?.GetRequestOnLoaded();
					break;		
				case TypeCommand.ChangeScene:
					LobbyMainPanel.Instance?.CommandLoadGame();
					break;
				default:
					if(Console.Instance != null) {
			            Console.Instance.Print(curentCommand.command.ToString());
			        }else{
						Debug.Log(curentCommand.info);
			        }
					break;		
			}
		}
		// curentIDCommand++;
		// commands.Remove(command);
		// command = null;
	}
	private static CommandCenterScript instance;
	public  static CommandCenterScript Instance{get => instance;}
	void Awake(){
		Application.runInBackground = true;
		if(instance == null){
			instance = this;
		}else{
			Debug.Log("CommandCenterScript twice");
			Destroy(this);
		}
	}
	void Start(){
		// curentIDCommand = NetworkCommand.CurrentIDCommand;
	}
	public void SendCommand(NetworkCommand command){
		Sender.SendCommand(command);
	}
}