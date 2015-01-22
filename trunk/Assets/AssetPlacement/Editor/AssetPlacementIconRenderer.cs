using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

public class AssetPlacementIconRenderer {

	//int textureWidth is 128! 
	private static int textureWidth = 128; 
	private static int textureHeight = 128;
	
	//TODO Do something better than this
	private static Vector3 cameraPosition = new Vector3 (-5.09f, 16.97f, -7.5f);
	private static Vector3 cameraRotation = new Vector3 (39.80953f, 44.82499f, -14.40204f);

	//CreateFileDirectory
	private static string CreateFileDirectory (string fixedName) {
		var directoryPath = Application.dataPath + AssetPlacementGlobals.IconRenderPath;
		string textureFilePath = directoryPath + fixedName + ".png";
		if (!Directory.Exists (directoryPath)) {
			Directory.CreateDirectory (directoryPath);
		}
		return textureFilePath;
	}
	
	private static Texture2D FindTakenScreenshot (string fixedName, string textureFilePath) {
		var resourcePath = "PlacementIcons/" + fixedName;
		if (File.Exists (textureFilePath)) {
			var texture = Resources.Load<Texture2D> (resourcePath);
			return texture;
		}
		else {
			return null;
		}
	}
	
	private static Camera CreateStageCamera() {
		string cameraName = AssetPlacementGlobals.CameraRender3D;
		GameObject cameraContainer = null;
		cameraContainer = GameObject.Find (cameraName);
		Camera stagedCamera = null;
		if (!cameraContainer) {
			cameraContainer = new GameObject (cameraName);
			stagedCamera = cameraContainer.AddComponent<Camera> ();
			stagedCamera.transform.localPosition = cameraPosition;
			stagedCamera.transform.Rotate (cameraRotation);

			//The transparency pick colour (.png)
			stagedCamera.backgroundColor = Color.magenta;
		} else {
			stagedCamera = cameraContainer.GetComponent<Camera> ();
		}
		return stagedCamera;
	}	
	
	private static GameObject CreateStage () {
		string stagedName = AssetPlacementGlobals.StageRender3D;
		GameObject stagedContainer = null;
		stagedContainer = GameObject.Find (stagedName);
		if (!stagedContainer) {
			stagedContainer = new GameObject (stagedName);
		}
		return stagedContainer;
	}
	
	private static Light CreateStageLightMain () {
		string stagedLightMainName = AssetPlacementGlobals.LightMainRender3D;
		GameObject stagedLightMainContainer = null;
		stagedLightMainContainer = GameObject.Find (stagedLightMainName);
		if (!stagedLightMainContainer) {
			stagedLightMainContainer = new GameObject (stagedLightMainName);
			stagedLightMainContainer.AddComponent<Light> ();
			stagedLightMainContainer.transform.position = new Vector3 (15.57f, 6.45f, 0.75f);
			var light = stagedLightMainContainer.GetComponent<Light> ();
			light.type = LightType.Point;
			light.range = 21;
			light.intensity = 6.8f;
		}
		
		return stagedLightMainContainer.GetComponent<Light> ();
	}
	
	private static Light CreateStageLightSub () {
		string stagedLightSubName = AssetPlacementGlobals.LightSubRender3D;
		GameObject stagedLightSubContainer = null;
		stagedLightSubContainer = GameObject.Find (stagedLightSubName);
		if (!stagedLightSubContainer) {
			stagedLightSubContainer = new GameObject (stagedLightSubName);
			stagedLightSubContainer.AddComponent<Light> ();
			stagedLightSubContainer.transform.position = new Vector3 (-3.28f, -1.52f, 1.87f);
			var light = stagedLightSubContainer.GetComponent<Light> ();
			light.type = LightType.Point;
			light.range = 205.2f;
			light.intensity = 0.3f;
			light.color = Color.gray;
		}
		
		return stagedLightSubContainer.GetComponent<Light> ();
	}
	
	private static Light CreateStageLightSun () {
		string stagedLightSunName = AssetPlacementGlobals.LightSunRender3D;
		GameObject stageLightSunContainer = null;
		stageLightSunContainer = GameObject.Find (stagedLightSunName);
		if (!stageLightSunContainer) {
			stageLightSunContainer = new GameObject (stagedLightSunName);
			stageLightSunContainer.AddComponent<Light> ();
			stageLightSunContainer.transform.position = new Vector3 (-2.35f, 5.54f, 15.01f);
			stageLightSunContainer.transform.Rotate (new Vector3 (49.5479f, 20.51822f, 120.4922f));
			var light = stageLightSunContainer.GetComponent<Light> ();
			light.type = LightType.Directional;
			light.intensity = 0.2f;
			light.color = Color.white;
		}
		
		return stageLightSunContainer.GetComponent<Light> ();
	}
	
	private static GameObject CreateStagedAsset (AssetPlacementData assetData, GameObject stagedContainer) {
		var stagedAsset = PrefabUtility.InstantiatePrefab (assetData.gameObject) as GameObject;
		stagedAsset.name = "StagedAsset";
		stagedAsset.transform.parent = stagedContainer.transform;
		stagedAsset.transform.localPosition = Vector3.zero;
		SceneView.RepaintAll ();
		return stagedAsset;
	}
	
	private static void TakeStageScreenshot (string textureFilePath, Camera stagedCamera, GameObject stagedAsset) {
		RenderTexture rt = new RenderTexture (textureWidth, textureHeight, 0, RenderTextureFormat.ARGB32);
		RenderTexture.active = rt;
		stagedCamera.targetTexture = rt;
		Texture2D screenShot = new Texture2D (textureWidth, textureHeight, TextureFormat.ARGB32, false);
		stagedCamera.Render ();
		screenShot.ReadPixels (new Rect (0, 0, textureWidth, textureHeight), 0, 0);
		
		for (int x = 0; x < textureWidth; x++) {
			for (int y = 0; y < textureHeight; y++) {
				var pixel = screenShot.GetPixel(x, y);
				if(pixel.r == Color.magenta.r && pixel.g == Color.magenta.g && pixel.b == Color.magenta.b) {  
 					screenShot.SetPixel(x, y, new Color(1.0f, 1.0f, 1.0f, 0.0f));
				}
			}

		}
		
		
		screenShot.alphaIsTransparency = true;
		
		screenShot.Apply ();
		
		var bytes = screenShot.EncodeToPNG ();
		RenderTexture.active = null;
		EditorWindow.DestroyImmediate (screenShot);
		File.WriteAllBytes (textureFilePath, bytes);
		stagedCamera.targetTexture = null;
		RenderTexture.active = null;
	}
	
	public static Texture2D CreateTextureFromCamera(AssetPlacementData assetData, ref bool hasMadeAnIconRenderAsset) {
		string fixedName = assetData.name.Replace('\\', '_'); 
		fixedName = fixedName.Replace('/', '_');
		fixedName = fixedName.Replace('.', '_');
		
		var textureFilePath = CreateFileDirectory (fixedName);
		
		var foundScreenShot = FindTakenScreenshot (fixedName, textureFilePath);
		if (foundScreenShot) {
			return foundScreenShot;
		} else {
			hasMadeAnIconRenderAsset = true;
			
			var stagedCamera = CreateStageCamera ();
			var stagedContainer = CreateStage ();
			var stagedAsset = CreateStagedAsset (assetData, stagedContainer); 	
			
			CreateStageLightMain ();
			CreateStageLightSub ();
			CreateStageLightSun ();
			
			//TODO Move and aim camera to point at the stage asset
			
			TakeStageScreenshot (textureFilePath, stagedCamera, stagedAsset);
			
			EditorWindow.DestroyImmediate (stagedAsset);
			
			return null;
		}
	}
	
	public static void CleanUpRender3DAssets () {
		var temp = GameObject.Find (AssetPlacementGlobals.CameraRender3D);
		if (temp) EditorWindow.DestroyImmediate (temp);
		temp = GameObject.Find (AssetPlacementGlobals.StageRender3D);
		if (temp) EditorWindow.DestroyImmediate (temp);
		temp = GameObject.Find (AssetPlacementGlobals.LightMainRender3D);
		if (temp) EditorWindow.DestroyImmediate (temp);
		temp = GameObject.Find (AssetPlacementGlobals.LightSubRender3D);
		if (temp) EditorWindow.DestroyImmediate (temp);
		temp = GameObject.Find (AssetPlacementGlobals.LightSunRender3D);
		if (temp) EditorWindow.DestroyImmediate (temp);
	}
}
