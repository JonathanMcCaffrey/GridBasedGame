using UnityEngine;
using System.Collections;

public class SaveLevelSelect : MonoBehaviour {
	public void onSelected() {
		EditorSave.instance.Save (LevelHeader.instance.mSlotNumber);
	}
}
