using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MyShedule.Dictionaries;
using MyShedule.ScheduleClasses;

namespace MyShedule.Dictionaries
{
    public class DictionaryConverter
    {
        public static List<LoadItem> EducationToList(dsShedule ds)
        {
            return (from dr in ds.Education select new LoadItem() 
            { 
                Groups = (dr.Group.Replace(" ", "").Split(new char[] { ',' })).ToList(),
                HoursSem = dr.HoursSem, 
                Discipline = dr.Discipline, 
                Teacher = dr.Teacher,
                LessonType = (LessonType)dr.LessonType,
            }
            ).ToList();
        }

        public static List<ScheduleRoom> RoomsToList(dsShedule ds)
        {
            return (from dr in ds.Room select new ScheduleRoom() 
            {
                DisciplinesLection = (dr.DisciplineLection.Split(new char[] { ',' })).ToList(),
                DisciplinesLabWork = (dr.DisciplineLabWork.Split(new char[] { ',' })).ToList(),
                DisciplinesPractice = (dr.DisciplinePractice.Split(new char[] { ',' })).ToList(),
                Lection = dr.Lection, LabWork = dr.LabWork, Practice = dr.Practice, Name = dr.Name                
            }).ToList();
        }

        public static List<ScheduleTeacher> TeachersToList(dsShedule ds)
        {
            return (from dr in ds.Teacher
                    select new ScheduleTeacher()
                    {
                        Id = dr.Id,
                        Name = dr.Name
                    }).ToList();
        }

        public static List<ScheduleGroup> GroupsToList(dsShedule ds)
        {
            return (from dr in ds.Group
                    select new ScheduleGroup()
                    {
                        Id = dr.Id,
                        Name = dr.Name
                    }).ToList();
        }

        public static List<ScheduleDiscipline> DisciplinesToList(dsShedule ds)
        {
            return (from dr in ds.Discipline
                    select new ScheduleDiscipline()
                    {
                        Id = dr.Id,
                        Name = dr.Name
                    }).ToList();
        }
    }
}
