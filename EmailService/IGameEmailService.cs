namespace EmailService
{
    interface IGameEmailService
    {
        void SendGameweekOpenEmail(string recipient, string url);
    }
}
