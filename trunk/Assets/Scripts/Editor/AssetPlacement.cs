using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public static class AssetPlacement {
	public const string SnapUpdateKey = "AssetPlacement.doSnapUpdate";

	private static bool doSnapUpdate = true;
	
	static AssetPlacement() {
		EditorApplication.update += Update;	
	}
	
	public static void Update() {
		doSnapUpdate = EditorPrefs.GetBool (SnapUpdateKey, false);
	}
}