using UnityEngine;
using System.Collections;

public class FacebookPortrait : MonoBehaviour {
	
	private UITexture image = null;
	
	public static FacebookPortrait instance = null;
	void Awake() {
		if (instance) {
			Destroy (gameObject);
		} else {
			DontDestroyOnLoad(gameObject);
			instance = this;
			
			image = GetComponent<UITexture>();
		}
	}
	
	public static void SetImage(Texture2D image) {
		if (instance) {
			instance.image.mainTexture = image;

		}
	}
}
