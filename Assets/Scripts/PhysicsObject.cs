using UnityEngine;
using System.Collections;

public class PhysicsObject : MonoBehaviour{
	public string ignoredTag;
	void OnCollisionEnter(Collision other){
		if(other.gameObject.tag != ignoredTag){
			if(rigidbody.isKinematic){
				rigidbody.isKinematic = false;
			}
		}
	}
}
