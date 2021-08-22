using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BasicStudentManagementSystem.UI
{
    public partial class AddDeleteCourses : Form
    {
        private AddEditStudentForm _addeditstudent;

        private readonly model.StudentManagementBasicEntities _dbdata;
        public AddDeleteCourses(AddEditStudentForm addeditstudent)
        {
            InitializeComponent();
            _addeditstudent = addeditstudent;
            _dbdata = new model.StudentManagementBasicEntities();
        }


        public void populategrid()
        {
            var record = _dbdata.CourseNames.Select(q => new
            {
                course = q.CourseName1,
                
                ID = q.id
            }).ToList();

            GVCourseName.DataSource = record;
            GVCourseName.Columns["course"].HeaderText = "Course Name";


            GVCourseName.Columns["ID"].Visible = false;
        }


        public void Clear()
        {
            txtNewName.ResetText();
            
        }



        private void btnSaveCourse_Click(object sender, EventArgs e)
        {
            try
            {
                var newCourse = txtNewName.Text;

                if (string.IsNullOrWhiteSpace(newCourse))
                {

                    MessageBox.Show("Error : Something is Wrong in Course Name. Please check.\n\r");
                }
                else
                {
                    var NewCourseName = new model.CourseName
                    {
                        CourseName1 = newCourse
                    };
                    _dbdata.CourseNames.Add(NewCourseName);
                    _dbdata.SaveChanges();
                    populategrid();
                    Clear();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void AddDeleteCourses_Load(object sender, EventArgs e)
        {
            populategrid();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                var ID = (int)GVCourseName.SelectedRows[0].Cells["id"].Value;
                var course = _dbdata.CourseNames.FirstOrDefault(q => q.id == ID);

                DialogResult dr = MessageBox.Show("Are You sure to want delete this record?", "Delete", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (dr == DialogResult.Yes)
                {
                    _dbdata.CourseNames.Remove(course);
                    _dbdata.SaveChanges();
                }
                populategrid();
            }
            catch (Exception)
            {
                MessageBox.Show("Something Is wrong Your Selection. Please try Again.");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are You sure to want leave??", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dr == DialogResult.Yes)
            {
                _addeditstudent.CourseNameLoad();
                Close();
            }
           
        }
    }
}
