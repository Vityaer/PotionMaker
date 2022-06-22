using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System; 
using DG.Tweening;
public class CardInHandScript : PlayerPlaceController{
    private List<CardController> controllers = new List<CardController>();
    public bool IsEmpty{get => (listCard.Count == 0);}
    public GameObject prefabCard;
    public GameObject prefabEmptyPlace;
    private Queue<GameObject> emptyPlaces = new Queue<GameObject>();
    public RectTransform mainBackground; 
    public Canvas mainCanvas;
    public Side side;
    public Transform content;
    void Awake(){
        instance = this;
    }
    void Start(){
        GameControllerScript.Instance.RegisterOnMyStep(CanGetUpCard);
        bonus.SetSide(Side.Me);
    }
    public static bool IsMyStep = false;
    private void CanGetUpCard(bool flag){
        IsMyStep = flag;
    }
    private void GetCards(){
        StartCoroutine(GetUpCardBeforeFullHand());   
    }
    IEnumerator GetUpCardBeforeFullHand(){
        for(int i = listCard.Count; i < 5; i++){
            if(DeckControllerScript.Instance.NotEmpty){
                GetUpCardFromDeck(DeckControllerScript.Instance.GetCard());  
                yield return new WaitForSeconds(0.2f);
            }else{
                break;
            }
        }
    }

    public void GetUpCardFromDeck(Card card){
        if(card != null){
            SendMessage(card);
            GameObject gameObjectEmptyPlace = Instantiate(prefabEmptyPlace, content);
            MoveControllerCard.Instance.CreateMoveCard(card, PointMove.Deck, GetPosForCard(requestDelta + controllers.Count, true), CreateCard, false);
            requestDelta++;
            OnChangeCountCard();
            emptyPlaces.Enqueue(gameObjectEmptyPlace);
            gameObjectEmptyPlace.GetComponent<EmptyPlaceForCard>().StartAnimOpen();

        }
    }
    public Vector3 GetPosForCard(int index, bool newCard = false){
        return transform.position + GetDelta(index, newCard);
    }
    public int requestDelta = -1;
    private Vector3 GetDelta(int index, bool newCard = false){
        Vector3 delta = Vector3.zero;
        delta.x = (2 * index - (controllers.Count + requestDelta) + 1f + (newCard ? -0.5f : 0f)) * 84.375f;
        return delta;
    }
    public override void CreateCard(Card card){
        if(requestDelta > 0) requestDelta -= 1;
    	RevertCard(card);
    }
    
    public void RevertCard(Card card){
        int numPlace = -1;
        if(emptyPlaces.Count > 0){
            GameObject empty = emptyPlaces.Dequeue();
            numPlace = empty.transform.GetSiblingIndex();
            Destroy(empty);
        }
        CardController gameObjectCard = Instantiate(prefabCard, content).GetComponent<CardController>();
        if(numPlace >= 0) gameObjectCard.transform.SetSiblingIndex(numPlace);
        gameObjectCard.SetData(card);
        controllers.Add(gameObjectCard);
        listCard.Add(card);
    }

    public override void StartStep(){
        if(IsEmpty && (DeckControllerScript.Instance.NotEmpty == false)){
            GameControllerScript.Instance.FinishStep();
        }else{
            PanelYourTurn.Instance.Show();
        	GetCards();
        }
    }
    [SerializeField] private HorizontalLayoutGroup horizontalLayoutGroup;
    public void RecalculateUI(){
        horizontalLayoutGroup.CalculateLayoutInputHorizontal();
    }
    public void DeleteCard(CardController controller){
        int index = controllers.FindIndex(x => x == controller);
        listCard.RemoveAt(index);
        controllers[index].DeleteCard();
        controllers.RemoveAt(index);
    }
    public void DropCard(){CardController.selectedCard?.Drop(); RecalculateUI();}
    private static CardInHandScript instance;
    public static CardInHandScript Instance{get => instance;}

    private void SendMessage(Card card){
        GiveCard giveCard = new GiveCard(card.ID, SideGive.Opponent, MainGameController.Instance.GetMyActorNumber(), isOrderFromServer: true);
        commandGiveCard.SetData(giveCard);
        CommandCenterScript.Instance?.Sender?.SendCommand(commandGiveCard);
    }
    NetworkCommand commandGiveCard = new NetworkCommand(TypeCommand.GiveCard);

    public int GetSiblingsIndex(Vector3 posDrop){
        int result = 0;
        foreach (Transform child in content)
            if(child.position.x <= posDrop.x)
                result++;
        return result;
    }

    [Header("Player info")]
    public Image imagePlayerColor;
    public override void ShowPlayerData(string name, Color color){
        imagePlayerColor.color = color;
    }

//Card move beatyful
    public void CreateEmptyPlaceClose(int siblingsIndex){
        GameObject gameObjectEmptyPlace = Instantiate(prefabEmptyPlace, content);
        gameObjectEmptyPlace.GetComponent<EmptyPlaceForCard>().StartAnimClose(siblingsIndex);
    }
    public EmptyPlaceForCard CreateEmptyPlaceOpen(int siblingsIndex){
        EmptyPlaceForCard controller = (Instantiate(prefabEmptyPlace, content)).GetComponent<EmptyPlaceForCard>();
        controller.StartAnimOpen(siblingsIndex);
        return controller;
    }

    public Action delCardCountChange;
    public void RegisterOnChangeCountCard(Action d){ delCardCountChange += d; }
    public void UnregisterOnChangeCountCard(Action d){ delCardCountChange -= d; }
    private void OnChangeCountCard(){ if(delCardCountChange != null) delCardCountChange(); }
//Animation
    [Header("Animation")]
    public Transform tr;
    public Transform openPoint, closePoint;
    Tween sequenceMove;
    public void AnimOpen(){
        sequenceMove.Kill();
        sequenceMove = DOTween.Sequence().Append(tr.DOMove(openPoint.position , 0.35f, false));
    }
    public void AnimClose(){
        sequenceMove.Kill();
        sequenceMove = DOTween.Sequence().Append(tr.DOMove(closePoint.position, 0.35f, false));
    }

    void OnDestroy(){
        sequenceMove.Kill();
    }
}


