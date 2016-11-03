using UnityEngine;
using System.Collections;

public interface InputEventListener
{
	void onInputEvent(InputPackage inputPackage);
}