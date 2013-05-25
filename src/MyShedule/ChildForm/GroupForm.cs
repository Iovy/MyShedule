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
    public partial class GroupForm : Form
    {
        public GroupForm()
        {
            InitializeComponent();
        }

        void GroupsForm_Load(object sender, EventArgs e)
        {
            InitGrid();

            this.FormClosing += new FormClosingEventHandler(GroupsForm_FormClosing);
        }

        void GroupsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("Сохранить перед закрытием? ", "внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                string filename = "Data\\Группы.xml";
                WriteXmlFile(filename);
            }
        }

      //  public List<SheduleLessonType> LessonTypes;
        public dsShedule ds;


        private void InitGrid()
        {
            dgvGroups.AutoGenerateColumns = false;

            DataGridViewTextBoxColumn clmn = new DataGridViewTextBoxColumn();
            DataGridViewCheckBoxColumn chkmn = new DataGridViewCheckBoxColumn();

            clmn.DataPropertyName = "Id";
            clmn.Name = "id";
            clmn.Visible = false;
            dgvGroups.Columns.Add(clmn);

            clmn = new DataGridViewTextBoxColumn();
            clmn.DataPropertyName = "Name";
            clmn.Name = "name";
            clmn.HeaderText = "Группа";
            clmn.Width = clmn.HeaderText.Length * 10;
            dgvGroups.Columns.Add(clmn);

            string filename = "Data\\Группы.xml";
            ReadXmlFile(filename);
            BindingSource source = new BindingSource();
            source.DataSource = ds.Group;
            dgvGroups.DataSource = source;

            bindingNavigator1.BindingSource = source;
            dgvGroups.RowPrePaint += new DataGridViewRowPrePaintEventHandler(dgvEducationLoad_RowPrePaint);
            dgvGroups.RowHeadersWidth = 25;
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
            SaveFileDialog SaveDlg = new SaveFileDialog();
            SaveDlg.DefaultExt = "xml";
            SaveDlg.FileName = "Группы.xml";
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
                ds.Group.WriteXml(filename);
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
                this.ds.Group.Clear();
                this.ds.Group.ReadXml(filename);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не могу открыть табель" + ex.Message);
            }
        }

    }
}
