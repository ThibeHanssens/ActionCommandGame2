namespace ActionCommandGame.BlazorApp
{
    public static class AppRoutes
    {
        public static class Home
        {
            public const string Index = "/";
        }

        public static class Account
        {
            public const string SignIn = "/account/sign-in";
            public const string Register = "/account/register";
        }

        public static class Player
        {
            public const string PlayerSelection = "/player-selection";
        }
    }
}
