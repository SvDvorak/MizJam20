using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxShooter : MonoBehaviour
{
	public GameObject BoxTemplate;
	public GameObject Target;

    void Start()
    {
        
    }

    void Update()
    {
	    //if (Input.GetMouseButtonUp(0))
		   // Fire();
    }

    private void Fire()
    {
	    var box = Instantiate(BoxTemplate, transform);
	    var spherePos = Random.onUnitSphere;
        box.transform.position = Target.transform.position + new Vector3(spherePos.x, spherePos.y, 0) * 4;
        var rb = box.GetComponent<Rigidbody2D>();
        rb.AddForce(-spherePos * (1 - spherePos.y * 0.5f + 0.5f) * 10, ForceMode2D.Impulse);
    }
}
