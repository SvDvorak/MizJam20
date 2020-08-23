using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlayerOxygen : MonoBehaviour
{
	public Slider OxygenBar;
	public CircleFade Fade;
	public float TimeUntilDeath;
	public float TimeUntilOxygenDeplete;

	public static float DeathUnitTimeLeft = 1;
	public static float OxygenUnitTimeLeft = 1;
	public static float NoOxygenToDeathUnitTime = 1;

	private float deathFadeSize = 0.1f;
	private Timer deathTimer;
	private Timer oxygenDepleteTimer;

	public void Start()
	{
		deathTimer = gameObject.AddComponent<Timer>().Init(TimeUntilDeath);
	    oxygenDepleteTimer = gameObject.AddComponent<Timer>().Init(TimeUntilOxygenDeplete);
	}

    public void Update()
    {
	    if (GameState.InCutscene || GameState.GameOver)
		    return;

		if(deathTimer.IsPlaying())
		{
			DeathUnitTimeLeft = deathTimer.UnitTimeLeft;
			NoOxygenToDeathUnitTime = Mathf.Clamp01(deathTimer.TimeLeft / (TimeUntilDeath - TimeUntilOxygenDeplete));
			
			if (NoOxygenToDeathUnitTime < 0.01)
			{
				GameState.GameOver = true;
			}
			else
			{
				var fadeAmount = deathFadeSize + NoOxygenToDeathUnitTime / (1 - deathFadeSize);
				Fade.SetFade(fadeAmount);
			}
		}
		if(oxygenDepleteTimer.IsPlaying())
		{
			OxygenUnitTimeLeft = oxygenDepleteTimer.UnitTimeLeft;
			OxygenBar.value = OxygenUnitTimeLeft;
		}
    }
}
