namespace project_api.Helpers
{
    public class Verifier
    {
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            string hashedInput = Encrypter.HashPassword(password);
                
            return hashedInput.Equals(hashedPassword, StringComparison.OrdinalIgnoreCase);
        }
    }
}
