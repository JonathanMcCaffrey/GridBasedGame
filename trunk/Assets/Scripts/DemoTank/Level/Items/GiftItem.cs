using UnityEngine;
using System.Collections;

public class GiftItem : MonoBehaviour {
	
	public GameObject takeEffect = null;
	
	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.GetComponent<PlayerObject> ()) {
			Inventory.FoundSpecialItem();
			
			GameObject.Instantiate(takeEffect);
			takeEffect.transform.position = gameObject.transform.position;
			
			Destroy(gameObject);
		}
	}
}
