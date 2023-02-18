using System.Collections.Generic;

namespace TranslateSqlToLinq
{
    public class SourseBinder
    {
        public static List<SourseItem> GetSoursesItem()
        {
            return new List<SourseItem>()
            {
            new SourseItem(1,"139/30",10),
            new SourseItem(2, "167/20",10),
            new SourseItem(3,"167/30",80),
            new SourseItem(4, "23/30",64),
            new SourseItem(5,"232/25",49),
            new SourseItem(6, "236/20",68),
            new SourseItem(8, "236/30",80),
            new SourseItem(9, "237/30",54),
            new SourseItem(10,"238/30",52),
            new SourseItem(11,"241/20",66),
            new SourseItem(12, "241/30",22),
            new SourseItem(13, "241/40",8),
            new SourseItem(14, "25/30",6),
            new SourseItem(15, "251/30",6),
            new SourseItem(16, "254/30",10),
            new SourseItem(17, "270/30",6),
            new SourseItem(18, "33/20",6),
            new SourseItem(19, "342/40",8),
            new SourseItem(20, "35/20",8),
            };
        }
    }
}
