using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewLanguage", menuName = "Custom ScriptableObject/New Language", order = 52)]
public class LanguageObject : ScriptableObject{
	public int IDLanguage;
	public GeneralDefinition generalInfo;
	public List<ReagentLocalization> listReagentLocalization = new List<ReagentLocalization>(); 
	public List<MessageLocalization> listMessageLocalization = new List<MessageLocalization>();
	public List<TextLocalization>    listTextLocalization    = new List<TextLocalization>();

	public string GetMessage(TypeMessage type){
		MessageLocalization message = listMessageLocalization.Find(x => x.name == type);
		if(message == null) {Debug.Log("not founded " + type.ToString());  return string.Empty;}
		return message.containText;
	}
	public string GetBigText(TypeBigText type){
		TextLocalization bigText = listTextLocalization.Find(x => x.name == type);
		if(bigText == null){ Debug.Log("not founded " + type.ToString()); return string.Empty;} 
		return bigText.containText;
	}
	public ReagentLocalization GetLocalizationReagent(ReagentName name){
		return listReagentLocalization.Find(x => x.reagentName == name);
	}
	public string GetWord(TypeWord type){
		string result = string.Empty; 
		switch(type){
			case TypeWord.Settings:
				result = generalInfo.Settings;
				break;
			case TypeWord.Nickname:
				result = generalInfo.Nickname;
				break;
			case TypeWord.Back:
				result = generalInfo.Back;
				break;
			case TypeWord.FinishStep:
				result = generalInfo.FinishStep;
				break;
			case TypeWord.Ready:
				result = generalInfo.Ready;
				break;
			case TypeWord.NotReady:
				result = generalInfo.NotReady;
				break;
			case TypeWord.StartGame:
				result = generalInfo.StartGame;
				break;
			case TypeWord.CreateRoom:
				result = generalInfo.CreateRoom;
				break;
			case TypeWord.RandomRoom:
				result = generalInfo.RandomRoom;
				break;
			case TypeWord.LeaveRoom:
				result = generalInfo.LeaveRoom;
				break;
			case TypeWord.ShowListRooms:
				result = generalInfo.ShowListRooms;
				break;	
			case TypeWord.language:
				result = generalInfo.language;
				break;	
			case TypeWord.sounds:
				result = generalInfo.sounds;
				break;	
			case TypeWord.music:
				result = generalInfo.music;
				break;	
			case TypeWord.countGame:
				result = generalInfo.countGame;
				break;	
			case TypeWord.countWin:
				result = generalInfo.countWin;
				break;
			case TypeWord.winRate:
				result = generalInfo.winRate;
				break;
			case TypeWord.resultWin:
				result = generalInfo.resultWin;
				break;
			case TypeWord.resultDefeat:
				result = generalInfo.resultDefeat;
				break;
			case TypeWord.resultDraw:
				result = generalInfo.resultDraw;
				break;
			case TypeWord.ApplicationName:
				result = generalInfo.ApplicationName;
				break;
			case TypeWord.PrintNickname:
				result = generalInfo.PrintNickname;
				break;
			case TypeWord.PrintRoomName:
				result = generalInfo.PrintRoomName;
				break;	
			case TypeWord.ConnectToServer:
				result = generalInfo.ConnectToServer;
				break;
			case TypeWord.ConnectToRoom:
				result = generalInfo.ConnectToRoom;
				break;	
			case TypeWord.NameRoom:
				result = generalInfo.NameRoom;
				break;
			case TypeWord.CountPlayers:
				result = generalInfo.CountPlayers;
				break;
			case TypeWord.Create:
				result = generalInfo.Create;
				break;	
			case TypeWord.ConnectToRandomRoom:
				result = generalInfo.ConnectToRandomRoom;
				break;	
			case TypeWord.Statistic:
				result = generalInfo.Statistic;
				break;		
			case TypeWord.Player:
				result = generalInfo.Player;
				break;
			case TypeWord.Room:
				result = generalInfo.Room;
				break;
			case TypeWord.InMainMenu:
				result = generalInfo.InMainMenu;
				break;
			case TypeWord.MyStep:
				result = generalInfo.MyStep;
				break;
			case TypeWord.OpponentStep:
				result = generalInfo.OpponentStep;
				break;	
			case TypeWord.FinishGame:
				result = generalInfo.FinishGame;
				break;	
			case TypeWord.PlayerPlace:
				result = generalInfo.PlayerPlace;
				break;
			case TypeWord.Score:
				result = generalInfo.Score;
				break;
			case TypeWord.Table:
				result = generalInfo.Table;
				break;
			case TypeWord.Closet:
				result = generalInfo.Closet;
				break;	
			case TypeWord.You:
				result = generalInfo.You;
				break;
			case TypeWord.GameAI:
				result = generalInfo.GameAI;
				break;
			case TypeWord.Online:
				result = generalInfo.Online;
				break;	
			case TypeWord.Exit:
				result = generalInfo.Exit;
				break;	
			case TypeWord.Rules:
				result = generalInfo.Rules;
				break;
			case TypeWord.Swap:
				result = generalInfo.Swap;
				break;
			case TypeWord.Select:
				result = generalInfo.Select;
				break;																		
			default:
				Debug.Log(type.ToString() + " not found");
				result = "problem";
				break;																	
		}
		return result;
	}
}

[System.Serializable]
public class ReagentLocalization{
	public string Name;
	public ReagentName reagentName;
	public string Description;
}

[System.Serializable]
public class MessageLocalization{
	public TypeMessage name;
	public string containText;
}
[System.Serializable]
public class TextLocalization{
	public TypeBigText name;
	[TextArea(minLines: 2, maxLines: 200)]
	public string containText;
}
[System.Serializable]
public class GeneralDefinition{
	public string ApplicationName;
	public string Settings;
	public string Nickname;
	public string Back;
	public string FinishStep;
	public string Ready;
	public string NotReady;
	public string StartGame;
	public string CreateRoom;
	public string RandomRoom;
	public string LeaveRoom;
	public string ShowListRooms;
	public string language;
	public string sounds;
	public string music;
	public string countGame;
	public string countWin;
	public string winRate;
	public string resultWin;
	public string resultDefeat;
	public string resultDraw;
	public string PrintNickname;
	public string PrintRoomName;
	public string ConnectToServer;
	public string ConnectToRoom;
	public string NameRoom;
	public string CountPlayers;
	public string Create;
	public string ConnectToRandomRoom;
	public string Statistic;
	public string Player;
	public string Room;
	public string InMainMenu;
	public string MyStep;
	public string OpponentStep; 
	public string FinishGame;
	public string PlayerPlace;
	public string Score;
	public string Table;
	public string Closet;
	public string You;
	public string GameAI;
	public string Online;
	public string Rules;
	public string Exit;
	public string Swap;
	public string Select;
}
public enum TypeWord{
	Settings,
	Nickname,
	Back,
	FinishStep,
	Ready,
	NotReady,
	StartGame,
	CreateRoom,
	RandomRoom,
	LeaveRoom,
	ShowListRooms,
	language,
	sounds,
	music,
	countGame,
	countWin,
	winRate,
	resultWin,
	resultDefeat,
	resultDraw,
	ApplicationName,
	PrintNickname,
	PrintRoomName,
	ConnectToServer,
	ConnectToRoom,
	NameRoom,
	CountPlayers,
	Create,
	ConnectToRandomRoom,
	Statistic,
	Player,
	Room,
	PlayerPlace,
	Score,
	InMainMenu,
	MyStep,
	OpponentStep,
	FinishGame,
	Table,
	Closet,
	You,
	GameAI,
	Online,
	Rules,
	Exit,
	Swap,
	Select
}

public enum TypeMessage{
	DefineQueuePlayers = 0,
	WaitPlayersLoaded = 1,
	GiveCardsAllPlayers = 2,
	SubjectsNotCreated = 3,
	ClosetEmpty = 4,
	NicknameEmpty = 5,
	SaveNewNickname = 6,
	SaveError = 7,
	YouAloneInGame = 8,
	PlayerLeftGame = 9,
	ClosetNotContainsReagent = 10
}
public enum TypeBigText{
	Rules_Closet = 0,
	Rules_Table = 1,
	Rules_Table_create_receipt = 2,
	Rules_count_action = 3,
	Rules_GreatElexir = 4,
	Rules_GreatAmulet = 5,
	Rules_Magic = 6,
	Rules_kind_cards = 7
}