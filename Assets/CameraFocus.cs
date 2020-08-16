using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFocus : MonoBehaviour
{
	public Transform Player;
	private Transform focusTransform;
	private Vector3 focusPosition;

	void Start()
    {
        
    }

    void Update()
    {
	    var target = focusTransform ? focusPosition : Player.position + new Vector3(-4, 0);

	    var toPlayer = target - transform.position;
	    toPlayer.z = 0;
	    transform.position += toPlayer * Time.deltaTime;
    }

    public void SetFocus(Transform focusTransform, Vector3 cameraPosition)
    {
	    this.focusTransform = focusTransform;
	    this.focusPosition = cameraPosition;
    }

    public void RemoveFocus(Transform focusedTransform)
    {
	    if (this.focusTransform == focusedTransform)
		    this.focusTransform = null;
    }
}
