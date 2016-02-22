using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {
	public float dropDelay = 1.0f;
	public float restDelay = 1.0f;
	public float holdDelay = 0.1f;
	public float gameWidth = 10;
	public float fallDamping = 0.7f;
	public Text scoreText;
	public Text lineText;
	public Text levelText;
	public Text startText;
	public GameObject stompPS;
	public GameObject[] blocks;
	public Text pauseText;
	public Text gameOverText;

	private List<List<GameObject>> blockComp;

	private GameObject currentBlock;
	private GameObject nextBlock;
	private int score = 0;
	private int lines = 0;
	private int level = 1;
	private bool gameOver = false;
	private bool started = false;

	// Use this for initialization
	void Start () {
		updateScore (0);
		blockComp = new List<List<GameObject>>();
	}

	void Update () {
		if (!started && Input.GetKeyDown (KeyCode.Space) ){
			startText.gameObject.SetActive(false);
			launchBlock();
		}
	}

	// Update is called once per frame
	public void launchBlock () {
		if (!gameOver) {
			if (currentBlock != null) {
				Transform[] blockChildren = currentBlock.GetComponentsInChildren<Transform> ();
				for (int i = 0; i < blockChildren.Length; i++) {
					if (blockChildren [i].CompareTag ("BlockComponent")) {
						int row = (int)Mathf.Round (blockChildren [i].position.y - 1.0f);
						int col = (int)Mathf.Round (blockChildren [i].position.z + 4.5f);

						if (row >= 20) {
							gameOver = true;
							gameOverText.gameObject.SetActive(true);
							return;
						}
						addToBlockComponents (row, col, blockChildren [i].gameObject);
					}
				}
				collapseRows ();
				currentBlock = nextBlock;
				currentBlock.transform.position = new Vector3 (0.0f, 21.5f, 0.0f);
			} else {
				currentBlock = (GameObject)Instantiate (blocks [Random.Range (0, blocks.Length)], new Vector3 (0.0f, 21.5f, 0.0f), Quaternion.identity);
			}

			nextBlock = (GameObject)Instantiate (blocks [Random.Range (0, blocks.Length)], new Vector3 (0.0f, -4.5f, 0.0f), Quaternion.identity);
			currentBlock.GetComponent<BlockController> ().go ();
		}
	}

	void addToBlockComponents(int row, int col, GameObject obj) {
		while (row >= blockComp.Count) {
			blockComp.Add (new List<GameObject>(){null,null,null,null,null,null,null,null,null,null});
		}

		blockComp [row] [col] = obj;
	}

	void collapseRows () {
		int toMove = 0;
		List<int> removedRows = new List<int>();
		for (int i = 0; i < blockComp.Count; i++) {
			int filled = 0;
			for (int j = 0; j < blockComp[i].Count; j++) {
				if (blockComp[i] [j] != null) {
					filled++;
				}
			}
			
			if (filled == gameWidth) {
				for (int k = 0; k < blockComp[i].Count; k++) {
					Destroy (blockComp[i] [k]);
				}
				toMove++;
				blockComp.RemoveAt(i);
				i--;
			} else {
				if (toMove > 0) {
					for (int l = 0; l < blockComp[i].Count; l++) {
						if (blockComp[i] [l] != null) {
							blockComp[i][l].GetComponent<BlockComponentController>().fall(blockComp[i] [l].transform.position + new Vector3(0.0f, -toMove, 0.0f));
						}
					}
				}
			}
			
			if (filled == 0) {
				break;
			}
		}

		updateScore (toMove);
	}

	void updateScore (int rows) {
		lines += rows;
		int newLevel = (int)Mathf.Clamp (Mathf.Floor (lines / 10.0f) + 1.0f, 1, 10);

		lineText.text = "Lines: " + lines;
		if (newLevel != level) {
			level = newLevel;
			dropDelay = 1.0f - (level - 1) * 0.1f;
		}
		levelText.text = "Level: " + level;

		score += (int) Mathf.Pow(2, rows - 1) * 10;
		scoreText.text = "Score: " + score;

	}

	public GameObject getBC(int row, int col) {
		if (row >= blockComp.Count || col >= blockComp [row].Count) {
			return null;
		}

		return blockComp [row] [col];
	}

	void prettyPrint() {
		for (int i = blockComp.Count - 1; i >= 0 ; i--) {
			string line = "";
			for (int j = 0; j < blockComp[i].Count; j++) {
				if (blockComp[i][j] != null) {
					line += "o ";
				} else {
					line += "  ";
				}
			}
			Debug.Log(line);
		}
		Debug.Log ("");
	}
}
