#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

//TODO Seems to be a bit laggy, or maybe its my computer. Find a way to debug lag in unity?

//TODO Create some kinda of settings popup that allows user to alter system hotkeys. ie. 
//Placing assets and selecting the placement system thing. 

[InitializeOnLoad]
public class AssetPlacement : EditorWindow {
	private static bool doSnapUpdate = true;
	
	static AssetPlacement() {
		EditorApplication.update += Update;	
	}
	
	public static void Update() {
		doSnapUpdate = EditorPrefs.GetBool (APGlobals.SnapUpdate, false);

		if (APWindow.instance) {
			APWindow.instance.Repaint();
		}
	}
}

#endif