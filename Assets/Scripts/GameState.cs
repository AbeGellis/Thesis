using UnityEngine;
using System.Collections.Generic;

public class CharacterState {
	public Player Character;
	public Vector2 Position;
	public Vector2 Velocity;
}

public class BulletState {
	public bool Enabled;
	public Vector2 Position;
	public Vector2 Velocity;
}

public class GameState {
	public CharacterState EnemyState, PlayerState;
	public BulletState[] BulletStates;
	public EnemyState[] EnemyStates;
	public EnemyState CurrentState;
	public int Frame;

	public int Hash() {
		return EnemyState.Position.GetHashCode () + ~PlayerState.Position.GetHashCode ();
	}

	public GameState(Player player, Player enemy, Bullet[] bullets, GameState currentState) {
		PlayerState = new CharacterState ();
		EnemyState = new CharacterState ();
		BulletStates = new BulletState[bullets.Length];
		
		PlayerState.Character = (Player) player.Clone ();
		PlayerState.Velocity = player.GetComponent<MotionComponent> ().Velocity;
		PlayerState.Position = player.transform.position;
		
		EnemyPlayer e = (EnemyPlayer) enemy.Clone ();
		e.foe = PlayerState.Character;
		EnemyState.Character = e;
		EnemyState.Velocity = enemy.GetComponent<MotionComponent> ().Velocity;
		EnemyState.Position = enemy.transform.position;
		
		EnemyStates = new EnemyState[e.States.Length];
		for (int i = 0; i < e.States.Length; ++i) {
			EnemyStates[i] = e.States[i].CreateCopy();
			if (e.States[i] == e.CurrentState)
				CurrentState = EnemyStates[i];
		}
		
		EnemyStates [0].OtherState [0] = EnemyStates [1];
		EnemyStates [0].OtherState [1] = EnemyStates [2];
		EnemyStates [1].OtherState [0] = EnemyStates [0];
		EnemyStates [1].OtherState [1] = EnemyStates [2];
		EnemyStates [2].OtherState [0] = EnemyStates [0];
		EnemyStates [2].OtherState [1] = EnemyStates [1];
		
		
		for (int i = 0; i < bullets.Length; ++i) {
			BulletStates[i] = new BulletState();
			BulletStates[i].Enabled = bullets[i].gameObject.activeSelf;
			BulletStates[i].Position = bullets[i].transform.position;
			BulletStates[i].Velocity = bullets[i].GetComponent<MotionComponent>().Velocity;
		}

		Frame = currentState != null ? currentState.Frame + 1 : 0;
	}

	
	public void RestoreGameState(Player player, Player enemy, Bullet[] bullets) {
		player.CopyFrom(PlayerState.Character);
		player.GetComponent<MotionComponent> ().Velocity = PlayerState.Velocity;
		player.transform.position = PlayerState.Position;
		
		var e = (EnemyPlayer) enemy;
		e.CopyFrom(EnemyState.Character);
		e.GetComponent<MotionComponent> ().Velocity = EnemyState.Velocity;
		e.transform.position = EnemyState.Position;
		
		for (int i = 0; i < e.States.Length; ++i) {
			e.States[i] = EnemyStates[i];
			if (EnemyStates[i] == CurrentState) {
				e.CurrentState = e.States[i];
				e.GetComponent<SpriteRenderer>().color = Utils.HSVToRGB (e.CurrentState.StateHue, .75f, 1f);
			}
		}
		
		for (int i = 0; i < bullets.Length; ++i) {
			bullets[i].gameObject.SetActive(BulletStates[i].Enabled);
			bullets[i].transform.position = BulletStates[i].Position;
			bullets[i].GetComponent<MotionComponent>().Velocity = BulletStates[i].Velocity;
		}
		
		StepBasedComponent.Frame = Frame;
	}
}