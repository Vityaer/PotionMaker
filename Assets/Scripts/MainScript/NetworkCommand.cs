using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class NetworkCommand{
	private static int commandID = -1;
	public static int CurrentIDCommand{get => commandID;}
	public static void SetCurrentID(int newID){if(newID > commandID) commandID = newID; }
	public TypeCommand type = TypeCommand.ChatMessage;
	public BaseCommand command = null;
	public void PrepareID(){command.GetID();}
	public string info{
		get{
			return JsonUtility.ToJson(command);
		}
	}
	public int ID{get => command.commandID;}
	
	public NetworkCommand(TypeCommand type){
		this.type = type;
		this.command = new BaseCommand();
	}
	public NetworkCommand(TypeCommand type, BaseCommand command){
		this.type = type;
		this.command = command;
	}
	public NetworkCommand(TypeCommand type, string stringCommand){
		this.type = type;
		command = new BaseCommand();
		Debug.Log(stringCommand);
		switch(type){
			case TypeCommand.PutOnTable:
				command = (GiveCard) JsonUtility.FromJson<GiveCard>(stringCommand);
				break;
			case TypeCommand.PuInCloset:
				command = (CardPutInCloset)JsonUtility.FromJson<CardPutInCloset>(stringCommand);	
				break;
			case TypeCommand.GetReagentFromCloset:
				command = (ObjectID)JsonUtility.FromJson<ObjectID>(stringCommand);	
				break;
			case TypeCommand.GetSubjectFromTable:
				command = (ObjectID)JsonUtility.FromJson<ObjectID>(stringCommand);	
				break;
			case TypeCommand.PlayerLeftScene:
				command = (ObjectID)JsonUtility.FromJson<ObjectID>(stringCommand);	
				break;
			case TypeCommand.MagicGetCardFromCloset:
				command = (ObjectID)JsonUtility.FromJson<ObjectID>(stringCommand);	
				break;
			case TypeCommand.MagicSwapSubjectAndIngedient:	
				command = (SwapSubjectAndIngredient)JsonUtility.FromJson<SwapSubjectAndIngredient>(stringCommand);	
				break;
			case TypeCommand.MagicSwapSubjectAndReagent:
				command = (SwapSubjectAndReagent)JsonUtility.FromJson<SwapSubjectAndReagent>(stringCommand);
				break;
			case TypeCommand.GiveCard:
				command = (GiveCard)JsonUtility.FromJson<GiveCard>(stringCommand);	
				break;
			case TypeCommand.ChatMessage:
				command = JsonUtility.FromJson<ChatMessage>(stringCommand);	
				break;
			case TypeCommand.SetQueuePlayer:
				command = (QueuePlayers)JsonUtility.FromJson<QueuePlayers>(stringCommand);	
				break;
			case TypeCommand.ChangeScene:
				command = JsonUtility.FromJson<BaseCommand>(stringCommand);
				break;
			case TypeCommand.CancelCrafting:
				command = JsonUtility.FromJson<BaseCommand>(stringCommand);
				break;
			case TypeCommand.LoadedGame:
				command = JsonUtility.FromJson<BaseCommand>(stringCommand);
				break;
			case TypeCommand.RequestLoaded:
				command = JsonUtility.FromJson<BaseCommand>(stringCommand);
				break;
			case TypeCommand.StartStep:
				command = JsonUtility.FromJson<BaseCommand>(stringCommand);
				break;
			case TypeCommand.FinishStep:
				command = JsonUtility.FromJson<BaseCommand>(stringCommand);
				break;
			default:
				Debug.Log(type.ToString() + " not exist");
				break;	
		}
	}
	public static int GetNewID(){ return ++commandID; }
	public void SetData(TypeCommand type, BaseCommand newCommand){ this.type = type; this.command = newCommand; }
	public void SetData(BaseCommand newCommand){ this.command = newCommand; }
}

public enum TypeCommand{
	PutOnTable = 1,
	PuInCloset = 2,
	StartStep = 3,
	CreatePlayers = 4,
	SetID = 5,
	ChatMessage = 6,
	GetReagentFromCloset = 7,
	GiveCard = 8,
	CancelCrafting = 9,
	FinishStep = 10,
	SetQueuePlayer = 11,
	GetSubjectFromTable = 12,
	MagicGetCardFromCloset = 21,
	MagicSwapSubjectAndReagent = 22,
	MagicSwapSubjectAndIngedient = 23,
	PlayerLeftScene = 24,
	ChangeScene = 25,
	RequestLoaded = 31,
	LoadedGame = 32,
	GameStatus = 42
}


//Classes command
	[System.Serializable]
	public class BaseCommand{
		public int commandID = 0; 
		public void GetID(){
			commandID = NetworkCommand.GetNewID();
		}
	}
	[System.Serializable]
	public class ActionWithCard: BaseCommand{
		public int ID;
	}

	[System.Serializable]
	public class GiveCard : BaseCommand{
		public int ID;
		public SideGive side;
		public int ActorNumber;
		public bool isOrderFromServer = false;
		public GiveCard(int name, SideGive side, int ActorNumber, bool isOrderFromServer){
			this.ActorNumber = ActorNumber;
			this.ID = name;
			this.side = side;
			this.isOrderFromServer = isOrderFromServer;
		}
	}
	[System.Serializable]
	public class CardPutInCloset : BaseCommand{
		public int ID;
		public SideGive side;
		public bool withReward = false;
		public CardPutInCloset(int name, SideGive side, bool withReward){
			this.ID = name;
			this.side = side;
			this.withReward = withReward;
		}
	}
	[System.Serializable]
	public class QueuePlayers : BaseCommand{
		public List<QueueStep> queue = new List<QueueStep>();
		public QueuePlayers(List<QueueStep> queue){
			this.queue = queue;
		} 
		public QueuePlayers(){}
	}

	[System.Serializable]
	public class QueueStep: BaseCommand{
		public int ActorNumber = -1;
		public int numQueue = -1;
		public QueueStep(int ActorNumber, int numQueue){
			this.ActorNumber = ActorNumber;
			this.numQueue    = numQueue;
		}
	}
	[System.Serializable]
	public class SwapSubjectAndReagent: BaseCommand{
		public int subjectName, reagentName;
		public SwapSubjectAndReagent(int subjectName, int reagentName){
			this.subjectName = subjectName;
			this.reagentName = reagentName;
		}
	}
	[System.Serializable]
	public class SwapSubjectAndIngredient: BaseCommand{
		public int subjectName;
		public int numSwap;
		public SwapSubjectAndIngredient(int subjectName, int numSwap){
			this.subjectName = subjectName;
			this.numSwap = numSwap;
		}
	}
	[System.Serializable]
	public class ChatMessage : BaseCommand{
		public string message;
	}
	[System.Serializable]
	public class ObjectID : BaseCommand{
		public int objectID;
		public ObjectID(int id){
			objectID = id;
		}
	}

	
public enum SideGive{
	Me,
	Opponent,
	Deck
}