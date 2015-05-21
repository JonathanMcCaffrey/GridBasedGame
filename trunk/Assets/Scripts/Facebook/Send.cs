using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Facebook.MiniJSON;

namespace Facebook {
	
	public class Send : MonoBehaviour {
		
		public static void Decoy() {
			FB.AppRequest ("Take this Decoy in Generic Game.",
			               Facebook.OGActionType.Send,
			               Value.Decoy,
			               null,
			               null,
			               1,
			               "generic-data",
			               "Send Decoy",
			               sendDecoyCallback);
			
		}
		
		static void sendDecoyCallback (FBResult result) {                                                                                                                              
			if (result != null) {       
				Debug.Log("Test");
			}                                                                                                                          
		}  
	}
}
