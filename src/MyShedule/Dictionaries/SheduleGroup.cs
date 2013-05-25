using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyShedule.Dictionaries
{
    public class ScheduleGroup
    { 
        public ScheduleGroup()
        {
            Name = String.Empty;
        }

        public ScheduleGroup(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public string Name { get; set; }
        public int Id { get; set; }
    }
}
