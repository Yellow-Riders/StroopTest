public class BarInfo
{
    private float barTime;
    private bool barCorrect;

    public BarInfo(float time, bool isCorrect)
    {
        barTime = time;
        barCorrect = isCorrect;
    }

    public float GetTime()
    {
        return barTime;
    }
    
    public bool GetCorrect()
    {
        return barCorrect;
    }
}
