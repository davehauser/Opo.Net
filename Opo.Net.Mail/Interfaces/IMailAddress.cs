namespace Opo.Net.Mail
{
    public interface IMailAddress
    {
        string Address { get; set; }
        string DisplayName { get; set; }
        string AccountName { get; set; }
        string Domain { get; set; }
    }
}
