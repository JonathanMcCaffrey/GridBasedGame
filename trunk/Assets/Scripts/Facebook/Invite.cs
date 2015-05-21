using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Facebook.MiniJSON;

namespace Facebook {
	
	public class Invite : MonoBehaviour {
		
		public static void Friend() {
			FB.AppRequest(
				"Friend, come join me in this generic game!",
				null,
				null,
				null,
				null,
				"data",
				"Game",
				requestCallback);
			
		}
		
		static void requestCallback (FBResult result) {           
			
		}  
	}
}
