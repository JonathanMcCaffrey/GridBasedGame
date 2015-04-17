using UnityEngine;
using System.Collections;

public class ButtonFacebook : MonoBehaviour, FacebookLoginListener {
	
	bool isLoggedIn = false;
	
	public void onClick() {
		
		if (isLoggedIn) {
			FacebookInvite.Invite();
		} else {
			FacebookLogin.instance.addListener (this);			
			FacebookLogin.instance.OnLoginSelected ();
		}
		
	}
	
	public void onFacebookLoggedIn ()
	{
		isLoggedIn = true;
		this.gameObject.GetComponentInParent<ButtonText> ().text = "Invite";
		this.gameObject.GetComponentInParent<ButtonText> ().RefreshButton ();
	}
}
