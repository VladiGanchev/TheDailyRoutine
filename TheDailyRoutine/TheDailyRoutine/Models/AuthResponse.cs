namespace TheDailyRoutine.Models
{
    public class AuthResponse
    {
        public string Token { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime TokenExpiration { get; set; }
    }
}