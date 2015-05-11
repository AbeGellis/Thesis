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
		EnemyStateState = (EnemyState) e.CurrentState.Clone ();
		ToReplace = e.CurrentState;

		for (int i = 0; i < bullets.Length; ++i) {
			BulletStates[i] = new BulletState();
			BulletStates[i].Enabled = bullets[i].gameObject.activeSelf;
			BulletStates[i].Position = bullets[i].transform.position;
			BulletStates[i].Velocity = bullets[i].GetComponent<MotionComponent>().Velocity;
		}

		ancestor = currentState;
	}

	public CharacterState EnemyState, PlayerState;
	public BulletState[] BulletStates;
	public EnemyState EnemyStateState, //Cloned to preserve current data
			ToReplace; //A reference to the state to override with the clone
	public int Frame;

	public GameState ancestor;
	public List<GameState> descendants = new List<GameState>();

	public void RestoreGameState(Player player, Player enemy, Bullet[] bullets) {
		player.CopyFrom(PlayerState.Character);
		player.GetComponent<MotionComponent> ().Velocity = PlayerState.Velocity;
		player.transform.position = PlayerState.Position;

		var e = (EnemyPlayer) enemy;
		e.CopyFrom(EnemyState.Character);
		e.GetComponent<MotionComponent> ().Velocity = EnemyState.Velocity;
		e.transform.position = EnemyState.Position;

		for (int i = 0; i < e.States.Length; ++i) {
			if (e.States[i] == ToReplace) {
				e.States[i] = EnemyStateState;
				e.CurrentState = EnemyStateState;
				e.GetComponent<SpriteRenderer>().color = Utils.HSVToRGB (EnemyStateState.StateHue, .75f, 1f);
				break;
			}
		}

		for (int i = 0; i < bullets.Length; ++i) {
			bullets[i].gameObject.SetActive(BulletStates[i].Enabled);
			bullets[i].transform.position = BulletStates[i].Position;
			bullets[i].GetComponent<MotionComponent>().Velocity = BulletStates[i].Velocity;
		}
	}
}

public class GameControl : MonoBehaviour {
	void Start() {
		StepBasedComponent.Frame = 0;
	}

	void Update() {
		if (Input.GetKeyDown (KeyCode.LeftControl))
			Application.LoadLevel (Application.loadedLevel);

	}

	void FixedUpdate() {
		if (!Input.GetKey (KeyCode.Tab)) {
			StepBasedComponent.GameStep ();
			g.AddLast(new GameState(Hero, Enemy, Bullets, g.Count > 0 ? g.Last.Value : null));
		}
		else {
			if (g.Count > 0) {
				g.Last.Value.RestoreGameState(Hero, Enemy, Bullets);
				g.RemoveLast();
				StepBasedComponent.Frame--;
			}
		}

	}
	
	public Player Hero, Enemy; 
	public Bullet[] Bullets;
	public LinkedList<GameState> g = new LinkedList<GameState>();
}