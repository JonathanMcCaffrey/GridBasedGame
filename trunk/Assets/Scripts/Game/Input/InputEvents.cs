

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputEvents  {	

	private List<InputEventListener> list = new List<InputEventListener>();

	private static InputEvents instance = null;
	private static InputEvents get() {
		if (instance == null) {
			instance = new InputEvents();
		}

		return instance;
	}

	private InputEvents() { }


	public static void Add(InputEventListener item) {
		if (!get().list.Contains (item)) {
			get().list.Add (item);
		}
	}

	public static void Reset()
	{
		get().list.Clear();
	}

	public static void Remove(InputEventListener item) {
		if (!get().list.Contains (item)) {
			get().list.Add (item);
		}
	}

	public static void Occured(InputPackage inputPackage) {
		foreach (InputEventListener item in get().list) {
			item.onInputEvent (inputPackage);
		}
	}
}