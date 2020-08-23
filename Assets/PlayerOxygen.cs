using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlayerOxygen : MonoBehaviour
{
	public Slider OxygenBar;
	public CircleFade Fade;
	public float TimeUntilDeath;
	public float TimeUntilOxygenDeplete;

	public static float DeathUnitTimeLeft;
	public static float OxygenUnitTimeLeft;
	public static float NoOxygenToDeathUnitTime;

	private float death;
	private float oxygenDeplete;
	private float deathFadeSize = 0.1f;
	private float fadeAmount;

	void Start()
    {
	    death = Time.time + TimeUntilDeath;
	    oxygenDeplete = Time.time + TimeUntilOxygenDeplete;
    }

    void Update()
    {
	    if (GameState.InCutscene || GameState.GameOver)
		    return;

        DeathUnitTimeLeft = Mathf.Clamp01((death - Time.time) / TimeUntilDeath);
        OxygenUnitTimeLeft = Mathf.Clamp01((oxygenDeplete - Time.time) / TimeUntilOxygenDeplete);
        NoOxygenToDeathUnitTime = Mathf.Clamp01((death - Time.time) / (TimeUntilDeath - TimeUntilOxygenDeplete));

        OxygenBar.value = OxygenUnitTimeLeft;

		if(NoOxygenToDeathUnitTime < 0.01)
		{
			GameState.GameOver = true;
		}
		else
		{
			fadeAmount = deathFadeSize + NoOxygenToDeathUnitTime / (1- deathFadeSize);
		}

		Fade.SetFade(fadeAmount);
    }
}
