using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyShedule
{
    public class EducationLoadAdapter
    {
        public EducationLoadAdapter()
        {
            Items = new List<LoadItem>();
        }

        public EducationLoadAdapter(IEnumerable<LoadItem> items)
        {
            Items = items.ToList();
        }

        public List<LoadItem> Items;

        public List<string> NamesGroups
        {
            get
            {
                return (from p in Items from g in p.Groups orderby g select g).Distinct().ToList();
            }
        }

        public List<string> NamesTeachers
        {
            get
            {
                return (from p in Items select p.Teacher).Distinct().ToList();
            }
        }

        public List<string> NamesDisciplines
        {
            get
            {
                return (from p in Items select p.Discipline).Distinct().ToList();
            }
        }
    }
}
