namespace EmailService
{
    public interface IGameEmailService
    {
        void SendGameweekOpenEmail(string recipient, string url);
        void SendPicksNotEnteredEmail(string recipient, string url);
    }
}
