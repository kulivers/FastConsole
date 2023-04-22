namespace sshWebApplicaiton
{
    public class MyService
    {
        public int I { get; private set; } = 0;

        public int GetValue()
        {
            return ++I;
        }
    }
}