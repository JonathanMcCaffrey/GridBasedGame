using UnityEngine;
using System.Collections;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Xml;
using System;

 
public class Tile : MonoBehaviour {

	public static string TexturePath = "Textures/DemoTank/";


	public enum EditorType { Tile, Button };
	public enum CollidableType { Floor, Wall, Damage, Bullet, Enemy, Player, Breakable };
	
	public EditorType mEditorType = EditorType.Button;
	public CollidableType mCollidableType = CollidableType.Floor;
	
	float mGridSize = 50.0f;
	
	public KeyCode mHotKey = KeyCode.None;
	
	
	void TileToGrid () {
		mGridSize = GetComponent<BoxCollider2D> ().size.x;
		if (mEditorType == EditorType.Tile) {
			Vector2 gridPosition = new Vector2 ((float)(((int)(this.transform.position.x / mGridSize)) * mGridSize) + mGridSize * 0.5f, (float)(((int)(this.transform.position.y / mGridSize)) * mGridSize) + mGridSize * 0.5f);
			this.transform.position = gridPosition;
		}
	}
	
	public void Start() {
		TileToGrid ();
		
		string textureName = GetComponent<SpriteRenderer> ().sprite.texture.name;

	}
	
	public void OnDrawGizmos () {
		TileToGrid ();	
	}
	
	public TileData GenerateData() {
		return new TileData (this);
	}
}
[Serializable]
public class TileData {
	Vector2 position;
	string textureName;
	Tile.CollidableType collidableType;
	
	public TileData(Tile aTile) {
		position = new Vector2 (aTile.gameObject.transform.position.x, aTile.gameObject.transform.position.y);
		textureName = Tile.TexturePath + aTile.GetComponent<SpriteRenderer> ().sprite.texture.name;
		collidableType = aTile.mCollidableType;
	}
}

