using UnityEngine;
using System.Collections;

public class InputPackage {
	private Vector3 mousePosition;
	private InputEventType inputEventType;

	public InputPackage() {
	
	}

	public InputPackage setMousePosition(Vector3 mousePosition) {
		this.mousePosition = mousePosition;
		return this;
	}

	public InputPackage setInputEventType(InputEventType inputEventType) {
		this.inputEventType = inputEventType;
		return this;
	}
}
