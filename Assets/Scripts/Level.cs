using UnityEngine;
using System.Collections.Generic;

public enum Terrain {None, Solid}

public enum Direction {None, Right, Up, Left, Down}

public class Level : MonoBehaviour {

	public static Level current;

	public Terrain[,] Tiles;
	public GameObject SolidTile;

	Vector2 tileSize;

	void Awake() {
		current = this;

		tileSize = SolidTile.GetComponent<SpriteRenderer> ().bounds.size;

		Tiles = new Terrain[,] {
			{Terrain.Solid, Terrain.Solid, Terrain.Solid, Terrain.Solid, Terrain.Solid, Terrain.Solid, Terrain.Solid, Terrain.Solid, Terrain.Solid, Terrain.Solid, Terrain.Solid, Terrain.Solid, Terrain.Solid},
			{Terrain.Solid, Terrain.None, Terrain.None, Terrain.None, Terrain.None, Terrain.None, Terrain.None, Terrain.None, Terrain.None, Terrain.None, Terrain.None, Terrain.None, Terrain.Solid},
			{Terrain.Solid, Terrain.None, Terrain.None, Terrain.None, Terrain.None, Terrain.None, Terrain.None, Terrain.None, Terrain.None, Terrain.None, Terrain.None, Terrain.None, Terrain.Solid},
			{Terrain.Solid, Terrain.None, Terrain.None, Terrain.None, Terrain.None, Terrain.None, Terrain.None, Terrain.None, Terrain.None, Terrain.None, Terrain.None, Terrain.None, Terrain.Solid},
			{Terrain.Solid, Terrain.None, Terrain.None, Terrain.None, Terrain.None, Terrain.None, Terrain.None, Terrain.None, Terrain.None, Terrain.None, Terrain.None, Terrain.None, Terrain.Solid},
			{Terrain.Solid, Terrain.None, Terrain.None, Terrain.None, Terrain.None, Terrain.None, Terrain.None, Terrain.None, Terrain.None, Terrain.None, Terrain.None, Terrain.None, Terrain.Solid},
			{Terrain.Solid, Terrain.None, Terrain.None, Terrain.None, Terrain.None, Terrain.None, Terrain.None, Terrain.None, Terrain.None, Terrain.None, Terrain.None, Terrain.None, Terrain.Solid},
			{Terrain.Solid, Terrain.None, Terrain.None, Terrain.None, Terrain.None, Terrain.None, Terrain.None, Terrain.None, Terrain.None, Terrain.None, Terrain.None, Terrain.None, Terrain.Solid},
			{Terrain.Solid, Terrain.None, Terrain.None, Terrain.None, Terrain.None, Terrain.None, Terrain.None, Terrain.None, Terrain.None, Terrain.None, Terrain.None, Terrain.None, Terrain.Solid},
			{Terrain.Solid, Terrain.Solid, Terrain.Solid, Terrain.Solid, Terrain.Solid, Terrain.Solid, Terrain.Solid, Terrain.Solid, Terrain.Solid, Terrain.Solid, Terrain.Solid, Terrain.Solid, Terrain.Solid}
		};
	}

	void Start() {
		if (Settings.GraphicsOn) {
			for (int x = 0; x < Tiles.GetLength(1); ++x)				
				for (int y = 0; y < Tiles.GetLength(0); ++y) {
					if (Tiles[y,x] == Terrain.Solid) {
						var g = (GameObject) Instantiate(SolidTile, transform.position + new Vector3(x * tileSize.x, y * tileSize.y, 0f), 
					                                 Quaternion.identity);
						g.transform.parent = transform;
					}
				}
			Camera.main.transform.position = new Vector3((((float) Tiles.GetLength(1) - .5f) * tileSize.x / 2f) ,
			                                             (((float) Tiles.GetLength(0) - .5f) * tileSize.y / 2f) ,
			                                             Camera.main.transform.position.z);
		}
	}

	public bool SolidAtPoint(Vector2 point) {
		int x = (int) (point.x / tileSize.x + .5f);
		int y = (int) (point.y / tileSize.y + .5f);
		return (Tiles [y, x] == Terrain.Solid);
	}

	public Vector2 PointOnEdge(Vector2 point, Direction side) {
		float x = Mathf.Floor(point.x / tileSize.x + .5f);
		float y = Mathf.Floor(point.y / tileSize.y + .5f);
		switch (side) {
		case Direction.Left:
			return new Vector2(tileSize.x * (x - .5f), point.y); 
		case Direction.Right:
			return new Vector2(tileSize.x * (x + .5f), point.y); 
		case Direction.Down:
			return new Vector2(point.x, tileSize.y * (y - .5f));
		case Direction.Up:
			return new Vector2(point.x, tileSize.y * (y + .5f));
		}

		return point;
	}
}