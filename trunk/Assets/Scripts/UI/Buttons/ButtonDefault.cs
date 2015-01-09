using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ButtonDefault {
	public bool disableModifiedControls = false;
	
	public GameObject background = null;
	public GameObject button = null;
	
	public Color backgroundColor = new Color(204,104,0, 216);
	public List<EventDelegate> onButtonClick = new List<EventDelegate>();
	
	UISprite getBackground() {
		return  background.GetComponent<UISprite> ();
	}
	
	UIButton getButton() {
		return  button.GetComponent<UIButton> ();
	}
	
	public void RefreshButton () {
		if (getBackground ().color != backgroundColor) {
			getBackground ().color = backgroundColor;
		}
		if (getButton ().onClick != onButtonClick) {
			getButton ().onClick = onButtonClick;
		}
	}
	
	public void OnDrawGizmos() {
		if (disableModifiedControls) {
			return;
		}
		
		if (!background || !button) {
			return;
		}
		
		RefreshButton ();
	}
}
