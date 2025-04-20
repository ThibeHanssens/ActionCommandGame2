using ActionCommandGame.Model;
using ActionCommandGame.Repository.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ActionCommandGame.Repository
{
    public class ActionCommandGameDbContext: IdentityDbContext
    {
        public ActionCommandGameDbContext(DbContextOptions<ActionCommandGameDbContext> options): base(options)
        {
            
        }

        public DbSet<PositiveGameEvent> PositiveGameEvents { get; set; }
        public DbSet<NegativeGameEvent> NegativeGameEvents { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<PlayerItem> PlayerItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.RemovePluralizingTableNameConvention();
            modelBuilder.ConfigureRelationships();

            base.OnModelCreating(modelBuilder);
        }

        public async Task Initialize()
        {
            var email = "thibe.hanssens@vives.be";
            //Password Test123$
            var passwordHash = "AQAAAAEAACcQAAAAECp9VnV5jgDyqQqacxkrC+OcWFUM1+mavZ4+mxxhqtm/dg9UTVq1vhgAKFsblrEXDA==";

            // 1) Ensure SuperAdmin role
            const string superRoleName = "SuperAdmin";
            var superNorm = superRoleName.ToUpperInvariant();
            var existingRole = Roles.SingleOrDefault(r => r.NormalizedName == superNorm);
            if (existingRole == null)
            {
                existingRole = new IdentityRole
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = superRoleName,
                    NormalizedName = superNorm,
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                };
                Roles.Add(existingRole);
            }

            // 2) Seed (or find) the demo user
            var user = Users.SingleOrDefault(u => u.NormalizedEmail == email.ToUpperInvariant());
            if (user == null)
            {
                user = new IdentityUser
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = email,
                    Email = email,
                    NormalizedUserName = email.ToUpperInvariant(),
                    NormalizedEmail = email.ToUpperInvariant(),
                    PasswordHash = passwordHash,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                };
                Users.Add(user);
            }

            // 3) Assign that user to SuperAdmin (if not already)
            var already = UserRoles.Any(ur => ur.UserId == user.Id && ur.RoleId == existingRole.Id);
            if (!already)
            {
                UserRoles.Add(new IdentityUserRole<string>
                {
                    UserId = user.Id,
                    RoleId = existingRole.Id
                });
            }

            // 4) Your existing seeded content:
            GeneratePositiveGameEvents();
            GenerateNegativeGameEvents();
            GenerateAttackItems();
            GenerateDefenseItems();
            GenerateFoodItems();
            GenerateDecorativeItems();

            //God Mode Item
            Items.Add(new Item
            {
                Name = "GOD MODE",
                Description = "Overpowered.",
                Attack = 1000000,
                Defense = 1000000,
                Fuel = 1000000,
                ActionCooldownSeconds = 1,
                Price = 10000000
            });

            Players.Add(new Player { UserId = user.Id, Name = "Johny John", Money = 100 });
            Players.Add(new Player { UserId = user.Id, Name = "Firstname Lastname", Money = 100000, Experience = 2000 });
            Players.Add(new Player { UserId = user.Id, Name = "Jinthe Henssens", Money = 500, Experience = 5 });
            Players.Add(new Player { UserId = user.Id, Name = "Erix Baillon", Money = 12345, Experience = 200 });

            await SaveChangesAsync();
        }

        private void GeneratePositiveGameEvents()
        {
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Little Stone", Probability = 1000 });
            PositiveGameEvents.Add(new PositiveGameEvent
            {
                Name = "Tiny Mushroom",
                Description = "Just a tiny mushroom growing beside the path—completely useless.",
                Probability = 500
            });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Empty Water Bottle", Probability = 1000 });
            PositiveGameEvents.Add(new PositiveGameEvent
            {
                Name = "Mysterious Paper",
                Description = "A piece of paper with nothing written on it. Waste of your hiking time.",
                Probability = 1000
            });
            PositiveGameEvents.Add(new PositiveGameEvent
            {
                Name = "Small Puddle",
                Description = "A tiny puddle of rainwater—too small to drink, too boring to notice.",
                Probability = 1000
            });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Crumbled Leaf", Description = "A dry, crumbled leaf. Congratulations.", Money = 1, Experience = 1, Probability = 2000 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Forgotten Note", Description = "Someone's shopping list—no use for you.", Money = 1, Experience = 1, Probability = 300 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Broken Compass", Description = "Totally broken, points nowhere.", Money = 1, Experience = 1, Probability = 300 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Map Piece", Description = "A torn map showing nowhere special.", Money = 1, Experience = 1, Probability = 300 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Shiny Pebble", Description = "Pretty, but worthless.", Money = 5, Experience = 3, Probability = 1000 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Used Shoelace", Money = 10, Experience = 5, Probability = 800 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Lost Sock", Money = 10, Experience = 5, Probability = 800 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Snail Shell", Description = "Just an empty snail shell. Nice.", Money = 10, Experience = 5, Probability = 800 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Interesting Rock", Description = "A rock that's slightly interesting.", Money = 12, Experience = 6, Probability = 700 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Edible Berry", Description = "Surprisingly tasty wild berry.", Money = 20, Experience = 8, Probability = 650 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Bird Feather", Description = "An unusual, colorful feather.", Money = 30, Experience = 10, Probability = 500 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Discarded Knife", Description = "Old, but still usable.", Money = 50, Experience = 13, Probability = 400 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Silver Ring", Description = "A simple silver ring found on the trail.", Money = 60, Experience = 15, Probability = 400 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Wooden Mask", Description = "An odd mask someone left behind.", Money = 100, Experience = 40, Probability = 350 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Friendly Dog", Description = "A dog joins you for a short while and brightens your day.", Money = 140, Experience = 50, Probability = 300 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Rusty Axe", Description = "Old axe, but still sharp enough.", Money = 160, Experience = 80, Probability = 300 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Wooden Flute", Description = "Plays nicely if you know how.", Money = 160, Experience = 80, Probability = 300 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Old Book", Description = "Pages are falling out, but it's interesting.", Money = 180, Experience = 80, Probability = 300 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Golden Coin", Description = "A rare gold coin lying on the trail.", Money = 300, Experience = 100, Probability = 110 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Energy Potion", Description = "Mysterious liquid that energizes you instantly.", Money = 300, Experience = 100, Probability = 80 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Meteor Stone", Description = "Fell from space, worth good money.", Money = 400, Experience = 150, Probability = 200 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Wooden Sculpture", Description = "Well-made carving someone abandoned.", Money = 500, Experience = 150, Probability = 150 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Hidden Backpack", Description = "A backpack containing valuable items.", Money = 1000, Experience = 200, Probability = 100 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Ancient Artifact", Description = "Extremely valuable object from ancient times.", Money = 60000, Experience = 1500, Probability = 5 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Photographer’s Camera", Description = "Lost by someone—still works perfectly.", Money = 3000, Experience = 400, Probability = 30 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Wallet Full of Cash", Description = "Someone's unfortunate loss, your lucky day.", Money = 2000, Experience = 350, Probability = 30 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Lost Car Keys", Description = "Keys to a fancy car—useless without the car.", Money = 20000, Experience = 1000, Probability = 10 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Prototype GPS", Description = "Super advanced hiking gadget someone dropped.", Money = 30000, Experience = 1500, Probability = 10 });
        }

        private void GenerateNegativeGameEvents()
        {
            NegativeGameEvents.Add(new NegativeGameEvent
            {
                Name = "Rock Slide",
                Description = "Suddenly rocks start falling from above, blocking your path.",
                DefenseWithGearDescription = "Your helmet and backpack absorb the impact, keeping you safe.",
                DefenseWithoutGearDescription = "You get hit and bruised, slowing you down a lot.",
                DefenseLoss = 2,
                Probability = 100
            });
            NegativeGameEvents.Add(new NegativeGameEvent
            {
                Name = "Bear Attack",
                Description = "An angry bear appears, roaring aggressively.",
                DefenseWithGearDescription = "Your bear spray scares the bear away safely.",
                DefenseWithoutGearDescription = "The bear charges at you, leaving you badly injured.",
                DefenseLoss = 3,
                Probability = 50
            });
            NegativeGameEvents.Add(new NegativeGameEvent
            {
                Name = "Hidden Hole",
                Description = "You accidentally step into a hidden hole and fall.",
                DefenseWithGearDescription = "Your gear protects you, making the fall painless.",
                DefenseWithoutGearDescription = "You twist your ankle badly when you fall, making it hard to walk.",
                DefenseLoss = 2,
                Probability = 100
            });
            NegativeGameEvents.Add(new NegativeGameEvent
            {
                Name = "Poisonous Plant",
                Description = "You accidentally touch a plant that makes your skin burn.",
                DefenseWithGearDescription = "Your clothing protects your skin from the plant’s poison.",
                DefenseWithoutGearDescription = "You feel immediate pain and itching—your skin turns red.",
                DefenseLoss = 3,
                Probability = 50
            });

        }

        private void GenerateAttackItems()
        {
            Items.Add(new Item { Name = "Slippers", Attack = 50, Price = 50 });
            Items.Add(new Item { Name = "Trailrunners", Attack = 300, Price = 300 });
            Items.Add(new Item { Name = "Hiking boots", Attack = 500, Price = 500 });
            Items.Add(new Item { Name = "Something better", Attack = 5000, Price = 15000 });
            Items.Add(new Item { Name = "Barefoot", Attack = 50, Price = 1000000 });
        }

        private void GenerateDefenseItems()
        {
            Items.Add(new Item { Name = "Bear Whistle", Defense = 20, Price = 20 });
            Items.Add(new Item { Name = "Map", Defense = 150, Price = 200 });
            Items.Add(new Item { Name = "Hiking poles", Defense = 500, Price = 1000 });
            Items.Add(new Item { Name = "Alpinism Helmet", Defense = 2000, Price = 10000 });
            Items.Add(new Item { Name = "Crampons", Defense = 2000, Price = 10000 });
            Items.Add(new Item { Name = "Ice axe", Defense = 20000, Price = 10000 });
        }

        private void GenerateFoodItems()
        {
            Items.Add(new Item { Name = "Orange", ActionCooldownSeconds = 50, Fuel = 4, Price = 8 });
            Items.Add(new Item { Name = "Egg", ActionCooldownSeconds = 45, Fuel = 5, Price = 10 });
            Items.Add(new Item { Name = "Cheese", ActionCooldownSeconds = 30, Fuel = 30, Price = 300 });
            Items.Add(new Item { Name = "Cheese and nuts", ActionCooldownSeconds = 25, Fuel = 100, Price = 500 });
            Items.Add(new Item { Name = "A lot of meat", ActionCooldownSeconds = 25, Fuel = 100, Price = 500 });
            Items.Add(new Item { Name = "Beetroot", ActionCooldownSeconds = 15, Fuel = 500, Price = 10000 });
//#if DEBUG
            Items.Add(new Item { Name = "Raw milk", ActionCooldownSeconds = 1, Fuel = 1000, Price = 1 });
//#endif
        }

        private void GenerateDecorativeItems()
        {
            Items.Add(new Item { Name = "Baby", Description = "Does nothing, but I want it.", Price = 10 });
            Items.Add(new Item { Name = "Cool item", Description = "If you're too broke to afford the really cool items.", Price = 100000 });
            Items.Add(new Item { Name = "Really cool item", Description = "Waste of time.", Price = 500000 });
        }

    }
}
