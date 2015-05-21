using UnityEngine;
using System.Collections;

namespace FacebookUI {
	
	namespace Button {
		
		public class LogInOrOut : BaseButton {
			
			public void Start() {
				Facebook.Login.instance.addListener (this);
			}
			
			
			public void onClick() {
				if (isLoggedIn) {
					Facebook.Login.instance.OnLogoutSelected();
				} else {		
					Facebook.Login.instance.OnLoginSelected ();
				}
			}
			
			public override void onFacebookLogIn ()
			{
				isLoggedIn = true;
				
				this.gameObject.GetComponentInParent<ButtonText> ().text = "Log Out";
				this.gameObject.GetComponentInParent<ButtonText> ().RefreshButton ();
			}
			
			public override void onFacebookLogOut ()
			{
				isLoggedIn = false;
				
				this.gameObject.GetComponentInParent<ButtonText> ().text = "Log In";
				this.gameObject.GetComponentInParent<ButtonText> ().RefreshButton ();
			}
		}
	}
}