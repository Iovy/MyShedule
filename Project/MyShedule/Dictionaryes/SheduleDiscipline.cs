using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScheduleDictionaries
{
    public class ScheduleDiscipline
    { 
        public ScheduleDiscipline()
        {
            Name = String.Empty;
        }

        public ScheduleDiscipline(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public string Name { get; set; }
        public int Id { get; set; }
    }
}
