using UnityEngine;
using System.Collections;

public static class Controls { 
	public static int Left = 1 << 1,
		Right = 1 << 2, 
		Jump = 1 << 3, 
		Shoot = 1 << 4;
}

[RequireComponent(typeof(MotionComponent))]
public class Player : StepBasedComponent, System.ICloneable {

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
	protected bool rising;
	protected bool facingRight;

	protected int shotTimer = 0;

	public void CopyFrom(Player original) {
		Gravity = original.Gravity;
		MoveSpeed = original.MoveSpeed;
		JumpVelocity = original.JumpVelocity;
		BoostReduction = original.BoostReduction;
		Health = original.Health;
		ShotSpeed = original.ShotSpeed;
		Bullets = original.Bullets;			//TODO serialize bullet active state
		ShotDelay = original.ShotDelay;

		facingRight = original.facingRight;
		shotTimer = original.shotTimer;
		onGround = original.onGround;
	}

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
			if (grounded) {
				my = JumpVelocity;
				rising = true;
			}
		} else if (my > 0f) {
			if (rising) {
				my = Mathf.Max (0f, my - BoostReduction);
				rising = false;
			}
		} else
			rising = false;

		if ((Movement & Controls.Shoot) > 0)
			HandleShoot ();

		motion.Velocity = new Vector2 (mx, my + Gravity);
		Movement = 0;

		if (shotTimer > 0)
			--shotTimer;

		if (Health <= 0f) {
			GetComponent<SpriteRenderer>().enabled = false;
			enabled = false;
			motion.Velocity = Vector2.zero;
			var t = GetComponent<GenerateTrail>();
			if (t != null)
				t.enabled = false;
		}

	}


	virtual public void WallHit(Direction contactDir) {
		if (contactDir == Direction.Up || contactDir == Direction.Down) {
			motion.Velocity = new Vector2 (motion.Velocity.x, 0f);
		}
	}

	virtual public GameObject HandleShoot(Vector2 velocity) {
		if (shotTimer > 0)
			return null;

		foreach (GameObject b in Bullets) {
			if (!b.activeSelf) {
				b.SetActive(true);
				b.GetComponent<MotionComponent> ().Velocity = velocity;
				b.GetComponent<CollisionComponent> ().Start ();
				b.transform.position = new Vector2(transform.position.x + (velocity.x > 0 ? coll.extents.x : -coll.extents.x), 
				                                   transform.position.y);

				shotTimer = ShotDelay;
				return b;
			}
		}
		return null;
	
	}

	public GameObject HandleShoot() {
		Vector2 velocity = facingRight ? Vector2.right * ShotSpeed : Vector2.right * -ShotSpeed;
		return HandleShoot (velocity);
	}

	virtual public void Landed() {}
	virtual public void LeftGround() {}
}
