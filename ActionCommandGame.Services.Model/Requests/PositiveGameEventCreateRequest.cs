using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActionCommandGame.Services.Model.Requests
{
    public class PositiveGameEventCreateRequest
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public int Money { get; set; }
        public int Experience { get; set; }
        public int Probability { get; set; }
    }
}
