using UnityEngine;
using System.Collections;

public abstract class MovingObject : MonoBehaviour {
	public float moveTime;
	public LayerMask blockingLayer;

	private BoxCollider2D boxCollider;
	private Rigidbody2D rb2d;
	private float inverseMoveTime;

	protected virtual void Start () {
		boxCollider = GetComponent<BoxCollider2D> ();
		rb2d = GetComponent<Rigidbody2D> ();
		inverseMoveTime = 1 / moveTime;
	}

	protected bool move(int xDir, int yDir, out RaycastHit2D hit) {
		Vector2 start = transform.position;
		Vector2 end = start + new Vector2 (xDir, yDir);

		boxCollider.enabled = false;
		hit = Physics2D.Raycast (start, end, blockingLayer);
		boxCollider.enabled = true;

		if (hit.transform == null) {
			StartCoroutine(smoothMovement (end));
			return true;
		} else {
			return false;
		}

	}

	
	protected IEnumerator smoothMovement (Vector3 end) {
		float sqrRemainingDistance = (transform.position - end).sqrMagnitude;
		while (sqrRemainingDistance > float.Epsilon) {
			Vector3 newPosition = Vector3.MoveTowards(rb2d.position, end, inverseMoveTime * Time.deltaTime);
			rb2d.MovePosition(newPosition);
			sqrRemainingDistance = (transform.position - end).sqrMagnitude;
			yield return null;
		}
	}
	protected virtual void attemptMove <T> (int xDir, int yDir) 
		where T: Component {
		RaycastHit2D hit;
		bool canMove = move (xDir, yDir, out hit);

		if (hit.transform == null) {
			return;
		}

		T hitComponent = hit.transform.GetComponent<T> ();

		if (!canMove && hitComponent != null) {
			onCantMove(hitComponent);
		}
		 
	}
	
	protected abstract void onCantMove<T> (T component)
		where T : Component;

}
