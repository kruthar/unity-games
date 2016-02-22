using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BlockController : MonoBehaviour {
	// Delay in seconds between each drop of the blocks position
	public float blockWidth;

	// Timer to keep track of time elapsed between drops
	private Vector3 dropVector = new Vector3(0.0f, -1.0f, 0.0f);
	private float leftBound, rightBound;
	private BlockComponentController[] childBlocks;
	private bool isResting = false;
	private float dropTimer, restingTimer, leftTimer, rightTimer;
	private GameController gc;
	private float dropDelay, restDelay, holdDelay;
	private float gameWidth;
	private bool going = false;
	private bool paused = false;
	private Text pauseText;

	private Vector3[] transformVectors = new Vector3[4]{
		new Vector3(0.0f, 1.0f, 0.0f),
		new Vector3(0.0f, 0.0f, 1.0f),
		new Vector3(0.0f, -1.0f, 0.0f),
		new Vector3(0.0f, 0.0f, -1.0f)
	};
	
	
	void Start () {
		gc = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
		dropDelay = gc.dropDelay;
		restDelay = gc.restDelay;
		holdDelay = gc.holdDelay;
		gameWidth = gc.gameWidth;
		leftBound = -gameWidth / 2;
		rightBound = gameWidth / 2 - blockWidth;
		childBlocks = GetComponentsInChildren<BlockComponentController> ();
		pauseText = gc.pauseText;
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.P)) {
			Debug.Log("pause");
			if (paused) {
				pauseText.gameObject.SetActive(false);
			} else {
				pauseText.gameObject.SetActive(true);
			}
			paused = !paused;
		}

		if (Input.GetKeyDown (KeyCode.R)) {
			Application.LoadLevel(Application.loadedLevel);
		}

		if (going && !paused) {
			bool left = Input.GetKey (KeyCode.LeftArrow);
			bool leftDown = Input.GetKeyDown (KeyCode.LeftArrow);
			bool right = Input.GetKey (KeyCode.RightArrow);
			bool rightDown = Input.GetKeyDown (KeyCode.RightArrow);
			bool down = Input.GetKey (KeyCode.DownArrow);
			bool up = Input.GetKeyDown (KeyCode.UpArrow);

			// Handle dropping if we have accumulated enough delay, otherwise increment timer
			if (!restingBlocks ()) {
				isResting = false;

				if (dropTimer >= dropDelay || down) {
					transform.position += dropVector;
					dropTimer = 0.0f;
				}

				dropTimer += Time.deltaTime;
			} else {
				isResting = true;
				restingTimer += Time.deltaTime;
			}

			// Test if we are resting on the floor or another block and have past the rest delay, if so, this piece is done
			if (isResting && restingTimer >= restDelay) {
				this.enabled = false;
				gc.launchBlock ();
			}

			float newZ = transform.position.z;

			if (left) {
				leftTimer += Time.deltaTime;
				if ((leftDown || leftTimer >= holdDelay) && !right && !leftTouchingBlocks ()) {
					leftTimer = Time.deltaTime;
					newZ -= 1;
				}
			}

			if (right) {
				rightTimer += Time.deltaTime;
				if ((rightDown || rightTimer >= holdDelay) && !left & !rightTouchingBlocks ()) {
					rightTimer = Time.deltaTime;
					newZ += 1;
				}
			}

			transform.position = new Vector3 (transform.position.x, transform.position.y, newZ);

			if (up) {
				Vector3 oldPos = transform.position;
				Quaternion oldRot = transform.rotation;

				transform.RotateAround (transform.position, new Vector3 (1.0f, 0.0f, 0.0f), 90);
				if (isColliding ()) {
					bool foundAlt = false;
					// try other transforms until we are not colliding
					for (int i = 0; i < transformVectors.Length; i++) {
						transform.position += transformVectors [i];
						if (isColliding ()) {
							transform.position += transformVectors [i] * -1;
						} else {
							foundAlt = true;
							break;
						}
					}

					if (!foundAlt) {
						transform.position = oldPos;
						transform.rotation = oldRot;
					}
				} 
			}
		}	
	}
	
	public void go() {
		going = true;
	}

	bool restingBlocks () {
		for (int i = 0; i < childBlocks.Length; i++) {
			if (childBlocks[i].resting) {
				return true;
			}
		}
		return false;
	}

	bool leftTouchingBlocks () {
		for (int i = 0; i < childBlocks.Length; i++) {
			if (childBlocks[i].leftTouching) {
				return true;
			}
		}
		return false;
	}

	bool rightTouchingBlocks () {
		for (int i = 0; i < childBlocks.Length; i++) {
			if (childBlocks[i].rightTouching) {
				return true;
			}
		}
		return false;
	}

	bool isColliding () {
		for (int i = 0; i < childBlocks.Length; i++) {
			if (childBlocks[i].isColliding()) {
				return true;
			}
		}
		return false;
	}
}
