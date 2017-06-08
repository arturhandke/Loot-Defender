using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMovement : MonoBehaviour {

	private GameObject player;

	void Start () {

		player = GameObject.FindWithTag ("Player");
		//this.transform.position = player.transform.position;

	}
	

	void Update () {
		Vector3 pos = new Vector3 ();

		pos.x = player.transform.position.x;
		pos.y = this.transform.position.y;
		pos.z = this.transform.position.z;
		this.transform.position = pos;
		
	}
}
