using System.Collections;
using System.Collections.Generic;
using Assets;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class BootText
{
	public List<string> Lines;
}

public class IntroPresenter : MonoBehaviour
{
	public TextLinePresenter IntroText;

	private BootText _bootText;

	public void Start()
	{
		if (GameState.Instance.SkipIntro)
			return;

		IntroText.OnFinish += IntroTextFinished;

		var textAsset = Resources.Load<TextAsset>("BootText");
		_bootText = JsonUtility.FromJson<BootText>(textAsset.text);

		IntroText.ShowText(string.Join("\n", _bootText.Lines));

		GameState.AddRunningCutscene(IntroText);
	}

	public void OnDestroy()
	{
		IntroText.OnFinish -= IntroTextFinished;
	}

	private void IntroTextFinished()
	{
		GameState.AddRunningCutscene(DOTween.Sequence()
			.AppendInterval(0.5f)
			.Append(IntroText.GetComponent<TMP_Text>().DOColor(Color.clear, 0.5f))
			.AppendCallback(GameState.StartGame));
	}

    public void Skip(InputAction.CallbackContext context)
    {
	    if (context.performed && IntroText.IsPlaying())
		    IntroText.FinishNow();
    }
}
