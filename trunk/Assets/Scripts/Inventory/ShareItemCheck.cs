
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Facebook.MiniJSON;
using System;


public class ShareItemCheck : MonoBehaviour {
	
	public GameObject shareItemDialog = null;
	
	public GameObject currentLayer = null;
	
	public void Start() {
		currentLayer.SetActive(false);
		
		if (Inventory.DidFindSpecialItem ()) {
			Inventory.ClearSpecialItem ();
			ShowShareDialog ();
		} else {
			Destroy(gameObject);
		}
	}
	
	public void OnDestroy() {
		currentLayer.SetActive (true);
	}
	
	private void ShowShareDialog() {
		var shareDialog = GameObject.Instantiate(shareItemDialog);
		shareDialog.transform.parent = gameObject.transform;
		shareDialog.transform.localScale = Vector3.one;
		shareDialog.transform.localPosition = Vector3.zero;
	}
}
