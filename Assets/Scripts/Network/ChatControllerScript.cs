using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class ChatControllerScript : MonoBehaviour{
    public Text textChat;
        public void Awake(){
        	instance = this;
        }

        public void PrintMessage(string message){
            textChat.text = string.Concat(textChat.text, "\n", message);
            message = string.Empty; 
        }
        private static ChatControllerScript instance;
        public static ChatControllerScript Instance{get => instance;}
}
