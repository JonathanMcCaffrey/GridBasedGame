using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Keys : MonoBehaviour {
	private Dictionary<KeyCode, bool> mKeyIsDown = new Dictionary<KeyCode, bool>();
	
	public static Keys instance = null;
	void Awake() {
		if (instance) {
			Destroy (gameObject);
		} else {
			DontDestroyOnLoad(gameObject);
			instance = this;
		}
	}
	
	
	List<IKeyListener> mListeners = new List<IKeyListener>();
	
	static public void AddListener(IKeyListener aListener) {
		if (!instance.mListeners.Contains (aListener)) {
			instance.mListeners.Add(aListener);	
		}
	}
	
	static public void RemoveListener(IKeyListener aListener) {
		if (instance.mListeners.Contains (aListener)) {
			instance.mListeners.Remove(aListener);
		}		
	}
	
	private void ActivateSelectedKey () {
		var curEvent = Event.current;
		
		if (curEvent.keyCode != KeyCode.None) {
			if(!mKeyIsDown.ContainsKey(curEvent.keyCode)) {
				mKeyIsDown.Add(curEvent.keyCode, true);
			}	
			
			if (!mKeyIsDown [curEvent.keyCode]) {
				foreach (IKeyListener listener in mListeners) {
					listener.OnKeyPressed (curEvent.keyCode);
				}
			}
			
			mKeyIsDown [curEvent.keyCode] = Input.GetKeyDown (curEvent.keyCode);
		}

		List<KeyCode> tempKeyList = new List<KeyCode>();
		foreach (var pairing in mKeyIsDown) {
			tempKeyList.Add(pairing.Key);
		}
		foreach (var keyCode in tempKeyList) {
			mKeyIsDown[keyCode] = Input.GetKeyDown (keyCode);
		}

	}
	
	void OnGUI() {
		ActivateSelectedKey ();	
	}
}
