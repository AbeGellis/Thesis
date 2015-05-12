using UnityEngine;
using System.Collections;
using System;

public class EnemyPlayer : Player {
	public RandomNumberGenerator RNG;
	public Player foe;
	public bool UseSeed;
	public int Seed;


	public EnemyState CurrentState;
	public EnemyState[] States = new EnemyState[3];

	private EnemyState GenerateState (float typeVal) {
		return (EnemyState) Activator.CreateInstance(EnemyState.StateTypes[Mathf.FloorToInt(typeVal * EnemyState.StateTypes.Length)]);
		//return (EnemyState) ScriptableObject.CreateInstance(EnemyState.StateTypes[Mathf.FloorToInt(typeVal * EnemyState.StateTypes.Length)]);
	}

	private FiringPattern GenerateFiringPattern (float typeVal) {
		return (FiringPattern) Activator.CreateInstance(FiringPattern.PatternTypes[Mathf.FloorToInt(typeVal * FiringPattern.PatternTypes.Length)]);
		//return (FiringPattern) ScriptableObject.CreateInstance(FiringPattern.PatternTypes[Mathf.FloorToInt(typeVal * FiringPattern.PatternTypes.Length)]);
	}

	private SpriteRenderer _spriteRenderer;

	public void Start() {
		if (UseSeed)
			UnityEngine.Random.seed = Seed;

		_spriteRenderer = GetComponent<SpriteRenderer> ();

		for (int i = 0; i < 3; ++i) {
			float val = RNG.GenerateValue("State " + i.ToString() + " Type", true);
			States[i] = GenerateState(val);

			States[i].ToControl = this;

			States[i].StateHue = RNG.GenerateValue("State " + i.ToString() + " Hue", false);

			for (int j = 0; j < 2; ++j) 
				States[i].Arguments[j] = RNG.GenerateValue("State " + i.ToString() + " Argument " + j.ToString(), false);
			for (int j = 0; j < 2; ++j) {
				float transitionType = RNG.GenerateValue("State " + i.ToString() + " Transition Type " + j.ToString(), true);
				States[i].TransitionConditions[j] = 
					EnemyState.TransitionTypes[Mathf.FloorToInt(transitionType * EnemyState.TransitionTypes.Length)];
				States[i].TransitionArguments[j] = 
					RNG.GenerateValue("State " + i.ToString() + " Transition Argument " + j.ToString(), false);
			}

			val = RNG.GenerateValue("State " + i.ToString() + " Firing Pattern Type", true);
			States[i].FirePattern = GenerateFiringPattern(val);

			for (int j = 0; j < 2; ++j) 
				States[i].FirePattern.Arguments[j] = RNG.GenerateValue("State " + i.ToString() + " Firing Pattern Argument " + j.ToString(), false);
		}

		States [0].OtherState [0] = States [1];
		States [0].OtherState [1] = States [2];
		States [1].OtherState [0] = States [0];
		States [1].OtherState [1] = States [2];
		States [2].OtherState [0] = States [0];
		States [2].OtherState [1] = States [1];

		SetActiveState (States [0]);
	}

	public override void BeginStep() {
		CurrentState.PlanMovement ();
	}

	public override void EndStep() {
		GameObject g = coll.CheckCollision (CollisionCategory.PlayerAttack);
		if (g) {
			Health -= g.GetComponent<Bullet>().Damage;
			g.SetActive(false);
		}
	}

	public void SetActiveState(EnemyState state) {
		if (_spriteRenderer)
			_spriteRenderer.color = Utils.HSVToRGB (state.StateHue, .75f, 1f);
		if (CurrentState != null)
			CurrentState.ExitState ();
		state.EnterState ();
		CurrentState = state;
	}

	new public void HandleInput(int input) {
		base.HandleInput (input);
	}

	override public void Landed() {
		base.Landed();
		CurrentState.Landed();
	}
	override public void WallHit(Direction contactDir) {
		base.WallHit (contactDir);
		CurrentState.WallHit (contactDir);
	}
}
