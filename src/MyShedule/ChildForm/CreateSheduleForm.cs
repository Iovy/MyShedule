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
    public partial class CreateSheduleForm : Form
    {
        public CreateSheduleForm(bool useChecking, DateTime starDate, DateTime finishDate)
        {
            InitializeComponent();

            useDateChecking = useChecking;

            StartDate = starDate;
            FinishDate = finishDate;
            Year = starDate.Year;
            Sem = 1;

            this.Load += new EventHandler(CreateShduleForm_Load);
        }

        bool useDateChecking;

        public DateTime FinishDate
        {
            get;
            set;
        }

        public DateTime StartDate
        {
            get;
            set;
        }

        public int Year
        {
            get;
            set;
        }

        public int Sem
        {
            get;
            set;
        }

        public DateTime FirstDaySem
        {
            get;
            set;
        }

        public DateTime LastDaySem
        {
            get;
            set;
        }

        void CreateShduleForm_Load(object sender, EventArgs e)
        {
            this.AcceptButton = btnCreateShedule;
            //выставим значения по умолчанию
            CreateYearList();
            cmbYear.Text = Year.ToString();

            CreateSemList();
            //1-й семестр
            cmbSem.Text = "I";

            int september = 9;
            // выставляем 1-е сентября начало семестра
            dtpFirstDaySem.Value = new DateTime(StartDate.Year, StartDate.Month, StartDate.Day);
            // выставляем 1-е января конец семестра
            dtpLastDaySem.Value = new DateTime(FinishDate.Year, FinishDate.Month, FinishDate.Day);

            this.cmbYear.SelectedValueChanged += new EventHandler(cmbYear_SelectedValueChanged);
            this.cmbSem.SelectedValueChanged += new EventHandler(cmbSem_SelectedValueChanged); 
        }

        void cmbSem_SelectedValueChanged(object sender, EventArgs e)
        {
            Sem = GetSem();

            if (Sem == 1)
            {
                // выставляем 1-е сентября
                dtpFirstDaySem.Value = new DateTime(Year, 9, 1);
                dtpLastDaySem.Value = new DateTime(Year + 1, 1, 1);
            }
            else if (Sem == 2)
            {
                // выставляем 1-е февраля
                dtpFirstDaySem.Value = new DateTime(Year, 2, 1);
                dtpLastDaySem.Value  = new DateTime(Year, 6, 1);
            }
        }

        private int GetSem()
        {
            int _sem = 0;
            string value = cmbSem.Text;
            if (value == "I")
                _sem = 1;
            else if (value == "II")
                _sem = 2;
            else
                _sem = 0;
            return _sem;
        }

        void cmbYear_SelectedValueChanged(object sender, EventArgs e)
        {
            Year = Convert.ToInt32(cmbYear.SelectedValue);
        }

        private void CreateYearList()
        {
            string[] years = new string[12] { "2009", "2010", "2011", "2012", "2013", "2014",
                                              "2015", "2016", "2017", "2018", "2019", "2020" };
            cmbYear.Items.AddRange(years);
        }
        
        private void CreateSemList()
        {
            string[] sems = new string[2] { "I", "II" };
            cmbSem.Items.AddRange(sems);
        } 

        private void btnCreateShedule_Click(object sender, EventArgs e)
        {
            Year = Convert.ToInt32(cmbYear.SelectedValue);
            Sem = GetSem();
            FirstDaySem = dtpFirstDaySem.Value;
            LastDaySem = dtpLastDaySem.Value;
            if (LastDaySem <= FirstDaySem)
            {
                string errorMsg = "Даты заданы некорректно: дата окончания \n\rсеместра должна быть позже даты начала семестра";
                MessageBox.Show(errorMsg, "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (useDateChecking)
            {
                if (!(LastDaySem.Year == FirstDaySem.Year && LastDaySem.Month == FirstDaySem.Month))
                {
                    string errorMsg = "Даты заданы некорректно: максимальная длительность\n\r экзаменационной сессии - 1 месяц";
                    MessageBox.Show(errorMsg, "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }
    }
}
