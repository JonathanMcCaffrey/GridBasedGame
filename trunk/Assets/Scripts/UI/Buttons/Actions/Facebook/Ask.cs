using UnityEngine;
using System.Collections;

namespace FacebookUI {
	
	namespace Button {
		
		public class Ask : BaseButton {
			
			public void onClick() {

				if (isLoggedIn) {
					Facebook.Ask.Decoy();

				} 
				
			}
		}
	}
}