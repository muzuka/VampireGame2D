using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMover : MonoBehaviour {

    public Transform cameraTransform;
    public float speed;
    public float maxSpeed;
    public float turnSpeed;

    float h = 0f;
    float v = 0f;
    float direction = 0f;

    Rigidbody rigidBody;

	// Use this for initialization
	void Start ()
    {
        rigidBody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");


        if ((h > 0.0f || h < 0.0f) || (v > 0.0f || v < 0.0f))
            transform.rotation = Quaternion.LookRotation(new Vector3(h, 0.0f, v));

        //float speed = new Vector2(h, v).magnitude;

        //rigidBody.AddForce(Input.GetAxis("Horizontal") * speed, 0.0f, 0.0f);
        //if(rigidBody.velocity.magnitude <= maxSpeed)
        //    rigidBody.AddForce(h * speed, 0.0f, v * speed);

        StickToWorldspace(this.transform, cameraTransform, ref direction, ref speed);
        speed *= 10;
        rigidBody.velocity = new Vector3(h * speed, 0.0f, v * speed);
    }

    public void StickToWorldspace(Transform root, Transform camera, ref float directionOut, ref float speedOut)
    {
        Vector3 rootDirection = root.forward;

        Vector3 stickDirection = new Vector3(h, 0.0f, v);

        speedOut = stickDirection.sqrMagnitude;

        Vector3 cameraDirection = camera.forward;
        cameraDirection.y = 0.0f;
        Quaternion referentialShift = Quaternion.FromToRotation(Vector3.forward, cameraDirection);

        Vector3 moveDirection = referentialShift * stickDirection;
        Vector3 axisSign = Vector3.Cross(moveDirection, rootDirection);

        Debug.DrawRay(new Vector3(root.position.x, root.position.y + 2.0f, root.position.z), moveDirection, Color.green);
        Debug.DrawRay(new Vector3(root.position.x, root.position.y + 2.0f, root.position.z), rootDirection, Color.green);
        Debug.DrawRay(new Vector3(root.position.x, root.position.y + 2.0f, root.position.z), stickDirection, Color.green);

        float angleRootToMove = Vector3.Angle(rootDirection, moveDirection) * (axisSign.y >= 0 ? -1f : 1f);

        angleRootToMove /= 180f;

        directionOut = angleRootToMove * turnSpeed;

    }
}
