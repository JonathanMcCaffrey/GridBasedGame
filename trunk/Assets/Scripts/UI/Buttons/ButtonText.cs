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
	
	void RefreshButton () {
		if (getLabel().text != text) {
			getLabel().text = text;
		}
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
