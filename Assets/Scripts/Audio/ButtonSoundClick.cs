using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSoundClick : MonoBehaviour{
    public void PlaySoundClick(){
		AudioControllerScript.Instance?.PlaySoundClickOnButton();
    }
}
