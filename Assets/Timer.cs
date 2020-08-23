using Assets;
using UnityEngine;

public class Timer : MonoBehaviour, ITimedTask
{
	private float startTime;
	private float timerLengthInSeconds;
	private float? pauseTimeLeft;
	private bool isPlaying;

	public Timer Init(float time, bool runDuringCutscene = true)
	{
		timerLengthInSeconds = time;
		startTime = Time.time + time;
		isPlaying = true;
		return this;
	}

	public float TimeLeft => Mathf.Clamp(Time.time - startTime, 0, timerLengthInSeconds);
	public float UnitTimeLeft => TimeLeft / timerLengthInSeconds;

	public void Pause()
	{
		if (!IsPlaying())
			return;

		pauseTimeLeft = TimeLeft;
		isPlaying = false;
	}

	public void Start()
	{
		if (IsPlaying())
			return;

		if (pauseTimeLeft.HasValue)
			startTime = Time.time + pauseTimeLeft.Value;
		else
			startTime = Time.time + timerLengthInSeconds;

		isPlaying = true;
	}

	public bool IsPlaying()
	{
		return isPlaying;
	}
}