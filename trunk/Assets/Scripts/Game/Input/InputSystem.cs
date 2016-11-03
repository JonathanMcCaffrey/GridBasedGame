using UnityEngine;
using System.Collections;

public class InputSystem : MonoBehaviour {

	private CaptureTap captureTap;

	void Update () {
	
		InputPackage inputPackage = new InputPackage();
		inputPackage.setMousePosition (Input.mousePosition);


		if (captureTap.IsCaptured ()) {
			inputPackage.setInputEventType (InputEventType.OnTap);

			InputEvents.Occured (inputPackage);
		}
	}
}
