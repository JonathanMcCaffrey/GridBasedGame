using UnityEngine;
using System.Collections;

namespace FacebookUI {
	
	namespace Button {
		
		public class Invite : BaseButton {
			
			public void onClick() {
				
				if (isLoggedIn) {
					FacebookInvite.Friend();
				} 
			}
		}
	}
}