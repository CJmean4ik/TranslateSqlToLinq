using System;
using System.Collections.Generic;
using System.Text;

namespace TranslateSqlToLinq.Entitys
{
    public class NewSourseItem : SourseItem
    {
        public int Total { get; set; }
        public NewSourseItem(int id, string item, int quantity, int total) : base(id, item, quantity)
        {
            Total = total;
        }        
    }
}
