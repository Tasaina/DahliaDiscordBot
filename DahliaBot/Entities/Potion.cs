using System;

namespace DahliaBot.Entities
{
    public class Potion {
        public Guid Id { get; set; }
    }
    public class GamePotion : Potion
    {
    }

    public class MetaPotion : Potion
    {
    }
}