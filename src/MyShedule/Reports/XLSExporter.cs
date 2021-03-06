﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Diagnostics;

using MyShedule.ScheduleClasses;
using Yogesh.ExcelXml;

namespace MyShedule.ReportExp
{
    public class XLSExporter : IExporter
    {
        public XLSExporter()
        {
            nameElements = new List<string>();
        }

        public XLSExporter(List<string> _nameElements, ScheduleWeeks _shedule, ScheduleClasses.View _view, string _fileName)
        {
            nameElements = _nameElements;
            shedule = _shedule;
            view = _view;
            fileName = _fileName;
        }

        private List<string> nameElements;

        public List<string> NameElements
        {
            get { return nameElements; }
            set { nameElements = value; }
        }

        private ScheduleWeeks shedule;

        public ScheduleWeeks Shedule
        {
            get { return shedule; }
            set { shedule = value; }
        }

        private ScheduleClasses.View view;

        public ScheduleClasses.View View
        {
            get { return view; }
            set { view = value; }
        }

        string fileName;
        ExcelXmlWorkbook book;
        public int lsnCntMult;
        int lessonHorizontalMult = 5;

        public void Export()
        {
            lsnCntMult = shedule.Setting.CountLessonsOfDay;
            
            book = new ExcelXmlWorkbook();
            Worksheet sheet = book[0];

            sheet.FreezeTopRows = 1;
            sheet.FreezeLeftColumns = 8;

            sheet.PrintOptions.Orientation = PageOrientation.Landscape;
            sheet.PrintOptions.SetMargins(0.5, 0.4, 0.5, 0.4);
            AlignmentOptionsBase alOpt = (AlignmentOptionsBase)sheet.Alignment;
            alOpt.Horizontal = Yogesh.ExcelXml.HorizontalAlignment.Center;
            alOpt.ShrinkToFit = true;
            alOpt.Vertical = VerticalAlignment.Center;
            alOpt.WrapText = true;
            FontOptionsBase font = (FontOptionsBase)sheet.Font;
            font.Name = "Arial";

            SetTextHeaderTable(sheet);

            SetWeeksHeaderTable(sheet);

            SetDaysTable(sheet, 1, 6, 1, 6);
            SetDaysTable(sheet, 1, 6, 6 * lsnCntMult * lessonHorizontalMult + 1, 12);

            int monthStart = shedule.FirstDaySem.Month;

            FillDays(sheet, monthStart, 1, Week.FirstWeek, Week.TreeWeek);
            FillDays(sheet, monthStart, 6 * lsnCntMult * lessonHorizontalMult + 1, Week.SecondWeek, Week.FourWeek);

            FillHours(sheet, 1);
            FillHours(sheet, 6 * lsnCntMult * lessonHorizontalMult + 1);

            int counter = 0;
            foreach (string name in nameElements)
            {
                FillEntitiesData(sheet, name, 8 + counter);
                counter+=2;
            }

            SetColumnSize(sheet);

            SaveToFile(book);
        }

        void SetColumnSize(Worksheet sheet)
        {
            for (int i = 0; i < 7; i++)
            {
                sheet.Columns(i).Width = 18;
            }
            sheet.Columns(7).Width = 32;

            for (int i = 8; i < 8 + nameElements.Count * 2; i+=2)
            {
                sheet.Columns(i).Width = 96;
                sheet.Columns(i + 1).Width = 96;
            }
        }

        public void FillEntitiesData(Worksheet sheet, string name, int column)
        {
            if (shedule == null)
                return;

            sheet[column, 0].Value = name;
            FontOptionsBase font = (FontOptionsBase)sheet[column, 0].Font;
            font.Bold = true;
            Range range = new Range(sheet[column, 0], sheet[column + 1, 0]);
            range.Merge();
            BorderOptionsBase brOptC = (BorderOptionsBase)range.Border;
            brOptC.Color = Color.Black;
            brOptC.Sides = BorderSides.All;

            int monthStart = shedule.FirstDaySem.Month;

            List<ScheduleLesson> tmp = shedule.GetLessonsByView(view, name).ToList();

            FillLessons(sheet, monthStart, 1, column, tmp, Week.FirstWeek, Week.TreeWeek);
            FillLessons(sheet, monthStart, 6 * lsnCntMult * lessonHorizontalMult + 1, column, tmp, Week.SecondWeek, Week.FourWeek);
        }

        public void FillDays(Worksheet sheet, int monthStart, int row, Week week1, Week week2)
        {
            int column = 1;

            for (int counterDay = 1; counterDay <= 6; counterDay++)
            {
                column = 1;
                for (int counterMonth = monthStart; counterMonth < monthStart + 5; counterMonth++)
                {
                    List<int> numbers = (from x in shedule.Days
                                          from p in x.Dates
                                          where x.Day == (ScheduleClasses.Day)counterDay &&
                                              p.Month == counterMonth && (x.Week == week1 || x.Week == week2)
                                          select p.Day).Distinct().OrderBy(e => e).ToList();

                    int k = 0;
                    foreach (int day in numbers)
                    {
                        sheet[column, row + k].Value += day.ToString();
                        FontOptionsBase font = (FontOptionsBase)sheet[column, row + k].Font;
                        font.Bold = true;
                        k++;
                    }
                    column++;
                }
                row += lsnCntMult * lessonHorizontalMult;
            }
        }

        public void FillHours(Worksheet sheet, int row)
        {
            for (int counterDay = 1; counterDay <= 6; counterDay++)
            {
                int Hour;
                for (Hour = 1; Hour <= lsnCntMult; Hour++)
                {
                    string str = "";
                    str = ScheduleTime.GetHourDiscription(Hour);
                    sheet[7, row + (Hour - 1) * lessonHorizontalMult].Value = str;
                    FontOptionsBase font = (FontOptionsBase)sheet[7, row + (Hour - 1) * lessonHorizontalMult].Font;
                    font.Bold = true;
                    Range range = new Range(sheet[7, row + (Hour - 1) * lessonHorizontalMult],
                        sheet[7, row + (Hour - 1) * lessonHorizontalMult + lessonHorizontalMult - 1]);
                    range.Merge();
                    BorderOptionsBase brOptC = (BorderOptionsBase)range.Border;
                    brOptC.Color = Color.Black;
                    brOptC.Sides = BorderSides.All;

                }
                row += (Hour - 1) * lessonHorizontalMult;
            }
        }

        public void FillLessons(Worksheet sheet, int monthStart, int row, int column, List<ScheduleLesson> tmp, Week week1, Week week2)
        {
            int startRow = row;

            for (int counterDay = 1; counterDay <= 6; counterDay++)
            {
                List<ScheduleLesson> query1 = (from x in tmp
                                              from p in x.Dates
                                              where x.Day == (ScheduleClasses.Day)counterDay && (x.Week == week1 || x.Week == week2)
                                              select x).ToList();

                List<int> Hours = (from x in query1 select x.Hour).Distinct().OrderBy(e => e).ToList();

                for (int i = 0; i < Hours.Count; i++)
                {
                    row = Hours[i] + ((Hours[i] - 1) * (lessonHorizontalMult - 1)) 
                        + ((counterDay - 1) * lsnCntMult * lessonHorizontalMult) + (startRow - 1);
                    List<ScheduleLesson> t1 = query1.Where(x => x.Hour == Hours[i] && x.Week == week1).ToList();
                    List<ScheduleLesson> t2 = query1.Where(x => x.Hour == Hours[i] && x.Week == week2).ToList();

                    ScheduleLesson lesson = (t1.Count > 0) ? t1.First() : null;
                    ScheduleLesson lesson2 = (t2.Count > 0) ? t2.First() : null;

                    if (lesson != null && lesson2 != null && lesson.IsEqual(lesson2))
                    {
                        WriteLesson(sheet, row, column, 0, lesson, false);
                    }
                    else if (lesson != null && lesson2 != null && !lesson.IsEqual(lesson2))
                    {
                        WriteLesson(sheet, row, column, 0, lesson, true);
                        WriteLesson(sheet, row, column, 1, lesson2, true);
                    }
                    else
                    {
                        if (lesson != null)
                        {
                            WriteLesson(sheet, row, column, 0, lesson, true);
                        }
                        if (lesson2 != null)
                        {
                            WriteLesson(sheet, row, column, 0, lesson2, true);
                        }
                    }

                    if (!(lesson != null && lesson2 != null && !lesson.IsEqual(lesson2)))
                    {
                        Range range1 = new Range(sheet[column, row], sheet[column + 1, row]);
                        range1.Merge();
                        Range range2 = new Range(sheet[column, row + 1], sheet[column + 1, row + 1]);
                        range2.Merge();
                        Range range3 = new Range(sheet[column, row + 2], sheet[column + 1, row + 2]);
                        range3.Merge();
                        Range range4 = new Range(sheet[column, row + 3], sheet[column + 1, row + 3]);
                        range4.Merge();
                        Range range5 = new Range(sheet[column, row + 4], sheet[column + 1, row + 4]);
                        range5.Merge();
                    }
                }
            }
        }

        void WriteLesson(Worksheet sheet, int row, int column, int subClmnShift, ScheduleLesson lesson, bool outdate)
        {
            string str = "";

            //0-я строчка
            if (view != ScheduleClasses.View.Group)
            {
                str = lesson.GroupsDescription;
                switch (view)
                {
                    case ScheduleClasses.View.Discipline: sheet[column + subClmnShift, row].Value = str;  
                        break;
                    case ScheduleClasses.View.Room: sheet[column + subClmnShift, row + 4].Value = str;
                        break;
                    case ScheduleClasses.View.Teacher: sheet[column + subClmnShift, row + 3].Value = str;
                        break;
                }
            }

            //1-я строчка
            if (view == ScheduleClasses.View.Discipline)
            {
                sheet[column + subClmnShift, row].Value = str;
                sheet[column + subClmnShift, row + 1].Value = "(" + ScheduleLessonType.Description(lesson.Type) + ")";
            }
            else
            {
                str = lesson.Discipline;
                sheet[column + subClmnShift, row].Value = str;
                sheet[column + subClmnShift, row + 1].Value = "(" + ScheduleLessonType.Description(lesson.Type) + ")";
            }

            //2-я строчка
            if (outdate)
            {
                str = lesson.DatesDescription;
                sheet[column + subClmnShift, row + 2].Value = str;
            }
            else
            {
                sheet[column + subClmnShift, row + 2].Value = "";
            }


            //3-я строчка
            if (view == ScheduleClasses.View.Teacher)
            {
                str = lesson.Room;
                sheet[column + subClmnShift, row + 4].Value = str;
            }

            if (view == ScheduleClasses.View.Room)
            {
                str = lesson.Teacher;
                sheet[column + subClmnShift, row + 3].Value = str;
            }

            if (view != ScheduleClasses.View.Teacher && view != ScheduleClasses.View.Room)
            {
                sheet[column + subClmnShift, row + 3].Value = lesson.Teacher;
                sheet[column + subClmnShift, row + 4].Value = lesson.Room;
            }
        }

        void SetWeeksHeaderTable(Worksheet sheet)
        {
            int i = 1;
            string week1 = "I" + Environment.NewLine + "Н Е Д Е Л Я";
            sheet[0,i].Value = week1;
            FontOptionsBase font = (FontOptionsBase)sheet[0, i].Font;
            font.Bold = true;
            AlignmentOptionsBase alOptR = (AlignmentOptionsBase)sheet[0, i].Alignment;
            alOptR.Rotate = 90;

            Range range = new Range(sheet[0, i], sheet[0, (i + lsnCntMult - 1) * 6 * lessonHorizontalMult]);
            range.Merge();
            BorderOptionsBase brOptC = (BorderOptionsBase)range.Border;
            brOptC.Color = Color.Black;
            brOptC.Sides = BorderSides.All;

            i += 6 * lsnCntMult * lessonHorizontalMult;

            string week2 = "II" + Environment.NewLine + "Н Е Д Е Л Я";

            sheet[0, i].Value = week2;
            font = (FontOptionsBase)sheet[0, i].Font;
            font.Bold = true;
            alOptR = (AlignmentOptionsBase)sheet[0, i].Alignment;
            alOptR.Rotate = 90;

            range = new Range(sheet[0, i], sheet[0, i*2 - 2]);
            range.Merge();
            brOptC = (BorderOptionsBase)range.Border;
            brOptC.Color = Color.Black;
            brOptC.Sides = BorderSides.All;
        }

        public void SetTextHeaderTable(Worksheet sheet)
        {
            sheet[0, 0].Value = "";

            int mountStart = shedule != null ? shedule.FirstDaySem.Month : 9;//сентябрь по умолчанию
            SetMonthsHeader(sheet, mountStart, 0, 1, 5);

            sheet[6, 0].Value = "День";
            FontOptionsBase font = (FontOptionsBase)sheet[6, 0].Font;
            font.Bold = true;
            AlignmentOptionsBase alOptR = (AlignmentOptionsBase)sheet[6, 0].Alignment;
            alOptR.Rotate = 90;
            BorderOptionsBase brOptC = (BorderOptionsBase)sheet[6, 0].Border;
            brOptC.Color = Color.Black;
            brOptC.Sides = BorderSides.All;

            sheet[7, 0].Value = "Часы";
            font = (FontOptionsBase)sheet[7, 0].Font;
            font.Bold = true;
            alOptR = (AlignmentOptionsBase)sheet[7, 0].Alignment;
            alOptR.Rotate = 90;
            brOptC = (BorderOptionsBase)sheet[7, 0].Border;
            brOptC.Color = Color.Black;
            brOptC.Sides = BorderSides.All;
        }

        public void SetDaysTable(Worksheet sheet, int dayStart, int columnIndex, int rowIndexStart, int rowIndexEnd)
        {
            for (int counterRow = rowIndexStart, counterDay = dayStart;
                counterRow <= rowIndexEnd * lsnCntMult * lessonHorizontalMult; counterRow += lsnCntMult * lessonHorizontalMult, counterDay++)
            {
                sheet[columnIndex, counterRow].Value = GetDayName(counterDay);
                FontOptionsBase font = (FontOptionsBase)sheet[columnIndex, counterRow].Font;
                font.Bold = true;
                AlignmentOptionsBase alOptR = (AlignmentOptionsBase)sheet[columnIndex, counterRow].Alignment;
                alOptR.Rotate = 90;

                Range range = new Range(sheet[columnIndex, counterRow], sheet[columnIndex, counterRow + lsnCntMult * lessonHorizontalMult - 1]);
                range.Merge();
                BorderOptionsBase brOptC = (BorderOptionsBase)range.Border;
                brOptC.Color = Color.Black;
                brOptC.Sides = BorderSides.All;
            }
        }

        static string GetDayName(int dayNumber)
        {
            switch (dayNumber)
            {
                case 1: return "понедельник";
                case 2: return "вторник";
                case 3: return "среда";
                case 4: return "четверг";
                case 5: return "пятница";
                case 6: return "суббота";
                case 7: return "воскресенье";
                default: return @"DAY N\A";
            }
        }

        public void SetMonthsHeader(Worksheet sheet, int monthStart, int rowIndex, int columnIndexStart, int columnIndexEnd)
        {
            for (int counterClmn = columnIndexStart, counterMonth = monthStart; counterClmn <= columnIndexEnd; counterClmn++)
            {
                sheet[counterClmn, rowIndex].Value = GetMonthName(counterMonth);
                FontOptionsBase font = (FontOptionsBase)sheet[counterClmn, rowIndex].Font;
                font.Bold = true;
                counterMonth = counterMonth == 12 ? 1 : counterMonth + 1;
                AlignmentOptionsBase alOptR = (AlignmentOptionsBase)sheet[counterClmn, rowIndex].Alignment;
                alOptR.Rotate = 90;
                BorderOptionsBase brOptC = (BorderOptionsBase)sheet[counterClmn, rowIndex].Border;
                brOptC.Color = Color.Black;
                brOptC.Sides = BorderSides.All;
            }
        }

        static string GetMonthName(int monthNumber)
        {
            switch (monthNumber)
            {
                case 1: return "январь";
                case 2: return "февраль";
                case 3: return "март";
                case 4: return "апрель";
                case 5: return "май";
                case 6: return "июнь";
                case 7: return "июль";
                case 8: return "август";
                case 9: return "сентябрь";
                case 10: return "октябрь";
                case 11: return "ноябрь";
                case 12: return "декабрь";
                default: return @"MOUTH N\A";
            }
        }

        void SaveToFile(ExcelXmlWorkbook book)
        {
            book.Export(fileName);

            Process.Start(fileName);
        }
    }
}
