using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

//[ExecuteInEditMode]
public class CircleFade : MonoBehaviour
{
	private Material material;
	private Camera gameCamera;

	public void Start()
	{
		material = GetComponent<Image>().material;
		gameCamera = Camera.main;
	}

	public void SetFade(float amount)
	{
		material.SetFloat("_CircleSize", amount);
	}

	public void Update()
	{
		if(!GameState.Ingame)
			material.SetFloat("_CircleSize", 0);

		var playerPos = PlayerControls.Player.transform.position;

		var offset = gameCamera.WorldToScreenPoint(playerPos) - gameCamera.WorldToScreenPoint(gameCamera.transform.position);

		material.SetVector("_FocusOffset", offset);
	}

	public void OnDisable()
	{
		material.SetFloat("_CircleSize", 1);
	}
}
