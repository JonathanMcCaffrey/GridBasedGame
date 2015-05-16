using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Facebook.MiniJSON;

public class FacebookSend : MonoBehaviour {

	public static void Decoy() {
		FB.AppRequest ("Take this Decoy in Generic Game.",
		               Facebook.OGActionType.Send,
		               FacebookValue.Decoy,
		               null,
		               null,
		               1,
		               "generic-data",
		               "Send Item",
		               sendDecoyCallback);
		
	}
	
	static void sendDecoyCallback (FBResult result) {                                                                                                                              
		if (result != null) {       
			Debug.Log("Test");
		}                                                                                                                          
	}  
}
