using UnityEngine;
using System.Collections;

public class BlockComponentController : MonoBehaviour {
	public bool resting = false;
	public bool leftTouching = false;
	public bool rightTouching = false;

	private GameController gc;
	private float gameWidth;
	private float fallDamping;
	private Vector3 fallTo = new Vector3();
	private bool falling = false;
	private GameObject stompPS;



	void Start() {
		gc = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
		gameWidth = gc.gameWidth;
		fallDamping = gc.fallDamping;
		stompPS = gc.stompPS;
	}
	// Update is called once per frame
	void Update () {
		resting = rayTest (new Vector3 (0.0f, -1.0f, 0.0f), 1.0f, "Floor");
		leftTouching = rayTest (new Vector3 (0.0f, 0.0f, -1.0f), 1.0f, "LeftWall");
		rightTouching = rayTest (new Vector3 (0.0f, 0.0f, 1.0f), 1.0f, "RightWall");

		if (falling) {
			transform.position = Vector3.Lerp (transform.position, fallTo, fallDamping);
			if (transform.position == fallTo) {
				Debug.Log("done falling");
				falling = false;
				fallTo = new Vector3();

				if (rayTestFloorOnly (new Vector3(0.0f, -1.0f, 0.0f), 1.0f, "Floor")) {
					GameObject tempPS = (GameObject) Instantiate (stompPS, transform.position + new Vector3(0.0f, -.5f, 0.0f), Quaternion.Euler(90, 0, 0));
					Destroy (tempPS, 1.0f);
				}
			}
		}
	}

	bool rayTestFloorOnly (Vector3 direction, float distance, string playAreaTag) {
		RaycastHit hit;
		
		if (Physics.Raycast (transform.position, direction, out hit, distance)) {
			GameObject hitObject = hit.collider.gameObject;
			GameObject hitParent = hitObject.transform.parent.gameObject;
			
			if (hitObject.CompareTag (playAreaTag)) {
				return true;
			} 
		}
		
		return false;
	}

	bool rayTest (Vector3 direction, float distance, string playAreaTag) {
		RaycastHit hit;
		
		if (Physics.Raycast (transform.position, direction, out hit, distance)) {
			GameObject hitObject = hit.collider.gameObject;
			GameObject hitParent = hitObject.transform.parent.gameObject;
			
			if (hitObject.CompareTag (playAreaTag) || (hitObject.CompareTag("BlockComponent") && hitParent != transform.parent.gameObject)) {
				return true;
			} 
		}

		return false;
	}

	public void fall(Vector3 destination) {
		if (!falling) {
			Debug.Log("set falling");
			falling = true;
			fallTo = destination;
		}
	}

	public bool isColliding() {
		int zPos = (int) Mathf.Round (transform.position.z + 4.5f);
		int yPos = (int) Mathf.Round (transform.position.y - 1.0f);

		if (zPos < 0 || zPos >= gameWidth || yPos < 0) {
			return true;
		}

		GameObject existing = gc.getBC(yPos, zPos);
		if (existing != null) {
			return true;
		}
		
		return false;
	}
}
