using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shipMovement : MonoBehaviour {
	public float speed;
	private Rigidbody2D rb;

	void Start () {
		rb = GetComponent<Rigidbody2D> ();
	}

	void Update () {
		float horizontal = rb.velocity.x;
		float vertical = rb.velocity.y;

		horizontal = Input.GetAxis ("Horizontal");
		vertical = Input.GetAxis ("Vertical");
		rb.velocity = new Vector3 (horizontal, vertical, 0) * speed;
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		Debug.Log ("Boom!!!");
	}
}
