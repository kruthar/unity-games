using UnityEngine;
using System.Collections;

public class Player : MovingObject {
	public int wallDamage;
	public int pointsPerFood;
	public int pointsPerSoda;
	public float restartLevelDelay = 1f;

	private Animator animator;
	private int food;

	// Use this for initialization
	protected override void Start () {
		animator = GetComponent<Animator> ();
		food = GameManager.instance.playerFoodPoints;
		base.Start ();
	}

	void onDisable() {
		GameManager.instance.playerFoodPoints = food;

	}
	
	// Update is called once per frame
	void Update () {
		if (!GameManager.instance.playersTurn) {
			return;
		}

		int horizontal = 0;
		int vertical = 0;

		horizontal = (int) Input.GetAxisRaw ("Horizontal");
		vertical = (int) Input.GetAxisRaw ("Vertical");

		if (horizontal != 0) {
			vertical = 0;
		}

		if (horizontal != 0 || vertical != 0) {
			attemptMove<Wall> (horizontal, vertical);
		}
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.CompareTag ("Exit")) {
			Invoke ("restart", restartLevelDelay);
		} else if (other.CompareTag ("Food")) {
			food += pointsPerFood;
			other.gameObject.SetActive (false);
		} else if (other.CompareTag ("Soda")) {
			food += pointsPerSoda;
			other.gameObject.SetActive (false);
		}
	}

	protected override void onCantMove <T> (T component) {
		Wall hitWall = component as Wall;
		hitWall.damageWall (wallDamage);
		animator.SetTrigger ("playerChop");
	}

	void restart() {
		Application.LoadLevel (Application.loadedLevel);
	}

	public void loseFood(int loss) {
		animator.SetTrigger ("playerHit");
		food -= loss;
		checkIfGameOver ();
	}

	protected override void attemptMove <T> (int xDir, int yDir) {
		food--;
		base.attemptMove<T> (xDir, yDir);

		RaycastHit2D hit;
		checkIfGameOver ();
		GameManager.instance.playersTurn = false;
	}

	void checkIfGameOver () {
		if (food <= 0) {
			GameManager.instance.GameOver();
		}
	}
}
