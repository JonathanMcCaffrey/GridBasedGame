using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Facebook.MiniJSON;

namespace Facebook {
	
	public class Send : MonoBehaviour {
		
		public static void GenericItem() {
			FB.AppRequest ("Take this Generic Item in Generic Game.",
			               Facebook.OGActionType.Send,
			               Value.GenericItem,
			               null,
			               null,
			               1,
			               "generic-data",
			               "Send Generic Item",
			               sendCallback);
			
		}
		
		public static void SpecialItem() {
			FB.AppRequest ("Take this Special Item in Generic Game.",
			               Facebook.OGActionType.Send,
			               Value.SpecialItem,
			               null,
			               null,
			               1,
			               "generic-data",
			               "Send Special Item",
			               sendCallback);
			
		}
		
		static void sendCallback (FBResult result) {                                                                                                                              
			if (result != null) {       
				//TODO Show feedback dialog that item was sent
			}                                                                                                                          
		}  
	}
}
