using System;

namespace DahliaBot.Entities
{
    public class Ingredient{
        public Guid Id { get; set; }
    }
    public class MetaIngredient : Ingredient{}
    public class GameIngredient : Ingredient{}
}