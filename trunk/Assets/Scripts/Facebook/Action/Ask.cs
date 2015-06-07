using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Facebook.MiniJSON;

namespace Facebook {
	
	public class Ask : MonoBehaviour {
		
		public static void GenericItem() {
			FB.AppRequest ("Need a Generic Item in Generic Game.",
			               Facebook.OGActionType.AskFor,
			               Value.GenericItem,
			               null,
			               null,
			               1,
			               "generic-data",
			               "Ask Decoy",
			               askCallback);
			
		}
		
		static void askCallback (FBResult result) {                                                                                                                              
			if (result != null) {       
				//TODO Show feedback dialog that item was asked
			}                                                                                                                          
		}  
	}
}
