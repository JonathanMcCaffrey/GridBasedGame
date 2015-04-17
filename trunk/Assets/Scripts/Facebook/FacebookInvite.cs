using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Facebook.MiniJSON;

public class FacebookInvite : MonoBehaviour {
	
	public static void Invite()                                                                                              
	{
		FB.AppRequest(
			"Come join me in this game!",
			null,
			null,
			null,
			null,
			"data",
			"Game",
			appRequestCallback);


	}                                                                                                                              
	static void appRequestCallback (FBResult result)                                                                              
	{                                                                                                                              
		if (result != null)                                                                                                        
		{                                                                                                                          
			
		}                                                                                                                          
	}  
}
