using UnityEngine;
using System.Collections;

public class InputSystem : MonoBehaviour {

	private CaptureTap captureTap = new CaptureTap();

	void Update () {
	
		InputPackage inputPackage = new InputPackage();

		//TODO Convert to world position, and get direction from player
		inputPackage.setMousePosition (Input.mousePosition);


		if (captureTap.IsCaptured ()) {
			inputPackage.setInputEventType (InputEventType.OnTap);

			InputEvents.Occured (inputPackage);
		}
	}
}
