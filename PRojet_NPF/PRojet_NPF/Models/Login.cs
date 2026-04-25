namespace PRojet_NPF.Models
{
    public class Login
    {
        public int ID { get; set; }
        public string Username { get; set; } = "";
        public string PasswordHash { get; set; } = "";
    }
}