
/*--------------------------------------------------------------------------------*
   Filename:  TableGUIManager.cs
   Tool:      objectiF, CSharpSSvr V7.1.23
 *--------------------------------------------------------------------------------*/
//..end "File Description"

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

using MyShedule.Dictionaries;
using MyShedule.ScheduleClasses;

namespace MyShedule
{
	public static class TableGUIManager
	{
		private static string emptyLesson = "Нет занятия";
		private static string splitLine = "-----------------------------------------------------";
		private static string splitLine2 = "--------------------------------------------------------------------";
		
		//private static string splitLine = "----------------------ҩҨҩ-----------------------";
		
		#region INITIALIZE COLUMNS DATAGRIDVIEW
		
		/// <summary>Инициализировать стобцы таблицы и задать стиль столбцам</summary>
		/// <param name="table">таблица</param> 
		/// <param name="countWeeks">количество недель расписания которые должна отобразиться в таблице</param>
		/// <param name="countDays">количество дней расписания которые должна отобразиться в таблице</param>
		/// <returns>Созданная таблица</returns>
		public static DataGridView InitializeGrid(DataGridView table, int countWeeks, int countDays)
		{
		    table.Columns.Clear();
		    InitLeftColumns(table);
		    InitCalendarColumns(table, countWeeks, countDays);
		    SetStyleTable(table);
		
		    return table;
		}
		
		private static void InitCalendarColumns(DataGridView table, int countWeeks, int countDays)
		{
		    for (int week = 1; week <= countWeeks; week++)
		    {
		        DataGridViewTextBoxColumn clmn = new DataGridViewTextBoxColumn();
		        for (int day = 1; day <= countDays; day++)
		        {
		            clmn = new DataGridViewTextBoxColumn()
		            {
		                HeaderText = ScheduleTime.GetDayDescription((ScheduleClasses.Day)day).ToUpper(),
		                Name = "w" + week.ToString() + "d" + day.ToString(),
		                Width = 220,
		                ReadOnly = true,
		                SortMode = DataGridViewColumnSortMode.NotSortable
		            };
		
		            clmn.DefaultCellStyle.BackColor = (day > (int)ScheduleClasses.Day.Friday) ? 
		                Color.LightBlue : new DataGridViewCellStyle().BackColor;
		
		            clmn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
		
		            table.Columns.Add(clmn);
		        }
		        clmn.DividerWidth = 5;
		    }
		}
		
		private static void InitLeftColumns(DataGridView table)
		{
		    DataGridViewCellStyle style = new DataGridViewCellStyle()
		    { 
		        BackColor = Color.LightYellow,                                                                        
		        Alignment = DataGridViewContentAlignment.MiddleCenter
		    };
		
		    table.Columns.Add(new DataGridViewTextBoxColumn()
		    {
		        Frozen = true,
		        Width = 150,
		        ReadOnly = true,
		        Name = "name",
		        HeaderText = "Преподаватель".ToUpper(),
		        SortMode = DataGridViewColumnSortMode.NotSortable,
		        DefaultCellStyle = style
		    });
		
		    table.Columns.Add(new DataGridViewTextBoxColumn()
		    {
		        HeaderText = "Часы".ToUpper(),
		        Name = "hour",
		        DataPropertyName = "Hour",
		        Frozen = true,
		        ReadOnly = true,
		        Width = 45,
		        SortMode = DataGridViewColumnSortMode.NotSortable,
		        DefaultCellStyle = style
		    });
		}
		
		private static void SetStyleTable(DataGridView table)
		{
		    table.AutoGenerateColumns = false;
		    table.AllowUserToAddRows = false;
		    table.RowHeadersWidth = 25;
		    table.RowsDefaultCellStyle.WrapMode = DataGridViewTriState.True;
		    table.RowsDefaultCellStyle.Font = new System.Drawing.Font(FontFamily.GenericSansSerif, 9);
		}
		
		#endregion
		
		#region FILL DATAGRIDVIEW
		
		private static bool IsLessonsEqualAndNonEmpty(ScheduleLesson item1, ScheduleLesson item2)
		{
		    return item1 != null && item2 != null && item1.Discipline == item2.Discipline &&
		           item1.Room == item2.Room && item1.Teacher == item2.Teacher && item1.Type == item2.Type;
		}
		
		private static DataGridViewRow CreateNewRow(DataGridView table, string nameElementProjection, string hourDescription)
		{
		    DataGridViewRow row = new DataGridViewRow(); 
		    row.Height = 130;
		    row.CreateCells(table);
		    row.Cells[0].Value = nameElementProjection;
		    row.Cells[1].Value = hourDescription;
		    return row;
		}
		
		private static ScheduleTime GetTimeAfterTwoWeek(ScheduleTime now)
		{
		    ScheduleTime afterTwoWeek = now.Copy();
		    afterTwoWeek.Week = (Week)(afterTwoWeek.WeekNumber + 2);
		    return afterTwoWeek;
		}
		
		private static string GetCellContentIdenticalLessons(ScheduleClasses.View view, ScheduleLesson item)
		{
		    return GetLessonInfoFormat1(view, item);
		}
		
		private static Employment FindEmployment(ScheduleClasses.View view, Employments items, string name, ScheduleTime time)
		{
		    switch (view)
		    {
		        case ScheduleClasses.View.Teacher: return items.Teachers.GetItem(name, time);
		
		        case ScheduleClasses.View.Group: return items.Groups.GetItem(name, time);
		
		        case ScheduleClasses.View.Room: return items.Rooms.GetItem(name, time);
		
		        default: return null;
		    }
		}
		
		private static void SetElementView(List<ScheduleLesson> LessonsByView, string nameElementProjection,
				ScheduleClasses.View view, ScheduleWeeks Shedule, DataGridView table, Employments Employments, bool watchAll)
		{
		    for (int Hour = 1; Hour <= Shedule.Setting.CountLessonsOfDay; Hour++)
		    {
		        DataGridViewRow row = CreateNewRow(table, nameElementProjection, ScheduleTime.GetHourDiscription(Hour));
		
		        for (int Week = 1, CellIndex = 2; Week <= Shedule.Setting.CountWeeksShedule; Week++)
		        {
		            for (int Day = 1; Day <= Shedule.Setting.CountDaysEducationWeek; Day++, CellIndex++)
		            {
		                //время занятия на 1-2 недели
		                ScheduleTime Time1 = new ScheduleTime((Week)Week, (ScheduleClasses.Day)Day, Hour);
		                //время занятия на 3-4 недели
		                ScheduleTime Time2 = GetTimeAfterTwoWeek(Time1);
		
		                //занятие на 1-2 недели
		                ScheduleLesson item1 = Shedule.FindLessonInList(LessonsByView, Time1);
		                //занятие на 3-4 недели
		                ScheduleLesson item2 = Shedule.FindLessonInList(LessonsByView, Time2);
		
		                string Room1 = item1 != null ? item1.Room : String.Empty;
		                string Room2 = item2 != null ? item2.Room : String.Empty;
		                
		                Employment employmentCell = FindEmployment(view, Employments, nameElementProjection, Time1);
		
		                string cellContent = item1 == null && item2 == null ? String.Empty : IsLessonsEqualAndNonEmpty(item1, item2) ? 
		                        GetCellContentIdenticalLessons(view, item1) : GetCellContentDiffrentLessons(view, item1, item2);
		
		                // задать значения ячейки в  
		                DataGridViewCell cell = row.Cells[CellIndex];
		                cell.Value = cellContent;
		                cell.Tag = new SchedulePointer(Time1, Time2, Room1, Room2);
		                // задать цвет и стиль ячейке
		                if (cellContent != String.Empty)
		                    cell.Style.BackColor = IsLessonsEqualAndNonEmpty(item1, item2) ? Color.LightGreen : Color.LightGreen;
		
		                if (employmentCell != null && employmentCell.Reason == ReasonEmployment.UserBlocked)
		                    SetCellBlockedStyle(cell);
		            }
		        }
		
		        if (watchAll || (!watchAll && Shedule.Lessons.Where(x => x.Hour == Hour && !x.IsEmpty).Count() > 0))
		            table.Rows.Add(row);
		    }
		
		    //добавить разделитель
		    int index = table.Rows.GetLastRow(DataGridViewElementStates.None);
		    if (index >= 0)
		        table.Rows[index].DividerHeight = 3;
		}
		
		private static string GetLessonInfoFormat1(ScheduleClasses.View view, ScheduleLesson lesson)
		{
		    string format = "{0} \n {1} \n {2} \n {3}";
		    switch (view)
		    {
		        case ScheduleClasses.View.Teacher: return String.Format(format, lesson.Discipline, ScheduleLessonType.Description(lesson.Type), lesson.GroupsDescription, lesson.Room);
		
		        case ScheduleClasses.View.Discipline: return String.Format(format, lesson.GroupsDescription, ScheduleLessonType.Description(lesson.Type), lesson.Teacher, lesson.Room);
		
		        case ScheduleClasses.View.Group: return String.Format(format, lesson.Discipline, ScheduleLessonType.Description(lesson.Type), lesson.Teacher, lesson.Room);
		
		        case ScheduleClasses.View.Room: return String.Format(format, lesson.Discipline, ScheduleLessonType.Description(lesson.Type), lesson.Teacher, lesson.GroupsDescription);
		
		        default: return String.Empty;
		    }
		}
		
		private static string GetLessonInfoFormat2(ScheduleClasses.View view, ScheduleLesson lesson)
		{
		    string format = "{0} , {1}\n{2} , {3}\n{4}";
		    switch (view)
		    {
		        case ScheduleClasses.View.Teacher: return String.Format(format, lesson.Discipline, ScheduleLessonType.Description(lesson.Type), 
		            lesson.GroupsDescription, lesson.Room, lesson.DatesDescription);
		
		        case ScheduleClasses.View.Discipline: return String.Format(format, lesson.GroupsDescription, ScheduleLessonType.Description(lesson.Type),
		            lesson.Teacher, lesson.Room, lesson.DatesDescription);
		
		        case ScheduleClasses.View.Group: return String.Format(format, lesson.Discipline, ScheduleLessonType.Description(lesson.Type),
		            lesson.Teacher, lesson.Room, lesson.DatesDescription);
		
		        case ScheduleClasses.View.Room: return String.Format(format, lesson.Discipline, ScheduleLessonType.Description(lesson.Type),
		            lesson.Teacher, lesson.GroupsDescription, lesson.DatesDescription);
		
		        default: return String.Empty;
		    }
		}
		
		private static string GetCellContentDiffrentLessons(ScheduleClasses.View view, ScheduleLesson item1, ScheduleLesson item2)
		{
		    string content = String.Empty;
		
		    content += item1 != null ? GetLessonInfoFormat2(view, item1) : emptyLesson;
		    content += Environment.NewLine + splitLine + Environment.NewLine;
		    content += item2 != null ? GetLessonInfoFormat2(view, item2) : emptyLesson;
		
		    return content;
		}
		
		private static IEnumerable<string> GetNamesItemsView(ScheduleClasses.View view, EducationLoadAdapter adapter, IEnumerable<ScheduleRoom> rooms)
		{
		    switch (view)
		    {
		        case ScheduleClasses.View.Teacher: return adapter.NamesTeachers;
		
		        case ScheduleClasses.View.Discipline: return adapter.NamesDisciplines;
		
		        case ScheduleClasses.View.Group: return adapter.NamesGroups;
		
		        case ScheduleClasses.View.Room: return (from r in rooms select r.Name).Distinct().ToList();
		
		        default: return new List<string>();
		    }
		}
		
		public static DataGridView FillDataGrid(ScheduleWeeks Shedule, DataGridView table,
			   ScheduleClasses.View view, EducationLoadAdapter adapter, IEnumerable<ScheduleRoom> Rooms, bool WatchAll)
		{
		    table.Rows.Clear();
		    //задать значение заколовку с именами элементов проекции
		    table.Columns["name"].HeaderText = new ScheduleView(view).Name.ToUpper();
		    //добавить в таблицу все занятия всех элементов проекции расписания
		    foreach (string elementView in GetNamesItemsView(view, adapter, Rooms).ToList()) 
		    {
		        //получить все занятия определенного элемента проекции расписания
		        List<ScheduleLesson> LessonsByView = Shedule.GetLessonsByView(view, elementView).ToList();
		        //добавить в таблицу все занятия элемента
		        SetElementView(LessonsByView, elementView, view, Shedule, table, Shedule.Employments, WatchAll);       
		    }
		    //проставить даты в заголовки столбцов таблицы
		    SetDatesColumns(table, Shedule);
		    return table;
		}
		
		private static void SetDatesColumns(DataGridView table, ScheduleWeeks Shedule)
		{
		    for (int WeekCounter = 1, CellCounter = 2; WeekCounter <= Shedule.Setting.CountWeeksShedule; WeekCounter++)
		    {
		        for (int DayCounter = 1; DayCounter <= Shedule.Setting.CountDaysEducationWeek; DayCounter++, CellCounter++)
		        {                   
		            table.Columns[CellCounter].HeaderText = ScheduleTime.GetDayDescription((ScheduleClasses.Day)DayCounter).ToUpper() + 
		                Environment.NewLine + Shedule.GetDay((Week)WeekCounter, (ScheduleClasses.Day)DayCounter).DatesDescription;
		        }
		    }
		}
		
		public static void SetCellBlockedStyle(DataGridViewCell cell)
		{
		    cell.Style.BackColor = Color.LightPink;
		    cell.Value = "Занято";
		    cell.Style.Font = new Font(FontFamily.GenericSansSerif, 12);
		    cell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
		}
		
		#endregion
	}
}