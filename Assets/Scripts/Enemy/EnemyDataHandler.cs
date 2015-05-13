using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyValues {
	public EnemyValues(int ID) {
		this.ID = ID;
		StoredVals = new Dictionary<string, GeneratedValue> ();
	}
	public int ID;
	public Dictionary<string, GeneratedValue> StoredVals;
}

public struct GeneratedValue {
	public GeneratedValue(float value, bool discrete) {
		Value = value;
		Discrete = discrete;
	}

	public float Value;
	public bool Discrete;
}

public class EnemyDataHandler : MonoBehaviour {
	public static List<EnemyValues> EnemyData;
	public int EnemyCount = 10;

	public static int CurrentID = 0;

	private static bool exists = false;
	private TextMesh text;
	private static int longestRun = 0, bestCandidate = 0;

	void Initialize() {
		EnemyData = new List<EnemyValues>();
		for (int i = 0; i < EnemyCount; ++i) 
			EnemyData.Add(new EnemyValues(i));
	}

	void Awake () {
		if (!exists) {

			exists = true;
			DontDestroyOnLoad(gameObject);
			Initialize();
			OnLevelWasLoaded(0);
			Evaluator.OnSucceed += EvalSuccess;
			Evaluator.OnFail += EvalFailure;
		}
		else
			Destroy (gameObject);
	}

	void OnLevelWasLoaded(int level) {
		UnityEngine.Random.seed = (int) System.DateTime.Now.Ticks;

		if (text == null)
			text = GetComponent<TextMesh>();

		switch (level) {
		case 0:
			text.text = "Simulating randomly-controlled player against\ncandidate "
				+ EnemyData[CurrentID].ID + 
				"\nCulled: " + (EnemyCount - EnemyData.Count);
			break;
		case 1:
			text.text = "Testing beatability of\ncandidate "
				+ EnemyData[CurrentID].ID + 
				"\nCulled: " + (EnemyCount - EnemyData.Count);
			break;
		case 2:
			text.text = "Player against candidate " + EnemyData[CurrentID].ID +
				"\nMove: WAD    Shoot: Space\nReset fight: R   Generate new enemies: G\nToggle trails: T   Quit: Esc";
			break;
		}
	}

	void EvalSuccess(Evaluator sender, int data) {

		if (sender.GetType() == typeof(RandomEvaluator)) 
			Debug.Log ("Candidate " + EnemyData[CurrentID].ID + " survived the random player.");
		if (sender.GetType() == typeof(BeatableEvaluator)) {
			Debug.Log ("Candidate " + EnemyData[CurrentID].ID + " was defeated in " + data + " cycles.");
			if (data > longestRun) {
				longestRun = data;
				bestCandidate = CurrentID;
			}
		}
		++CurrentID;
		
		Progress (sender, data);
	}

	void EvalFailure(Evaluator sender, int data) {
		
		if (sender.GetType() == typeof(RandomEvaluator)) 
			Debug.Log ("Candidate " + EnemyData[CurrentID].ID + " was beaten by the random player.");
		if (sender.GetType () == typeof(BeatableEvaluator))
			Debug.Log ("Candidate " + EnemyData [CurrentID].ID + " was not defeated in " + data + " cycles.");

		Debug.Log ("Culling candidate " + EnemyData [CurrentID].ID + ".");

		EnemyData.RemoveAt (CurrentID);

		Progress (sender, data);

	}

	void Progress(Evaluator sender, int data) {
		if (CurrentID == EnemyData.Count) {
			if (EnemyData.Count > 0) 
				GoToNextScene (sender, data);
			else {
				Debug.Log ("No candidates remaining. Restarting search.");
				Initialize();
				Application.LoadLevel ("randomplayertest");
			}
		}
		else 
			Application.LoadLevel (Application.loadedLevel);
	}

	void GoToNextScene(Evaluator sender, int data) {
		if (sender.GetType() == typeof(RandomEvaluator)) {
			CurrentID = 0;
			string output = "Candidates ";
			foreach (EnemyValues e in EnemyData)
				output += e.ID + " ";
			output += " pass the first test.";
			Debug.Log (output);
			Application.LoadLevel("beatabletest");
		}
		if (sender.GetType() == typeof(BeatableEvaluator)) {
			Debug.Log("Candidate " + bestCandidate + " selected as best.");
			CurrentID = bestCandidate;
			Application.LoadLevel("main");
		}
	}

	static public float GetValue(string valueName, bool discrete) {
		if (EnemyData[CurrentID].StoredVals.ContainsKey(valueName))
			return EnemyData[CurrentID].StoredVals[valueName].Value;
		else {
			float val = Random.Range (0f, 1f);
			EnemyData[CurrentID].StoredVals.Add(valueName, new GeneratedValue(val, discrete));
			return val;
		}

	}

	void Update() {
		if (Input.GetKeyDown (KeyCode.G)) {
			Initialize();
			Application.LoadLevel ("randomplayertest");
		}

		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit();
		}
	}
}