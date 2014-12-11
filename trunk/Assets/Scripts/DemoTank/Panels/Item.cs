using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {
	
	public UILabel mTitleLabel = null;
	public UILabel mDescriptionLabel = null;
	public UILabel mMarkingLabel = null;

	public int mOrder = 0;

	void Start() {
		mMarkingLabel.gameObject.SetActive (false);
	}

	public void onSelected() {
	//	EditorSave.instance;


		Destroy (LevelSelect.instance.gameObject);
	}


}
