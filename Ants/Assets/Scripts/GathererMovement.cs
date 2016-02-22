using UnityEngine;
using System.Collections;

public class GathererMovement : MonoBehaviour {
	public GameObject target;
	public GameObject mound;
	public float speed;

	private LayerMask floorMask;
	private AntResourceController arc;
	// Use this for initialization
	void Start () {
		arc = gameObject.GetComponent<AntResourceController> ();
		floorMask = LayerMask.GetMask ("Floor");
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log (transform.forward);
		if (arc.returning) {
			transform.LookAt (mound.transform.position);
		} else {
			transform.LookAt (target.transform.position);
		}

//		Vector3 rotationEulers = transform.rotation.eulerAngles;
//		rotationEulers.y = 0;
//		transform.Rotate (rotationEulers);

		Vector3 forward = transform.forward;
		forward.y = 0;
		transform.position += forward * speed * Time.deltaTime;

		RaycastHit groundHit;
		
		if (Physics.Raycast (transform.position, Vector3.down, out groundHit, 10, floorMask)) {
			transform.rotation = Quaternion.LookRotation (Vector3.Cross (transform.right, groundHit.normal));
		} else {
			Debug.Log ("Are we flying?");
		}
	}	
}
