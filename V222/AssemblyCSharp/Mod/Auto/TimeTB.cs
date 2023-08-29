namespace UglyBoy;

public class TimeTB
{
	public int m;

	public int s;

	public string tb;

	private long lastTime;

	public TimeTB(string tb)
	{
		this.tb = tb;
		lastTime = mSystem.currentTimeMillis();
	}

	public void Update2()
	{
		if (mSystem.currentTimeMillis() - lastTime >= 1000)
		{
			s++;
			lastTime = mSystem.currentTimeMillis();
		}
		if (s >= 60)
		{
			s = 0;
			m++;
		}
	}
}
