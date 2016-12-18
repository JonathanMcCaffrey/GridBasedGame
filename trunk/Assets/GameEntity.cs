using UnityEngine;
using System.Collections;

public class GameEntity : MonoBehaviour {

	private AllegianceType allegianceType = AllegianceType.None;

	public void setAllegianceType(AllegianceType allegianceType) {
		this.allegianceType = allegianceType;
	}
	public AllegianceType getAllegianceType() {
		return allegianceType;
	}

	public void Damage() {
		//TODO

	}
}
