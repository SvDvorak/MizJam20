using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

//[ExecuteInEditMode]
public class CircleFade : MonoBehaviour
{
	private Material material;
	private Camera gameCamera;
	private float fadeInAmount;
	private bool isFadingIn;

	public void Start()
	{
		material = GetComponent<Image>().material;
		gameCamera = Camera.main;
		material.SetFloat("_CircleSize", 0);
	}

	public void SetFade(float amount)
	{
		if(!isFadingIn)
			material.SetFloat("_CircleSize", amount);
	}

	public void Update()
	{
		if(isFadingIn)
			material.SetFloat("_CircleSize", fadeInAmount);

		var playerPos = PlayerControls.Player.transform.position;

		var offset = Vector3.zero;
		if(!isFadingIn)
			offset = gameCamera.WorldToScreenPoint(playerPos) - gameCamera.WorldToScreenPoint(gameCamera.transform.position);

		material.SetVector("_FocusOffset", offset);
	}

	private void FadeIn()
	{
		isFadingIn = true;
		DOTween
			.To(() => fadeInAmount, x => fadeInAmount = x, 1, 1f)
			.OnComplete(() => isFadingIn = false);
	}

	public void OnEnable()
	{
		GameState.GameStarted += FadeIn;
	}

	public void OnDisable()
	{
		material.SetFloat("_CircleSize", 1);
		GameState.GameStarted += FadeIn;
	}
}
