using UnityEngine;
using System.Collections;

public class ComputerControlledPlayer : Player {

	private bool OnLeftWall = false, OnRightWall = false;

	public int InputPressed;

	//Returns true if there are no "useless" button inputs
	public bool UsefulInput(int input) {
		bool InputJump = (input & Controls.Jump) > 0, 
			InputLeft = (input & Controls.Left) > 0,
			InputRight = (input & Controls.Right) > 0,
			InputShoot = (input & Controls.Shoot) > 0;

		if (InputJump && !onGround && !rising)
			return false;

		if (InputRight) {
			if (InputLeft)
				return false;

			if (OnRightWall)
				return false;
		}

		if (InputLeft && OnLeftWall)
			return false;

		if (InputShoot) {
			if (shotTimer > 0)
				return false;

			if (System.Array.Find(Bullets, b => !b.activeSelf) == null)
				return false;
		}

		return true;
	}

	override public void BeginStep() {
		HandleInput (InputPressed);
	}

	override public void EndStep() {
		Vector3 l = transform.position - (Vector3)coll.widthvec + Vector3.left * MoveSpeed,
			r = transform.position + (Vector3)coll.widthvec + Vector3.right * MoveSpeed;

		OnLeftWall = Level.current.SolidAtPoint (l);
		OnRightWall = Level.current.SolidAtPoint (r);

		GameObject g = coll.CheckCollision (CollisionCategory.EnemyAttack);
		if (g) {
			Health -= g.GetComponent<Bullet>().Damage;
			g.SetActive(false);
		}
	}
}
