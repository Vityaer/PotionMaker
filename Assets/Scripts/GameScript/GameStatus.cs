using System.Collections;
using System.Collections.Generic;
using UnityEngine;
	
[System.Serializable]
public class GameStatus : BaseCommand{
	public QueuePlayers queuePlayers = new QueuePlayers();
	public List<DataClosetResource> closetResource = new List<DataClosetResource>();
	public List<DataSubject> tableSubject = new List<DataSubject>();
	public List<DataPlayer> dataPlayers = new List<DataPlayer>();
	public List<ReagentName> deck = new List<ReagentName>();
	public DataSubject subjectOnWorkbench;
	public int currentQueue = -1;
}
[System.Serializable]
public class DataClosetResource{
	public List<ReagentName> card = new List<ReagentName>();
}
[System.Serializable]
public class DataSubject{
	public ReagentName main;
	public List<ReagentName> ingredients = new List<ReagentName>();
}
[System.Serializable]
public class DataPlayer{
	public List<ReagentName> cardsInHand = new List<ReagentName>();
	public int score;
	public Color color;
}