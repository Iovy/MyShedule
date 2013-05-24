using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using ScheduleDictionaries;

namespace ScheduleClasses
{
    public class SettingShedule
    {
        public SettingShedule()
        {
        }

        public SettingShedule(int countWeeksShedule, int countDayEducationalWeek, int countDaysShedule, int countLessonsOfDay,
            int countEducationalWeekBySem, int maxCountLessonsOfWeekDay, int maxCountLessonsOfWeekEnd, int firstLessonsOfWeekDay,
            int firstLessonsOfWeekEnd, int lastLessonsOfWeekDay, int lastLessonsOfWeekEnd)
        {
            CountWeeksShedule = countWeeksShedule;
            CountDaysEducationWeek = countDayEducationalWeek;
            CountDaysShedule = countDaysShedule;
            CountLessonsOfDay = countLessonsOfDay;
            CountEducationalWeekBySem = countEducationalWeekBySem;
            MaxCountLessonsOfWeekDay = maxCountLessonsOfWeekDay;
            MaxCountLessonsOfWeekEnd = maxCountLessonsOfWeekEnd;
            FirstLessonsOfWeekDay = firstLessonsOfWeekDay;
            FirstLessonsOfWeekEnd = firstLessonsOfWeekEnd;
            LastLessonsOfWeekDay = lastLessonsOfWeekDay;
            LastLessonsOfWeekEnd = lastLessonsOfWeekEnd;
        }

        //количество недель в расписании, по умолчанию расписание двух-недельное
        //максимальное значение 4, если необходимо больше следует добавить поле в перечисление Week
        public int CountWeeksShedule;
        //количество дней в учебной неделе, по умолчанию расписание семи-дневное
        //максимальное значение 7, если необходимо больше следует добавить поле в перечисление Day.. хотя вряд ли есть недели где больше 7 дней :)
        public int CountDaysEducationWeek;
        //количество учебных дней в расписании
        public int CountDaysShedule;
        //в день не больше восьми пар
        //максимальное значение не ограничено 
        public int CountLessonsOfDay;
        //количество учебных недель в семестре
        //максимальное значение не ограничено
        public int CountEducationalWeekBySem;

        //----------------------------------------------------------
        //количество пар в будни
        public int MaxCountLessonsOfWeekDay;
        //количество пар в выходные
        public int MaxCountLessonsOfWeekEnd;
        //первая пара с которой может начинаться учебный день в будни
        public int FirstLessonsOfWeekDay;
        //первая пара с которой может начинаться учебный день в выходной
        public int FirstLessonsOfWeekEnd;
        //последня пара которой завершается учебный день в будни
        public int LastLessonsOfWeekDay;
        //последня пара которой завершается учебный день в выходной
        public int LastLessonsOfWeekEnd;
    }
}