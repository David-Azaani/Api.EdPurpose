namespace Api.Basic.Services
{
    public interface IMailService
    {
        void Send(string subject, string message);
    }
}

// note -6