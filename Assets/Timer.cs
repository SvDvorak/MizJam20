using Assets;
using UnityEngine;

public class Timer : MonoBehaviour, ITimedTask
{
	private float startTime;
	private float timerLengthInSeconds;
	private float? pauseElapsedSoFar;
	private bool isPlaying;

	public Timer Init(float time)
	{
		timerLengthInSeconds = time;
		startTime = Time.time;
		isPlaying = true;
		return this;
	}

	public float Elapsed => Mathf.Clamp(Time.time - startTime, 0, timerLengthInSeconds);
	public float UnitElapsed => Elapsed / timerLengthInSeconds;
	public float TimeLeft => timerLengthInSeconds - Elapsed;
	public float UnitTimeLeft => TimeLeft / timerLengthInSeconds;

	public void Pause()
	{
		if (!IsPlaying())
			return;

		pauseElapsedSoFar = Elapsed;
		isPlaying = false;
	}

	public void Start()
	{
		if (IsPlaying())
			return;

		if (pauseElapsedSoFar.HasValue)
			startTime = Time.time - pauseElapsedSoFar.Value;
		else
			startTime = Time.time;

		isPlaying = true;
	}

	public void Update()
	{
		if(GameState.InCutscene)
			Pause();
		else
			Start();
	}

	public bool IsPlaying()
	{
		return isPlaying;
	}
}