using Assets;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameOverPresenter : MonoBehaviour
{
	public TextLinePresenter GameOverText;
	private bool showingGameOverText;

	void Update()
    {
	    if (!showingGameOverText && GameState.GameOver)
	    {
		    showingGameOverText = true;

			GameOverText.StartTask();
	    }
    }

    public void Restart(InputAction.CallbackContext context)
    {
	    if (showingGameOverText && context.performed)
		    SceneManager.LoadScene(0);
    }
}
