//..begin "File Description"
/*--------------------------------------------------------------------------------*
   Filename:  MainForm.cs
   Tool:      objectiF, CSharpSSvr V7.1.23
 *--------------------------------------------------------------------------------*/
//..end "File Description"

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

using MyShedule.Dictionaries;
using MyShedule.ScheduleClasses;
using MyShedule.ReportExp;

namespace MyShedule
{	
	partial class MainForm
	:	System.Windows.Forms.Form
	{
		public MainForm()
		{
		    InitializeComponent();
		    this.Load += new EventHandler(MainForm_Load);
		
		    if (dgvSchedule.ColumnCount <= 0)
		        cmbView.Enabled = false;
		    else
		        cmbView.Enabled = true;
		
		    LoadDictionatyes();
		}
		
		EducationLoadAdapter EducationAdapter;
		dsShedule ScheduleDataSet;
		ScheduleWeeks Schedule;
		List<ScheduleRoom> Rooms;
		List<ScheduleTeacher> Teachers;
		List<ScheduleGroup> Groups;
		List<ScheduleDiscipline> Disciplines;
		bool BrushActive = false;
		bool WatchAll = true;
		
		public ScheduleClasses.View ViewShedule
		{
			get
			{
			    return (ScheduleClasses.View)Convert.ToInt32(cmbView.ComboBox.SelectedValue);
			}
		}
		
		scheduleType curSheduleType;
		
		#region MAIN FORM INITIALIZATION
		
		private void InitDistributeList()
		{
		    lvDistribute.View = System.Windows.Forms.View.Details;
		    lvDistribute.Columns.Clear();
		    lvDistribute.Columns.Add("Состояние", 150);
		    lvDistribute.Columns.Add("Элемент нагрузки", 600);
		    lvDistribute.Columns.Add("Причина", 400);
		}
		
		void MainForm_Load(object sender, EventArgs e)
		{
		    //emps = new Employments();
		
		    //SettingShedule setting = GetSetting();
		    //shedule = new SheduleWeeks(Rooms, setting);
		
		    //InitMainForm
		    this.WindowState = FormWindowState.Maximized;
		    splitContainer1.Panel2Collapsed = true;
		
		    InitTabControl();
		
		    InitDistributeList();
		
		    //Init combo box view projections
		    InitCmbViewProjection();
		
		    //Init Main Grid
		    //InitDataGridView();
		
		    //Brushes
		    tsbBlockTime.Enabled = false;
		    tsbFreeTime.Enabled = false;
		    BrushActive = false;
		
		    //Events
		    this.cmbView.SelectedIndexChanged += new EventHandler(cmbView_SelectedIndexChanged);
		    this.dgvSchedule.CellDoubleClick += new DataGridViewCellEventHandler(dgvSchedule_CellDoubleClick);
		    this.dgvSchedule.CellClick += new DataGridViewCellEventHandler(dgvShedule_CellClick);
		    this.dgvSchedule.MouseEnter += new EventHandler(dgvSchedule_MouseEnter);
		    this.dgvSchedule.MouseLeave += new EventHandler(dgvSchedule_MouseLeave);
		}
		
		private void InitTabControl()
		{
		    ImageList list = new ImageList();
		    list.Images.Add(MyShedule.Properties.Resources.comment);
		    list.Images.Add(MyShedule.Properties.Resources.arrow_refresh);
		    tabControl1.ImageList = list;
		    tabControl1.TabPages[0].ImageIndex = 0;
		    tabControl1.TabPages[1].ImageIndex = 1;
		}
		
		void dgvSchedule_MouseLeave(object sender, EventArgs e)
		{
		    if (BrushActive)
		        this.Cursor = System.Windows.Forms.Cursors.Default;
		}
		
		void dgvSchedule_MouseEnter(object sender, EventArgs e)
		{
		    if (BrushActive)
		        this.Cursor = System.Windows.Forms.Cursors.Hand;
		}
		
		void cmbView_SelectedIndexChanged(object sender, EventArgs e)
		{
		    dgvSchedule = TableGUIManager.FillDataGrid(Schedule, dgvSchedule, ViewShedule, EducationAdapter, Rooms, WatchAll);
		}
		
		private void InitCmbViewProjection()
		{
		    BindingSource bs = new BindingSource();
		    bs.DataSource = ScheduleView.BasicViews;
		    cmbView.ComboBox.DisplayMember = "Description";
		    cmbView.ComboBox.ValueMember = "TypeCode";
		    cmbView.ComboBox.DataSource = bs;
		    cmbView.AutoSize = false;
		    cmbView.Width = 170;
		    cmbView.Select(0, 0);
		}
		
		#endregion
		
		#region CREATE SCHEDULE FUNCS
		
		private void tsbCreateShedule_Click(object sender, EventArgs e)
		{
		    if (!CheckInputData)
		        return;
		
		    CreateSheduleForm frmShedule = new CreateSheduleForm(false, new DateTime(DateTime.Now.Year, 9, 1), new DateTime(DateTime.Now.Year, 12, 31));
		
		    if (frmShedule.ShowDialog() != System.Windows.Forms.DialogResult.OK)
		        return;
		
		    ScheduleGenerator reactor = new ScheduleGenerator(EducationAdapter, Rooms, 
		        GetSetting(getWeeksInSem(frmShedule.FirstDaySem, frmShedule.LastDaySem)),
		        frmShedule.FirstDaySem, frmShedule.LastDaySem, Schedule == null ? new Employments() : Schedule.Employments);
		
		    Schedule = reactor.Generate();
		
		    UpdateTableShedule();
		
		    UpdateDistributeList(reactor.Results);
		
		    WatchTriggerStateChange(false);
		
		    if (dgvSchedule.ColumnCount <= 0)
		        cmbView.Enabled = false;
		    else
		        cmbView.Enabled = true;
		
		    curSheduleType = scheduleType.leesons;
		}
		
		private void tsbCreateClearShedule_Click(object sender, EventArgs e)
		{
		    if (!CheckInputData)
		        return;
		
		    CreateSheduleForm frmShedule = new CreateSheduleForm(false, new DateTime(DateTime.Now.Year, 9, 1), new DateTime(DateTime.Now.Year, 12, 31));
		    if (frmShedule.ShowDialog() != System.Windows.Forms.DialogResult.OK)
		        return;
		
		    Schedule = new ScheduleWeeks(Rooms, GetSetting(getWeeksInSem(frmShedule.FirstDaySem, frmShedule.LastDaySem)), frmShedule.FirstDaySem, frmShedule.LastDaySem);
		    UpdateTableShedule();
		
		    WatchTriggerStateChange(true);
		
		    if (dgvSchedule.ColumnCount <= 0)
		        cmbView.Enabled = false;
		    else
		        cmbView.Enabled = true;
		
		    curSheduleType = scheduleType.leesons;
		}
		
		private void tsiCreate_Exam_Click(object sender, EventArgs e)
		{
		    if (!CheckInputData)
		        return;
		
		    CreateSheduleForm frmShedule = new CreateSheduleForm(true, new DateTime(DateTime.Now.Year + 2, 1, 1), new DateTime(DateTime.Now.Year + 2, 1, 31));
		    if (frmShedule.ShowDialog() != System.Windows.Forms.DialogResult.OK)
		        return;
		
		    SettingShedule settingBuf1 = GetSetting(getWeeksInSem(frmShedule.FirstDaySem, frmShedule.LastDaySem));
		    settingBuf1.CountLessonsOfDay = 1;
		    settingBuf1.FirstLessonsOfWeekDay = 1;
		    settingBuf1.FirstLessonsOfWeekEnd = 1;
		    settingBuf1.LastLessonsOfWeekDay = 1;
		    settingBuf1.LastLessonsOfWeekEnd = 1;
		    settingBuf1.MaxCountLessonsOfWeekDay = 1;
		    settingBuf1.MaxCountLessonsOfWeekEnd = 1;
		    Schedule = new ScheduleWeeks(Rooms, settingBuf1, frmShedule.FirstDaySem, frmShedule.LastDaySem);
		    UpdateTableShedule();
		
		    WatchTriggerStateChange(true);
		
		    if (dgvSchedule.ColumnCount <= 0)
		        cmbView.Enabled = false;
		    else
		        cmbView.Enabled = true;
		
		    curSheduleType = scheduleType.exams;
		}
		
		private SettingShedule GetSetting(int countOfWeeksInSem)
		{
		    SettingsAplication stg = new SettingsAplication();
		    stg.CountEducationalWeekBySem = countOfWeeksInSem;
		    return new SettingShedule(stg.CountWeeksShedule, stg.CountDayEducationalWeek, stg.CountDaysShedule,
		         stg.CountLessonsOfDay, stg.CountEducationalWeekBySem, stg.MaxCountLessonsOfWeekDay, stg.MaxCountLessonsOfWeekEnd,
		         stg.FirstLessonsOfWeekDay, stg.FirstLessonsOfWeekEnd, stg.LastLessonsOfWeekDay, stg.LastLessonsOfWeekEnd);
		}
		
		public bool CheckInputData
		{
			get
			{
			    bool check = true;
			    if (EducationAdapter.Items.Count == 0)
			    {
			        MessageBox.Show("Нагрузка не задана, составьте или загрузите нагрузку!");
			        check = false;
			    }
			
			    if (Rooms.Count == 0)
			    {
			        MessageBox.Show("Аудитории не заданы, заполните список аудиторий!");
			        check = false;
			    }
			    return check;
			}
		}
		
		private void UpdateDistributeList(List<DistributeResult> results)
		{
		    lvDistribute.Items.Clear();
		    ImageList imgList = new ImageList();
		    imgList.Images.Add(MyShedule.Properties.Resources.accept);
		    imgList.Images.Add(MyShedule.Properties.Resources.cancel);
		    lvDistribute.SmallImageList = imgList;
		
		    foreach (DistributeResult r in results)
		    {
		        string complete = r.Complete ? "успешно" : "не распределена";
		        ListViewItem item = new ListViewItem(complete);
		        item.SubItems.Add(r.ItemInfo);
		        item.SubItems.Add(r.Reason);
		        item.ImageIndex = r.Complete ? 0 : 1;
		        lvDistribute.Items.Add(item);
		
		    }
		}
		
		private int getWeeksInSem(DateTime firstDay, DateTime lastDay)
		{
		    int weeksInSem = 0;
		    if (firstDay.Year == lastDay.Year)
		    {
		        int daysInSem = 0;
		        for (int i = 0; i <= lastDay.Month - firstDay.Month; i++)
		        {
		            daysInSem += DateTime.DaysInMonth(firstDay.Year, firstDay.Month + i);
		        }
		        daysInSem = daysInSem - firstDay.Day + 1;
		        daysInSem = daysInSem - DateTime.DaysInMonth(lastDay.Year, lastDay.Month) + lastDay.Day;
		        weeksInSem = daysInSem / 7;
		        if (daysInSem % 4 != 0)
		            weeksInSem++;
		        return weeksInSem;
		    }
		    else
		    {
		        int daysInSem = 0;
		        for (int i = 0; i <= 12 - firstDay.Month; i++)
		        {
		            daysInSem += DateTime.DaysInMonth(firstDay.Year, firstDay.Month + i);
		        }
		        for (int i = 0; i < lastDay.Month; i++)
		        {
		            daysInSem += DateTime.DaysInMonth(lastDay.Year, i + 1);
		        }
		        daysInSem = daysInSem - firstDay.Day + 1;
		        daysInSem = daysInSem - DateTime.DaysInMonth(lastDay.Year, lastDay.Month) + lastDay.Day;
		        weeksInSem = daysInSem / 7;
		        if (daysInSem % 4 != 0)
		            weeksInSem++;
		        return weeksInSem;
		    }
		}
		
		#endregion
		
		void dgvSchedule_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
		    if (e.ColumnIndex <= 1 || e.RowIndex < 0)
		        return;
		
		    LessonForm frmLesson = new LessonForm(curSheduleType);
		    SchedulePointer Tag = dgvSchedule.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag as SchedulePointer;
		    frmLesson.txtSheduleTime.Text = ScheduleTime.GetDescription(Tag.Time1);
		    frmLesson.Employments = Schedule.Employments;
		    frmLesson.ds = ScheduleDataSet;
		    frmLesson.Adapter = EducationAdapter;
		    frmLesson.Rooms = Rooms;
		    frmLesson.curClmn = dgvSchedule.CurrentCell.ColumnIndex;
		
		    frmLesson.Time1 = Tag.Time1;
		    frmLesson.Lesson1 = Schedule.GetLesson(Tag.Time1, Tag.Room1);
		
		    frmLesson.Time2 = Tag.Time2;
		    frmLesson.Lesson2 = Schedule.GetLesson(Tag.Time2, Tag.Room2);
		
		    frmLesson.Shedule = Schedule;
		
		    if (frmLesson.ShowDialog() == System.Windows.Forms.DialogResult.OK)
		    {
		        UpdateTableShedule();
		        if (curSheduleType == scheduleType.exams)
		        {
		            if (frmLesson.Lesson1 != null)
		            {
		                OneDayTimeBlocked(dgvSchedule.Rows[e.RowIndex].Cells[e.ColumnIndex + 1], frmLesson.Time1.Week);
		                OneDayTimeBlocked(dgvSchedule.Rows[e.RowIndex].Cells[e.ColumnIndex + 2], frmLesson.Time1.Week);
		                OneDayTimeBlocked(dgvSchedule.Rows[e.RowIndex].Cells[e.ColumnIndex + 3], frmLesson.Time1.Week);
		            }
		            if (frmLesson.Lesson2 != null)
		            {
		                OneDayTimeBlocked(dgvSchedule.Rows[e.RowIndex].Cells[e.ColumnIndex + 1], frmLesson.Time2.Week);
		                OneDayTimeBlocked(dgvSchedule.Rows[e.RowIndex].Cells[e.ColumnIndex + 2], frmLesson.Time2.Week);
		                OneDayTimeBlocked(dgvSchedule.Rows[e.RowIndex].Cells[e.ColumnIndex + 3], frmLesson.Time2.Week);
		            }
		        }
		    }
		}
		
		void dgvShedule_CellClick(object sender, DataGridViewCellEventArgs e)
		{
		    if (e.ColumnIndex > 1 && e.RowIndex >= 0)
		    {
		        DataGridViewCell cell = dgvSchedule.Rows[e.RowIndex].Cells[e.ColumnIndex];
		
		        if (BrushActive)
		        {
		            if (tsbFreeTime.Checked)
		                TimeFree(cell);
		            if (tsbBlockTime.Checked)
		                TimeBlocked(cell);
		        }
		        else
		        {
		            ViewSelectLesson(cell);
		        }
		    }
		}
		
		private void UpdateTableShedule()
		{
		    dgvSchedule = TableGUIManager.InitializeGrid(dgvSchedule, Schedule.Setting.CountWeeksShedule,
		        Schedule.Setting.CountDaysEducationWeek);
		
		    dgvSchedule = TableGUIManager.FillDataGrid(Schedule, dgvSchedule, ViewShedule,
		        EducationAdapter, Rooms, WatchAll);
		}
		
		#region BRUSH WOKERS
		
		private void tsbBrushTrigger_Click(object sender, EventArgs e)
		{
		    ChangeBrushState();
		}
		
		private void ChangeBrushState(bool value)
		{
		    BrushActive = value;
		    UpdateBrushUI();
		}
		
		private void ChangeBrushState()
		{
		    BrushActive = !BrushActive;
		    UpdateBrushUI();
		}
		
		private void UpdateBrushUI()
		{
		    tsbBlockTime.Enabled = BrushActive;
		    tsbFreeTime.Enabled = BrushActive;
		
		    this.Cursor = BrushActive ? System.Windows.Forms.Cursors.Hand : System.Windows.Forms.Cursors.Default;
		    tsbFreeTime.Checked = !BrushActive;
		    tsbBlockTime.Checked = BrushActive;
		    tsbBrushTrigger.Image = BrushActive ? MyShedule.Properties.Resources.pencil_delete : MyShedule.Properties.Resources.pencil_add;
		    tsbBrushTrigger.Text = BrushActive ? "Выкл" : "Вкл";
		}
		
		#endregion
		
		#region BLOCK_FREE ENTITIES FUNCS
		
		private void TimeFree(DataGridViewCell cell)
		{
		    SchedulePointer pointer = cell.Tag as SchedulePointer;
		    SetCellFreeStyle(cell, pointer);
		
		    ScheduleLesson lesson1 = Schedule.GetLesson(pointer.Time1, pointer.Room1);
		    if (lesson1 != null)
		        lesson1.Clear();
		
		    ScheduleLesson lesson2 = Schedule.GetLesson(pointer.Time2, pointer.Room2);
		    if (lesson2 != null)
		        lesson2.Clear();
		
		    string Name = dgvSchedule.Rows[cell.RowIndex].Cells[0].Value.ToString();
		    Schedule.Employments.RemoveInView(ViewShedule, new Employment(Name, pointer.Time1, ReasonEmployment.Another));
		    Schedule.Employments.RemoveInView(ViewShedule, new Employment(Name, pointer.Time2, ReasonEmployment.Another));
		}
		
		private static void SetCellFreeStyle(DataGridViewCell cell, SchedulePointer pointer)
		{
		    cell.Value = String.Empty;
		    cell.Style.BackColor = (pointer.Time1.DayNumber > (int)ScheduleClasses.Day.Friday) ? Color.LightBlue : new DataGridViewCellStyle().BackColor;
		}
		
		private void TimeBlocked(DataGridViewCell cell)
		{
		    TimeFree(cell);
		
		    SchedulePointer pointer = cell.Tag as SchedulePointer;
		    string Name = dgvSchedule.Rows[cell.RowIndex].Cells[0].Value.ToString();
		    SetCellBlockedStyle(cell, pointer.Time1, pointer.Time2);
		
		    Schedule.Employments.AddInView(ViewShedule, new Employment(Name, pointer.Time1, ReasonEmployment.UserBlocked));
		    Schedule.Employments.AddInView(ViewShedule, new Employment(Name, pointer.Time2, ReasonEmployment.UserBlocked));
		}
		
		private void OneDayTimeBlocked(DataGridViewCell cell, Week weekNum)
		{
		    TimeFree(cell);
		
		    SchedulePointer pointer = cell.Tag as SchedulePointer;
		    string Name = dgvSchedule.Rows[cell.RowIndex].Cells[0].Value.ToString();
		   // SetCellBlockedStyle(cell, pointer.Time1, pointer.Time1);
		
		    if (weekNum == Week.FirstWeek || weekNum == Week.SecondWeek)
		        Schedule.Employments.AddInView(ViewShedule, new Employment(Name, pointer.Time1, ReasonEmployment.UserBlocked));
		    else
		        Schedule.Employments.AddInView(ViewShedule, new Employment(Name, pointer.Time2, ReasonEmployment.UserBlocked));
		}
		
		private void SetCellBlockedStyle(DataGridViewCell cell, ScheduleTime FirstDayTime, ScheduleTime SecondDayTime)
		{
		    cell.Style.BackColor = Color.LightPink;
		    string msg = "Занято";
		    if (FirstDayTime == SecondDayTime)
		        msg += "\n\r" + FirstDayTime.Description;
		    cell.Value = msg;
		    cell.Style.Font = new Font(FontFamily.GenericSansSerif, 12);
		    cell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
		}
		
		private void tsbFreeTime_Click(object sender, EventArgs e)
		{
		    tsbFreeTime.Checked = true;
		    tsbBlockTime.Checked = false;
		}
		
		private void tsbBlockTime_Click(object sender, EventArgs e)
		{
		    tsbBlockTime.Checked = true;
		    tsbFreeTime.Checked = false;
		}
		
		#endregion
		
		private void tsbOptions_Click(object sender, EventArgs e)
		{
		    SheduleSettingForm frmSetting = new SheduleSettingForm();
		    frmSetting.ShowDialog();
		}
		
		private void tsbDown_Click(object sender, EventArgs e)
		{
		    splitContainer1.Panel2Collapsed = true;
		}
		
		private void tsbUp_Click(object sender, EventArgs e)
		{
		    splitContainer1.Panel2Collapsed = false;
		}
		
		private void tsbWatchTrigger_Click(object sender, EventArgs e)
		{
		    WatchTriggerStateChange();
		}
		
		private void WatchTriggerStateChange()
		{
		    if (Schedule == null)
		        return;
		
		    WatchAll = !WatchAll;
		    WatchUpdateUI();
		}
		
		private void WatchTriggerStateChange(bool value)
		{
		    if (Schedule == null)
		        return;
		
		    WatchAll = value;
		    WatchUpdateUI();
		}
		
		private void WatchUpdateUI()
		{
		    tsbWatchTrigger.Text = WatchAll ? "Показать часть" : "Показать все";
		    tsbWatchTrigger.Image = WatchAll ? MyShedule.Properties.Resources.table_row_delete : MyShedule.Properties.Resources.table_row_insert;
		    tsbWatchTrigger.Checked = WatchAll;
		
		    dgvSchedule = TableGUIManager.InitializeGrid(dgvSchedule, Schedule.Setting.CountWeeksShedule, Schedule.Setting.CountDaysEducationWeek);
		    dgvSchedule = TableGUIManager.FillDataGrid(Schedule, dgvSchedule, ViewShedule, EducationAdapter, Rooms, WatchAll);
		}
		
		#region EXCHANGE FUNCS
		
		public int NumSelectLesson = 0;
		public ScheduleLesson L1;
		public ScheduleLesson L2;
		public ScheduleLesson L3;
		public ScheduleLesson L4;
		public ScheduleTime T1;
		public ScheduleTime T2;
		public ScheduleTime T3;
		public ScheduleTime T4;
		
		private void button1_Click(object sender, EventArgs e)
		{
		    NumSelectLesson = 1;
		}
		
		private void btnSelectLesson2_Click(object sender, EventArgs e)
		{
		    NumSelectLesson = 2;
		}
		
		private void ExchangeLessons(ScheduleLesson item1, ScheduleLesson item2, ScheduleTime time1, ScheduleTime time2)
		{
		    if (item1 != null && item2 != null)
		    {
		        ScheduleLesson tmp1 = item1.Copy();
		        ScheduleLesson less1 = Schedule.GetLesson(time1, item1.Room);
		        ScheduleLesson less2 = Schedule.GetLesson(time2, item2.Room);
		
		        Schedule.Employments.Remove(less1.Teacher, less1.Groups, less1.Room, less1.Time);
		        Schedule.Employments.Remove(less2.Teacher, less2.Groups, less2.Room, less2.Time);
		
		        less1.UpdateFields(item1.Teacher, item1.Discipline, item1.Groups, item1.Type, item2.Dates);
		        less1.Time = item2.Time.Copy();
		        less1.Room = item2.Room;
		                       
		        less2.UpdateFields(item2.Teacher, item2.Discipline, item2.Groups, item2.Type, tmp1.Dates);
		        less2.Time = tmp1.Time.Copy();
		        less2.Room = tmp1.Room;
		
		        Schedule.Employments.Add(less2.Teacher, less2.Groups, less2.Room, less2.Time, ReasonEmployment.UnionLesson);
		        Schedule.Employments.Add(less1.Teacher, less1.Groups, less1.Room, less1.Time, ReasonEmployment.UnionLesson);
		    }
		    else if (item1 == null && item2 != null)
		    {
		        ScheduleLesson lsn = Schedule.GetLesson(time1, item2.Room);
		        if (lsn.GroupsDescription != "" || lsn.Teacher != "" || lsn.Discipline != "")
		        {
		            MessageBox.Show("Занято для " + item2.Week.ToString(), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
		            return;
		        }
		        ScheduleLesson lsn2 = Schedule.GetLesson(time2, item2.Room).Copy();
		        Schedule.Employments.Remove(lsn2.Teacher, lsn2.Groups, lsn2.Room, lsn2.Time);
		        lsn.UpdateFields(item2.Teacher, item2.Discipline, item2.Groups, item2.Type, lsn.Dates);
		        item2.Clear();
		        Schedule.Employments.Add(lsn.Teacher, lsn.Groups, lsn.Room, lsn.Time, ReasonEmployment.UnionLesson);
		    }
		    else if (item1 != null && item2 == null)
		    {
		        ScheduleLesson lsn = Schedule.GetLesson(time2, item1.Room);
		        if (lsn.GroupsDescription != "" || lsn.Teacher != "" || lsn.Discipline != "")
		        {
		            MessageBox.Show("Занято для " + item1.Week.ToString(), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
		            return;
		        }
		        ScheduleLesson lsn2 = Schedule.GetLesson(time1, item1.Room).Copy();
		        Schedule.Employments.Remove(lsn2.Teacher, lsn2.Groups, lsn2.Room, lsn2.Time);
		        lsn.UpdateFields(item1.Teacher, item1.Discipline, item1.Groups, item1.Type, lsn.Dates);
		        item1.Clear();
		        Schedule.Employments.Add(lsn.Teacher, lsn.Groups, lsn.Room, lsn.Time, ReasonEmployment.UnionLesson);
		    }
		}
		
		private void btnExchangeLessons_Click(object sender, EventArgs e)
		{
		    ExchangeLessons(L1, L3, T1, T3);
		    ExchangeLessons(L2, L4, T2, T4);
		    UpdateTableShedule();
		}
		
		#endregion
		
		private void SetDataSelectLessonForm(ScheduleLesson item1, ScheduleLesson item2, Label txtTeacher, Label txtDiscipline,
				Label txtRoom, Label txtTypeLesson)
		{
		    if (item1 == null && item2 == null)
		    {
		        txtTeacher.Text = txtDiscipline.Text = txtRoom.Text = txtTypeLesson.Text = String.Empty;
		        return;
		    }
		
		    txtTeacher.Text = (item1 != null ? item1.Teacher : String.Empty) + " / " + (item2 != null ? item2.Teacher : String.Empty);
		
		    txtDiscipline.Text = (item1 != null ? item1.Discipline : String.Empty) + " / " + (item2 != null ? item2.Discipline : String.Empty);
		
		    txtRoom.Text = (item1 != null ? item1.Room : String.Empty) + " / " + (item2 != null ? item2.Room : String.Empty);
		
		    txtTypeLesson.Text = (item1 != null ? ScheduleLessonType.Description(item1.Type) : String.Empty) + " / " +
		                         (item2 != null ? ScheduleLessonType.Description(item2.Type) : String.Empty);
		}
		
		private void ViewSelectLesson(DataGridViewCell cell)
		{
		    SchedulePointer Tag = cell.Tag as SchedulePointer;
		
		    ScheduleLesson lesson1 = Schedule.GetLesson(Tag.Time1, Tag.Room1);
		    ScheduleLesson lesson2 = Schedule.GetLesson(Tag.Time2, Tag.Room2);
		
		    if (NumSelectLesson == 1)
		    {
		        SetDataSelectLessonForm(lesson1, lesson2, txtTeacher1, txtDiscipline1, txtRoom1, txtTypeLesson1);
		
		        L1 = lesson1;
		        L2 = lesson2;
		        T1 = Tag.Time1.Copy();
		        T2 = Tag.Time2.Copy();
		    }
		    else if (NumSelectLesson == 2)
		    {
		        SetDataSelectLessonForm(lesson1, lesson2, txtTeacher2, txtDiscipline2, txtRoom2, txtTypeLesson2);
		
		        L3 = lesson1;
		        L4 = lesson2;
		        T3 = Tag.Time1.Copy();
		        T4 = Tag.Time2.Copy();
		    }
		
		    NumSelectLesson = 0;
		}
		
		#region SAVE_LOAD SCHEDULE FUNCS
		
		private void tsmiSaveSheduleToFile_Click(object sender, EventArgs e)
		{
		    SaveFileDialog frmSave = new SaveFileDialog();
		    frmSave.FileName = "расписание.xml";
		    frmSave.Filter = "(*.xml)|*.xml";
		
		    if (frmSave.ShowDialog() == System.Windows.Forms.DialogResult.OK)
		    {
		        try
		        {
		            ScheduleSerializer.SaveData(frmSave.FileName, Schedule);
		        }
		        catch (Exception ex)
		        {
		            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
		            return;
		        }
		    }
		}
		
		private void tsmiOpenSheduleFromFile_Click(object sender, EventArgs e)
		{
		    OpenFileDialog frmOpen = new OpenFileDialog();
		    frmOpen.DefaultExt = "xml";
		    frmOpen.Filter = "(*.xml)|*.xml";
		
		    if (frmOpen.ShowDialog() == System.Windows.Forms.DialogResult.OK)
		    {
		        try
		        {
		            Schedule = ScheduleSerializer.ReadData(frmOpen.FileName);
		
		            if (Schedule != null)
		                Schedule.Employments.Clear();
		
		            UpdateTableShedule();
		        }
		        catch (Exception ex)
		        {
		            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
		            return;
		        }
		
		        if (dgvSchedule.ColumnCount <= 0)
		            cmbView.Enabled = false;
		        else
		            cmbView.Enabled = true;
		    }
		}
		
		private void LoadDictionatyes()
		{
		    ScheduleDataSet = new dsShedule();
		    Rooms = new List<ScheduleRoom>();
		    Teachers = new List<ScheduleTeacher>();
		    Groups = new List<ScheduleGroup>();
		    Disciplines = new List<ScheduleDiscipline>();
		
		    try
		    {
		        string filename = @"Data/Нагрузка.xml";
		        ScheduleDataSet.Education.ReadXml(filename);
		        EducationAdapter = new EducationLoadAdapter(DictionaryConverter.EducationToList(ScheduleDataSet));
		    }
		    catch (Exception)
		    {
		        MessageBox.Show("Не могу открыть файл c нагрузкой");
		    }
		
		    try
		    {
				string filename = @"Data/Аудитории.xml";
		        ScheduleDataSet.Room.ReadXml(filename);
		        Rooms = DictionaryConverter.RoomsToList(ScheduleDataSet);
		    }
		    catch (Exception)
		    {
		        MessageBox.Show("Не могу открыть файл с аудиториями");
		    }
		
		    try
		    {
				string filename = @"Data/Преподаватели.xml";
		        ScheduleDataSet.Teacher.ReadXml(filename);
		        Teachers = DictionaryConverter.TeachersToList(ScheduleDataSet);
		    }
		    catch (Exception)
		    {
		        MessageBox.Show("Не могу открыть файл с преподавателями");
		    }
		
		    try
		    {
				string filename = @"Data/Группы.xml";
		        ScheduleDataSet.Group.ReadXml(filename);
		        Groups = DictionaryConverter.GroupsToList(ScheduleDataSet);
		    }
		    catch (Exception)
		    {
		        MessageBox.Show("Не могу открыть файл с группами");
		    }
		
		    try
		    {
				string filename = @"Data/Дисциплины.xml";
		        ScheduleDataSet.Discipline.ReadXml(filename);
		        Disciplines = DictionaryConverter.DisciplinesToList(ScheduleDataSet);
		    }
		    catch (Exception)
		    {
		        MessageBox.Show("Не могу открыть файл с дисциплинами");
		    }
		}
		
		#endregion
		
		#region DICTIONARIES MENU
		
		private void tsbRooms_Click(object sender, EventArgs e)
		{
		    RoomsForm frmRoom = new RoomsForm();
		    frmRoom.ds = ScheduleDataSet;
		    frmRoom.ShowDialog();
		    Rooms = DictionaryConverter.RoomsToList(frmRoom.ds);
		}
		
		private void tsbEducationLoad_Click(object sender, EventArgs e)
		{
		    EdicationLoadForm frmEdicationLoad = new EdicationLoadForm(Teachers, Disciplines);
		    frmEdicationLoad.SheduleDataSet = ScheduleDataSet;
		    frmEdicationLoad.ShowDialog();
		    EducationAdapter = new EducationLoadAdapter(DictionaryConverter.EducationToList(frmEdicationLoad.SheduleDataSet));
		}
		
		private void tbsTeachers_Click(object sender, EventArgs e)
		{
		    TeachersForm frmTeacher = new TeachersForm();
		    frmTeacher.ds = ScheduleDataSet;
		    frmTeacher.ShowDialog();
		    Teachers = DictionaryConverter.TeachersToList(frmTeacher.ds);
		}
		
		private void tsbGroups_Click(object sender, EventArgs e)
		{
		    GroupForm frmGroup = new GroupForm();
		    frmGroup.ds = ScheduleDataSet;
		    frmGroup.ShowDialog();
		    Groups = DictionaryConverter.GroupsToList(frmGroup.ds);
		}
		
		private void tsbDisciplines_Click(object sender, EventArgs e)
		{
		    DisciplineForm frmDiscipline = new DisciplineForm();
		    frmDiscipline.ds = ScheduleDataSet;
		    frmDiscipline.ShowDialog();
		    Disciplines = DictionaryConverter.DisciplinesToList(frmDiscipline.ds);
		}
		
		#endregion
		
		#region REPORT FUNCS
		
		private void tsiExportWord_Click(object sender, EventArgs e)
		{
		    ChooseGroupsForm choose = new ChooseGroupsForm();
		
		    choose.adapter = EducationAdapter;
		    choose.Rooms = Rooms;
		
		    if (Schedule == null)
		    {
		        MessageBox.Show("Расписание не задано!");
		        return;
		    }
		    if (choose.ShowDialog() == System.Windows.Forms.DialogResult.OK && choose.ChooseNames.Count > 0)
		    {
                stampParamsForm param = new stampParamsForm(Schedule.FirstDaySem.Year);

                if(param.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
		            SaveFileDialog frmSave = new SaveFileDialog();
		            frmSave.Filter = "(*.doc)|*.doc";
		            frmSave.FileName = "report1.doc";
		            if (frmSave.ShowDialog() == DialogResult.OK && frmSave.FileName != "")
		            {
		                // Проверяем наличие файла
		                if (File.Exists(frmSave.FileName) == true)
		                {
		                    try
		                    {
		                        File.Delete(frmSave.FileName);
		                    }
		                    catch (Exception ex)
		                    {
		                        MessageBox.Show(ex.Message);
		                    }
		                }
                        createReport(new DocExporter(choose.ChooseNames, Schedule, choose.ChooseView, frmSave.FileName, param.parametrs));
		            }
                }
		    }
		}
		
		private void tsiExport_Excel_Click(object sender, EventArgs e)
		{
		    ChooseGroupsForm choose = new ChooseGroupsForm();
		
		    choose.adapter = EducationAdapter;
		    choose.Rooms = Rooms;
		
		    if (Schedule == null)
		    {
		        MessageBox.Show("Расписание не задано!");
		        return;
		    }
		
		    if (choose.ShowDialog() == System.Windows.Forms.DialogResult.OK && choose.ChooseNames.Count > 0)
		    {
		         SaveFileDialog frmSave = new SaveFileDialog();
		        frmSave.Filter = "(*.xls)|*.xls";
		        frmSave.FileName = "schedule1.xls";
		        if (frmSave.ShowDialog() == DialogResult.OK && frmSave.FileName != "")
		        {
		            // Проверяем наличие файла
		            if (File.Exists(frmSave.FileName) == true)
		            {
		                try
		                {
		                    File.Delete(frmSave.FileName);
		                }
		                catch (Exception ex)
		                {
		                    MessageBox.Show(ex.Message);
		                }
		            }
		            createReport(new XLSExporter(choose.ChooseNames, Schedule, choose.ChooseView, frmSave.FileName));
		        }
		    }
		}
		
		private void createReport(IExporter strategy)
		{
		    IExporter exp = strategy;
		    exp.Export();
		}
		
		#endregion
	}
}