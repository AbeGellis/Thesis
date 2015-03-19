using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CollisionComponent))]
public class MotionComponent : StepBasedComponent {

	public Vector2 Velocity;
	public bool PassThroughWalls;

	public delegate void WallHitEvent(Direction contactDir);
	public event WallHitEvent OnWallHit;

	private CollisionComponent coll;

	public void Start() {
		coll = GetComponent<CollisionComponent> ();
	}

	override public void Step() {
		transform.position += (Vector3) Velocity;
		

		if (!PassThroughWalls) {

			Vector3 d = transform.position - (Vector3) coll.heightvec,
					u = transform.position + (Vector3) coll.heightvec;

			if (Level.current.SolidAtPoint(d)) {
				transform.position = (Vector3) Level.current.PointOnEdge(d, Direction.Up) + (Vector3) coll.heightvec;
				if (OnWallHit != null)
					OnWallHit(Direction.Down);
			}
			else if (Level.current.SolidAtPoint(u)) {
				transform.position = (Vector3) Level.current.PointOnEdge(u, Direction.Down) - (Vector3) coll.heightvec;
				if (OnWallHit != null)
					OnWallHit(Direction.Up);
			}

			Vector3 l = transform.position - (Vector3) coll.widthvec,
					r = transform.position + (Vector3) coll.widthvec;

			if (Level.current.SolidAtPoint(r)) {
				transform.position = (Vector3) Level.current.PointOnEdge(r, Direction.Left) - (Vector3) coll.widthvec;
				if (OnWallHit != null)
					OnWallHit(Direction.Right);
			}
			else if (Level.current.SolidAtPoint(l)) {
				transform.position = (Vector3) Level.current.PointOnEdge(l, Direction.Right) + (Vector3) coll.widthvec;
				if (OnWallHit != null)
					OnWallHit(Direction.Left);
			}

		}
		else if (OnWallHit != null) {
			if (Level.current.SolidAtPoint(transform.position + (Vector3) coll.extents) || 
			    Level.current.SolidAtPoint(transform.position - (Vector3) coll.extents))
				OnWallHit(Direction.None);
		}

		Debug.DrawLine (transform.position - (Vector3)coll.extents, transform.position + (Vector3)coll.extents, Color.blue);

	}
}
