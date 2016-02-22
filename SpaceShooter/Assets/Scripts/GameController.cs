using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	public GameObject hazard;
	public Vector3 spawnValue;
	public int hazardCount;
	public float spawnWait;
	public float startWait;
	public float waveWait;
	public GUIText scoreText;
	public GUIText restartText;
	public GUIText gameoverText;


	private bool gameover;
	private bool restart;
	private int score;

	IEnumerator spawnWaves () {
		yield return new WaitForSeconds (startWait);
		while (true) {
			for (int i = 0; i < hazardCount; i++) {
				Vector3 spawnPosition = new Vector3 (Random.Range (-spawnValue.x, spawnValue.x), spawnValue.y, spawnValue.z);
				Quaternion spawnRotation = Quaternion.identity;
				Instantiate (hazard, spawnPosition, spawnRotation);
				yield return new WaitForSeconds (spawnWait);
			}
			yield return new WaitForSeconds (waveWait);

			if (gameover) {
				restartText.text = "Press R for Restart";
				restart = true;
				break;
			}
		}
	}

	void Update() {
		if (restart) {
			if (Input.GetKeyDown (KeyCode.R)) {
				Application.LoadLevel(Application.loadedLevel);
			}

		}
	}

	void Start () {
		StartCoroutine (spawnWaves ());
		score = 0;
		gameover = false;
		restart = false;
		restartText.text = "";
		gameoverText.text = "";
		updateScore ();
	}

	void updateScore () {
		scoreText.text = "Score: " + score;
	}

	public void addScore(int newScoreValue) {
		score += newScoreValue;
		updateScore ();
	}

	public void gameOver() {
		gameover = true;
		gameoverText.text = "Game Over";
	}
}
