using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;using ScheduleDictionaries;

namespace ScheduleClasses
{
    /// <summary> Класс занятие, представляет собой расписание на одно занятие </summary>
    [Serializable]
    public class ScheduleLesson : ICloneable
    {
        #region Copy and Clone SheduleLesson

        public ScheduleLesson Copy()
        {
            return Clone() as ScheduleLesson;
        }

        public object Clone() 
        { 
            return CloneObject(this) as object; 
        }

        public static object CloneObject(object obj) 
        {
            using (System.IO.MemoryStream memStream = new System.IO.MemoryStream()) 
            { 
                BinaryFormatter binaryFormatter = new BinaryFormatter(null, new StreamingContext(StreamingContextStates.Clone));
                binaryFormatter.Serialize(memStream, obj);
                memStream.Seek(0, System.IO.SeekOrigin.Begin); 
                return binaryFormatter.Deserialize(memStream); 
            } 
        }

        #endregion

        #region Constructors SheduleLesson

        //пустой конструктор для сериализации
        public ScheduleLesson()
        { }

        public ScheduleLesson(ScheduleTime time, string room, IEnumerable<DateTime> dates)
        {
            //main atributtes
            Teacher = String.Empty;
            Discipline = String.Empty;
            Groups = new List<string>();
            Room = room;
            Type = LessonType.Lection;
            //set dates
            Dates = dates.ToList();
            //set times
            Time = time;
        }

        public ScheduleLesson(ScheduleTime time, string room, IEnumerable<DateTime> dates, 
            string teaher, string discipline, IEnumerable<string> groups, LessonType type)
        {
            //main atributtes
            Teacher = teaher;
            Discipline = discipline;
            Groups = groups.ToList();
            Room = room;
            Type = type;
            //set dates
            Dates = dates.ToList();
            //set times
            Time = time;
        }

        #endregion

        #region Main Fields SheduleLesson

        //times fields
        public ScheduleTime Time { get; set; }

        public Week Week { get { return Time.Week; } }

        public Day Day { get { return Time.Day; } }

        //number lesson in day
        public int Hour { get { return Time.Hour; } }

        //main fields
        /// <summary> Преподаватель </summary>
        public string Teacher { get; set; }

        /// <summary> Дисциплина </summary>
        public string Discipline { get; set; }

        /// <summary> Тип занятия перечисление</summary>
        public LessonType Type { get; set; }

        /// <summary> Тип занятия цифровой код </summary>
        public int TypeCode { get { return (int)Type; } }

        /// <summary> Аудитория </summary>
        public string Room { get; set; }

        /// <summary> Список групп у которых проводится занятие </summary>
        public List<string> Groups;

        /// <summary> Список календарных дат в которые проводится занятие </summary>
        public List<DateTime> Dates;

        #endregion

        /// <summary> Пустое занятие? проверка проставлены ли все атрибуты занятию, если нет то занятие считается не проставленным и пустым </summary>
        public bool IsEmpty
        {
            get
            {
                return Teacher == String.Empty || Discipline == String.Empty || Groups == null || Groups.Count == 0 || Room == String.Empty;
            }
        }

        /// <summary> Очистить занятие </summary>
        public void Clear()
        {
            Teacher = String.Empty;

            Discipline = String.Empty;

            Groups = new List<string>();

            Type = LessonType.Lection;
        }

        /// <summary> Переназначить занятие </summary>
        public void UpdateFields(string teaher, string discipline, IEnumerable<string> groups, LessonType type)
        {
            Teacher = teaher;

            Discipline = discipline;

            Groups = groups.ToList();

            Type = type;
        }

        /// <summary> Переназначить занятие </summary>
        public void UpdateFields(string teaher, string discipline, IEnumerable<string> groups, LessonType type, IEnumerable<DateTime> dates)
        {
            Dates = dates.ToList();

            UpdateFields(teaher, discipline, groups.ToList(), type);
        }

        public bool IsEqual(ScheduleLesson item)
        {
            return this.Teacher == item.Teacher && this.Discipline == item.Discipline &&
                this.Room == item.Room && this.Type == item.Type;// && this.Groups == item.Groups;
        }

        //public static bool operator ==(SheduleLesson item1, SheduleLesson item2)
        //{
        //    return item1.Teacher == item2.Teacher && item1.Discipline == item2.Discipline &&
        //        item1.Room == item2.Room && item1.Type == item2.Type && item1.Groups == item2.Groups;
        //}

        //public static bool operator !=(SheduleLesson item1, SheduleLesson item2)
        //{
        //    return !(item1 == item2);
        //}

        /// <summary> Строка со списком групп через запятую у которых провидится занятие </summary>
        public string GroupsDescription
        {
            get
            {
                string value = String.Empty;
                foreach (string group in Groups)
                    value += Groups.Last() == group ? group : group + ',';

                return value;
            }
        }

        /// <summary> строка со списком календарных дат занятия в формате {0:dd}.{0:MM} </summary>
        public string DatesDescription
        {
            get
            {
                string DatesLine = String.Empty;
                foreach (DateTime dt in Dates)
                    DatesLine += String.Format("{0:dd}.{0:MM}., ", dt);

                return DatesLine;
            }
        }

        /// <summary> Расписание на один день для группы </summary>
        public string WrapSheduleGroup {
            get { return Discipline + Environment.NewLine + ScheduleLessonType.Description(Type) + 
                Environment.NewLine + Teacher + Environment.NewLine + Room;
            }
        }

        /// <summary> Расписание на один день для преподавателя </summary>
        public string WrapSheduleTeacher {
            get { return Discipline + Environment.NewLine + GroupsDescription + Environment.NewLine + 
                    ScheduleLessonType.Description(Type) + Environment.NewLine + Room;
            }
        }

        /// <summary> Расписание на один день для дисциплины </summary>
        public string WrapSheduleDiscipline {
            get { return Teacher + Environment.NewLine + GroupsDescription + Environment.NewLine + 
                    ScheduleLessonType.Description(Type) + Environment.NewLine + Room;
            }
        }

        /// <summary> Расписание на один день для аудитории </summary>
        public string WrapSheduleRoom {
            get { return Teacher + Environment.NewLine + GroupsDescription + Environment.NewLine + 
                    ScheduleLessonType.Description(Type) + Environment.NewLine + Discipline;
            }
        }
    }
}
