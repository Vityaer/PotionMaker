using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuScript : MonoBehaviour{
    public bool isOpenUI;
    public GameObject menuObject;
    void Update(){
        if(Input.GetKeyDown(KeyCode.Escape)){   
            ShowHideMenu();
        }
    }
    public void ShowHideMenu(){
        isOpenUI = !isOpenUI;
        menuObject.SetActive(isOpenUI);
    }
}
