using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Facebook.MiniJSON;

public class FacebookInvite : MonoBehaviour {
	
	public static void Invite()                                                                                              
	{
		SendGift ();

		return;

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

	public static void SendGift()                                                                                              
	{
		FB.AppRequest ("Take this Gift",
		              Facebook.OGActionType.Send,
		              "Gift",
		              null,
		              "Gift",
		              "Send Gift",
		              appRequestCallback);
		             
		
	}   

	public static void AskForGift()                                                                                              
	{
		FB.AppRequest ("Need a Gift",
		               Facebook.OGActionType.AskFor,
		               "Gift",
		               null,
		               "Gift",
		               "Ask for Gift",
		               appRequestCallback);
		
		
	}   


	static void appRequestCallback (FBResult result)                                                                              
	{                                                                                                                              
		if (result != null)                                                                                                        
		{                                                                                                                          
			
		}                                                                                                                          
	}  
}
