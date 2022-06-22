using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Xml;
using UnityEditor;
internal class SaveLoadScript : MonoBehaviour{
	private static string NameFileGame    = "FileGame.data";

//Core	
	public static bool SaveGame(GameSave game){
			try{
		    	StreamWriter sw = CreateStream(NameFileGame, false);
			    sw.WriteLine( JsonUtility.ToJson(game) );
				sw.Close();
				return true;
			}catch{
				AndroidPlugin.PluginControllerScript.ToastPlugin.Show(LanguageControllerScript.GetMessage(TypeMessage.SaveError), isLong: true);
				return false;
			}
		}

	public static void LoadGame(GameSave game){
		List<string> rows = ReadFile(NameFileGame);
		if(rows.Count > 0) JsonUtility.FromJsonOverwrite(rows[0], game);
	}
	
	public static void ClearFile(){
    	StreamWriter sw = CreateStream(NameFileGame, false);
		sw.Close();
	}
	private static string GetPrefix(){
			string prefixNameFile = string.Empty;
		#if UNITY_EDITOR_WIN
			prefixNameFile = Application.dataPath;	
	    #endif
	    #if UNITY_ANDROID && !UNITY_EDITOR
			prefixNameFile = Application.persistentDataPath;	
	    #endif
			return prefixNameFile;
	}
	private static List<string> ReadFile(string NameFile){
		CheckFile(NameFile);
		List<string> ListResult = new List<string>(); 
		try{
			ListResult = new List<string>(File.ReadAllLines(Path.Combine (GetPrefix(), NameFile) ));
		}catch{}
		return ListResult;
	}
	private static StreamWriter CreateStream(string NameFile, bool AppendFlag){
    	return new StreamWriter(Path.Combine(GetPrefix(), NameFile), append: AppendFlag);
    }
    public static void CheckFile(string NameFile){
    	if(!File.Exists(Path.Combine (GetPrefix(), NameFile) ))
    		CreateFile(NameFile);
    }
    public static void CreateFile(string NameFile){
        StreamWriter sw = CreateStream(NameFile, false);
        sw.Close();
    }
}
