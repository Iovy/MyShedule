using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MyShedule.Dictionaries;
using MyShedule.ScheduleClasses;

namespace MyShedule
{
    public partial class EdicationLoadForm : Form
    {
        public List<ScheduleLessonType> LessonTypes;
        public List<ScheduleTeacher> Teachers;
        public List<ScheduleDiscipline> Disciplines;
        public dsShedule SheduleDataSet;

        public EdicationLoadForm(List<ScheduleTeacher> tch, List<ScheduleDiscipline> dsp)
        {
            InitializeComponent();

            SheduleDataSet = new dsShedule();

            this.Load += new EventHandler(EdicationLoadForm_Load);

            Teachers = tch;
            Disciplines = dsp;
        }

        void EdicationLoadForm_Load(object sender, EventArgs e)
        {
            InitLessonTypes();

            InitGrid();

            this.FormClosing += new FormClosingEventHandler(EdicationLoadForm_FormClosing);
        }

        void EdicationLoadForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("Сохранить перед закрытием? ", "внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                string filename = "Data\\Нагрузка.xml";
                WriteXmlFile(filename);
            }
        }

        private void InitGrid()
        {
            DataGridViewTextBoxColumn clmn = new DataGridViewTextBoxColumn();
            clmn.DataPropertyName = "Id";
            clmn.Name = "id";
            clmn.Visible = false;
            dgvEducationLoad.Columns.Add(clmn);

            DataGridViewComboBoxColumn cmbClmn = new DataGridViewComboBoxColumn();
            cmbClmn.DataPropertyName = "Teacher";
            cmbClmn.Name = "teacher";
            cmbClmn.HeaderText = "Преподаватель";
            cmbClmn.DisplayMember = "Name";
            cmbClmn.ValueMember = "Name";
            BindingSource bs = new BindingSource();
            bs.DataSource = Teachers;
            cmbClmn.DataSource = bs;
            dgvEducationLoad.Columns.Add(cmbClmn);

            cmbClmn = new DataGridViewComboBoxColumn();
            cmbClmn.DataPropertyName = "Discipline";
            cmbClmn.Name = "discipline";
            cmbClmn.HeaderText = "Дисциплина";
            cmbClmn.DisplayMember = "Name";
            cmbClmn.ValueMember = "Name";
            bs = new BindingSource();
            bs.DataSource = Disciplines;
            cmbClmn.DataSource = bs;
            dgvEducationLoad.Columns.Add(cmbClmn);

            clmn = new DataGridViewTextBoxColumn();
            clmn.DataPropertyName = "Group";
            clmn.Name = "group";
            clmn.HeaderText = "Группа";
            clmn.Width = 250;
            clmn.ReadOnly = true;
            dgvEducationLoad.Columns.Add(clmn);

            DataGridViewButtonColumn btnClmn = new DataGridViewButtonColumn();
            btnClmn.Width = 25;
            btnClmn.MinimumWidth = 25;
            btnClmn.Text = "...";
            btnClmn.UseColumnTextForButtonValue = true;
            btnClmn.Resizable = DataGridViewTriState.False;
            dgvEducationLoad.Columns.Add(btnClmn);
            dgvEducationLoad.Click += new EventHandler(addGroupsBtn_Click);

            clmn = new DataGridViewTextBoxColumn();
            clmn.DataPropertyName = "HoursSem";
            clmn.Name = "hoursSem";
            clmn.HeaderText = "Часы";
            clmn.Width = 60;
            dgvEducationLoad.Columns.Add(clmn);

            cmbClmn = new DataGridViewComboBoxColumn();
            cmbClmn.DataPropertyName = "LessonType";
            cmbClmn.Name = "LessonType";
            cmbClmn.HeaderText = "Тип занятия";
            cmbClmn.DisplayMember = "Detail";
            cmbClmn.ValueMember = "TypeCode";
            bs = new BindingSource();
            bs.DataSource = LessonTypes;
            cmbClmn.DataSource = bs;
            dgvEducationLoad.Columns.Add(cmbClmn);

            string filename = "Data\\Нагрузка.xml";
            ReadXmlFile(filename);
            BindingSource source = new BindingSource();
            source.DataSource = SheduleDataSet.Education;
            dgvEducationLoad.DataSource = source;

            bindingNavigator1.BindingSource = source;
            dgvEducationLoad.RowPrePaint+=new DataGridViewRowPrePaintEventHandler(dgvEducationLoad_RowPrePaint);
            dgvEducationLoad.RowHeadersWidth = 25;
        }

        void addGroupsBtn_Click(object sender, EventArgs e)
        {
            if (dgvEducationLoad.CurrentCell.ColumnIndex == 0)
            {
                ChooseGroupForm chsGrpForm = new ChooseGroupForm();
                chsGrpForm.ds = SheduleDataSet;

                if (chsGrpForm.ShowDialog() == System.Windows.Forms.DialogResult.OK && chsGrpForm.ChooseNames.Count > 0)
                {
                   // DocExporter exp = new DocExporter();
                    List<string> choosenGroups = chsGrpForm.ChooseNames;
                    string resStr = "";
                    foreach (string group in choosenGroups)
                    {
                        if(choosenGroups.IndexOf(group)==0)
                        {
                            resStr += group;
                        }
                        else 
                        {
                            resStr += ", " + group;
                        }
                    }

                    dgvEducationLoad.Rows[dgvEducationLoad.CurrentCell.RowIndex].Cells[4].Value = resStr;
                }
            }
        }

        private void dgvEducationLoad_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            // получаем текущий элемент DataGridView
            DataGridView grid = sender as DataGridView;
            if (grid != null)
            {
                if (e.RowIndex % 2 == 1)
                    // нечетная строка
                    grid.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightYellow;
                else
                    // четная строка
                    grid.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            }
        }

        private void InitLessonTypes()
        {
            LessonTypes = new List<ScheduleLessonType>();
            LessonTypes.Add(new ScheduleLessonType(LessonType.Lection));
            LessonTypes.Add(new ScheduleLessonType(LessonType.Labwork));
            LessonTypes.Add(new ScheduleLessonType(LessonType.Practice));

        }

        private void tsbSaveFile_Click(object sender, EventArgs e)
        {
            SaveFileDialog SaveDlg = new SaveFileDialog();
            SaveDlg.DefaultExt = "xml";
            SaveDlg.FileName = "Нагрузка.xml";
            SaveDlg.Filter = "(*.xml)|*.xml";

            DialogResult RDlg = SaveDlg.ShowDialog();
            string filename = SaveDlg.FileName;
            if (RDlg == DialogResult.OK && filename != "")
                WriteXmlFile(filename);
        }

        private void WriteXmlFile(string filename)
        {
            try
            {
                SheduleDataSet.Education.WriteXml(filename);
            }
            catch (Exception)
            {
                MessageBox.Show("Не могу сохранить в файл");
            }
        }

        private void tsbReadFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog opendlg = new OpenFileDialog();
            opendlg.DefaultExt = "xml";
            opendlg.Filter = "(*.xml)|*.xml";
            DialogResult res = opendlg.ShowDialog();
            string filename = opendlg.FileName;
            if (res == DialogResult.OK && filename != "")
            {
                ReadXmlFile(filename);
            }
        }

        private void ReadXmlFile(string filename)
        {
            try
            {
                this.SheduleDataSet.Education.Clear();
                this.SheduleDataSet.Education.ReadXml(filename);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не могу открыть табель" + ex.Message);
            }
        }
    }
}
