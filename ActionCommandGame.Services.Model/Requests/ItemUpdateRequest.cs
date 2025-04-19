using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActionCommandGame.Services.Model.Requests
{
    public class ItemUpdateRequest : ItemCreateRequest
    {
        public int Id { get; set; }
    }
}
