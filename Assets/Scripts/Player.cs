using UnityEngine;
using System.Collections;

public static class Controls { 
	public static int Left = 1 << 1,
		Right = 1 << 2, 
		Jump = 1 << 3, 
		Shoot = 1 << 4;
}

[RequireComponent(typeof(MotionComponent))]
public class Player : StepBasedComponent {

	public float Gravity;
	public float MoveSpeed;
	public float JumpVelocity;
	public float BoostReduction;

	protected int Movement;

	protected MotionComponent motion;
	protected CollisionComponent coll;

	public void Awake() {
		motion = GetComponent<MotionComponent> ();
		coll = GetComponent<CollisionComponent> ();
	}

	override public void OnEnable() {
		base.OnEnable ();
		motion.OnWallHit += WallHit;
	}

	override public void OnDisable() {
		base.OnDisable ();
		motion.OnWallHit -= WallHit;
	}

	protected void HandleInput(int input) {
		Movement = Movement | input;
	}

	override public void BeginStep() {
		float mx = 0f, my = motion.Velocity.y; //motion.Velocity.x;
		if ((Movement & Controls.Left) > 0)
			mx -= MoveSpeed;
		else if ((Movement & Controls.Right) > 0)
			mx += MoveSpeed;
		if ((Movement & Controls.Jump) > 0) { 
			if (Level.current.SolidAtPoint (new Vector2 (transform.position.x, transform.position.y - coll.extents.y - .1f)))
				my = JumpVelocity;
		} else if (my > 0f)
			my = Mathf.Max (0f, my - BoostReduction);

		motion.Velocity = new Vector2 (mx, my + Gravity);
		Movement = 0;
	}

	public void WallHit(Direction contactDir) {
		if (contactDir == Direction.Up || contactDir == Direction.Down)
			motion.Velocity = new Vector2 (motion.Velocity.x, 0f);
	}
}
