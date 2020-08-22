using UnityEngine;
using UnityEngine.UI;

public class PlayerOxygen : MonoBehaviour
{
	public Slider OxygenBar;
	public float TimeUntilDeath;
	public float TimeUntilOxygenDeplete;

	public static float DeathUnitTimeLeft;
	public static float OxygenUnitTimeLeft;
	public static float NoOxygenToDeathUnitTime;

	private float _death;
	private float _oxygenDeplete;

	void Start()
    {
	    _death = Time.time + TimeUntilDeath;
	    _oxygenDeplete = Time.time + TimeUntilOxygenDeplete;
    }

    void Update()
    {
        DeathUnitTimeLeft = (_death - Time.time) / TimeUntilDeath;
        OxygenUnitTimeLeft = (_oxygenDeplete - Time.time) / TimeUntilOxygenDeplete;
        NoOxygenToDeathUnitTime = Mathf.Clamp01((_death - Time.time) / (TimeUntilDeath - TimeUntilOxygenDeplete));

        OxygenBar.value = OxygenUnitTimeLeft;
    }
}
