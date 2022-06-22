using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerPlaceController : MonoBehaviour{
	public virtual void StartStep(){}
	public virtual void CreateCard(Card card){}
	public BonusScore bonus;
	public void AddPoint(int point){
		bonus.Show(point);
        score += point;
        textScore.text = score.ToString();
	}
    public GameObject playerData;
    public Text textScore;
	public int Score{get => score;}
	[SerializeField] protected int score = 0; 
    public PointMove pointMove;
    public void PreparePlace(){
        textScore.text = "0";
        playerData.SetActive(true);
    }
    public void DeleteAll(){
    	Destroy(gameObject);
    }
    public List<Card> listCard = new List<Card>();
    public int CountCardInHand{get => listCard.Count;}
    public virtual void ShowPlayerData(string name, Color playerColor){}
}
