﻿using ActionCommandGame.Abstractions;

namespace ActionCommandGame.Model
{
    public class Player: IIdentifiable, IHasExperience
    {

        public int Id { get; set; }
        public required string Name { get; set; }
        public int Money { get; set; }
        public int Experience { get; set; }
        public DateTime? LastActionExecutedDateTime { get; set; }

        public required string UserId { get; set; }

        public int? CurrentFuelPlayerItemId { get; set; }
        public PlayerItem CurrentFuelPlayerItem { get; set; } = null!;
        public int? CurrentAttackPlayerItemId { get; set; }
        public PlayerItem CurrentAttackPlayerItem { get; set; } = null!;
        public int? CurrentDefensePlayerItemId { get; set; }
        public PlayerItem CurrentDefensePlayerItem { get; set; } = null!;

        public IList<PlayerItem> Inventory { get; set; } = new List<PlayerItem>();

    }
}
