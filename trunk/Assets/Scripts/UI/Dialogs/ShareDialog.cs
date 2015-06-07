using UnityEngine;
using System.Collections;

public class ShareDialog : MonoBehaviour {
	public void onDiscardClick() {
		Destroy (gameObject);
	}
	
	public void onShareClick() {
		Facebook.Send.SpecialItem ();
		Destroy (gameObject);
	}
}
