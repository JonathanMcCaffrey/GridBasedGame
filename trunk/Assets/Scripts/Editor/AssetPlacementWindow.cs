#define USING_SNAZZY_GRID

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

public class AssetPlacementWindow :  EditorWindow {
	bool shouldShowAll = false;
	bool shouldShowLabels = false;
	
	
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
	private Texture backgroundAlpha = null;
	private Texture windowTitle = null;
	void Load() {
		string path = "Assets/"+AssetPlacementKeys.InstallPath+"AssetPlacement/Resources/GUI/";
		background = AssetDatabase.LoadAssetAtPath(path+"BG.jpg",typeof(Texture)) as Texture;
		if (!background) {
			Debug.Log ("AssetPlacement InstallPath Needs Fixing");
		}
		
		backgroundAlpha = AssetDatabase.LoadAssetAtPath(path+"BGAlpha.png",typeof(Texture)) as Texture;
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
			
			if (AssetPlacementChoiceSystem.instance.tabList.Count < selectedTabNumber) {
				AssetPlacementChoiceSystem.instance.selectedTab = AssetPlacementChoiceSystem.instance.tabList [selectedTabNumber];
			}
		} else {
			distanceFromTop += 8;
		}
	}
	
	public int textureWidth = 128; 
	public int textureHeight = 128;
	
	
	Texture2D CreateTextureFromCamera(AssetPlacementData assetData) {
		//TODO Have it create and delete the staged object it finds
		string fixedName = assetData.name.Replace('\\', '_');
		fixedName = fixedName.Replace('/', '_');
		fixedName = fixedName.Replace('.', '_');
		
		var directoryPath = Application.dataPath + "/Resources/PlacementIcons/";
		string textureFilePath = directoryPath + fixedName + ".png";
		if (!Directory.Exists (directoryPath)) {
			Directory.CreateDirectory(directoryPath);
		}
		
		var resourcePath = "PlacementIcons/" + fixedName;
		if (File.Exists (textureFilePath)) {
			var texture = Resources.Load<Texture2D>(resourcePath);
			return texture;
		}
		
		Camera stagedCamera = null;
		foreach(var camera in Camera.allCameras) {
			if(camera.gameObject.name == "RenderIconCamera") {
				stagedCamera = camera;
			}
		}
		if(!stagedCamera) {
			return null;
		}
		
		var stagedParent = GameObject.FindGameObjectWithTag("IconTextureCanvas") as GameObject;
		if (!stagedParent) {
			return null;
		}
		
		var stagedAsset = PrefabUtility.InstantiatePrefab(assetData.gameObject) as GameObject; 	
		stagedAsset.name = "StagedAsset";
		DontDestroyOnLoad (stagedAsset);
		stagedAsset.transform.parent = stagedParent.transform;
		stagedAsset.transform.localPosition = Vector3.zero;
		
		SceneView.RepaintAll ();
		
		
		RenderTexture rt = new RenderTexture(textureWidth, textureHeight, 24000);
		RenderTexture.active = rt;
		stagedCamera.targetTexture = rt;
		
		Texture2D screenShot = new Texture2D (textureWidth, textureHeight);
		stagedCamera.Render ();
		RenderTexture.active = rt;
		screenShot.ReadPixels (new Rect (0, 0, Screen.width, Screen.height), 0, 0);
		screenShot.Apply ();
		
		var bytes = screenShot.EncodeToPNG ();
		
		RenderTexture.active = null;
		DestroyImmediate (screenShot);
		
		File.WriteAllBytes (textureFilePath, bytes);
		
		
		stagedCamera.targetTexture = null;
		RenderTexture.active = null; 
		
		DestroyImmediate (stagedAsset);
		
		return null;
	}
	
	void CreateHotkeyLabel (Rect buttonRect, string keyLabel) {
		Rect labelRect = new Rect (buttonRect.x + buttonRect.width * 0.7f, buttonRect.y + buttonRect.height * 0.7f, buttonRect.width * 0.3f, buttonRect.width * 0.3f);
		
		GUI.DrawTexture (new Rect (labelRect.x - labelRect.width * 0.1f,
		                           labelRect.y - labelRect.height * 0.1f, 
		                           labelRect.width,
		                           labelRect.height)
		                 , backgroundAlpha);
		
		GUIStyle labelStyle = new GUIStyle ();
		labelStyle.fontSize = 18;
		labelStyle.fontStyle = FontStyle.Bold;
		labelStyle.normal.textColor = Color.black;
		GUI.Label (labelRect, keyLabel, labelStyle);
	}
	
	void CreateTabLabel (AssetPlacementData assetData, Rect buttonRect) {
		Rect labelRect = new Rect(buttonRect.x + buttonRect.width * 0.1f, buttonRect.y + buttonRect.height * 0.1f, buttonRect.width * 0.8f, buttonRect.width * 0.2f); 
		
		GUI.DrawTexture (new Rect (labelRect.x,
		                           labelRect.y - labelRect.height * 0.1f, 
		                           labelRect.width,
		                           labelRect.height)
		                 , backgroundAlpha);
		
		var labelStyle = new GUIStyle ();
		labelStyle.fontSize = 14;
		labelStyle.fontStyle = FontStyle.Bold;
		labelStyle.normal.textColor = Color.black;
		GUI.Label (labelRect, assetData.tab, labelStyle);
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
			Texture2D usedTexture = null;
			if (assetData.gameObject.GetComponent<SpriteRenderer> ()) {
				usedTexture = assetData.gameObject.GetComponent<SpriteRenderer> ().sprite.texture;
			} if (assetData.gameObject.GetComponent<MeshRenderer> ()) {
				var tempTexture = CreateTextureFromCamera(assetData);
				if(tempTexture) {
					usedTexture = tempTexture;
				}
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
			
			if(shouldShowLabels) {
				CreateHotkeyLabel (buttonRect, keyLabel);
				if(shouldShowAll) {
					CreateTabLabel (assetData, buttonRect);
				}
			}
			
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

			float toggleHeight = 16;
			shouldShowLabels = EditorGUI.Toggle (new Rect (0, distanceFromTop, width, toggleHeight), "Show Labels", shouldShowLabels);
			distanceFromTop += toggleHeight;

			CreateToggleTabSelection (width, ref distanceFromTop);
			CreateAssetButtons (width, ref distanceFromTop);
			
			/*EditorPrefs.SetBool (AssetPlacement.SnapUpdateKey,
			                     EditorGUI.Toggle (new Rect(-1, distanceFromTop, width, 20),  "Update Auto Snap", EditorPrefs.GetBool (AssetPlacement.SnapUpdateKey, false)));
			distanceFromTop += 20;*/
		}
	}
}