using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamDepth : MonoBehaviour {
	public float depth;
	GameObject player;
	Vector3 lastPlayerPosition;

	// Use this for initialization
	void Start () {
		player = GameObject.FindWithTag ("Player");
		lastPlayerPosition = new Vector3 ();
		lastPlayerPosition = player.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 deltaPos = new Vector3 ();
		deltaPos = lastPlayerPosition - player.transform.position;
		deltaPos.x = deltaPos.x / (1 - depth);
		deltaPos.y = 0;
		deltaPos.z = 0;

		this.transform.position -= deltaPos;
		lastPlayerPosition = player.transform.position;
	}
}
