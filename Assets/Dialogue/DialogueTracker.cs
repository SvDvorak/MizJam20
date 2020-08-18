using System.Collections;
using System.Collections.Generic;
using Assets;
using UnityEngine;

public class DialogueTracker : MonoBehaviour
{
	public TextLinePresenter TextLinePresenter;

	private GameObject player;
	private bool played;

	private int TEMPStoryIndex = 0;
	private bool _started;

	public void Start()
    {
	    player = GameObject.FindGameObjectWithTag("Player");
    }

	public void OnEnable()
	{
		TextLinePresenter.OnFinish += LineFinished;
	}

	public void OnDisable()
	{
		TextLinePresenter.OnFinish += LineFinished;
	}

	private void LineFinished()
	{
		ShowNextText();
	}

	public void Update()
	{
		if (GameState.Dialogue == null || _started)
			return;

		ShowNextText();
		_started = true;
	}

	private void ShowNextText()
	{
		var storyChapter = GameState.Dialogue.Story[0];
		if(storyChapter.Lines.Count > TEMPStoryIndex)
		{
			TextLinePresenter.ShowText(storyChapter.Lines[TEMPStoryIndex].Text);
			TEMPStoryIndex += 1;
		}
	}
}
