using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour {

    public Transform target;
    public Vector3 offset;
    public float rotateSpeed;
    public float smooth;

    Vector3 targetPosition;
    Vector3 lookDir;

    Vector3 velocityCamSmooth = Vector3.zero;
    float camSmoothDampTime = 0.1f;
	
	// Update is called once per frame
	void LateUpdate ()
    {
        Vector3 characterOffset = target.position + new Vector3(0.0f, 1.5f);

        lookDir = characterOffset - this.transform.position;
        lookDir.y = 0;
        lookDir.Normalize();
        Debug.DrawRay(this.transform.position, lookDir, Color.green);

        targetPosition = characterOffset + target.up * offset.y - lookDir * offset.z;
        Debug.DrawRay(target.position, Vector3.up * offset.y, Color.red);
        Debug.DrawRay(target.position, -1f * target.forward * offset.z, Color.blue);
        Debug.DrawLine(target.position, targetPosition, Color.magenta);

        //transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smooth);
        smoothPosition(this.transform.position, targetPosition);
        transform.LookAt(target);
        //transform.Rotate(target.up * (Input.GetAxis("Rotate") * rotateSpeed), Space.World);
        //transform.position = target.position + offset;
	}

    void smoothPosition(Vector3 fromPos, Vector3 toPos)
    {
        this.transform.position = Vector3.SmoothDamp(fromPos, toPos, ref velocityCamSmooth, camSmoothDampTime);
    }
}
