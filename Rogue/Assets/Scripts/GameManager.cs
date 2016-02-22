using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	public static GameManager instance = null;
	public BoardManager boardScript;
	public int level = 3;
	public int playerFoodPoints = 100;
	[HideInInspector] public bool playersTurn = true;
	public float turnDelay = .1f;

	private List<Enemy> enemies;
	private bool enemiesMoving;

	// Use this for initialization
	void Awake () {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}

		DontDestroyOnLoad (gameObject);
		enemies = new List<Enemy> ();
		boardScript = GetComponent<BoardManager> ();
		initGame ();
	}

	void initGame () {
		enemies.Clear ();
		boardScript.setupScene (level);
	}

	public void GameOver() {
		enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (playersTurn || enemiesMoving) {
			return;
		} else {
			StartCoroutine(moveEnemies());
		}
	}

	public void addEnemyToList(Enemy script) {
		enemies.Add (script);
	}

	IEnumerator moveEnemies() {
		enemiesMoving = true;
		yield return new WaitForSeconds(turnDelay);
		if (enemies.Count == 0) {
			yield return new WaitForSeconds(turnDelay);
		}

		for (int i = 0; i < enemies.Count; i++) {
			enemies[i].moveEnemy();
			yield return new WaitForSeconds(enemies[i].moveTime);
		}

		playersTurn = true;
		enemiesMoving = false;
	}
}
