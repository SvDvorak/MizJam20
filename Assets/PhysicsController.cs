using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsController : MonoBehaviour
{
	private Rigidbody2D rb;
	private Vector3 localRestPos;
	private float restRot;

	void Start()
    {
	    localRestPos = transform.position - transform.parent.position;
	    restRot = GetRot(transform) - GetRot(transform.parent);
	    rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
	    var targetPos = transform.parent.position + localRestPos;
	    var toRestPos = targetPos - transform.position;

	    var targetRot = GetRot(transform.parent) + restRot;
		var toRestRot = targetRot - GetRot(transform);

		rb.AddForce(toRestPos * rb.mass * 20 / Time.deltaTime);
		rb.AddTorque(toRestRot * Time.deltaTime);
	}

    private float GetRot(Transform t) => t.rotation.eulerAngles.z;
}
