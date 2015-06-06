using UnityEngine;
using System.Collections;

namespace MenuUI {
	
	namespace Button {
		
		public class Play : MonoBehaviour {
			
			public void onClick() {
				Application.LoadLevel("3");
			}
		}
	}
}