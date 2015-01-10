using UnityEngine;
using System.Collections;

public class ButtonFacebook : MonoBehaviour {
	public void onClick() {
		FacebookLogin.instance.OnLoginSelected ();
	}
}
