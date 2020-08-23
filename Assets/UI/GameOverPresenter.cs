using Assets;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameOverPresenter : MonoBehaviour
{
	public TextLinePresenter GameOverText;
	public CircleFade Fade;
	private bool showingGameOverText;

	void Update()
    {
	    if (!showingGameOverText && GameState.GameOver)
	    {
		    showingGameOverText = true;

			Fade.FadeOut(2).OnComplete(() => GameOverText.StartTask());
	    }
    }

    public void Restart(InputAction.CallbackContext context)
    {
	    if (showingGameOverText && context.performed)
		    SceneManager.LoadScene(0);
    }
}
