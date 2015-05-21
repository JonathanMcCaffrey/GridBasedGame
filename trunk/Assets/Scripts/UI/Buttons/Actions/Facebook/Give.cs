using UnityEngine;
using System.Collections;

namespace FacebookUI {
	
	namespace Button {
		
		public class Give : BaseButton {
			
			public void onClick() {
				
				if (isLoggedIn) {
					Facebook.Send.Decoy();
				} 
			}
		}
	}
}