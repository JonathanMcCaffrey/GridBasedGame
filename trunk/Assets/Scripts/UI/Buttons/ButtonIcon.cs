using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ButtonIcon : MonoBehaviour {
	public ButtonDefault baseButton = new ButtonDefault();
	public GameObject icon = null;
	public Sprite iconSprite = null;
	
	UI2DSprite getIcon() {
		return  icon.GetComponent<UI2DSprite> ();
	}
	
	public void onClick() {
		Debug.Log ("Button OnClick Not Set");
	}
	
	void RefreshButton () {
		if (iconSprite) {
			if (getIcon ().name != iconSprite.name) {
				getIcon ().sprite2D = iconSprite;
			}
		}
	}
	
	public void OnDrawGizmos() {
		baseButton.OnDrawGizmos ();
		
		if (baseButton.disableModifiedControls) {
			return;
		}
		
		if (!icon) {
			return;
		}

		RefreshButton ();
	}
}
