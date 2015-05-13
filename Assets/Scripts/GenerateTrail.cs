using UnityEngine;
using System.Collections.Generic;

public class GenerateTrail : StepBasedComponent {
	public int length;
	public GameObject trailPrefab;

	private struct TrailState {
		public TrailState(Vector2 position, Color color) {
			this.position = position;
			this.color = color;
		}
		public Vector2 position;
		public Color color;
	}

	private static GameObject trailHolder;

	private LinkedList<TrailState> states;
	private Transform[] trailPositions;
	private SpriteRenderer[] trailSprites;

	private static Vector2 OFFSCREEN = new Vector2(-1000f,-1000f);
	private bool on = true;
	private SpriteRenderer sr;

	void Awake () {
		if (trailHolder == null)
			trailHolder = new GameObject ("Trails");

		states = new LinkedList<TrailState>();
		trailPositions = new Transform[length];
		trailSprites = new SpriteRenderer[length];
		sr = GetComponent<SpriteRenderer> ();
		for (int i = 0; i < length; ++i) {
			var g = (GameObject)Instantiate (trailPrefab, OFFSCREEN, Quaternion.identity);
			g.transform.localScale = transform.lossyScale;
			g.transform.parent = trailHolder.transform;
			trailPositions [i] = g.transform;
			trailSprites[i] = g.GetComponent<SpriteRenderer>();
			trailSprites[i].sortingOrder = sr.sortingOrder - (i + 1);
		}
	}

	void Update() {
		if (Input.GetKeyDown (KeyCode.T)) {
			on = !on;
			if (!on) {
				foreach (Transform t in trailPositions)
					t.position = OFFSCREEN;
			}
		}
	}

	override public void OnDisable() {
		base.OnDisable ();
		foreach (Transform t in trailPositions)
			if (t != null) t.position = OFFSCREEN;
		states.Clear ();
	}

	override public void OnEnable() {
		base.OnEnable ();
	}

	override public void BeginStep() {
		states.AddFirst (new TrailState(transform.position,sr.color));
		if (states.Count > length) 
			states.RemoveLast ();

		if (on) {
			int i = 0;
			foreach (TrailState t in states) {
				Color c = t.color;
				trailSprites [i].color = new Color (c.r, c.g, c.b, Mathf.Lerp (0, .3f, (float)(length - i) / length));
				trailPositions [i].position = t.position;
				++i;
			}
		}
	}
}
