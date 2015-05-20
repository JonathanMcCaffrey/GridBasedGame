using UnityEngine;
using System.Collections;

namespace FacebookUI {
	
	namespace Button {
		
		public abstract class BaseButton : MonoBehaviour, FacebookLoginListener {
			
			protected bool isLoggedIn = false;
			
			public void Start() {
				gameObject.SetActive (false);
			}
			
			public virtual void onClick() {
				
			}
			
			public virtual void onFacebookLogIn ()
			{
				isLoggedIn = true;
				
				gameObject.SetActive (true);
				
			}
			
			public virtual void onFacebookLogOut ()
			{
				isLoggedIn = false;
				
				gameObject.SetActive (false);
				
			}
		}
	}
}