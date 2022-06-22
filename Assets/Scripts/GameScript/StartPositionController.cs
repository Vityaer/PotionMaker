using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPositionController : MonoBehaviour{
    void Awake(){ instance = this; }
    public List<CardOpponentHandScript> startPositoins = new List<CardOpponentHandScript>();
    public CardOpponentHandScript GetControllerCard(int numQueue){
        CardOpponentHandScript result = startPositoins[1]; 
        if(GameControllerScript.Instance.CountPlayer == 2){
    		  result = startPositoins[1];
    	}else{
    		result = startPositoins[numQueue - 1];
    	}
        return result;
    }
    private static StartPositionController instance;
    public static StartPositionController Instance{get => instance;}
}
