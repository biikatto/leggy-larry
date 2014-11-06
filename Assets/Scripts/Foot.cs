using UnityEngine;
using System.Collections;

public class Foot : MonoBehaviour{
	// Keeps track of whether a foot is grounded or not

	private bool grounded;
	int objectsTouching;

	public bool Grounded(){
		return grounded;
	}

	void OnCollisionEnter(Collision collision){
		// Always ignore collisions with parts of Larry
		if(collision.gameObject.tag != "Larry"){
			objectsTouching++;
			grounded = true;
		}
	}

	void OnCollisionExit(Collision collision){
		// Always ignore collisions with parts of Larry
		if(collision.gameObject.tag != "Larry"){
			objectsTouching--;
			if(objectsTouching <= 0){
				grounded = false;
				objectsTouching = 0;
			}
		}
	}
}
