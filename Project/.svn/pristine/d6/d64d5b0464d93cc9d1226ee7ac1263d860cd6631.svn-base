using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MyShedule
{
    public partial class DisciplineForm : Form
    {
        public DisciplineForm()
        {
            InitializeComponent();
        }

        void DisciplineForm_Load(object sender, EventArgs e)
        {
            InitGrid();

            this.FormClosing += new FormClosingEventHandler(DisciplineForm_FormClosing);
        }

        void DisciplineForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("Сохранить перед закрытием? ", "внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == System.Windows.Forms.DialogResult.OK)
                Save();
        }

        //  public List<SheduleLessonType> LessonTypes;
        public dsShedule ds;


        private void InitGrid()
        {
            dgvDisciplines.AutoGenerateColumns = false;

            DataGridViewTextBoxColumn clmn = new DataGridViewTextBoxColumn();
            DataGridViewCheckBoxColumn chkmn = new DataGridViewCheckBoxColumn();

            clmn.DataPropertyName = "Id";
            clmn.Name = "id";
            clmn.Visible = false;
            dgvDisciplines.Columns.Add(clmn);

            clmn = new DataGridViewTextBoxColumn();
            clmn.DataPropertyName = "Name";
            clmn.Name = "name";
            clmn.HeaderText = "Дисциплина";
            clmn.Width = clmn.HeaderText.Length * 8;
            dgvDisciplines.Columns.Add(clmn);

            BindingSource source = new BindingSource();
            source.DataSource = ds.Discipline;
            dgvDisciplines.DataSource = source;

            bindingNavigator1.BindingSource = source;
            dgvDisciplines.RowPrePaint += new DataGridViewRowPrePaintEventHandler(dgvEducationLoad_RowPrePaint);
            dgvDisciplines.RowHeadersWidth = 25;
        }

        private void dgvEducationLoad_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            // получаем текущий элемент DataGridView
            DataGridView grid = sender as DataGridView;
            if (grid != null)
            {
                if (e.RowIndex % 2 == 1)
                    // нечетная строка
                    grid.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGray;
                else
                    // четная строка
                    grid.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            }
        }

        private void tsbSaveFile_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void Save()
        {
            SaveFileDialog SaveDlg = new SaveFileDialog();
            SaveDlg.DefaultExt = "xml";
            SaveDlg.FileName = "Дисциплины.xml";
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

                ds.Discipline.WriteXml(filename);
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
                this.ds.Discipline.Clear();
                this.ds.Discipline.ReadXml(filename);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не могу открыть табель" + ex.Message);
            }
        }

    }
}
