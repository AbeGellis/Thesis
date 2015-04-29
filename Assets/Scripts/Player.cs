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
	public int Health = 100;
	public float ShotSpeed;
	public GameObject[] Bullets;
	public int ShotDelay;

	protected int Movement;

	protected MotionComponent motion;
	protected CollisionComponent coll;

	protected bool onGround;

	protected bool facingRight;

	private int _shotTimer = 0;

	virtual public void Awake() {
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

	override public void Step() {
		bool grounded = (Level.current.SolidAtPoint (new Vector2 (transform.position.x, transform.position.y - coll.extents.y - .1f)));

		if (!grounded && onGround) {
			onGround = false;
			LeftGround();
		}
		else if (!onGround && grounded) {
			onGround = true;
			Landed();
		}

		float mx = 0f, my = motion.Velocity.y; //motion.Velocity.x;
		if ((Movement & Controls.Left) > 0) {
			mx -= MoveSpeed;
			facingRight = false;
		} else if ((Movement & Controls.Right) > 0) {
			mx += MoveSpeed;
			facingRight = true;
		}

		if ((Movement & Controls.Jump) > 0) { 
			if (grounded)
				my = JumpVelocity;
		} else if (my > 0f)
			my = Mathf.Max (0f, my - BoostReduction);

		motion.Velocity = new Vector2 (mx, my + Gravity);
		Movement = 0;

		if (_shotTimer > 0)
			--_shotTimer;
	}


	virtual public void WallHit(Direction contactDir) {
		if (contactDir == Direction.Up || contactDir == Direction.Down) {
			motion.Velocity = new Vector2 (motion.Velocity.x, 0f);
		}
	}

	virtual public void HandleShoot(Vector2 velocity) {
		if (_shotTimer > 0)
			return;

		foreach (GameObject b in Bullets) {
			if (!b.activeSelf) {
				b.SetActive(true);
				b.GetComponent<MotionComponent> ().Velocity = velocity;
				b.GetComponent<CollisionComponent> ().Start ();
				b.transform.position = new Vector2(transform.position.x + (velocity.x > 0 ? coll.extents.x : -coll.extents.x), 
				                                   transform.position.y);

				_shotTimer = ShotDelay;
				return;
			}
		}
	
	}

	public void HandleShoot() {
		Vector2 velocity = facingRight ? Vector2.right * ShotSpeed : Vector2.right * -ShotSpeed;
		HandleShoot (velocity);
	}

	virtual public void Landed() {}
	virtual public void LeftGround() {}
}
