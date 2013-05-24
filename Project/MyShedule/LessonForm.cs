//..begin "File Description"
/*--------------------------------------------------------------------------------*
   Filename:  LessonForm.cs
   Tool:      objectiF, CSharpSSvr V7.1.23
 *--------------------------------------------------------------------------------*/
//..end "File Description"

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ScheduleDictionaries;
using ScheduleClasses;


namespace MyShedule
{	
	public enum scheduleType
	{
		leesons,
		exams
	}
	
	public partial class LessonForm
	:	System.Windows.Forms.Form
	{
		public LessonForm(scheduleType type)
		{
		    InitializeComponent();
		    Type = type;
		    this.Load += new EventHandler(LessonForm_Load);
		}
		
		public ScheduleWeeks Shedule;
		scheduleType Type;
		public dsShedule ds;
		public EducationLoadAdapter Adapter;
		public Employments Employments;
		public List<ScheduleRoom> Rooms;
		public ScheduleTime Time1;
		public ScheduleTime Time2;
		public ScheduleLesson Lesson1;
		public ScheduleLesson Lesson2;
		public int curClmn;
		
		void LessonForm_Load(object sender, EventArgs e)
		{
		    InitControlsForm();
		
		    //SetTeachersUI();
		
		    SetTeachersUI(cmbTeacher1, Lesson1, Time1);
		    SetTeachersUI(cmbTeacher2, Lesson2, Time2);
		
		    SetDisciplinesUI(cmbDiscipline1, Lesson1);
		    SetDisciplinesUI(cmbDiscipline2, Lesson2);
		
		    SetRoomsUI(cmbRooms1,Lesson1,Time1);
		    SetRoomsUI(cmbRooms2, Lesson2, Time2);
		
		    SetLessonTypesUI(cmbTypeLesson1, Lesson1);
		    SetLessonTypesUI(cmbTypeLesson2, Lesson2);
		
		    SetGroupsUI(lvSelectGroup1, lvGroups1, Lesson1, Time1);
		    SetGroupsUI(lvSelectGroup2, lvGroups2, Lesson2, Time2);
		
		    SetDatesUI(lvDates1, Lesson1);
		    SetDatesUI(lvDates2, Lesson2);
		
		    if (Shedule.Days[curClmn - 2].Dates[0].DayOfWeek >= Shedule.FirstDaySem.DayOfWeek ||
		        Shedule.Days[curClmn - 2].Week != Shedule.getWeekNumberOfDay(Shedule.FirstDaySem))
		    {
		        dtpDate1.Value = Shedule.Days[curClmn - 2].Dates[0];
		        dtpDate2.Value = Shedule.Days[curClmn - 2].Dates[1];
		
		        if (lvDates1.Items.Count == 0)
		        {
		            int counter = 0;
		            foreach (DateTime date in Shedule.Days[curClmn - 2].Dates)
		            {
		                if (counter % 2 == 0)
		                {
		                    lvDates1.Items.Add(new ListViewItem(String.Format("{0:dd} {0:MMMM} {0:yyyy}", date)));
		                }
		                counter++;
		            }
		        }
		
		        if (lvDates2.Items.Count == 0)
		        {
		            int counter = 0;
		            foreach (DateTime date in Shedule.Days[curClmn - 2].Dates)
		            {
		                if (counter % 2 != 0)
		                {
		                    lvDates2.Items.Add(new ListViewItem(String.Format("{0:dd} {0:MMMM} {0:yyyy}", date)));
		                }
		                counter++;
		            }
		        }
		    }
		    else
		    {
		        dtpDate2.Value = Shedule.Days[curClmn - 2].Dates[0];
		        dtpDate1.Value = Shedule.Days[curClmn - 2].Dates[1];
		
		        if (lvDates2.Items.Count == 0)
		        {
		            int counter = 0;
		            foreach (DateTime date in Shedule.Days[curClmn - 2].Dates)
		            {
		                if (counter % 2 == 0)
		                {
		                    lvDates2.Items.Add(new ListViewItem(String.Format("{0:dd} {0:MMMM} {0:yyyy}", date)));
		                }
		                counter++;
		            }
		        }
		
		        if (lvDates1.Items.Count == 0)
		        {
		            int counter = 0;
		            foreach (DateTime date in Shedule.Days[curClmn - 2].Dates)
		            {
		                if (counter % 2 != 0)
		                {
		                    lvDates1.Items.Add(new ListViewItem(String.Format("{0:dd} {0:MMMM} {0:yyyy}", date)));
		                }
		                counter++;
		            }
		        }
		    }
		}
		
		private void SetDatesUI(ListView dates, ScheduleLesson lesson)
		{
		    if (lesson == null)
		       return;
		
		    dates.Items.Clear();
		    foreach (DateTime date in lesson.Dates)
		    {
		        string title = String.Format("{0:dd} {0:MMMM} {0:yyyy}", date);
		        dates.Items.Add(new ListViewItem(title) { ImageIndex = 1, Tag = date } );
		    }
		    
		}
		
		private void SetGroupsUI(ListView ChooiceGroups, ListView AccessGroups,
				ScheduleLesson lesson, ScheduleTime time)
		{
		    List<string> AccGroups = new List<string>();
		    List<string> ChcGroups = new List<string>();
		
		    foreach (string group in Adapter.NamesGroups)
		        if (Employments.Groups.IsFree(group, time))
		            AccGroups.Add(group);
		
		    if (lesson != null && !lesson.IsEmpty)
		        ChcGroups.AddRange(lesson.Groups);
		
		    foreach (string group in ChcGroups)
		        ChooiceGroups.Items.Add(new ListViewItem(group) { ImageIndex = 0 });
		
		    foreach (string group in AccGroups)
		        AccessGroups.Items.Add(new ListViewItem(group) { ImageIndex = 0 });
		}
		
		private void SetLessonTypesUI(ComboBox cmb, ScheduleLesson lesson)
		{
		    BindingSource bs = new BindingSource();
		    if (Type == scheduleType.leesons)
		        bs.DataSource = ScheduleLessonType.GetBaseType();
		    else
		        bs.DataSource = SheduleExamType.GetBaseType();
		    cmb.DisplayMember = "Detail";
		    cmb.ValueMember = "TypeCode";
		    cmb.DataSource = bs;
		    cmb.Text = (lesson != null && !lesson.IsEmpty) ? ScheduleLessonType.Description(lesson.Type) : String.Empty;
		    if (lesson == null || lesson.IsEmpty)
		        cmb.SelectedItem = null;
		}
		
		private void SetRoomsUI(ComboBox cmb, ScheduleLesson lesson, ScheduleTime time)
		{
		    List<string> rooms = new List<string>();
		    foreach (ScheduleRoom room in Rooms)
		        if (Employments.Rooms.IsFree(room.Name, time))
		            rooms.Add(room.Name);
		
		    if (lesson != null)
		        rooms.Add(lesson.Room);
		    BindingSource bs = new BindingSource();
		    bs.DataSource = rooms;
		    cmb.DataSource = bs;
		    cmb.Text = (lesson != null && !lesson.IsEmpty) ? lesson.Room : String.Empty;
		    if (lesson == null || lesson.IsEmpty)
		        cmb.SelectedItem = null;
		}
		
		private void SetDisciplinesUI(ComboBox cmb, ScheduleLesson lesson)
		{
		    BindingSource bs = new BindingSource();
		    bs.DataSource = Adapter.NamesDisciplines;
		    cmb.DataSource = bs;
		    cmb.Text = (lesson != null && !lesson.IsEmpty) ? lesson.Discipline : String.Empty;
		    if (lesson == null || lesson.IsEmpty)
		        cmb.SelectedItem = null;
		}
		
		private void SetTeachersUI(ComboBox cmb, ScheduleLesson lesson, ScheduleTime time)
		{
		    List<string> Teachers = new List<string>();
		    foreach (string teacher in Adapter.NamesTeachers)
		        if (Employments.Teachers.IsFree(teacher, time))
		            Teachers.Add(teacher);
		
		    if (lesson != null)
		        Teachers.Add(lesson.Teacher);
		    BindingSource bs = new BindingSource();
		    bs.DataSource = Teachers;
		    cmb.DataSource = bs;
		    cmb.Text = (lesson != null && !lesson.IsEmpty) ? lesson.Teacher : String.Empty;
		    if (lesson == null || lesson.IsEmpty)
		        cmb.SelectedItem = null;
		}
		
		private void InitControlsForm()
		{
		    ImageList imgs = new ImageList();
		    imgs.Images.Add(MyShedule.Properties.Resources.group);
		    imgs.Images.Add(MyShedule.Properties.Resources.date);
		    imgs.Images.Add(MyShedule.Properties.Resources.note);
		
		    lvSelectGroup1.View = System.Windows.Forms.View.Details;
		    lvSelectGroup1.Columns.Clear();
		    lvSelectGroup1.Columns.Add("Выбранные", 80);
		    lvSelectGroup1.SmallImageList = imgs;
		    lvSelectGroup1.LabelWrap = true;
		
		    lvGroups1.View = System.Windows.Forms.View.Details;
		    lvGroups1.Columns.Clear();
		    lvGroups1.Columns.Add("Доступные", 80);
		    lvGroups1.SmallImageList = imgs;
		
		    lvDates1.View = System.Windows.Forms.View.Details;
		    lvDates1.Columns.Clear();
		    lvDates1.Columns.Add("Дата", 120);
		    lvDates1.SmallImageList = imgs;
		    lvDates1.HeaderStyle = ColumnHeaderStyle.None;
		
		    lvSelectGroup2.View = System.Windows.Forms.View.Details;
		    lvSelectGroup2.Columns.Clear();
		    lvSelectGroup2.Columns.Add("Выбранные", 80);
		    lvSelectGroup2.SmallImageList = imgs;
		
		    lvGroups2.View = System.Windows.Forms.View.Details;
		    lvGroups2.Columns.Clear();
		    lvGroups2.Columns.Add("Доступные", 80);
		    lvGroups2.SmallImageList = imgs;
		
		    lvDates2.View = System.Windows.Forms.View.Details;
		    lvDates2.Columns.Clear();
		    lvDates2.Columns.Add("Дата", 120);
		    lvDates2.SmallImageList = imgs;
		    lvDates2.HeaderStyle = ColumnHeaderStyle.None;
		
		    tabControl.ImageList = imgs;
		    tabControl.TabPages[0].ImageIndex = 2;
		    tabControl.TabPages[1].ImageIndex = 2;
		
		    this.lvDates1.SelectedIndexChanged += new EventHandler(lvDates1_SelectedIndexChanged);
		
		    this.AcceptButton = btnAccept;
		}
		
		private void btnAccept_Click(object sender, EventArgs e)
		{
		    if (!((cmbDiscipline1.Text != "" && cmbRooms1.Text != "" &&
		        cmbTypeLesson1.SelectedValue != null && cmbTeacher1.Text != "") || 
		        ((cmbDiscipline1.Text == "" && cmbRooms1.Text == "" &&
		        cmbTypeLesson1.SelectedValue == null && cmbTeacher1.Text == ""))))
		    {
		        MessageBox.Show("Введены не все \n\rзначения для 1-2 недели","Ошибка",MessageBoxButtons.OK,MessageBoxIcon.Error);
		        return;
		    }
		    if (!((cmbDiscipline2.Text != "" && cmbRooms2.Text != "" &&
		        cmbTypeLesson2.SelectedValue != null && cmbTeacher2.Text != "") ||
		        ((cmbDiscipline2.Text == "" && cmbRooms2.Text == "" &&
		        cmbTypeLesson2.SelectedValue == null && cmbTeacher2.Text == ""))))
		    {
		        MessageBox.Show("Введены не все \n\rзначения для 3-4 недели", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
		        return;
		    }
		
		    if(Lesson1 != null)
		        Employments.Remove(Lesson1.Teacher, Lesson1.Groups, Lesson1.Room, Time1);
		
		    if (Lesson1 == null)
		        Lesson1 = Shedule.GetLesson(Time1, cmbRooms1.Text);
		
		    if (Lesson1 != null)
		    {
		        Lesson1.Discipline = cmbDiscipline1.Text;
		
		        Lesson1.Room = cmbRooms1.Text;
		
		        Lesson1.Type = (LessonType)cmbTypeLesson1.SelectedValue;
		
		        Lesson1.Teacher = cmbTeacher1.Text;
		
		        Lesson1.Groups.Clear();
		        foreach (ListViewItem Item in lvSelectGroup1.Items)
		            Lesson1.Groups.Add(Item.Text);
		
		        Lesson1.Dates.Clear();
		        foreach (ListViewItem Item in lvDates1.Items)
		            Lesson1.Dates.Add(Convert.ToDateTime(Item.Text));
		
		        Employments.Add(Lesson1.Teacher, Lesson1.Groups, Lesson1.Room, Time1, ReasonEmployment.UnionLesson);
		    }
		
		    //-------------------------------------------
		    if (Lesson2 != null)
		       Employments.Remove(Lesson2.Teacher, Lesson2.Groups, Lesson2.Room, Time2);
		
		    if (Lesson2 == null)
		       Lesson2 = Shedule.GetLesson(Time2, cmbRooms2.Text);
		
		    if (Lesson2 != null)
		    {
		
		        Lesson2.Discipline = cmbDiscipline2.Text;
		
		        Lesson2.Room = cmbRooms2.Text;
		
		        Lesson2.Type = (LessonType)cmbTypeLesson2.SelectedValue;
		
		        Lesson2.Teacher = cmbTeacher2.Text;
		
		        Lesson2.Groups.Clear();
		        foreach (ListViewItem Item in lvSelectGroup2.Items)
		            Lesson2.Groups.Add(Item.Text);
		
		        Lesson2.Dates.Clear();
		        foreach (ListViewItem row in lvDates2.Items)
		            Lesson2.Dates.Add(Convert.ToDateTime(row.Text));
		
		        Employments.Add(Lesson2.Teacher, Lesson2.Groups, Lesson2.Room, Time2, ReasonEmployment.UnionLesson);
		    }
		
		    this.DialogResult = System.Windows.Forms.DialogResult.OK;
		
		    this.Close();
		}
		
		private void btnReject_Click(object sender, EventArgs e)
		{
		    this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		
		    this.Close();
		}
		
		private void btnAddGroup_Click_1(object sender, EventArgs e)
		{
		    if (lvGroups1.SelectedItems.Count > 0)
		    {
		        ListViewItem item = lvGroups1.SelectedItems[0];
		        string group = item.Text;
		        lvGroups1.Items.Remove(item);
		        lvSelectGroup1.Items.Add(group);
		    }
		}
		
		private void btnRemoveGroup_Click_1(object sender, EventArgs e)
		{
		    if (lvSelectGroup1.SelectedItems.Count > 0)
		    {
		        ListViewItem item = lvSelectGroup1.SelectedItems[0];
		        string group = item.Text;
		        lvSelectGroup1.Items.Remove(item);
		        lvGroups1.Items.Add(group);
		    }
		}
		
		private void btnAddAllGroups_Click_1(object sender, EventArgs e)
		{
		    foreach (ListViewItem item in lvGroups1.Items)
		    {
		        string group = item.Text;
		        lvGroups1.Items.Remove(item);
		        lvSelectGroup1.Items.Add(group);
		    }
		}
		
		private void btnRemoveAllGroups_Click_1(object sender, EventArgs e)
		{
		    foreach (ListViewItem item in lvSelectGroup1.Items)
		    {
		        string group = item.Text;
		        lvSelectGroup1.Items.Remove(item);
		        lvGroups1.Items.Add(group);
		    }
		}
		
		private void btnAddDate1_Click(object sender, EventArgs e)
		{
		    if (Lesson1 != null)
		    {
		        Lesson1.Dates.Add(dtpDate1.Value);
		        lvDates1.Items.Add(new ListViewItem(String.Format("{0:dd} {0:MMMM} {0:yyyy}", dtpDate1.Value)));
		    }
		}
		
		private void btnEditDate1_Click(object sender, EventArgs e)
		{
		    if (CurrentDate != null)
		    {
		        if (CurrentDate.Text != "")
		        {
		            DateTime date = Convert.ToDateTime(CurrentDate.Text);
		            CurrentDate.Text = String.Format("{0:dd} {0:MMMM} {0:yyyy}", dtpDate1.Value);
		            for (int i = 0; i < Lesson1.Dates.Count; i++)
		            {
		                if (Lesson1.Dates[i] == date)
		                    Lesson1.Dates[i] = dtpDate1.Value;
		            }
		        }
		    }
		}
		
		private void btnRemoveDate1_Click(object sender, EventArgs e)
		{
		    if (lvDates1.SelectedItems.Count > 0)
		    {
		        DateTime date = Convert.ToDateTime(lvDates1.SelectedItems[0].Text);
		        ListViewItem item = lvDates1.SelectedItems[0];
		        lvDates1.Items.Remove(item);
		        if (Lesson1 != null)
		        {
		            Lesson1.Dates.Remove(date);
		        }
		    }
		}
		
		private void btnAddDate2_Click(object sender, EventArgs e)
		{
		    if (Lesson2 != null)
		    {
		        Lesson2.Dates.Add(dtpDate2.Value);
		        lvDates2.Items.Add(new ListViewItem(String.Format("{0:dd} {0:MMMM} {0:yyyy}", dtpDate2.Value)));
		    }
		}
		
		private void btnEditDate2_Click(object sender, EventArgs e)
		{
		    if (CurrentDate != null)
		    {
		        if (CurrentDate.Text != "")
		        {
		            DateTime date = Convert.ToDateTime(CurrentDate.Text);
		            CurrentDate.Text = String.Format("{0:dd} {0:MMMM} {0:yyyy}", dtpDate1.Value);
		            for (int i = 0; i < Lesson1.Dates.Count; i++)
		            {
		                if (Lesson2.Dates[i] == date)
		                    Lesson2.Dates[i] = dtpDate2.Value;
		            }
		        }
		    }
		}
		
		private void btnRemoveDate2_Click(object sender, EventArgs e)
		{
		    if (lvDates2.SelectedItems.Count > 0)
		    {
		        DateTime date = Convert.ToDateTime(lvDates2.SelectedItems[0].Text);
		        ListViewItem item = lvDates2.SelectedItems[0];
		        lvDates2.Items.Remove(item);
		        if (Lesson2 != null)
		        {
		            Lesson2.Dates.Remove(date);
		        }
		    }
		}
		
		public ListViewItem CurrentDate = new ListViewItem();
		
		void lvDates1_SelectedIndexChanged(object sender, EventArgs e)
		{
		    if (lvDates1.SelectedItems.Count > 0)
		    {
		        dtpDate1.Value = Convert.ToDateTime(lvDates1.SelectedItems[0].Text);
		        CurrentDate = lvDates1.SelectedItems[0];
		    }
		}
		
		private void btnAddGroup2_Click(object sender, EventArgs e)
		{
		    if (lvGroups2.SelectedItems.Count > 0)
		    {
		        ListViewItem item = lvGroups2.SelectedItems[0];
		        string group = item.Text;
		        lvGroups2.Items.Remove(item);
		        lvSelectGroup2.Items.Add(group);
		    }
		}
		
		private void btnRemoveGroup2_Click(object sender, EventArgs e)
		{
		    if (lvSelectGroup2.SelectedItems.Count > 0)
		    {
		        ListViewItem item = lvSelectGroup2.SelectedItems[0];
		        string group = item.Text;
		        lvSelectGroup2.Items.Remove(item);
		        lvGroups2.Items.Add(group);
		    }
		}
		
		private void btnAddAllGroups2_Click(object sender, EventArgs e)
		{
		    foreach (ListViewItem item in lvGroups2.Items)
		    {
		        string group = item.Text;
		        lvGroups2.Items.Remove(item);
		        lvSelectGroup2.Items.Add(group);
		    }
		}
		
		private void btnRemoveAllGroups2_Click(object sender, EventArgs e)
		{
		    foreach (ListViewItem item in lvSelectGroup2.Items)
		    {
		        string group = item.Text;
		        lvSelectGroup2.Items.Remove(item);
		        lvGroups2.Items.Add(group);
		    }
		}
	}
}