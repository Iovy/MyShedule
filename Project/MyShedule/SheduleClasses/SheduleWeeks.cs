using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScheduleDictionaries;

namespace ScheduleClasses
{
    
    /* класс расписание содержит дни и уроки, управляет днями и уроками */
    /// <summary> Класс расписание </summary>
    [Serializable]
    public class ScheduleWeeks
    {
        #region Constructors, Fields and Initialize Days

        //пустой конcтруктор необходим для сериализации расписания
        public ScheduleWeeks() { 
            FirstDaySem = DateTime.Now; 
        }

        public ScheduleWeeks(List<ScheduleRoom> rooms, SettingShedule setting, DateTime firstDaySem, DateTime lastDaySem)
        {
            Rooms = rooms;
            Setting = setting;
            FirstDaySem = firstDaySem;
            LastDaySem = lastDaySem;
            InitializeDays();
            Employments = new Employments();
            Employments.Clear();
        }

        public List<ScheduleDay> Days;

        public DateTime FirstDaySem;

        public DateTime LastDaySem;

        public List<ScheduleRoom> Rooms;

        public SettingShedule Setting;

        public Employments Employments;

        //инициализация расписания
        private void InitializeDays()
        {
            Days = new List<ScheduleDay>();

            DateTime TempDate = FirstDaySem;
            DateTime DateCounter = FirstDaySem;

            DateTime MondayDate = getMondayOfWeek(TempDate);
            TempDate = MondayDate;
            DateCounter = MondayDate;
            for (int week = 1; week <= (Setting.CountWeeksShedule + 2); week++)
            {
                TempDate = DateCounter;

                for (int day = 1; day <= Setting.CountDaysEducationWeek; day++) 
                {
                    Days.Add(new ScheduleDay((Week)week, (Day)day, Rooms, Setting, DateCounter));
                    removeExcessDays();
                    removeExcessDaysInLessons();
                    DateCounter = DateCounter.AddDays(1);
                }

                DateCounter = TempDate + TimeSpan.FromDays(7);
            }

            //if (FirstDaySem.Month == 2) ;
            //{
            //    for (int i = 0; i < Setting.CountWeeksShedule; i++)
            //    {
            //        for (int day = i * Setting.CountDaysEducationWeek * 2;
            //            day < Setting.CountDaysEducationWeek + (i * Setting.CountDaysEducationWeek * 2); day++)
            //        {
            //            SheduleDay bufferDay = Days[day];
            //            Week bufferWeek = Days[day + Setting.CountDaysEducationWeek].Week;
            //            Days[day] = Days[day + Setting.CountDaysEducationWeek];
            //            Days[day].Week = bufferDay.Week;
            //            Days[day + Setting.CountDaysEducationWeek] = bufferDay;
            //            Days[day + Setting.CountDaysEducationWeek].Week = bufferWeek;
            //        }
            //    }
            //}
        }

        private void removeExcessDaysInLessons()
        {
            foreach (ScheduleLesson lesson in Days[Days.Count - 1].Lessons)
            {
                int delIndex = -1;
                bool dontStop = true;
                do
                {
                    dontStop = true;
                    for (int i = 0; i < lesson.Dates.Count && dontStop; i++)
                    {
                        if ((lesson.Dates[i].Month < FirstDaySem.Month && lesson.Dates[i].Year == FirstDaySem.Year) 
                            || lesson.Dates[i].Date > LastDaySem)
                        {
                            delIndex = i;
                            if (delIndex != -1)
                                lesson.Dates.RemoveAt(delIndex);
                            dontStop = false;
                        }
                    }
                }
                while (!dontStop);
            }
        }

        private void removeExcessDays()
        {
            int delIndex = -1;
            bool dontStop = true;
            do
            {
                dontStop = true;
                for (int j = 0; j < Days[Days.Count - 1].Dates.Count && dontStop; j++)
                {
                    if (((Days[Days.Count - 1].Dates[j].Month < FirstDaySem.Month && Days[Days.Count - 1].Dates[j].Year == FirstDaySem.Year) ^
                        (Days[Days.Count - 1].Dates[j].Month > FirstDaySem.Month && Days[Days.Count - 1].Dates[j].Year < FirstDaySem.Year)) 
                        || Days[Days.Count - 1].Dates[j].Date > LastDaySem)
                    {
                        delIndex = j;
                        if (delIndex != -1)
                            Days[Days.Count - 1].Dates.RemoveAt(delIndex);
                        dontStop = false;
                    }
                }
            }
            while (!dontStop);
        }

        public Week getWeekNumberOfDay(DateTime date)
        {
            foreach (ScheduleDay day in Days)
            {
                if (day.Dates.IndexOf(date) > -1)
                    return day.Week;
            }
            return Week.Another;
        }

        DateTime getMondayOfWeek(DateTime baseDate)
        {
            int dayOfWeek = (int)baseDate.DayOfWeek;
            int startDay = baseDate.Day - dayOfWeek;
            if (dayOfWeek == 1)
                startDay = baseDate.Day;
            int startMonth = baseDate.Month;
            int startYear = baseDate.Year;
            if (startDay < 0)
            {
                startMonth = baseDate.Month - 1;
                if (startMonth <= 0)
                {
                    startYear--;
                    startMonth = 12;
                }
                startDay = DateTime.DaysInMonth(startYear, startMonth) + startDay + 1; ;
            }

            return new DateTime(startYear, startMonth, startDay);
        }

        #endregion

        /// <summary> Cписок всех занятий из расписания </summary>
        public IEnumerable<ScheduleLesson> Lessons { get { return from day in Days from lesson in day.Lessons select lesson; } }

        /// <summary> Получить расписание занятия </summary>
        public ScheduleLesson GetLesson(ScheduleTime Time, string Room) {
            IEnumerable<ScheduleLesson> query = Lessons.Where(x => x.Time == Time && x.Room == Room);
            return query.Count() > 0 ? query.First() : null;
        }

        /// <summary> Найти расписание занятия из списка занятий</summary>
        public ScheduleLesson FindLessonInList(IEnumerable<ScheduleLesson> items, ScheduleTime time) {
            IEnumerable<ScheduleLesson> query = items.Where(x => x.Time == time);
            return query.Count() > 0 ? query.First() : null;
        }

        /// <summary> Получить расписание определенного дня </summary>
        public ScheduleDay GetDay(Week week, Day day) { return Days.Single(e => e.Week == week && e.Day == day); }
        public ScheduleDay GetDay(ScheduleTime time)   { return GetDay(time.Week, time.Day); }

        /// <summary> Получить все занятия определенного дня </summary>
        public IEnumerable<ScheduleLesson> GetLessonsOfDay(Week week, Day day) { return Days.Single(e => e.Week == week && e.Day == day).Lessons; } 
        public IEnumerable<ScheduleLesson> GetLessonsOfDay(ScheduleDay day)     { return GetLessonsOfDay(day.Week, day.Day); }
        

        #region GET NAMES ELEMENTS BY VIEW

        /// <summary> Cписок ФИО преподавателей из расписания </summary>
        public IEnumerable<string> TeachersNames    { get { return (from x in Lessons where !x.IsEmpty select x.Teacher).Distinct(); } }

        /// <summary> Cписок названий групп из расписания </summary>
        public IEnumerable<string> GroupsNames      { get { return (from x in Lessons where !x.IsEmpty from g in x.Groups select g).Distinct(); } }

        /// <summary> Cписок названий дисциплин из расписания </summary>
        public IEnumerable<string> DisciplinesNames { get { return (from x in Lessons where !x.IsEmpty select x.Discipline).Distinct(); } }

        /// <summary> Cписок названий аудиторий из расписания </summary>
        public IEnumerable<string> RoomsNames       { get { return (from x in Lessons where !x.IsEmpty select x.Room).Distinct(); } }

        #endregion

        #region GET LESSONS BY VIEW

        /// <summary> Получить список список занятий определенного преподавателя </summary>
        public IEnumerable<ScheduleLesson> GetLessonsTeacher(string Teacher) { return from x in Lessons where x.Teacher == Teacher select x; }

        /// <summary> Получить список список занятий определенной группы</summary>
        public IEnumerable<ScheduleLesson> GetLessonsGroup(string Group) { return from x in Lessons from g in x.Groups where g == Group select x; }

        /// <summary> Получить список список занятий определенной дисциплины</summary>
        public IEnumerable<ScheduleLesson> GetLessonsDiscipline(string Discipline) { return from x in Lessons where x.Discipline == Discipline select x; }

        /// <summary> Получить список список занятий определенной аудитоии</summary>
        public IEnumerable<ScheduleLesson> GetLessonsRoom(string Room) { return from x in Lessons where x.Room == Room && !x.IsEmpty select x; }

        /// <summary> Получить все занятия по определенному элементу проекции </summary>
        public IEnumerable<ScheduleLesson> GetLessonsByView(View view, string name) {
            return view == View.Discipline ? GetLessonsDiscipline(name) : view == View.Group ? GetLessonsGroup(name) : 
                   view == View.Room ? GetLessonsRoom(name) : view == View.Teacher ? GetLessonsTeacher(name) : new List<ScheduleLesson>();
        }

        #endregion
    }
}
