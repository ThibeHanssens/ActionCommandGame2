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
            public const string Action = "/play/action";
            public const string Shop = "/play/shop";
            public const string Inventory = "/play/inventory";
        }
        public static class Player
        {
            public const string PlayerSelection = "/play/player-selection";
        }
        public static class Admin
        {
            public const string Index = "/admin";
            public const string Items = "/admin/items";
            public const string GENegatives = "/admin/ge-negative";
            public const string GEPoitives = "/admin/ge-positive";
        }
    }
}
