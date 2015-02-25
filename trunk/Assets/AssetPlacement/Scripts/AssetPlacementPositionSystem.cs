using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEditor;

//TODO Wrap this since it will probably break mobile
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Xml.Serialization;


public class AssetPlacementPositionSystem : MonoBehaviour {
	public float xPosition = 0;
	public float yPosition = 0;
	
	private float adjustX = 0;
	private float adjustY = 0;
	
	public static Vector3 selectedPosition = Vector3.zero;
	
	//TODO Cool shader effect for placing assets? (Might appear laggy in editor)
	//bool shouldShowCoolShaderEffectOverlayForPlacingAssetsWithinTheEditor = true;

	public GameObject marker = null;
	

	private float distance = 500;
	public bool isMarkerActive = false;
	
	public static AssetPlacementPositionSystem instance = null;
	void Awake() {
		if (instance && instance != this) {
			Destroy (gameObject);
		} else {
			DontDestroyOnLoad(gameObject);
			instance = this;
		}
	}
	
	Vector3 FindPlacementPosition () {
		Vector2 fixedPos = new Vector2 (Event.current.mousePosition.x + adjustX,
		                                -Event.current.mousePosition.y + adjustY + Camera.current.pixelHeight);
		var ray = Camera.current.ScreenPointToRay (fixedPos);
		Vector3 position = ray.GetPoint (distance);
		xPosition = position.x;
		yPosition = position.y;
		selectedPosition = new Vector3 (xPosition, yPosition, distance);
		return position;
	}


	Texture2D markerTexture = null;
	void CreateMarker () {
		if (marker == null && AssetPlacementChoiceSystem.instance) {
			marker = GameObject.Find (AssetPlacementGlobals.PositionMarker);
			if (marker) {
				return;
			}
			string path = "Assets/" + AssetPlacementGlobals.InstallPath + "AssetPlacement/Resources/GUI/";

			if(markerTexture == null) {
				markerTexture = AssetDatabase.LoadAssetAtPath (path + "PositionMarker.png", typeof(Texture2D)) as Texture2D;
			}

			if (markerTexture) {
				marker = new GameObject (AssetPlacementGlobals.PositionMarker);
				marker.AddComponent<SpriteRenderer> ();
				var renderer = marker.GetComponent<SpriteRenderer> ();
				var sprite = Sprite.Create(markerTexture, new Rect(0,0,markerTexture.width,markerTexture.height), new Vector2(0.5f, 0.5f));
				sprite.name = "PositionMarker";
				renderer.sprite = sprite;

				marker.transform.parent = gameObject.transform;
				marker.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f); 
			}
		}
	}
	
	bool ShouldMoveMarker () {
		if (!isMarkerActive) {
			if (marker) { marker.SetActive (false); }
			return false;
		} 
		
		if (marker) { 
			marker.SetActive (true);
			return true;
		}
		
		return false;
	}
	
	void MoveDebugMarker (Vector3 position) {
		if (ShouldMoveMarker ()) {
			marker.transform.localPosition = position;
			//TODO Set the debug marker to the camera, and find out what your messing up
		}
	}
	
	public void OnDrawGizmos() {
		CreateMarker ();
		
		if (Camera.current) {
			var position = FindPlacementPosition ();
			MoveDebugMarker (position);
		}
	}
}
