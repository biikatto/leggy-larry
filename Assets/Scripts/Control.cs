using UnityEngine;
// using System.Collections;
using System;

public class Control : MonoBehaviour{

	public float balancingForce = 2f;
	public float footForce = 20f;
	public float leaningForce = 5f;
	public float frictionForce = -1f;

	private float leftX;
	private float leftY;
	private float rightX;
	private float rightY;
	private float leftTrigger;
	private float rightTrigger;


	private Rigidbody leftLeg;
	private Rigidbody leftUpperLeg;
	private Rigidbody rightLeg;
	private Rigidbody rightUpperLeg;
	private Rigidbody head;
	private Rigidbody chest;
	private Rigidbody belly;

	private Foot leftFoot;
	private Foot rightFoot;

	void Start(){
		leftLeg = GameObject.Find("Character1_LeftLeg").rigidbody;
		leftUpperLeg = GameObject.Find("Character1_LeftUpLeg").rigidbody;
		rightLeg = GameObject.Find("Character1_RightLeg").rigidbody;
		rightUpperLeg = GameObject.Find("Character1_RightUpLeg").rigidbody;
		head = GameObject.Find("Character1_Neck1").rigidbody;
		chest = GameObject.Find("Character1_Spine1").rigidbody;
		belly = GameObject.Find("LeggyLarry_Root").rigidbody;

		leftFoot = leftLeg.GetComponent<Foot>();
		rightFoot = rightLeg.GetComponent<Foot>();
	}

	void FixedUpdate(){
		// Apply an upward balancing force for standstill balancing
		// (not so realistic but it will do for now)
		BalancingForce(balancingForce * Vector3.up);

		float rTrigger = Input.GetAxis("Right Trigger");
		float lTrigger = Input.GetAxis("Left Trigger");

		// Square our triggers for exponential response
		SetLegContraction(true, rTrigger * rTrigger);
		SetLegContraction(false, lTrigger * lTrigger);

		LeanLeft(rTrigger * leaningForce);
		LeanRight(lTrigger * leaningForce);

		//SetLegAngle(true, Input.GetAxis("Right X")+1f*0.5f);
		//SetLegAngle(false, Input.GetAxis("Left X")+1f*0.5f);

		//SetLegContraction(true, 1f-(Input.GetAxis("Right Y")+1f*0.5f));
		//SetLegContraction(false, 1f-(Input.GetAxis("Left Y")+1f*0.5f));

		FootForce(true, new Vector2(
					Input.GetAxis("Right X"),
					Input.GetAxis("Right Y")) * footForce);

		FootForce(false, new Vector2(
					Input.GetAxis("Left X"),
					Input.GetAxis("Left Y")) * footForce);

		//Debug.Log(rightLeg.hingeJoint.spring.targetPosition);
	}

	Vector2 LarryAngle(){
		return new Vector2(
			(head.transform.position-belly.transform.position).x,
			(head.transform.position-belly.transform.position).y
		);
	}

	void FootForce(bool left, Vector2 force){
		//  Apply a Vector2 force to a foot, but only if the
		// other foot is grounded. Only allow horizontal movement
		// toward the body if this foot is grounded.
		Foot thisFoot = leftFoot;
		Foot otherFoot = rightFoot;
		if(!left){
			thisFoot = rightFoot;
			otherFoot = leftFoot;
		}
		if(otherFoot.Grounded()){
			if(thisFoot.Grounded()){
				if(((force.x > 0) & !left) | ((force.x < 0) & left)){
					thisFoot.rigidbody.AddForce(new Vector3(0, force.y, 0));
				}else{
					thisFoot.rigidbody.AddForce(new Vector3(force.x, force.y, 0));
				}
			}else{
				thisFoot.rigidbody.AddForce(new Vector3(force.x, force.y, 0));
				otherFoot.rigidbody.AddForce(new Vector3(0, frictionForce, 0));
			}
		}else{
			thisFoot.rigidbody.AddForce(new Vector3(force.x * 0.1f, force.y * 0.1f, 0));
		}
	}

	void SetLegAngle(bool left, float rotation){
		Rigidbody leg = leftUpperLeg;
		float maxRotation = 90f;
		JointSpring spring = leg.hingeJoint.spring;
		if(!left){
			leg = rightUpperLeg;
			maxRotation *= -1f;
		}
		spring.targetPosition = rotation * maxRotation;
		leg.hingeJoint.spring = spring;
	}

	void SetLegContraction(bool left, float contraction){
		// TODO: fix this so the upper leg flexes at the same time,
		// that way the feet won't cross so easily
		Rigidbody leg = leftLeg;
		float maxContraction = -90f;
		float minContraction = -5f;
		JointSpring spring = leg.hingeJoint.spring;
		if(!left){
			leg = rightLeg;
			//maxContraction *= -1f;
		}
		spring.targetPosition = contraction * (maxContraction - minContraction) - minContraction;
		leg.hingeJoint.spring = spring;
	}

	void BalancingForce(Vector3 force, bool relative){
		// Apply force to the torso and head to provide balance
		if(relative){
			head.AddRelativeForce(0.5f * force);
			chest.AddRelativeForce(force);
			belly.AddRelativeForce(1.5f * force);
		}else{
			head.AddForce(0.5f * force);
			chest.AddForce(force);
			belly.AddForce(1.5f * force);
		}
	}

	void BalancingForce(Vector3 force){
		BalancingForce(force, false);
	}

	void LeanRight(float amount){
		BalancingForce(amount * Vector3.right, true);
	}

	void LeanLeft(float amount){
		BalancingForce(amount * Vector3.left, true);
	}

	// Debug variables

	float lastLeftX;
	float lastLeftY;
	float lastRightX;
	float lastRightY;
	float lastLeftTrigger;
	float lastRightTrigger;


	float threshold = 0.001f;

	void DebugPrint(){
		leftX = Input.GetAxis("Left X");
		leftY = Input.GetAxis("Left Y");
		rightX = Input.GetAxis("Right X");
		rightY = Input.GetAxis("Right Y");
		float trigger = Input.GetAxis("Triggers");

		if(trigger > 0){
			leftTrigger = trigger;
		}else if (trigger < 0){
			rightTrigger = Math.Abs(trigger);
		}else{
			leftTrigger = 0f;
			rightTrigger = 0f;
		}

		if(Math.Abs(leftX - lastLeftX) > threshold){
			Debug.Log("Left X: "+leftX);
			lastLeftX = leftX;
		}
		if(Math.Abs(leftY - lastLeftY) > threshold){
			Debug.Log("Left Y: " + leftY);
			lastLeftY = leftY;
		}
		if(Math.Abs(rightX - lastRightX) > threshold){
			Debug.Log("Right X: " + rightX);
			lastRightX = rightX;
		}
		if(Math.Abs(rightY - lastRightY) > threshold){
			Debug.Log("Right Y: " + rightY);
			lastRightY = rightY;
		}
		if(Math.Abs(leftTrigger - lastLeftTrigger) > threshold){
			Debug.Log("Left Trigger: " + leftTrigger);
			lastLeftTrigger = leftTrigger;
		}
		if(Math.Abs(rightTrigger - lastRightTrigger) > threshold){
			Debug.Log("Right Trigger: " + rightTrigger);
			lastRightTrigger = rightTrigger;
		}
	}
}
