using UnityEngine;
using System.Collections;

public class FacebookPortrait : MonoBehaviour, Facebook.LoginListener {
	private bool shouldRefreshImage = false;
	private UITexture image = null;
	
	public static FacebookPortrait instance = null;
	
	public void Start() {
		if (instance) {
			Destroy (gameObject);
		} else {
			DontDestroyOnLoad(gameObject);
			instance = this;
			
			image = GetComponent<UITexture>();
			
			if (FB.IsLoggedIn) {
				gameObject.SetActive (true);
				Facebook.Login.instance.RefreshPortrait();
			} else {
				gameObject.SetActive (false);
			}
		}
		
		Facebook.Login.instance.addListener(this);
		
	}
	
	public void OnDestroy() {
		instance = null;
		Facebook.Login.instance.removeListener(this);
	}
	
	public static void SetImage(Texture2D image) {
		if (instance) {
			instance.image.mainTexture = image;
		}
	}
	
	public virtual void onFacebookLogIn() {
		gameObject.SetActive (true);
		
		gameObject.transform.localScale = Vector3.one;
	}
	
	public virtual void onFacebookLogOut() {
		gameObject.SetActive (false);
		
		gameObject.transform.localScale = Vector3.zero;
		
	}
}

