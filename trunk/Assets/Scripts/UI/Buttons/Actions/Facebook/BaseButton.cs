using UnityEngine;
using System.Collections;

namespace FacebookUI {
	
	namespace Button {
		
		public class BaseButton : MonoBehaviour, Facebook.LoginListener {
			
			protected bool isLoggedIn = false;
			
			public virtual void Start() {
				isLoggedIn = FB.IsLoggedIn;
				
				if (isLoggedIn) {
					onFacebookLogIn();
				} else {
					onFacebookLogOut();
				}
				
				Facebook.Login.instance.addListener (this);
			}
			
			public void OnDestroy() {
				Facebook.Login.instance.removeListener (this);
			}
			
			public virtual void onClick() {
				
			}
			
			public virtual void onFacebookLogIn()
			{
				isLoggedIn = true;
				
				gameObject.SetActive (true);
				
			}
			
			public virtual void onFacebookLogOut()
			{
				isLoggedIn = false;
				
				gameObject.SetActive (false);
				
			}
		}
	}
}