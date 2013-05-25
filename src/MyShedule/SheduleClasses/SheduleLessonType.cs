﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MyShedule.Dictionaries;
using MyShedule.ScheduleClasses;

namespace MyShedule.ScheduleClasses
{
    public class ScheduleLessonType //: IEmploymenType
    {
        public ScheduleLessonType(LessonType type)
        {
            Type = type;
        }

        /// <summary>
        /// По какому критерию отображать расписание
        /// </summary>
        public LessonType Type
        {
            get;
            set;
        }
        /// <summary>
        /// Числовое значение перечисления, используется в привязке к выпадающему списку
        /// </summary>
        public int TypeCode
        {
            get
            {
                return (int)Type;
            }
        }

        /// <summary>
        /// Описание проекции
        /// </summary>
        public string Detail
        {
            get
            {
                return Description(Type);
            }
        }

        public static string Description(LessonType type)
        {
            switch (type)
            {
                case LessonType.Lection: return "Лекция";
                case LessonType.Labwork: return "Лабораторная";
                case LessonType.Practice: return "Практика";
                default :  return String.Empty;
            }
        }

        public static List<ScheduleLessonType> GetBaseType()
        {
            List<ScheduleLessonType> LessonTypes = new List<ScheduleLessonType>();
            LessonTypes.Add(new ScheduleLessonType(LessonType.Lection));
            LessonTypes.Add(new ScheduleLessonType(LessonType.Labwork));
            LessonTypes.Add(new ScheduleLessonType(LessonType.Practice));
            return LessonTypes;
        }
    }

}