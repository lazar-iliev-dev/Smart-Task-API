namespace SmartTaskAPI.Models;

    public class User
    {
        public Guid UserId { get; set; } = Guid.NewGuid();
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty; // Gehasht speichern /  Store hashed
        public string Role { get; set; } = "User"; // z.B. Admin, User
    }