using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public static class AssetPlacement {
	private static bool doSnapUpdate = true;
	
	static AssetPlacement() {
		EditorApplication.update += Update;	
	}
	
	public static void Update() {
		doSnapUpdate = EditorPrefs.GetBool (AssetPlacementKeys.SnapUpdate, false);
	}
}