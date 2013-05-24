﻿using System;
//using ScheduleDictionaries;

namespace ScheduleClasses
{
    /// <summary> значение ячейки в таблице, указывает на адрес занятия в расписании </summary>
    public class SchedulePointer
    {
        #region Constructors

        public SchedulePointer(ScheduleTime time1, ScheduleTime time2, string room1, string room2)
        {
            Time1 = time1;
            Time2 = time2;
            Room1 = room1;
            Room2 = room2;
        }

        #endregion

        /// <summary> копировать указатель на ячейку </summary>
        public SchedulePointer Copy() { return new SchedulePointer(Time1, Time2, Room1, Room2); }

        /// <summary> время занятия на 1-2 недели </summary>
        public ScheduleTime Time1 { get; set; }

        /// <summary> время занятия на 3-4 недели </summary>
        public ScheduleTime Time2 { get; set; }

        /// <summary> аудитория в которой проходит занятие на 1-2 недели </summary>
        public string Room1 { get; set; }

        /// <summary> аудитория в которой проходит занятие на 3-4 недели </summary>
        public string Room2 { get; set; }
    }
}