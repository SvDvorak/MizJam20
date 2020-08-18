using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

internal class DialogueReader : MonoBehaviour
{
	private static int _storyIndex = -1;

	private string _awaitedState = "";
	private int _stateIndex;
	private bool _started;

	public void Start()
	{
		var textAsset = Resources.Load<TextAsset>("Dialogue");
		GameState.Dialogue = new Dialogue(JsonUtility.FromJson<InputDialogue>(textAsset.text));
	}

	//public void Update()
	//{
	//	if(GameState.HasOccurred("STARTED") && !_started)
	//	{
	//		_started = true;
	//		_textRead = DOTween.Sequence().AppendInterval(10).AppendCallback(UpdateText);
	//	}

	//	if(Input.GetKeyDown(KeyCode.Q))
	//		_textRead.Complete(true);

	//	if(_awaitedState != "" && GameState.HasOccurred(_awaitedState))
	//	{
	//		_textRead.Kill();
	//		_awaitedState = "";
	//		_stateIndex = 0;
	//		UpdateText();
	//	}
	//}

	//private void UpdateText()
	//{
	//	string line = null;
	//	string[] mods = null;
	//	var interval = 1;
	//	var nextStoryIndex = NextIndexWithinList(_storyIndex, _dialogue.Story);
	//	if(_awaitedState != "")
	//	{
	//		var nextIndex = NextIndexWithinList(_stateIndex, _state.Waiting, _state.Repeat);
	//		if(_state != null && nextIndex != -1)
	//		{
	//			var parsedLine = new ParsedLine(_state.Waiting[nextIndex]);
	//			line = parsedLine.Line;
	//			mods = parsedLine.Mods;
	//			_stateIndex += 1;

	//			interval += _state.DelayBetweenEach ? 1 : 0;

	//			if(_stateIndex - nextIndex > 1)
	//				interval += 4;
	//		}
	//	}
	//	else if(nextStoryIndex != -1)
	//	{
	//		_storyIndex = nextStoryIndex;
	//		var storyLine = _storyLines[_storyIndex];
	//		line = storyLine.Line;
	//		mods = storyLine.Mods;
	//	}


	//	if(line != null)
	//	{
	//		var audioId = -1;

	//		if(mods != null)
	//		{
	//			foreach(var mod in mods)
	//			{
	//				if(mod == "PAUSE")
	//				{
	//					interval += 3;
	//				}
	//				else if(mod.StartsWith("AUDIO_"))
	//				{
	//					audioId = int.Parse(mod.Split('_')[1]);
	//				}
	//				else if(mod.StartsWith("AWAIT_"))
	//				{
	//					var awaitKey = mod.Remove(0, "AWAIT_".Length);
	//					_awaitedState = awaitKey;
	//					_state = _dialogue.States.SingleOrDefault(x => x.Name == _awaitedState);
	//					_stateIndex = -1;
	//					interval += 3;
	//				}
	//				else
	//				{
	//					Debug.Log("Activating event " + mod);
	//					GameState.AddStateChange(mod);
	//				}
	//			}
	//		}

	//		Show(line, audioId);
	//	}

	//	_textRead.AppendInterval(interval);
	//	_textRead.AppendCallback(UpdateText);
	//}


	private static int NextIndexWithinList<T>(int dialogueIndex, T[] list, bool repeat = false)
	{
		if(list == null || dialogueIndex + 1 >= list.Length && !repeat)
			return -1;
		return (dialogueIndex + 1) % list.Length;
	}
}