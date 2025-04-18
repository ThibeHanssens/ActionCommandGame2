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
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Mundane Pebbles", Probability = 1000 });
            PositiveGameEvents.Add(new PositiveGameEvent
            {
                Name = "Hidden Geode",
                Description = "A crystal‑lined geode glimmers in a crevice—pry it open to reveal its secrets.",
                Probability = 500
            });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Dust and Silt", Probability = 1000 });
            PositiveGameEvents.Add(new PositiveGameEvent
            {
                Name = "Blank Scroll",
                Description = "This scrap of parchment bears no writing—your breath fogs it, but no message appears.",
                Probability = 1000
            });
            PositiveGameEvents.Add(new PositiveGameEvent
            {
                Name = "Underground Spring",
                Description = "A trickle of clear water pools at your feet, cool and surprisingly pure.",
                Probability = 1000
            });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Junk Scraps", Money = 1, Experience = 1, Probability = 2000 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Prospector’s Note", Money = 1, Experience = 1, Probability = 300 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Abandoned Toolkit", Money = 1, Experience = 1, Probability = 300 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Map Fragment", Money = 1, Experience = 1, Probability = 300 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Glimmering Shard", Money = 5, Experience = 3, Probability = 1000 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Worn Pickaxe Head", Money = 10, Experience = 5, Probability = 800 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Broken Hammer", Money = 10, Experience = 5, Probability = 800 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Intricate Shell", Money = 10, Experience = 5, Probability = 800 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Fossil Fragment", Money = 12, Experience = 6, Probability = 700 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Cave Fungus", Money = 20, Experience = 8, Probability = 650 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Ancient Relic", Money = 30, Experience = 10, Probability = 500 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Metal Scrap", Money = 50, Experience = 13, Probability = 400 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Jeweled Ring", Money = 60, Experience = 15, Probability = 400 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Ornate Mask", Money = 100, Experience = 40, Probability = 350 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Geode Cluster", Money = 140, Experience = 50, Probability = 300 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Ancient Blade", Money = 160, Experience = 80, Probability = 300 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Mystic Instrument", Money = 160, Experience = 80, Probability = 300 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Lost Manuscript", Money = 180, Experience = 80, Probability = 300 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Rare Gemstone", Money = 300, Experience = 100, Probability = 110 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Elixir Vial", Money = 300, Experience = 100, Probability = 80 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Meteor Fragment", Money = 400, Experience = 150, Probability = 200 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Carved Bust", Money = 500, Experience = 150, Probability = 150 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Hidden Chest", Money = 1000, Experience = 200, Probability = 100 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Alien Artifact", Money = 60000, Experience = 1500, Probability = 5 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Curator’s Prize", Money = 3000, Experience = 400, Probability = 30 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Gold Nugget", Money = 2000, Experience = 350, Probability = 30 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Vault Key", Money = 20000, Experience = 1000, Probability = 10 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Prototype Device", Money = 30000, Experience = 1500, Probability = 10 });
        }

        private void GenerateNegativeGameEvents()
        {
            NegativeGameEvents.Add(new NegativeGameEvent
            {
                Name = "Tunnel Collapse",
                Description = "The ceiling groans before a shower of rock crashes around you.",
                DefenseWithGearDescription = "Your reinforced helmet and armor deflect most of the debris.",
                DefenseWithoutGearDescription = "Rocks slam into you—bruises and cuts slow you down.",
                DefenseLoss = 2,
                Probability = 100
            });
            NegativeGameEvents.Add(new NegativeGameEvent
            {
                Name = "Giant Cave Spider",
                Description = "You disturb a massive spider that skitters toward you.",
                DefenseWithGearDescription = "Your protective gear shields you from its venomous fangs.",
                DefenseWithoutGearDescription = "It sinks fangs into you—pain and paralysis set in.",
                DefenseLoss = 3,
                Probability = 50
            });
            NegativeGameEvents.Add(new NegativeGameEvent
            {
                Name = "Hidden Pitfall",
                Description = "The floor gives way beneath your feet, plunging you into darkness.",
                DefenseWithGearDescription = "Your padding cushions the fall, sparing serious injury.",
                DefenseWithoutGearDescription = "You hit the ground hard—your ribs ache and breath is stolen.",
                DefenseLoss = 2,
                Probability = 100
            });
            NegativeGameEvents.Add(new NegativeGameEvent
            {
                Name = "Toxic Gas Leak",
                Description = "A noxious cloud billows from the walls—breathing becomes painful.",
                DefenseWithGearDescription = "Your mask filters most toxins—you cough but stay standing.",
                DefenseWithoutGearDescription = "You inhale the poisonous air—your head spins and lungs burn.",
                DefenseLoss = 3,
                Probability = 50
            });
        }

        private void GenerateAttackItems()
        {
            Items.Add(new Item { Name = "Wooden Pickaxe", Attack = 50, Price = 50 });
            Items.Add(new Item { Name = "Iron Pickaxe", Attack = 300, Price = 300 });
            Items.Add(new Item { Name = "Diamond Pickaxe", Attack = 500, Price = 500 });
            Items.Add(new Item { Name = "Something better", Attack = 5000, Price = 15000 });
            Items.Add(new Item { Name = "Toothpick", Attack = 50, Price = 1000000 });
        }

        private void GenerateDefenseItems()
        {
            Items.Add(new Item { Name = "Underwear", Defense = 20, Price = 20 });
            Items.Add(new Item { Name = "Baselayer", Defense = 150, Price = 200 });
            Items.Add(new Item { Name = "Protective clothing", Defense = 500, Price = 1000 });
            Items.Add(new Item { Name = "Iron Shield", Defense = 2000, Price = 10000 });
            Items.Add(new Item { Name = "Diamond Shield", Defense = 2000, Price = 10000 });
            Items.Add(new Item { Name = "Obsidian Shield", Defense = 20000, Price = 10000 });
        }

        private void GenerateFoodItems()
        {
            Items.Add(new Item { Name = "Orange", ActionCooldownSeconds = 50, Fuel = 4, Price = 8 });
            Items.Add(new Item { Name = "Coffee", ActionCooldownSeconds = 45, Fuel = 5, Price = 10 });
            Items.Add(new Item { Name = "Good snack", ActionCooldownSeconds = 30, Fuel = 30, Price = 300 });
            Items.Add(new Item { Name = "Cheese and nuts", ActionCooldownSeconds = 25, Fuel = 100, Price = 500 });
            Items.Add(new Item { Name = "Energy drink", ActionCooldownSeconds = 25, Fuel = 100, Price = 500 });
            Items.Add(new Item { Name = "A real meal", ActionCooldownSeconds = 15, Fuel = 500, Price = 10000 });
#if DEBUG
            Items.Add(new Item { Name = "Developer Food", ActionCooldownSeconds = 1, Fuel = 1000, Price = 1 });
#endif
        }

        private void GenerateDecorativeItems()
        {
            Items.Add(new Item { Name = "Baby", Description = "Does nothing. Do you feel special now?", Price = 10 });
            Items.Add(new Item { Name = "Blue Medal", Description = "If you're too broke afford the Golden Crown.", Price = 100000 });
            Items.Add(new Item { Name = "Golden Crown", Description = "Waste of time.", Price = 500000 });
        }

    }
}
