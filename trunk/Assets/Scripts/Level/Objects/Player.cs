using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public const string NAME = "Player";

	//Deprecated. Remove this static
	public static Player instance;
	public void Awake() {
		instance = this;
	}

	public static GameObject Create() {
		GameObject player = new GameObject (NAME);

		player.AddComponent<Player> ().InitializePlayer();

		//player.transform.parent = GameObject.Find (Layers.Game.PLAYER).transform;
			
		return player;
	}

	public void InitializePlayer() {

		Rigidbody2D rigidbody2D =  gameObject.AddComponent<Rigidbody2D> ();
		rigidbody2D.gravityScale = 0;


		gameObject.AddComponent<BoxCollider2D> ().size = new Vector2 (1, 1);

		gameObject.AddComponent<GridControls> ();

		gameObject.AddComponent<SpriteRenderer> ().sprite = Resources.Load("Textures/DemoTank/Ball_Red", typeof(Sprite)) as Sprite;

		WeaponSystem weaponSystem = gameObject.AddComponent<WeaponSystem> ();

		weaponSystem.addTapFire (new SingleShot ());
		weaponSystem.addTapFire (new SingleShot ());
		weaponSystem.addTapFire (new SingleShot ());
		weaponSystem.addTapFire (new SingleShot ());

		gameObject.AddComponent<WeaponControls> ();

	}
}
