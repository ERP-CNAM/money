namespace MoneyApp.Services
{
    public class AcceptableAmounts
    {
        public const decimal Standard = 15m; // Full subscription amount
        public const decimal Reduced = 7.5m; // First month half off

        public static bool IsAllowed(decimal value) =>
            value == Standard || value == Reduced;
    }
}
