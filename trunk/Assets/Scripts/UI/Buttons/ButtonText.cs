using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ButtonText : MonoBehaviour {
	public ButtonDefault baseButton = new ButtonDefault();
	
	public GameObject labelObject = null;
	public string text = "";
	
	UILabel getLabel() {
		return  labelObject.GetComponent<UILabel> ();
	}
	
	UIButton getButton() {
		return  baseButton.button.GetComponent<UIButton> ();
	}
	
	public void onClick() {
		Debug.Log ("Button OnClick Not Set");
	}
	
	public void RefreshButton () {
		if (getLabel().text != text) {
			getLabel().text = text;
		}
		
		if (gameObject.name != "Button - " + getLabel ().text + " - Text") {
			gameObject.name = "Button - " + getLabel ().text + " - Text";
		}
	}
	
	public void Awake() {
		OnDrawGizmos ();
	}
	
	public void OnDrawGizmos() {
		baseButton.OnDrawGizmos ();
		
		if (baseButton.disableModifiedControls) {
			return;
		}
		
		if (!labelObject) {
			return;
		}
		
		RefreshButton ();
	}
}
