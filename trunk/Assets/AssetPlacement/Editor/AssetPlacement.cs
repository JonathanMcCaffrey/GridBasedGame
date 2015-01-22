using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[InitializeOnLoad]
public class AssetPlacement : EditorWindow {
	private static bool doSnapUpdate = true;
	
	static AssetPlacement() {
		EditorApplication.update += Update;	
	}
	
	public static void Update() {
		doSnapUpdate = EditorPrefs.GetBool (AssetPlacementGlobals.SnapUpdate, false);

		if (AssetPlacementWindow.instance) {
			AssetPlacementWindow.instance.Repaint();
		}
	}
}