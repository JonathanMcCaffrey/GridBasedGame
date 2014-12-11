using UnityEngine;
using System.Collections;

public class OpenLevelSelect : MonoBehaviour {
	public GameObject mDefaultLevelSelect = null;
	
	public void onSelected() {
		if(mDefaultLevelSelect) {
			var tempLevelSelect = GameObject.Instantiate (mDefaultLevelSelect) as GameObject;
			tempLevelSelect.transform.parent = gameObject.transform.parent;
			tempLevelSelect.transform.localScale = Vector3.one;
			tempLevelSelect.transform.localPosition = Vector3.zero;
		}
	}
}
