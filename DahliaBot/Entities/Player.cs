using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Discord;

namespace DahliaBot.Entities
{
    public class Player
    {
        [Key] public ulong Id { get; set; }

        public virtual List<MetaIngredient> MetaIngredients { get; set; } = new List<MetaIngredient>();
        public virtual List<GameIngredient> GameIngredients { get; set; } = new List<GameIngredient>();
        public virtual List<MetaPotion> MetaPotions { get; set; }
        public virtual List<GamePotion> GamePotions { get; set; }
        protected Player()
        {
        }

        public Player(ulong id)
        {
            Id = Id;
        }

        public void Collect(Ingredient ingredient)
        {
            if (ingredient is MetaIngredient metaIngredient) MetaIngredients.Add(metaIngredient);
            if (ingredient is GameIngredient gameIngredient) GameIngredients.Add(gameIngredient);
        }
    }

}