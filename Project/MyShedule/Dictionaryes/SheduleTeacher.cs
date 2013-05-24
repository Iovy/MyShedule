using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScheduleDictionaries
{
    public class ScheduleTeacher
    {
        public ScheduleTeacher()
        {
            Name = String.Empty;
        }

        public ScheduleTeacher(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public string Name { get; set; }
        public int Id { get; set; }
    }
}
