namespace FastConsole;

public class EventClass
{
    public event EventHandler<string> OnRecieve;
    protected virtual void CallOnMessageEvent(string e)
    {
        OnRecieve?.Invoke(this, e);
    }

    public async Task Run()
    {
        while (true)
        {
            CallOnMessageEvent("opaaa");
            await Task.Delay(1000);
        }
    }

}