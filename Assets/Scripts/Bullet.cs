using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MotionComponent))]
[RequireComponent(typeof(CollisionComponent))]
public class Bullet : StepBasedComponent {
	public int Damage = 5;

	public void Start() {
		GetComponent<MotionComponent> ().OnWallHit += WallHit;
		gameObject.SetActive (false);
	}

	void WallHit(Direction contactDir) {
		gameObject.SetActive (false);
	}
}
