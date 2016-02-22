 using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public float speed = 6.0f;

	private Vector3 movement;
	private Animator anim;
	private Rigidbody rb;
	private int floorMask;
	private float camRayLength = 100;

	void Awake () {
		floorMask = LayerMask.GetMask ("Floor");
		anim = GetComponent<Animator> ();
		rb = GetComponent<Rigidbody> ();
	}

	void FixedUpdate () {
		float h = Input.GetAxisRaw ("Horizontal");
		float v = Input.GetAxisRaw ("Vertical");

		move (h, v);
		turning ();
		animating (h, v);
	}

	void move (float h, float v) {
		movement.Set (h, 0.0f, v);
		movement = movement.normalized * speed * Time.deltaTime;

		rb.MovePosition (transform.position + movement);
	}

	void turning () {
		Ray cameray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit floorHit;

		if (Physics.Raycast (cameray, out floorHit, camRayLength, floorMask)) {
			Vector3 playerToMouse = floorHit.point - transform.position;
			playerToMouse.y = 0;

			Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
			rb.MoveRotation (newRotation);
		}
	}

	void animating (float h, float v) {
		bool walking = h != 0.0f || v != 0.0f;
		anim.SetBool ("IsWalking", walking);
	}
}
    