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
            public const string Profile = "/play/profile";
        }
        public static class Game
        {
            public const string Shop = "/play/shop";
        }
        public static class Player
        {
            public const string PlayerSelection = "/play/player-selection";
        }
    }
}
