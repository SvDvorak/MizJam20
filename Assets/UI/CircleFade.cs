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
	private float fadeAmount;

	public void Start()
	{
		material = GetComponent<Image>().material;
		gameCamera = Camera.main;
		material.SetFloat("_CircleSize", 0);
	}

	public void SetFade(float amount)
	{
		fadeAmount = amount;
	}

	public void Update()
	{
		material.SetFloat("_CircleSize", fadeAmount);

		var playerPos = PlayerControls.Player.transform.position;

		var offset = Vector3.zero;
		if(GameState.Playing || GameState.GameOver)
			offset = gameCamera.WorldToScreenPoint(playerPos) - gameCamera.WorldToScreenPoint(gameCamera.transform.position);

		material.SetVector("_FocusOffset", offset);
	}

	private void FadeIn()
	{
		GameState.AddRunningCutscene(DOTween
			.To(() => fadeAmount, x => fadeAmount = x, 1, 1f));
	}

	public void OnEnable()
	{
		GameState.GameStarted += FadeIn;
	}

	public void OnDisable()
	{
		material.SetFloat("_CircleSize", 1);
		GameState.GameStarted -= FadeIn;
	}

	public Sequence FadeOut(float waitFirst)
	{
		return GameState.AddRunningCutscene(DOTween.Sequence()
			.AppendInterval(waitFirst)
			.Append(DOTween.To(() => fadeAmount, x => fadeAmount = x, 0, 0.5f)));
	}
}
