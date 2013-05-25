using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MyShedule.ScheduleClasses;
using NUnit.Framework;
using Yogesh.ExcelXml;

namespace MyShedule.ReportExp
{
    [TestFixture]
    class XLSExpTester
    {
        XLSExporter exp = new XLSExporter();
        
        [Test]
        public void dayNameSetting_testFirstWeek()
        {
            exp.lsnCntMult = 6;

            ExcelXmlWorkbook book = new ExcelXmlWorkbook();
            Worksheet sheet = book[0];

            string[] daysOfWeek = new string[6] 
            { "понедельник", "вторник", "среда", "четверг", "пятница", "суббота" };

            exp.SetDaysTable(sheet, 1, 6, 1, 6);
            for (int i = 0; i < 6; i++)
                Assert.AreEqual(daysOfWeek[i], sheet[6, i * exp.lsnCntMult * 5 + 1].Value.ToString());
        }

        [Test]
        public void dayNameSetting_testWrongStartDay()
        {
            exp.lsnCntMult = 6;

            ExcelXmlWorkbook book = new ExcelXmlWorkbook();
            Worksheet sheet = book[0];

            exp.SetDaysTable(sheet, 8, 6, 1, 6);
            for (int i = 0; i < 6; i++)
                Assert.AreEqual(@"DAY N\A", sheet[6, 0 * exp.lsnCntMult * 5 + 1].Value.ToString());
        }

        [Test]
        public void dayFilling_testFirstWeekMonday()
        {
            exp.lsnCntMult = 6;
            exp.Shedule = loadTestingShedule("schedule1.xml");

            ExcelXmlWorkbook book = new ExcelXmlWorkbook();
            Worksheet sheet = book[0];

            int monthStart = 9;

            exp.FillDays(sheet, monthStart, 1, Week.FirstWeek, Week.TreeWeek);

            List<List<string>> firstMonDates = new List<List<string>>();

            List<string> septMon = new List<string> {"1", "15", "29"};
            List<string> octMon = new List<string> {"13", "27"};
            List<string> novMon = new List<string> {"10", "24"};
            List<string> decMon = new List<string> {"8", "22"};

            firstMonDates.Add(septMon);
            firstMonDates.Add(octMon);
            firstMonDates.Add(novMon);
            firstMonDates.Add(decMon);

            Assert.AreEqual("1", sheet[1, 1].Value);

            int j = 1;
            foreach(List<string> daysOfMonth in firstMonDates)
            {
                for(int i=0; i<daysOfMonth.Count; i++)
                {
                    Assert.AreEqual(daysOfMonth[i],sheet[j,i + 1].Value);
                }
                j++;
            }
        }

        [Test]
        public void dayFilling_testSecondWeekTuesday()
        {
            exp.lsnCntMult = 6;
            exp.Shedule = loadTestingShedule("schedule1.xml");

            ExcelXmlWorkbook book = new ExcelXmlWorkbook();
            Worksheet sheet = book[0];

            int monthStart = 9;

            exp.FillDays(sheet, monthStart, 6 * exp.lsnCntMult * 5 + 1, Week.SecondWeek, Week.FourWeek);

            List<List<string>> secondTueDates = new List<List<string>>();

            List<string> septTue = new List<string> { "9", "23" };
            List<string> octTue = new List<string> { "7", "21" };
            List<string> novTue = new List<string> { "4", "18" };
            List<string> decTue = new List<string> { "2", "16", "30" };

            secondTueDates.Add(septTue);
            secondTueDates.Add(octTue);
            secondTueDates.Add(novTue);
            secondTueDates.Add(decTue);

            int j = 1;
            foreach (List<string> daysOfMonth in secondTueDates)
            {
                for (int i = 0; i < daysOfMonth.Count; i++)
                {
                    Assert.AreEqual(daysOfMonth[i], sheet[j, i + 211].Value);
                }
                j++;
            }
        }

        [Test]
        public void testTextHeaderTableSetting()
        {
            ExcelXmlWorkbook book = new ExcelXmlWorkbook();
            Worksheet sheet = book[0];

            exp.SetTextHeaderTable(sheet);
            Assert.AreEqual("День",sheet[6,0].Value);
            Assert.AreEqual("Часы",sheet[7,0].Value);
        }

        [Test]
        public void monthHeaderSetting_testWrongClmnNumber()
        {
            ExcelXmlWorkbook book = new ExcelXmlWorkbook();
            Worksheet sheet = book[0];

            exp.SetMonthsHeader(sheet, 3, 0, 4, 1);
            for (int i = 0; i < 4; i++)
            {
                Assert.AreEqual(null, sheet[i + 1, 0].Value);
            }        
        }

        [Test]
        public void monthHeaderSetting_testFirstSem()
        {
            ExcelXmlWorkbook book = new ExcelXmlWorkbook();
            Worksheet sheet = book[0];

            string[] monthes = new string[10] { "сентябрь", "октябрь", "ноябрь", "декабрь", "январь", "февраль", "март", "апрель", "май", "июнь" };

            exp.SetMonthsHeader(sheet, 9, 0, 1, 4);
            for (int i = 0; i < 4; i++)
            {
                Assert.AreEqual(monthes[i], sheet[i + 1, 0].Value);
            }
        }

        [Test]
        public void monthHeaderSetting_testSecondSem()
        {
            ExcelXmlWorkbook book = new ExcelXmlWorkbook();
            Worksheet sheet = book[0];

            string[] monthes = new string[10] 
            { "сентябрь", "октябрь", "ноябрь", "декабрь", "январь", "февраль", "март", "апрель", "май", "июнь" };

            exp.SetMonthsHeader(sheet, 2, 0, 1, 5);
            for (int i = 5; i <= 9; i++)
            {
                Assert.AreEqual(monthes[i], sheet[i - 4, 0].Value);
            }
        }

        [Test]
        public void monthHeaderSetting_testWrongStartMonthNum()
        {
            ExcelXmlWorkbook book = new ExcelXmlWorkbook();
            Worksheet sheet = book[0];

            exp.SetMonthsHeader(sheet, 13, 0, 1, 4);
            for (int i = 0; i < 4; i++)
            {
                Assert.AreEqual(@"MOUTH N\A", sheet[i + 1, 0].Value);
            }
        }

        [Test]
        public void entitiesNameHeader_testGroupNames()
        {
            exp.lsnCntMult = 6;
            exp.Shedule = loadTestingShedule("schedule1.xml");

            exp.View = View.Group;

            ExcelXmlWorkbook book = new ExcelXmlWorkbook();
            Worksheet sheet = book[0];
            
            int i = 8;
            foreach (string name in exp.Shedule.GroupsNames)
            {
                exp.FillEntitiesData(sheet, name, i);
                i++;
            }

            i = 8;
            foreach (string name in exp.Shedule.GroupsNames)
            {
                Assert.AreEqual(name, sheet[i, 0].Value);
                i++;
            }
        }

        [Test]
        public void entitiesNameHeader_testTeachersNames()
        {
            exp.lsnCntMult = 6;
            exp.Shedule = loadTestingShedule("schedule1.xml");

            exp.View = View.Group;

            ExcelXmlWorkbook book = new ExcelXmlWorkbook();
            Worksheet sheet = book[0];

            int i = 8;
            foreach (string name in exp.Shedule.TeachersNames)
            {
                exp.FillEntitiesData(sheet, name, i);
                i++;
            }

            i = 8;
            foreach (string name in exp.Shedule.TeachersNames)
            {
                Assert.AreEqual(name, sheet[i, 0].Value);
                i++;
            }
        }

        [Test]
        public void entitiesNameHeader_testRoomsNames()
        {
            exp.lsnCntMult = 6;
            exp.Shedule = loadTestingShedule("schedule1.xml");

            exp.View = View.Group;

            ExcelXmlWorkbook book = new ExcelXmlWorkbook();
            Worksheet sheet = book[0];

            int i = 8;
            foreach (string name in exp.Shedule.RoomsNames)
            {
                exp.FillEntitiesData(sheet, name, i);
                i++;
            }

            i = 8;
            foreach (string name in exp.Shedule.RoomsNames)
            {
                Assert.AreEqual(name, sheet[i, 0].Value);
                i++;
            }
        }

        [Test]
        public void entitiesNameHeader_testDisciplinesNames()
        {
            exp.lsnCntMult = 6;
            exp.Shedule = loadTestingShedule("schedule1.xml");

            exp.View = View.Group;

            ExcelXmlWorkbook book = new ExcelXmlWorkbook();
            Worksheet sheet = book[0];

            int i = 8;
            foreach (string name in exp.Shedule.DisciplinesNames)
            {
                exp.FillEntitiesData(sheet, name, i);
                i++;
            }

            i = 8;
            foreach (string name in exp.Shedule.DisciplinesNames)
            {
                Assert.AreEqual(name, sheet[i, 0].Value);
                i++;
            }
        }

        [Test]
        public void hoursFilling_testFirstDay()
        {
            exp.lsnCntMult = 6;

            ExcelXmlWorkbook book = new ExcelXmlWorkbook();
            Worksheet sheet = book[0];  

            exp.FillHours(sheet, 1);
            exp.FillHours(sheet, 6 * exp.lsnCntMult * 5 + 1);
            
            string[] hours = new string[6] 
                {"1-2", "3-4", "5-6", "7-8", "9-10", "11-12"};

            for (int i = 0; i < 6; i++)
            {
                Assert.AreEqual(hours[i], sheet[7, i * 5 + 1].Value);
            }
        }

        [Test]
        public void hoursFilling_testTwoWeeks()
        {
            exp.lsnCntMult = 6;

            ExcelXmlWorkbook book = new ExcelXmlWorkbook();
            Worksheet sheet = book[0];

            exp.FillHours(sheet, 1);
            exp.FillHours(sheet, 6 * exp.lsnCntMult * 5 + 1);

            string[] hours = new string[6] { "1-2", "3-4", "5-6", "7-8", "9-10", "11-12" };

            for (int i = 1, j = 0; i <= 6 * exp.lsnCntMult * 5; i += 5, j++)
            {
                Assert.AreEqual(hours[j], sheet[7, i].Value);
                if (j == 5)
                    j = -1;
            }

            for (int i = 1; i <= 6 * exp.lsnCntMult * 5; i += 5)
            {
                Assert.AreEqual(sheet[7, i].Value, sheet[7, i + 6 * exp.lsnCntMult * 5].Value);
            }
        }

        [Test]
        public void testEntitiesDataFilling()
        {
            exp.lsnCntMult = 6;
            exp.Shedule = loadTestingShedule("schedule1.xml");
            exp.View = View.Group;

            int monthStart = exp.Shedule.FirstDaySem.Month;

            ExcelXmlWorkbook book = new ExcelXmlWorkbook();
            Worksheet sheet = book[0];

            int column = 8;
            foreach (string name in exp.Shedule.GroupsNames)
            {
                List<ScheduleLesson> tmp = exp.Shedule.GetLessonsByView(exp.View, name).ToList();

                exp.FillLessons(sheet, monthStart, 1, column, tmp, Week.FirstWeek, Week.TreeWeek);
                column += 2;
            }

            Assert.AreEqual("Организация ЭВМ",sheet[8,1].Value);
            Assert.AreEqual("(Лекция)", sheet[8, 2].Value);
            Assert.AreEqual("01.09., 29.09., 27.10., 24.11., 22.12., ", sheet[8, 3].Value);
            Assert.AreEqual("Андреев А.Е.", sheet[8, 4].Value);
            Assert.AreEqual("В-1301", sheet[8, 5].Value);

            Assert.AreEqual("Организация ЭВМ", sheet[9, 1].Value);
            Assert.AreEqual("(Практика)", sheet[9, 2].Value);
            Assert.AreEqual("15.09., 13.10., 10.11., 08.12., 05.01., ", sheet[9, 3].Value);
            Assert.AreEqual("Забалуева А.Ф.", sheet[9, 4].Value);
            Assert.AreEqual("В-1301", sheet[9, 5].Value);

            Assert.AreEqual("Организация ЭВМ", sheet[10, 1].Value);
            Assert.AreEqual("(Лекция)", sheet[10, 2].Value);
            Assert.AreEqual("01.09., 29.09., 27.10., 24.11., 22.12., ", sheet[10, 3].Value);
            Assert.AreEqual("Андреев А.Е.", sheet[10, 4].Value);
            Assert.AreEqual("В-1301", sheet[10, 5].Value);

            Assert.AreEqual(null, sheet[11, 1].Value);
            Assert.AreEqual(null, sheet[11, 2].Value);
            Assert.AreEqual(null, sheet[11, 3].Value);
            Assert.AreEqual(null, sheet[11, 4].Value);
            Assert.AreEqual(null, sheet[11, 5].Value);
        }

        ScheduleWeeks loadTestingShedule(string FileName)
        {
            ScheduleWeeks Shedule = ScheduleSerializer.ReadData(FileName);

            if (Shedule != null)
                Shedule.Employments.Clear();

            return Shedule;
        }
    }
}
