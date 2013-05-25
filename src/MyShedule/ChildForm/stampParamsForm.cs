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
    public partial class stampParamsForm : Form
    {
        public stampParamsForm(int Year)
        {
            InitializeComponent();
            parametrs = new List<string>();
            prorectName.Text = "Гоник И.Л.";
            facultyName.Text = "ФПО";
            _sem = 1;
            _course = 1;
            _year = Year;

            CreateSemList();
            CreateCourseList();

            semNum.Text = "I";
            courseNum.Text = "1";
        }

        public List<string> parametrs;

        int _sem;
        int _course;
        int _year;

        private void btnAccept_Click(object sender, EventArgs e)
        {
            parametrs.Clear();
            parametrs.Add(prorectName.Text);
            parametrs.Add(facultyName.Text);
            parametrs.Add(semNum.Text);
            parametrs.Add(courseNum.Text);
            if (semNum.Text == "I")
            {
                parametrs.Add((_year + 1).ToString());
                parametrs.Add(_year.ToString() + "-" + (_year + 1).ToString());
            }
            else
            {
                parametrs.Add(_year.ToString());
                parametrs.Add((_year - 1).ToString() + "-" + _year.ToString());
            }

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void btnReject_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void CreateSemList()
        {
            string[] sems = new string[2] { "I", "II" };
            semNum.Items.AddRange(sems);
        }

        private void CreateCourseList()
        {
            string[] courses = new string[6] { "1", "2", "3", "4", "5", "6" };
            courseNum.Items.AddRange(courses);
        } 

        private int GetSem()
        {
            int _sem = 0;
            string value = semNum.Text;
            if (value == "I")
                _sem = 1;
            else if (value == "II")
                _sem = 2;
            else
                _sem = 0;
            return _sem;
        }
    }
}
