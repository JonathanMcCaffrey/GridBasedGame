using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Facebook.MiniJSON;

namespace Facebook {
	
	public class Ask : MonoBehaviour {
		
		public static void Decoy() {
			FB.AppRequest ("Give a Decoy in Generic Game.",
			               Facebook.OGActionType.AskFor,
			               Value.Decoy,
			               null,
			               null,
			               1,
			               "generic-data",
			               "Ask Decoy",
			               sendDecoyCallback);
			
		}
		
		static void sendDecoyCallback (FBResult result) {                                                                                                                              
			if (result != null) {       
				Debug.Log("Test");
			}                                                                                                                          
		}  
	}
}
