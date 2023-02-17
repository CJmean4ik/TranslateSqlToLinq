using System;
using System.Collections.Generic;
using System.Text;

namespace TranslateSqlToLinq
{
   public class SourseItem
    {
        public int Id { get; set; }
        public string item { get; set; }
        public int quantity { get; set; }

        public SourseItem(int id, string item, int quantity)
        {
            Id = id;
            this.item = item;
            this.quantity = quantity;
        }
    }

}
