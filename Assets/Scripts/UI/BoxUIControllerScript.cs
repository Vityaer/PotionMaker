using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxUIControllerScript : MonoBehaviour{


	public delegate void Del(int num);
    public Del observerInteractive;
    public void SubscribeOnInteractive(Del d){ observerInteractive += d; }
    public void UnsubscribeOnInteractive(Del d){ observerInteractive -= d; }
    public void PickUpItem(int num){ if(observerInteractive != null) observerInteractive(num); }
}
