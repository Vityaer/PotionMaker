using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR_WIN
[CustomEditor(typeof(Card))]
[CanEditMultipleObjects]
public class CardEditor : Editor{
	Card cardScript = null;
	public override void OnInspectorGUI(){
		if(cardScript == null) cardScript = (Card)target;
		EditorGUILayout.LabelField("Card");
		EditorGUILayout.PropertyField(serializedObject.FindProperty("ID"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("name"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("typeCard"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("colorCard"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("reagentName"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("amountScore"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("description"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("receipt"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("countRequireIngredient"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("reagentStore"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("specialImageIngredient"));
        serializedObject.ApplyModifiedProperties();
    }
}
#endif
