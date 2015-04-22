using UnityEngine;
using System.Collections.Generic;

public enum CollisionCategory {None, Block, Player, Enemy, PlayerAttack, EnemyAttack}

public class CollisionComponent : MonoBehaviour {

	[SerializeField]
	private CollisionCategory Category;

	public Vector2 size;
	[HideInInspector]
	public Vector2 extents;
	[HideInInspector]
	public Vector2 heightvec, widthvec;

	public void Start() {
		extents = size / 2f;
		heightvec = new Vector2 (0f, extents.y);
		widthvec = new Vector2 (extents.x, 0f);
		Debug.Log ("start coll");
	}

	public void SetCategory(CollisionCategory c) {
		if (c == CollisionCategory.None)
			return;

		if (Category != CollisionCategory.None)
			categories [Category].Remove (this);
		Category = c;
		categories [Category].Add (this);
	}

	public CollisionCategory GetCategory() {
		return Category;
	}

	static Dictionary<CollisionCategory, HashSet<CollisionComponent>> categories;

	static CollisionComponent() {
		categories = new Dictionary<CollisionCategory, HashSet<CollisionComponent>> ();
		categories.Add (CollisionCategory.Block, new HashSet<CollisionComponent> ());
		categories.Add (CollisionCategory.Player, new HashSet<CollisionComponent> ());
		categories.Add (CollisionCategory.Enemy, new HashSet<CollisionComponent> ());
		categories.Add (CollisionCategory.PlayerAttack, new HashSet<CollisionComponent> ());
		categories.Add (CollisionCategory.EnemyAttack, new HashSet<CollisionComponent> ());
	}

	public void OnEnable() {
		categories [Category].Add (this);
	}

	public void OnDisable() {
		categories [Category].Remove (this);
	}

	public GameObject CheckCollision(CollisionCategory category) {
		foreach (CollisionComponent c in categories[category]) {
			if (Mathf.Abs(c.transform.position.x - transform.position.x) < (c.extents.x + extents.x) &&
			    Mathf.Abs(c.transform.position.y - transform.position.y) < (c.extents.y + extents.y))
				return c.gameObject;
		}
		return null;
	}
}
