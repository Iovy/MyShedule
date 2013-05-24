using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyShedule
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

        public static List<SheduleRoom> RoomsToList(dsShedule ds)
        {
            return (from dr in ds.Room select new SheduleRoom() 
            {
                DisciplinesLection = (dr.DisciplineLection.Split(new char[] { ',' })).ToList(),
                DisciplinesLabWork = (dr.DisciplineLabWork.Split(new char[] { ',' })).ToList(),
                DisciplinesPractice = (dr.DisciplinePractice.Split(new char[] { ',' })).ToList(),
                Lection = dr.Lection, LabWork = dr.LabWork, Practice = dr.Practice, Name = dr.Name                
            }).ToList();
        }

        public static List<SheduleTeacher> TeachersToList(dsShedule ds)
        {
            return (from dr in ds.Teacher
                    select new SheduleTeacher()
                    {
                        Id = dr.Id,
                        Name = dr.Name
                    }).ToList();
        }

        public static List<SheduleGroup> GroupsToList(dsShedule ds)
        {
            return (from dr in ds.Group
                    select new SheduleGroup()
                    {
                        Id = dr.Id,
                        Name = dr.Name
                    }).ToList();
        }

        public static List<SheduleDiscipline> DisciplinesToList(dsShedule ds)
        {
            return (from dr in ds.Discipline
                    select new SheduleDiscipline()
                    {
                        Id = dr.Id,
                        Name = dr.Name
                    }).ToList();
        }
    }
}
