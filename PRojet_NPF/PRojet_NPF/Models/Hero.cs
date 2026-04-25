using System.Collections.Generic;

namespace PRojet_NPF.Models
{
    public class Hero
    {
        public int ID { get; set; }
        public string Name { get; set; } = "";
        public int Health { get; set; }
        public string? ImageURL { get; set; }
        public ICollection<Spell> Spells { get; set; } = new List<Spell>();
    }
}