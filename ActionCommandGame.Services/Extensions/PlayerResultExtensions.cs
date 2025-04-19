using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActionCommandGame.Services.Model.Results;

namespace ActionCommandGame.Services.Extensions
{
    public static class PlayerResultExtensions
    {
        public static int GetLevel(this PlayerResult player)
        {
            // Level = floor((√[100*(2XP+25)] + 50) / 100)
            return (int)Math.Floor((Math.Sqrt(100 * (2 * player.Experience + 25)) + 50) / 100);
        }
    }
}
