using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

//[ExecuteInEditMode]
public class CircleFade : MonoBehaviour
{
	private float deathPauseMinSize = 0.1f;
	private Material material;
	private Camera gameCamera;

	public void Start()
	{
		material = GetComponent<Image>().material;
		gameCamera = Camera.main;
	}

	public void Update()
	{
		material.SetFloat("_CircleSize", deathPauseMinSize + PlayerOxygen.NoOxygenToDeathUnitTime / (1 - deathPauseMinSize));
		var playerPos = new Vector3();
		if(PlayerControls.Player != null)
			playerPos = PlayerControls.Player.transform.position;

		var offset = gameCamera.WorldToScreenPoint(playerPos) - gameCamera.WorldToScreenPoint(gameCamera.transform.position);

		material.SetVector("_FocusOffset", offset);
	}

	public void OnDisable()
	{
		material.SetFloat("_CircleSize", 1);
	}
}
