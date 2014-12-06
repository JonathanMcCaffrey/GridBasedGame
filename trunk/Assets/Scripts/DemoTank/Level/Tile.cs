using UnityEngine;
using System.Collections;
using System.Runtime.Serialization;
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
	
	void OnMouseDown() {
		if (Editor.instance.mIsOn) {
			if (this.mEditorType != EditorType.Button) {
				Destroy (this.gameObject);
			}
		}
	}
}
[Serializable]
public class TileData {
	public float mX;
	public float mY;
	public string mTextureName;
	public Tile.CollidableType mCollidableType;
	
	public TileData(Tile aTile) {
		mX = aTile.gameObject.transform.position.x;
		mY = aTile.gameObject.transform.position.y;
		
		mTextureName = Tile.TexturePath + aTile.GetComponent<SpriteRenderer> ().sprite.texture.name;
		mCollidableType = aTile.mCollidableType;
	}
}

