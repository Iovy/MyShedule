using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

using MyShedule.ScheduleClasses;
using ExportToRTF;

namespace MyShedule.ReportExp
{
    class DocExporter : IExporter
    {
        public DocExporter()
        {
            NameElements = new List<string>();
        }

        public DocExporter(List<string> _nameElements, ScheduleWeeks _shedule, ScheduleClasses.View _view, string _fileName, List<string> _stampParams)
        {
            NameElements = _nameElements;
            shedule = _shedule;
            view = _view;
            fileName = _fileName;
            stampParams = _stampParams;
        }

        string fileName;

        List<string> NameElements;

        List<string> stampParams;

        ScheduleWeeks shedule;

        ScheduleClasses.View view;

        WordDocument wordDocument = new WordDocument(WordDocumentFormat.A4);

        //Инициализируем необходимые шрифты
        private static Font caption0 = new Font("Times New Roman", 12, FontStyle.Bold);
        private static Font caption1 = new Font("Times New Roman", 12, FontStyle.Regular);
        private static Font caption = new Font("Times New Roman", 14, FontStyle.Bold);
        private static Font normalBold = new Font("Times New Roman", 10, FontStyle.Bold);
        private static Font normal = new Font("Times New Roman", 10, FontStyle.Regular);

        public void Export()
        {
            //http://www.faqdot.net/post/ExportToWord.aspx

            foreach (string name in NameElements)
            {

                //wordDocument.SetPageNumbering(1);

                WriteHeaderReport();

                // Создаем таблицу для поставщика
                WordTable table = wordDocument.NewTable(normal, Color.Black, 13, 9, 10);
                table.SetContentAlignment(ContentAlignment.MiddleCenter);
                table.SetColumnsWidth(new int[] { 3, 5, 5, 5, 5, 5, 5, 5, 35 });
                // table.SetContentAlignment(ContentAlignment.MiddleLeft);
                table.Rows[0].SetFont(caption0);
                ////Записываем в ячейку
                SetTextHeaderTable(table, name);

                table.SetFont(caption);
                SetWeeksHeaderTable(table);

                table.SetFont(caption0);
                SetDaysTable(table, 1, 6, 1, 6);
                SetDaysTable(table, 1, 6, 7, 12);

                table.SetFont(normal);
                FillDaysNumbers(table, name);

                table.SetFont(normal);
                table.SaveToDocument(10000, 0);
            }

            CreateReport(wordDocument);

        }

        private void FillDaysNumbers(WordTable table, string name)
        {
            if (shedule == null)
                return;

            int monthStart = shedule.FirstDaySem.Month;

            List<ScheduleLesson> tmp = shedule.GetLessonsByView(view, name).ToList();

            FillLessons(table, monthStart, 1, tmp, Week.FirstWeek, Week.TreeWeek);
            FillLessons(table, monthStart, 7, tmp, Week.SecondWeek, Week.FourWeek);
        }

        private void FillLessons(WordTable table, int monthStart, int row, List<ScheduleLesson> tmp, Week week1, Week week2)
        {
            int column = 1;

            for (int counterDay = 1; counterDay <= 6; counterDay++)
            {
                column = 1;
                for (int counterMonth = monthStart; counterMonth < monthStart + 5; counterMonth++)
                {

                    List<int> numbers = (from x in tmp
                                         from p in x.Dates
                                         where x.Day == (ScheduleClasses.Day)counterDay &&
                                             p.Month == counterMonth && (x.Week == week1 || x.Week == week2)
                                         select p.Day).Distinct().OrderBy(e => e).ToList();

                    foreach (int day in numbers)
                    {
                        table.Cell(row, column).WriteLine();
                        table.Cell(row, column).Write(day.ToString());
                    }
                    column++;
                }

                List<ScheduleLesson> query1 = (from x in tmp
                                              from p in x.Dates
                                              where x.Day == (ScheduleClasses.Day)counterDay && (x.Week == week1|| x.Week == week2)
                                              select x).ToList();

                List<int> Hours = (from x in query1 select x.Hour).Distinct().OrderBy(e => e).ToList();

                foreach (int hour in Hours)
                {
                    string str = "";
                    str = ScheduleTime.GetHourDiscription(hour);
                    table.Cell(row, 7).Write(str);
                    table.Cell(row, 7).WriteLine();
                }

                for (int i = 0; i < Hours.Count; i++)
                {
                    List<ScheduleLesson> t1 = query1.Where(x => x.Hour == Hours[i] && x.Week == week1).ToList();
                    List<ScheduleLesson> t2 = query1.Where(x => x.Hour == Hours[i] && x.Week == week2).ToList();

                    ScheduleLesson lesson = (t1.Count > 0) ? t1.First() : null;
                    ScheduleLesson lesson2 = (t2.Count > 0) ? t2.First() : null;

                    if (lesson != null && lesson2 != null && lesson.IsEqual(lesson2))
                    {
                        WriteLesson(table, row, lesson, false);


                        if (i + 1 < Hours.Count)
                        {
                            List<ScheduleLesson> Next1 = query1.Where(x => x.Hour == Hours[i + 1] && x.Week == week1).ToList();
                            ScheduleLesson next1 = (t1.Count > 0) ? t1.First() : null;

                            List<ScheduleLesson> Next2 = query1.Where(x => x.Hour == Hours[i + 1] && x.Week == week2).ToList();
                            ScheduleLesson next2 = (t1.Count > 0) ? t1.First() : null;


                            if (next1 != null && next2 != null && next1.IsEqual(next2) && lesson.IsEqual(next1))
                                i++;
                        }
                    }
                    else
                    {
                        if (lesson != null)
                        {

                            if(lesson2 == null)
                                WriteLesson(table, row, lesson, false);
                            else
                                WriteLesson(table, row, lesson, true);

                            if (i + 1 < Hours.Count)
                            {
                                List<ScheduleLesson> Next = query1.Where(x => x.Hour == Hours[i + 1] && x.Week == week1).ToList();
                                ScheduleLesson next = (Next.Count > 0) ? Next.First() : null;

                                if (next != null && lesson.IsEqual(next))
                                    i++;
                            }
                        }

                        if (lesson2 != null)
                        {
                            if (lesson == null)
                                WriteLesson(table, row, lesson2, false);
                            else
                                WriteLesson(table, row, lesson2, true);

                            if (i + 1 < Hours.Count)
                            {
                                List<ScheduleLesson> Next = query1.Where(x => x.Hour == Hours[i + 1] && x.Week == week2).ToList();
                                ScheduleLesson next = (Next.Count > 0) ? Next.First() : null;

                                if (next != null && lesson2.IsEqual(next))
                                    i++;
                            }
                        }
                    }

                }
                row++;
            }
        }

        private void WriteLesson(WordTable table, int row, ScheduleLesson lesson, bool outdate)
        {

            string str = "";

            //0-я строчка
            if (view != ScheduleClasses.View.Group)
            {
                str = lesson.GroupsDescription;
                table.Cell(row, 8).Write(str);
                table.Cell(row, 8).WriteLine();
            }

            //1-я строчка
            if (view == ScheduleClasses.View.Discipline)
            {
                str += " (" + ScheduleLessonType.Description(lesson.Type) + ")";
                table.Cell(row, 8).Write(str);
                table.Cell(row, 8).WriteLine();
            }
            else
            {
                str = lesson.Discipline + " (" + ScheduleLessonType.Description(lesson.Type) + ")";
                table.Cell(row, 8).Write(str);
                table.Cell(row, 8).WriteLine();
            }

            //2-я строчка
            if (outdate)
            {
                str = lesson.DatesDescription;
                getDateTimeOfLesson(str);
                table.Cell(row, 8).Write(str);
                table.Cell(row, 8).WriteLine();
            }

            //3-я строчка
            if (view == ScheduleClasses.View.Teacher)
            {
                str = lesson.Room;
                table.Cell(row, 8).Write(str);
                table.Cell(row, 8).WriteLine();
            }

            if (view == ScheduleClasses.View.Room)
            {
                str = lesson.Teacher;
                table.Cell(row, 8).Write(str);
                table.Cell(row, 8).WriteLine();
            }

            if (view != ScheduleClasses.View.Teacher && view != ScheduleClasses.View.Room)
            {
                str = lesson.Teacher + " " + lesson.Room;
                table.Cell(row, 8).Write(str);
                table.Cell(row, 8).WriteLine();
            }
        }

        private List<DateTime> getDateTimeOfLesson(string dateTimeDescript)
        {
            List<DateTime> allDates = new List<DateTime>();

            string[] datesMass = dateTimeDescript.Split(',');
            for (int i = 0; i < (datesMass.Length - 1); i++)
            {
                string[] dayMonth = datesMass[i].Split('.');
                DateTime day = new DateTime(DateTime.Today.Year,int.Parse(dayMonth[1]),int.Parse(dayMonth[0]));
                allDates.Add(day);
            }

            return allDates;
        }

        private static void SetWeeksHeaderTable(WordTable table)
        {
            
            int i = 1;
            string week1 = "I" + Environment.NewLine + "Н Е Д Е Л Я";
            table.Cell(i, 0).Write(week1);

            WordCellRange wsr = table.CellRange(i, 0, i + 5, 0);
            wsr.MergeCells();

            i = i + 6;

            string week2 = "II" + Environment.NewLine + "Н Е Д Е Л Я";

            table.Cell(i, 0).Write(week2);

            wsr = table.CellRange(i, 0, i + 5, 0);
            wsr.MergeCells();
        }

        private void WriteHeaderReport()
        {
            //Выводим заголовок
            wordDocument.SetFont(caption);
            wordDocument.SetTextAlign(WordTextAlign.Center);
            wordDocument.WriteLine("ФАКУЛЬТЕТ ПОСЛЕВУЗОВСКОГО ОБРАЗОВАНИЯ");
            wordDocument.WriteLine("");

            wordDocument.SetFont(caption1);
            wordDocument.SetTextAlign(WordTextAlign.Right);
            wordDocument.WriteLine('"' + "УТВЕРЖДАЮ" + '"');
            wordDocument.WriteLine("Проректор по учебной работе");
            wordDocument.WriteLine("__________________" + stampParams[0]);
            wordDocument.WriteLine('"' + "______" + '"' + "______________" + stampParams[4] + " г.");
            wordDocument.WriteLine("");

            wordDocument.SetFont(caption0);
            wordDocument.SetTextAlign(WordTextAlign.Center);
            wordDocument.WriteLine("Расписание занятий по второму высшему образованию " + stampParams[3] + " курса " + stampParams[1]);
            wordDocument.WriteLine("(специальность 230101 " + '"' + "Вычислительные машины, комплексы, системы и сети" + '"' + ")");
            wordDocument.WriteLine("на " + stampParams[2] + " семестр " + stampParams[5] + " уч. года");
            wordDocument.WriteLine("");
        }

        private void SetTextHeaderTable(WordTable table, string group)
        {
            table.Cell(0, 0).Write("");

            int mountStart = shedule != null ? shedule.FirstDaySem.Month : 9;//сентябрь по умолчанию
            SetMouthsHeader(table, mountStart, 0, 1, 5);

            table.Cell(0, 6).Write("День");
            table.Cell(0, 7).Write("Часы");
            table.Cell(0, 8).Write(group);
        }

        private void SetDaysTable(WordTable table, int dayStart, int columnIndex, int rowIndexStart, int rowIndexEnd)
        {
            for (int counterRow = rowIndexStart, counterDay = dayStart; counterRow <= rowIndexEnd; counterRow++, counterDay++)
            {
                table.Cell(counterRow, columnIndex).Write(GetDayName(counterDay));
            }
        }

        private void SetMouthsHeader(WordTable table, int mouthStart, int rowIndex, int columnIndexStart, int columnIndexEnd)
        {
            for (int counterClmn = columnIndexStart, counterMouth = mouthStart; counterClmn <= columnIndexEnd; counterClmn++)
            {
                table.Cell(rowIndex, counterClmn).Write(GetMouthName(counterMouth));
                counterMouth = counterMouth == 12 ? 1 : counterMouth + 1;
            }
        }

        enum dwdwdc { d, dq, frf };

        private static string GetDayName(int dayNumber)
        {
            switch (dayNumber)
            {
                case 1: return "Пон.";
                case 2: return "Вт.";
                case 3: return "Ср.";
                case 4: return "Четв.";
                case 5: return "Пят.";
                case 6: return "Суб.";
                case 7: return "Вос.";
                default: return @"DAY N\A";
            }
        }

        private static string GetMouthName(int monthNumber)
        {
            //Янв, Фев, Мар, Апр, Май, Июн, Июл, Авг, Сен, Окт, Ноя, Дек
            switch (monthNumber)
            {
                case 1: return "Янв";         case 2: return "Фев";         case 3: return "Мар";
                case 4: return "Апр";         case 5: return "Май";         case 6: return "Июн";
                case 7: return "Июл";         case 8: return "Авг";         case 9: return "Сен";
                case 10: return "Окт";        case 11: return "Ноя";        case 12: return "Дек";
                default: return @"MOUTH N\A";
            }
        }

        private void CreateReport(WordDocument wordDocument)
        {
                // Сохраняем и открываем файл
                wordDocument.SaveToFile(fileName);

                // Запускаем связанную с этим расширением программу
                Process.Start(fileName);
        }
    }
}
