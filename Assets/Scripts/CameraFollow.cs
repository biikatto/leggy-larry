using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour{
	public Transform followMe;
	public float dampTime = 0.15f;

	private Vector3 velocity = Vector3.zero;

	void Update () 
    {
        if (followMe)
        {
            Vector3 point = camera.WorldToViewportPoint(followMe.position);
            Vector3 delta = followMe.position - camera.ViewportToWorldPoint(
            		new Vector3(
            		0.5f,
            		0.5f,
            		point.z));
            Vector3 destination = transform.position + delta;
            transform.position = Vector3.SmoothDamp(
            		transform.position,
            		destination,
            		ref velocity,
            		dampTime);
        }

    }
}
