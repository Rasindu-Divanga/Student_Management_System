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
    
    public partial class AddEditStudentForm : Form
    {
        //Database Connection
        private readonly model.StudentManagementBasicEntities _dbdata;
        public AddEditStudentForm()
        {
            InitializeComponent();
            _dbdata = new model.StudentManagementBasicEntities();
        }

        

        private void label1_MouseLeave(object sender, EventArgs e)
        {
            label1.Font = new Font(label1.Font.Name, 35, FontStyle.Regular);
        }

        private void label1_MouseEnter(object sender, EventArgs e)
        {
            label1.Font = new Font(label1.Font.Name, 38, FontStyle.Regular);
        }

        //GridView Loader
        public void populategrid()
        {
            var record = _dbdata.Studentdetails.Select(q => new
            {
                Studentname = q.StudentName,
                studentBD = q.StudentBD,
                age = q.Age,
                coursename = q.CourseName,
                phone = q.PhoneNumber,
                sex = q.sex,
                ID =q.id
            }).ToList();

            gvDetails.DataSource = record;
            gvDetails.Columns["Studentname"].HeaderText = "Student Name";
            gvDetails.Columns["studentBD"].HeaderText = "Birthday";
            gvDetails.Columns["age"].HeaderText = "Age";
            gvDetails.Columns["coursename"].HeaderText = "Course";
            gvDetails.Columns["phone"].HeaderText = "Phone Number";
            gvDetails.Columns["sex"].HeaderText = "Sex";

            gvDetails.Columns["ID"].Visible = false;
        }

        //Clear
        public void Clear()
        {
            txtStudentName.ResetText();
            dtBD.ResetText();
            cmbAge.Items.Clear();
            cmbCourseName.ResetText();
            txtPhoneNumber.Clear();
            cmbSex.Items.Clear();
        }

        //Load Course Names for combo box
        public void CourseNameLoad()
        {
            var CourseNames = _dbdata.CourseNames.ToList();
            cmbCourseName.DataSource = CourseNames;
            cmbCourseName.ValueMember = "id";
            cmbCourseName.DisplayMember = "CourseName1";
        }

        //Add Button Code
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                var studentname = txtStudentName.Text;
                var studentbd = dtBD.Value;
                var age = cmbAge.Text;
                var coursename = cmbCourseName.Text;
                var phonenumber = txtPhoneNumber.Text;
                var sex = cmbSex.Text;

                var isValid = true;
                var error = "";

                //Check the text Boxes All ready.
                if (string.IsNullOrWhiteSpace(studentname))
                {
                    isValid = false;
                    error += "Error : Something is Wrong in Student Name. Please check.\n\r";
                }


                if (string.IsNullOrWhiteSpace(age))
                {
                    isValid = false;
                    error += "Error : Something is Wrong in Age Box. Please check.\n\r";
                }


                if (string.IsNullOrWhiteSpace(coursename))
                {
                    isValid = false;
                    error += "Error : Something is Wrong in Course Name. Please check.\n\r";
                }


                if (phonenumber.Length > 10 || phonenumber.Length < 10)
                {
                    isValid = false;
                    error += "Error : Something is Wrong in Student Phone Number. Please check.\n\r";
                }


                if (string.IsNullOrWhiteSpace(sex))
                {
                    isValid = false;
                    error += "Error : Something is Wrong in Student's Sex Type. Please check.\n\r";
                }

                if (isValid)
                {
                    var Student = new model.Studentdetail
                    {
                        StudentName = studentname,
                        StudentBD = studentbd,
                        Age = int.Parse(age),
                        CourseName = coursename,
                        PhoneNumber = phonenumber,
                        sex = sex
                    };
                    _dbdata.Studentdetails.Add(Student);
                    _dbdata.SaveChanges();

                    MessageBox.Show("Save Successfull.");
                    Clear();
                    populategrid();
                    CourseNameLoad();
                }
                else
                {
                    MessageBox.Show(error);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Some Error! Please Check And Try Again.");
            }

        }

        //Form Loader
        private void AddEditStudentForm_Load(object sender, EventArgs e)
        {

            CourseNameLoad();

            populategrid();

        }

       private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                var ID = (int)gvDetails.SelectedRows[0].Cells["id"].Value;
                var Student = _dbdata.Studentdetails.FirstOrDefault(q => q.id == ID);

                var studentEditForm = new StudentEditForm (Student, this);
                studentEditForm.ShowDialog();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
           
            
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                var ID = (int)gvDetails.SelectedRows[0].Cells["id"].Value;
                var Student = _dbdata.Studentdetails.FirstOrDefault(q => q.id == ID);

                DialogResult dr = MessageBox.Show("Are You sure to want delete this record?", "Delete", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (dr == DialogResult.Yes)
                {
                    _dbdata.Studentdetails.Remove(Student);
                    _dbdata.SaveChanges();
                }
                populategrid();
            }
            catch (Exception)
            {
                MessageBox.Show("Something Is wrong Your Selection. Please try Again.");
            }
        }

        private void createDeleteCoursesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                 var adddeletecourse = new AddDeleteCourses(this);
                adddeletecourse.ShowDialog();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
    }
}
