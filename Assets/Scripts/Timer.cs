using UnityEngine;

public class Timer {
	
	float timeConsumed;
	float timeLimit;
	public delegate void timerFunction();

	public Timer (float tl)
	{
		timeConsumed = 0.0f;
		timeLimit = tl;
	}

	public void update (timerFunction func)
	{
		timeConsumed += Time.deltaTime;
		if(timeConsumed >= timeLimit)
		{
			func();
			timeConsumed = 0.0f;
		}
	}
}
