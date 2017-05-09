using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YummyCures.Models
{
    public class Ingredient
    {
        public int IngredientID { get; set; }

        public Content Content { get; set; }
        public int ContentID { get; set; }

        public string IngredientName { get; set; }
        public int IngredientQuantity { get; set; }
        public string UnitOfMeasure { get; set; }
    }
}