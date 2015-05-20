using UnityEngine;
using System.Collections;

namespace FacebookUI {
	
	namespace Button {
		
		public class LogInOrOut : BaseButton {

			public void Start() {

			}

			
			public void onClick() {
				if (isLoggedIn) {
					FacebookReceive.Given();
					
				} else {
					FacebookLogin.instance.addListener (this);			
					FacebookLogin.instance.OnLoginSelected ();
				}
				
			}
			
			public void onFacebookLogIn ()
			{
				isLoggedIn = true;
				
				this.gameObject.GetComponentInParent<ButtonText> ().text = "Log Out";
				this.gameObject.GetComponentInParent<ButtonText> ().RefreshButton ();
			}
			
			public void onFacebookLogOut ()
			{
				isLoggedIn = false;
				
				this.gameObject.GetComponentInParent<ButtonText> ().text = "Log In";
				this.gameObject.GetComponentInParent<ButtonText> ().RefreshButton ();
			}
		}
	}
}