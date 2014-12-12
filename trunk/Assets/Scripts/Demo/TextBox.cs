using UnityEngine;
using System.Collections;

public class TextBox : MonoBehaviour {
	private string mText = "";
	string mPlaceHolder = "type here";
	public string mControlName = "temp";
	
	private string getText() {
		return (GUI.GetNameOfFocusedControl () == mControlName || mText.Length > 0) ? mText : mPlaceHolder;
	}
	
	private Color getColour() {
		return mText.Length > 0 ? Color.white : Color.gray;
	}
	
	private Rect getRect() {
		if (mIsFullScreen) {
			return new Rect (0, 0, Screen.width, Screen.height);
		} else {
			return mBoxSize;
		}
	}
	
	public Rect mBoxSize = new Rect (0, 0, 20, 20);
	public bool mIsFullScreen = false;
	
	void OnGUI() {
		GUI.color = getColour ();
		GUI.SetNextControlName (mControlName);
		var tempString = GUI.TextField (getRect (), getText ());
		if ((GUI.GetNameOfFocusedControl () == mControlName)) {
			mText = tempString;
		}
		GUI.color = Color.black;
	}
}
