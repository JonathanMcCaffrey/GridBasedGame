using UnityEngine;
using System.Collections;

public class CaptureTap {

	//Doxygen styled comments for future use
	float tapTime = 1;	//!< Total delta time before we assume this is not a tap event
	float deltaTime = 0;	//!< Current delta time after tap
	bool wasTouchDown = false;	//!< Was the previous touch down. For comparison
	bool isPossibleTap = false;	//!< Could the current touch down event be a tap?

	public bool IsCaptured () {
		bool isTouchCaptured = false;
		bool isTouchDown = Input.GetMouseButtonDown (0);

		if (isPossibleTap) {
			deltaTime += Time.deltaTime;

			if (deltaTime > tapTime) {
				isPossibleTap = false;
				deltaTime = 0;
			}
		}

		if (wasTouchDown != isTouchDown) {
			if (isTouchDown) {
				isPossibleTap = true;
				deltaTime = 0;
			}

			if (!isTouchDown && isPossibleTap) {
				isTouchCaptured = true;
				deltaTime = 0;
			}
		}

		wasTouchDown = Input.GetMouseButtonDown (0);

		return isTouchCaptured;

	}
}
