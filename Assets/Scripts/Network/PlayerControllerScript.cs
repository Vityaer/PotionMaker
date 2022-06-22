using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
[System.Serializable]
public class PlayerController{
	public Side side;
	public Player player;
	private string nickName = "AI";
	public string NickName{
		get{
			if(player != null) {return player.NickName;}
			else{return nickName;}
		}
	}
	public int queueStep = -1;
	public Owner owner;
	private int actorNumber = -1;
	public int ActorNumber{
		get{
			if(player != null){
				return player.ActorNumber;
			}else{
				return actorNumber;
			}
		}
	}
	public Color color{get{
		if(player != null) {return AsteroidsGame.GetColor(player.GetPlayerNumber());}
		else{return Color.blue;}
		}
	}
	public PlayerPlaceController cardController;
	public PlayerController(Player p, Side side = Side.Opponent){
		this.side = side;
		player = p;
		owner = Owner.Human;
	}
	public PlayerController(int ActorNumber, Side side = Side.Opponent){
		this.player = null;
		this.actorNumber = ActorNumber;
		this.side = side;
		owner = (side == Side.Opponent) ? Owner.AI : Owner.Human;
	}
	public PlayerController(int ActorNumber, string name, Side side = Side.Opponent){
		this.player = null;
		this.actorNumber = ActorNumber;
		nickName = name;
		this.side = side;
		owner = (side == Side.Opponent) ? Owner.AI : Owner.Human;
	}
	public void SetQueueStep(int num){
		queueStep = num;
		SetStartPosition();
	}
	private void SetStartPosition(){
		if(side == Side.Opponent){
			cardController = StartPositionController.Instance.GetControllerCard(queueStep);
		}else{
			cardController = CardInHandScript.Instance;
		}
		cardController.ShowPlayerData(NickName, color);
		cardController.PreparePlace();
	}
	public void StartStep(int queue){
		if(queue == queueStep){
			cardController.StartStep();
			if((owner == Owner.AI) &&(this.player == null)){
				BaseAIPlayer.Instance.DoStep(cardController);
			}
		}
	}
	public void AddCard(Card card, PointMove pointStart = PointMove.Deck){
        MoveControllerCard.Instance.CreateMoveCard(card, pointStart, cardController.pointMove, cardController.CreateCard, (pointStart == PointMove.Deck) );
	}
	public void AddPoints(int point){cardController.AddPoint(point);}
	public void DeleteAll(){
		cardController?.DeleteAll();
	}
	public int CountCardInHand{get => cardController.CountCardInHand;}
}

public class PlayerScoreComparer : IComparer<PlayerController>{
    public int Compare(PlayerController p1, PlayerController p2){
        if (p1.cardController.Score < p2.cardController.Score)
            return 1;
        else if (p1.cardController.Score > p2.cardController.Score)
            return -1;
        else
            return 0;
    }
}   

public enum Owner{
	AI,
	Human
}