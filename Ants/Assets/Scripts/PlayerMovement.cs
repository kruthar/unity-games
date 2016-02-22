using UnityEngine;
using System.Collections;

public class PlayerMovement: MonoBehaviour {
	public float moveSpeed;
	public float turnSpeed;
	
	private LayerMask floorMask;
	
	// Use this for initialization
	void Start () {
		floorMask = LayerMask.GetMask ("Floor");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float horizontal = Input.GetAxisRaw ("Horizontal");
		float vertical = Input.GetAxisRaw ("Vertical");

		if (horizontal != 0f || vertical != 0f) {
			transform.RotateAround (transform.position, transform.up, horizontal * turnSpeed * Time.deltaTime);

			Vector3 forward = transform.forward;
			forward.y = 0;
			transform.position += forward * vertical * moveSpeed * Time.deltaTime;
			
			RaycastHit groundHit;
			
			if (Physics.Raycast (transform.position, Vector3.down, out groundHit, 10, floorMask)) {
				transform.rotation = Quaternion.LookRotation (Vector3.Cross (transform.right, groundHit.normal));
			} else {
				Debug.Log ("Are we flying?");
			}
		}
	}	
}
