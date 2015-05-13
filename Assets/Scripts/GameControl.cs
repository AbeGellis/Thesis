using UnityEngine;
using System.Collections.Generic;


public class GameControl : MonoBehaviour {
	void Start() {
		StepBasedComponent.Frame = 0;
		Time.timeScale = 1f;
	}

	void Update() {
		if (Input.GetKeyDown (KeyCode.R))
			Application.LoadLevel (Application.loadedLevel);

	}

	void FixedUpdate() {
		//if (!Input.GetKey (KeyCode.Tab)) {
		//	g.AddLast(new GameState(Hero, Enemy, Bullets, g.Count > 0 ? g.Last.Value : null));
			StepBasedComponent.GameStep ();
		/*}
		else {
			if (g.Count > 0) {
				g.Last.Value.RestoreGameState(Hero, Enemy, Bullets);
				g.RemoveLast();
			}
		}*/

	}
	
	public Player Hero, Enemy; 
	public Bullet[] Bullets;
	//public LinkedList<GameState> g = new LinkedList<GameState>();
}