using UnityEngine;
using System.Collections;

namespace Panels {
	public class Close : MonoBehaviour {
		public GameObject mCloseItem = null;
		
		public void onSelected() {
			if (mCloseItem) {
				Destroy(mCloseItem);
				mCloseItem = null;
			}
		}
	}	
}