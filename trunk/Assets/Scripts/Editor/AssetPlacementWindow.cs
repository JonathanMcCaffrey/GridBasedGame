using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class AssetPlacementWindow :  EditorWindow {
	
	bool shouldShowAll = true;
	
	//TODO add more features to this window
	[MenuItem( "Edit/Asset Placement Window" )]
	static void Init() {
		AssetPlacementWindow window = (AssetPlacementWindow)EditorWindow.GetWindow( typeof( AssetPlacementWindow ) );
		window.title = "AP";
		window.minSize = new Vector2(200,100);
	}
	
	static void RefreshAutoSnap (Rect snapSize) {
		/* //TODO Deprecated Code. Update to use SnazzyGrid
		if (EditorPrefs.GetBool (AssetPlacement.SnapUpdateKey)) {
			EditorPrefs.SetFloat (AutoGridSnap.MoveSnapXKey, snapSize.width);
			EditorPrefs.SetFloat (AutoGridSnap.MoveSnapYKey, snapSize.height);
		}
		*/
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
			
			if (Utils.GameObjectFunctions.HasMesh(placedAsset)) {
				var snapSize = Utils.GameObjectFunctions.CreateRectFromMeshes(placedAsset);
				RefreshAutoSnap (snapSize);
			}
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
	//[MenuItem("Edit/Commands/ToggleAutoSnapUpdate #_%_f")]
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
			int selectedTabNumber = EditorPrefs.GetInt (AssetPlacementKeys.SelectedTabKey);
			selectedTabNumber = EditorGUI.Popup (new Rect (0, distanceFromTop, width, popupHeight), selectedTabNumber, AssetPlacementChoiceSystem.instance.tabNames.ToArray ());
			EditorPrefs.SetInt (AssetPlacementKeys.SelectedTabKey, selectedTabNumber);
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
			if (usedTexture && GUI.Button (new Rect ((width / 3) * xVal, distanceFromTop + (width / 3) * yVal, (width / 3), (width / 3)), usedTexture)) {
				EditorPrefs.SetInt (AssetPlacementKeys.SelectedAssetNumber, index);

				//AssetPlacementKeys.SelectedAssetNumber

			
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