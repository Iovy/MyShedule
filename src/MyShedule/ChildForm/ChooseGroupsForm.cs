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
    public partial class ChooseGroupsForm : Form
    {
        public ChooseGroupsForm()
        {
            InitializeComponent();
        }

        public List<string> GroupsNames = new List<string>();
        public List<string> ChooseNames = new List<string>();
        public EducationLoadAdapter adapter = new EducationLoadAdapter();
        public List<ScheduleRoom> Rooms = new List<ScheduleRoom>();


        public ScheduleClasses.View ChooseView;
        public List<string> names;

        private void btnAccept_Click(object sender, EventArgs e)
        {
            ChooseNames.Clear();
            foreach (Object obj in ListGroups.CheckedItems)
            {
                ChooseNames.Add(obj.ToString());
            }

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void btnReject_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void ChooseGroupsForm_Load(object sender, EventArgs e)
        {
            int i = 0;
            ListGroups.Items.Clear();
            foreach (string group in adapter.NamesGroups)
            {
                ListGroups.Items.Add(group);
                ListGroups.SetItemChecked(i, true);
                i++;
            }

            BindingSource bs = new BindingSource();
            bs.DataSource = ScheduleView.BasicViews;
            cmbProjection.DisplayMember = "Description";
            cmbProjection.ValueMember = "TypeCode";
            cmbProjection.DataSource = bs;

            ChooseView =  (ScheduleClasses.View)Convert.ToInt32(cmbProjection.SelectedValue);

            this.cmbProjection.SelectedIndexChanged += new EventHandler(cmbProjection_SelectedIndexChanged);
        }



        void cmbProjection_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScheduleClasses.View view = (ScheduleClasses.View)Convert.ToInt32(cmbProjection.SelectedValue);
            ChooseView = view;

            List<string> names = new List<string>();

            switch (view)
            {
                case ScheduleClasses.View.Group: names = adapter.NamesGroups; break;
                case ScheduleClasses.View.Discipline: names = adapter.NamesDisciplines; break;
                case ScheduleClasses.View.Room: names = (from r in Rooms select r.Name).Distinct().ToList(); break;
                case ScheduleClasses.View.Teacher: names = adapter.NamesTeachers; break;
                default: names = new List<string>(); break;
            }

            int i = 0;
            ListGroups.Items.Clear();
            foreach (string name in names)
            {
                ListGroups.Items.Add(name);
                ListGroups.SetItemChecked(i, true);
                i++;
            }
        }
    }
}
