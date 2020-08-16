using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusArea : MonoBehaviour
{
	public Transform CameraPositionOnFocus;
	private CameraFocus gameCamera;

	private void Start()
	{
		gameCamera = Camera.main.GetComponent<CameraFocus>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.attachedRigidbody.tag == "Player")
			gameCamera.SetFocus(transform, CameraPositionOnFocus.position);
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.attachedRigidbody.tag == "Player")
			gameCamera.RemoveFocus(transform);
	}
}
