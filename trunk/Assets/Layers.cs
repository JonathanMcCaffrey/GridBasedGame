using UnityEngine;
using System.Collections;


public static class Layers {

	public const string GAME = "gameLayer";


	public static class Game {
		public const string PROJECTILE = "projectTileLayer";

		public static class Projectile {
			public const string ENEMY = "enemyProjectileLayer";
			public const string PLAYER = "playerProjectileLayer";
		}

		public const string OBJECT = "objectLayer";
		public const string PLAYER = "playerLayer";
		public const string ENEMY = "enemyLayer";
		public const string PICKUP = "pickupLayer";
	}


	public static void Create() {


		Transform gameLayerTransform = new GameObject(GAME).transform;


		Transform projectileLayerTransform = addNewLayer (Game.PROJECTILE, gameLayerTransform);
		addNewLayer (Game.Projectile.ENEMY, projectileLayerTransform);
		addNewLayer (Game.Projectile.PLAYER, projectileLayerTransform);

		addNewLayer (Game.OBJECT, gameLayerTransform);
		addNewLayer (Game.PLAYER, gameLayerTransform);
		addNewLayer (Game.ENEMY, gameLayerTransform);
		addNewLayer (Game.PICKUP, gameLayerTransform);
	}

	/**
	 * Create a layer with Name, attached to the passed in transform. Returns the new layers transform 
	 */
	private static Transform addNewLayer(string withLayerName, Transform withTransform) {
		GameObject newLayer = new GameObject (withLayerName);
		newLayer.transform.parent = withTransform;

		return newLayer.transform;
	}
}
