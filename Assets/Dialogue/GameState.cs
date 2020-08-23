using System;
using System.Collections.Generic;
using Assets;
using DG.Tweening;
using UnityEngine;

public class GameState : MonoBehaviour
{
	public static Dialogue Dialogue;
	public int DebugRunningCutscenes;
	public bool SkipIntro;

	public static bool InCutscene => RunningCutscenes.Count > 0;
	public static bool GameOver;
	public static bool Playing => !GameOver && !InCutscene;

	public static Action GameStarted;
	private static readonly List<ITimedTask> RunningCutscenes = new List<ITimedTask>();

	public static GameState Instance { get; private set; }

	public void Awake()
	{
		Instance = this;
		GameOver = false;
	}

	public static void StartGame()
	{
		GameStarted?.Invoke();
	}

	public static T AddRunningCutscene<T>(T cutscene) where T : Tween
	{
		var tweenTask = new TweenTask(cutscene);
		RunningCutscenes.Add(tweenTask);
		return cutscene;
	}

	public void Update()
	{
		var i = 0;
		while (i < RunningCutscenes.Count)
		{
			var cutscene = RunningCutscenes[i];

			if (!cutscene.IsPlaying())
				RunningCutscenes.Remove(cutscene);
			else
				i += 1;
		}

		DebugRunningCutscenes = RunningCutscenes.Count;
	}

	public static TimedTask AddRunningCutscene(TimedTask timedTask)
	{
		RunningCutscenes.Add(timedTask);
		return timedTask;
	}
}