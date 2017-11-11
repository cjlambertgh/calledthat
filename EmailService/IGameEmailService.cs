namespace EmailService
{
    public interface IGameEmailService
    {
        void SendGameweekOpenEmail(string recipient, string url);
    }
}
