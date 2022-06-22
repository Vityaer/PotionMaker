using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CardOpponentHandScript : PlayerPlaceController{
	public List<OpponentCardController> opponentCards = new List<OpponentCardController>();
    public bool IsEmpty{get => (listCard.Count == 0);}
    public GameObject prefabCard;
	[Header("Other controllers")]
    public RectTransform mainBackground; 
    public Transform content;
    void Start(){
        bonus.SetSide(Side.Opponent);
    }
    public override void CreateCard(Card card){
        if(card != null){
    		OpponentCardController cardController = Instantiate(prefabCard, content).GetComponent<OpponentCardController>();
    		cardController.SetData(card);
    		opponentCards.Add(cardController);
            if(openCard) cardController.Open();
        	listCard.Add(card);
        }else{
            Debug.Log("прислали какую-то хуйню");
        }
    }
    [SerializeField] private HorizontalLayoutGroup horizontalLayoutGroup;
    public void RecalculateUI(){
        horizontalLayoutGroup.CalculateLayoutInputHorizontal();
    }
    public void SendCardToCloset(Card card){
    	OpponentCardController controller  = opponentCards.Find(x => x.Data.reagentName == card.reagentName);
    	controller?.MoveToCloset();
    }
    public void SendCardToTable(Card card){
    	OpponentCardController controller  = opponentCards.Find(x => x.Data.reagentName == card.reagentName);
    	controller?.MoveToTable();
    }
    public void DeleteCard(Card card){
        OpponentCardController controller = opponentCards.Find(x => x.Data.reagentName == card.reagentName);
        controller?.Delete();
    }
    public void DeleteCard(OpponentCardController controller, Card card){
        bool flag = listCard.Remove(card);
        opponentCards.Remove(controller);
    }
    public bool ContainsCard(Card card){
        return (listCard.Find(x => x.reagentName == card.reagentName) != null) ;
    }
    public void PutCardInCloset(Card card){
        opponentCards.Find(x => x.Data.reagentName == card.reagentName)?.MoveToCloset();
    }
    public void PutCardOnTable(Card card){
        opponentCards.Find(x => x.Data.reagentName == card.reagentName)?.MoveToTable();
    }
    public override void StartStep(){ instance = this; }
    [Header("Player info")]
    public Text playerName;
    public Image imagePlayerColor;
    public override void ShowPlayerData(string name, Color color){
        playerName.text = name;
        imagePlayerColor.color = color;
    }
    private static CardOpponentHandScript instance;
    public static CardOpponentHandScript Instance{get => instance;}

    public void GetCards(){
        StartCoroutine(GetUpCardBeforeFullHand());   
    }
    IEnumerator GetUpCardBeforeFullHand(){
        GiveCard giveCard = null;
        for(int i = listCard.Count; i < 5; i++){
            if(DeckControllerScript.Instance.NotEmpty){
                Card card = DeckControllerScript.Instance.GetCard();
                giveCard = new GiveCard(card.ID, SideGive.Opponent, ActorNumber: 1, isOrderFromServer: false);
                DeckControllerScript.Instance.GiveCardInHand(giveCard);  
                yield return new WaitForSeconds(0.2f);
            }else{
                break;
            }
        }
    }

    bool openCard = false; 
    [ContextMenu("Open/close all card")]
    public void OpenCloseAllCard(){
        openCard = !openCard;
        foreach(OpponentCardController controller in opponentCards){
            if(openCard){
                controller.Open();
            }else{
                controller.Close();
            }
        }
    } 
}
