
public interface IObserver 
{
    public void OnNotify(int currentScore, int lampNumber) { }
    public void LastLampNotify(int currentScore) { }
    public void NotifyIncorrect() { }
}
