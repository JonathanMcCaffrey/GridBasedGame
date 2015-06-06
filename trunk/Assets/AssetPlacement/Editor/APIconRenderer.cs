#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

public class APIconRenderer {
	private static string CreateFileDirectory (string fixedName) {
		var directoryPath = Application.dataPath + APGlobals.IconRenderPath;
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
	
	private static GameObject CreateStage () {
		string stagedName = APGlobals.StageRender3D;
		GameObject stagedContainer = null;
		stagedContainer = GameObject.Find (stagedName);
		if (!stagedContainer) {
			stagedContainer = new GameObject (stagedName);
		}
		return stagedContainer;
	}
	
	private static Camera CreateStageCamera(GameObject stagedContainer) {
		string cameraName = APGlobals.CameraRender3D;
		GameObject cameraContainer = null;
		cameraContainer = GameObject.Find (cameraName);
		Camera stagedCamera = null;
		if (!cameraContainer) {
			cameraContainer = new GameObject (cameraName);
			stagedCamera = cameraContainer.AddComponent<Camera> ();
			stagedCamera.fieldOfView = 60.0f;
			
			//The transparency pick colour (.png)
			stagedCamera.backgroundColor = Color.magenta;
		} else {
			stagedCamera = cameraContainer.GetComponent<Camera> ();
		}
		
		cameraContainer.transform.parent = stagedContainer.transform;
		
		return stagedCamera;
	}	
	
	private static Light CreateStageLightMain (GameObject stagedContainer) {
		string stagedLightMainName = APGlobals.LightMainRender3D;
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
		
		stagedLightMainContainer.transform.parent = stagedContainer.transform;
		
		return stagedLightMainContainer.GetComponent<Light> ();
	}
	
	private static Light CreateStageLightSub (GameObject stagedContainer) {
		string stagedLightSubName = APGlobals.LightSubRender3D;
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
		
		stagedLightSubContainer.transform.parent = stagedContainer.transform;
		
		return stagedLightSubContainer.GetComponent<Light> ();
	}
	
	private static Light CreateStageLightSun (GameObject stagedContainer) {
		string stagedLightSunName = APGlobals.LightSunRender3D;
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
		
		stageLightSunContainer.transform.parent = stagedContainer.transform;
		
		return stageLightSunContainer.GetComponent<Light> ();
	}
	
	private static GameObject CreateStagedAsset (APData assetData, GameObject stagedContainer) {
		var stagedAsset = PrefabUtility.InstantiatePrefab (assetData.gameObject) as GameObject;
		stagedAsset.name = "StagedAsset";
		stagedAsset.transform.parent = stagedContainer.transform;
		stagedAsset.transform.localPosition = Vector3.zero;
		SceneView.RepaintAll ();
		return stagedAsset;
	}

	private static int textureWidth = 128; 
	private static int textureHeight = 128;
	
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
	
	static void FocusStageCameraOnAsset (Camera stagedCamera, GameObject stagedAsset, GameObject stagedContainer) {
		Vector3 min = Vector3.zero;
		Vector3 max = Vector3.zero;

		stagedAsset.transform.Rotate (new Vector3 (-27, -10, 0));
		Utils.GameObjectUtils.GetMaxMinPointFromGameObject (stagedAsset, ref max, ref min);

		float extraPush = max.z - min.z;
		Vector2 point = new Vector2 (max.x - min.x, max.y - min.y);
		float distance = Mathf.Sqrt (point.x * point.x + point.y * point.y);
		float height = distance / 2.0f;
		float zoomOut = 1.85f;
		Vector3 distanceVector = new Vector3 (0, 0, -height * zoomOut - extraPush);

		stagedCamera.gameObject.transform.position = distanceVector;

		Vector3 reverseTransformVector = new Vector3 ((max.x + min.x) / 2.0f, (max.y + min.y) / 2.0f, (max.z + min.z) / 2.0f);

		stagedCamera.gameObject.transform.position = new Vector3 (stagedCamera.gameObject.transform.position.x + reverseTransformVector.x, 
		                                                          stagedCamera.gameObject.transform.position.y + reverseTransformVector.y, 
		                                                          stagedCamera.gameObject.transform.position.z + reverseTransformVector.z); 

		stagedCamera.gameObject.transform.LookAt (reverseTransformVector);
		
		stagedContainer.transform.position = new Vector3 (-5000, -5000, -5000);
		
		SceneView.RepaintAll ();
	}
	
	public static Texture2D CreateTextureFromCamera(APData assetData, ref bool hasMadeAnIconRenderAsset) {
		string fixedName = assetData.name.Replace('/', '_'); 
		fixedName = fixedName.Replace('.', '_');
		
		var textureFilePath = CreateFileDirectory (fixedName);
		
		var foundScreenShot = FindTakenScreenshot (fixedName, textureFilePath);
		if (foundScreenShot) {
			return foundScreenShot;
		} else {
			hasMadeAnIconRenderAsset = true;
			
			var stagedContainer = CreateStage ();
			var stagedCamera = CreateStageCamera (stagedContainer);
			var stagedAsset = CreateStagedAsset (assetData, stagedContainer); 	
			
			CreateStageLightMain (stagedContainer);
			CreateStageLightSub (stagedContainer);
			CreateStageLightSun (stagedContainer);
			
			FocusStageCameraOnAsset (stagedCamera, stagedAsset, stagedContainer);
			TakeStageScreenshot (textureFilePath, stagedCamera, stagedAsset);
			
			AssetDatabase.ImportAsset(textureFilePath);

			EditorWindow.DestroyImmediate (stagedAsset);
			
			return null;
		}
	}
	
	public static void CleanUpRender3DAssets () {
		AssetDatabase.Refresh();

		var temp = GameObject.Find (APGlobals.CameraRender3D);
		if (temp) EditorWindow.DestroyImmediate (temp);
		temp = GameObject.Find (APGlobals.StageRender3D);
		if (temp) EditorWindow.DestroyImmediate (temp);
		temp = GameObject.Find (APGlobals.LightMainRender3D);
		if (temp) EditorWindow.DestroyImmediate (temp);
		temp = GameObject.Find (APGlobals.LightSubRender3D);
		if (temp) EditorWindow.DestroyImmediate (temp);
		temp = GameObject.Find (APGlobals.LightSunRender3D);
		if (temp) EditorWindow.DestroyImmediate (temp);
	}
}


#endif