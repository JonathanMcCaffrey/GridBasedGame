#define USING_SNAZZY_GRID

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class AssetPlacementWindow :  EditorWindow {
	bool shouldShowAll = true;
	
	//TODO add more features to this window
	
	public static AssetPlacementWindow instance = null;
	
	[MenuItem( "Edit/Asset Placement Window" )]
	static void Init() {
		if (!instance) {
			AssetPlacementWindow window = (AssetPlacementWindow)EditorWindow.GetWindow (typeof(AssetPlacementWindow));
			window.title = "AP";
			window.minSize = new Vector2 (200, 100);
			instance = window;
		}
	}
	
	static void RefreshAutoSnap (GameObject placedAsset) {
		#if USING_SNAZZY_GRID
		SnazzyToolsEditor.SnapPos(true, true, true);
		#else
		if (Utils.GameObjectFunctions.HasMesh(placedAsset)) {
			/*var snapSize = Utils.GameObjectFunctions.CreateRectFromMeshes(placedAsset);
		if (EditorPrefs.GetBool (AssetPlacement.SnapUpdateKey)) {
			EditorPrefs.SetFloat (AutoGridSnap.MoveSnapXKey, snapSize.width);
			EditorPrefs.SetFloat (AutoGridSnap.MoveSnapYKey, snapSize.height);
		}*/
		}
		#endif
		
	}
	
	static void SetTabContainerParent (GameObject placedAsset) {
		if (AssetPlacementChoiceSystem.instance) {
			if (AssetPlacementChoiceSystem.TabContainerDictionary.ContainsKey (AssetPlacementChoiceSystem.selectedAsset.tab)) {
				var container = AssetPlacementChoiceSystem.TabContainerDictionary [AssetPlacementChoiceSystem.selectedAsset.tab];
				placedAsset.transform.parent = container.transform;
			}
		}
	}
	
	
	//TODO find a better hotkey
	[MenuItem("Edit/Commands/PlaceAsset #_%_d")]
	static void PlaceAsset() {
		if (AssetPlacementChoiceSystem.selectedAsset != null && AssetPlacementChoiceSystem.selectedAsset.gameObject != null) {
			var placedAsset = PrefabUtility.InstantiatePrefab( AssetPlacementChoiceSystem.selectedAsset.gameObject) as GameObject; 	
			placedAsset.transform.localPosition = AssetPlacementPositionSystem.selectedPosition;
			Selection.activeGameObject = placedAsset;
			
			SetTabContainerParent (placedAsset);
			RefreshAutoSnap (placedAsset);
		}
	}	
	
	//TODO find a better hotkey
	[MenuItem("Edit/Commands/StartPlaceAsset #_%_e")]
	static void SelectPlaceAssetSystem() {
		if (AssetPlacementChoiceSystem.instance) {
			Selection.activeGameObject = AssetPlacementChoiceSystem.instance.gameObject;
		}
	}	
	
	//TODO find a better hotkey
	//TODO Re-add later
	#if !USING_SNAZZY_GRID
	[MenuItem("Edit/Commands/ToggleAutoSnapUpdate #_%_f")]
	#endif
	static void ToggleAutoSnapUpdate() {
		EditorPrefs.SetBool (AssetPlacementKeys.SnapUpdate, !EditorPrefs.GetBool (AssetPlacementKeys.SnapUpdate));
	}	
	
	void OnInspectorUpdate() {
		if (background == null) {
			Load();
		}
	}
	
	private Texture background = null;
	private Texture windowTitle = null;
	void Load() {
		string path = "Assets/"+AssetPlacementKeys.InstallPath+"AssetPlacement/Resources/GUI/";
		background = AssetDatabase.LoadAssetAtPath(path+"BG.jpg",typeof(Texture)) as Texture;
		if (!background) {
			Debug.Log ("AssetPlacement InstallPath Needs Fixing");
		}
		
		windowTitle = AssetDatabase.LoadAssetAtPath(path+"Title.jpg",typeof(Texture)) as Texture;
	}
	
	void CreateTitleLogo (float width, ref float distanceFromTop) {
		EditorGUI.LabelField (new Rect (0, 0, width, 64), new GUIContent (windowTitle, "//TODO Add Tooltip"));
		distanceFromTop += 64;
	}
	
	void CreateToggleTabSelection (float width, ref float distanceFromTop) {
		float toggleHeight = 16;
		shouldShowAll = EditorGUI.Toggle (new Rect (0, distanceFromTop, width, toggleHeight), "Show All", shouldShowAll);
		distanceFromTop += toggleHeight;
		if (!shouldShowAll) {
			float popupHeight = 20;
			int selectedTabNumber = EditorPrefs.GetInt (AssetPlacementKeys.SelectedTab);
			selectedTabNumber = EditorGUI.Popup (new Rect (0, distanceFromTop, width, popupHeight), selectedTabNumber, AssetPlacementChoiceSystem.instance.tabNames.ToArray ());
			EditorPrefs.SetInt (AssetPlacementKeys.SelectedTab, selectedTabNumber);
			distanceFromTop += popupHeight;
			AssetPlacementChoiceSystem.instance.selectedTab = AssetPlacementChoiceSystem.instance.tabList [selectedTabNumber];
		}
	}
	
	void CreateAssetButtons (float width, ref float distanceFromTop) {
		int index = 0;
		float xVal = 0;
		float yVal = 0;
		
		foreach (var assetData in AssetPlacementChoiceSystem.instance.assetList) {
			if (assetData.tab != AssetPlacementChoiceSystem.instance.selectedTab.name && !shouldShowAll) {
				index++;
				continue;
			}
			Texture usedTexture = null;
			if (assetData.gameObject.GetComponent<SpriteRenderer> ()) {
				usedTexture = assetData.gameObject.GetComponent<SpriteRenderer> ().sprite.texture;
			}
			else {
				//TODO add all cases
			}
			
			
			var buttonStyle = EditorPrefs.GetInt (AssetPlacementKeys.SelectedAssetNumber) == index ? GUI.skin.box : GUI.skin.button;
			
			//TODO Make this work with hotkeys
			
			var buttonRect = new Rect ((width / 3) * xVal, distanceFromTop + (width / 3) * yVal, (width / 3), (width / 3));
			if (usedTexture && GUI.Button (buttonRect, usedTexture, buttonStyle)) {
				EditorPrefs.SetInt (AssetPlacementKeys.SelectedAssetNumber, index);
			}
			
			
			string keyLabel = assetData.keyCode.ToString();
			if(keyLabel.Length > 1) {
				keyLabel = keyLabel.Remove(0,keyLabel.Length - 1); 
			}

			GUI.Label(new Rect(buttonRect.x + buttonRect.width * 0.75f, 
			                   buttonRect.y + buttonRect.height * 0.75f, 
			                   buttonRect.width * 0.25f, 
			                   buttonRect.width * 0.25f), keyLabel);
			
			index++;
			xVal++;
			if (xVal > 2) {
				xVal = 0;
				yVal++;
			}
		}
	}
	
	public void OnGUI() {
		instance = this;

		if (background) {
			float width = Screen.width;
			
			EditorGUI.DrawPreviewTexture (new Rect (0, 0, Screen.width, Screen.height), background);
			
			float distanceFromTop = 0.0f;
			CreateTitleLogo (width, ref distanceFromTop);
			CreateToggleTabSelection (width, ref distanceFromTop);
			CreateAssetButtons (width, ref distanceFromTop);
			
			
			/*EditorPrefs.SetBool (AssetPlacement.SnapUpdateKey,
			                     EditorGUI.Toggle (new Rect(-1, distanceFromTop, width, 20),  "Update Auto Snap", EditorPrefs.GetBool (AssetPlacement.SnapUpdateKey, false)));
			distanceFromTop += 20;*/
			
		}
	}
}