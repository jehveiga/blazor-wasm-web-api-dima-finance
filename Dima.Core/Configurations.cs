namespace Dima.Core
{
    public static class Configurations
    {
        public const int DEFAULT_STATUS_CODE = 200;
        public const int PAGE_NUMBER = 1;
        public const int PAGE_SIZE = 25;

        public static string CONNECTION_STRING { get; set; } = string.Empty;
        public static string BACKEND_URL { get; set; } = string.Empty;
        public static string FRONTEND_URL { get; set; } = string.Empty;
    }
}