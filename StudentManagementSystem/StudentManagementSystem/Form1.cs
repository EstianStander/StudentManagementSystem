using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace StudentManagementSystem
{
    public partial class Form1 : Form
    {
        private string filePath = "students.txt";

        public Form1()
        {
            InitializeComponent();
        }

        // Event Handler for Add Button
        private void btnAdd_Click(object sender, EventArgs e)
        {
            string studentID = txtStudentID.Text;
            string name = txtName.Text;
            string ageText = txtAge.Text;
            string course = txtCourse.Text;

            if (string.IsNullOrWhiteSpace(studentID) || string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(ageText) || string.IsNullOrWhiteSpace(course))
            {
                MessageBox.Show("All fields are required.");
                return;
            }

            if (!int.TryParse(ageText, out int age))
            {
                MessageBox.Show("Please enter a valid age.");
                return;
            }

            string studentRecord = $"{studentID},{name},{age},{course}";
            File.AppendAllText(filePath, studentRecord + Environment.NewLine);
            MessageBox.Show("Student added successfully.");
            ClearInputFields();
            LoadStudents();
        }

        // Event Handler for View All Button
        private void btnViewAll_Click(object sender, EventArgs e)
        {
            LoadStudents();
        }

        // Event Handler for Update Button
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string studentID = txtStudentID.Text;
            if (string.IsNullOrWhiteSpace(studentID))
            {
                MessageBox.Show("Please enter a Student ID to update.");
                return;
            }

            string name = txtName.Text;
            string ageText = txtAge.Text;
            string course = txtCourse.Text;

            if (!int.TryParse(ageText, out int age))
            {
                MessageBox.Show("Please enter a valid age.");
                return;
            }

            List<string> lines = File.ReadAllLines(filePath).ToList();
            bool studentFound = false;

            for (int i = 0; i < lines.Count; i++)
            {
                string[] fields = lines[i].Split(',');
                if (fields[0] == studentID)
                {
                    lines[i] = $"{studentID},{name},{age},{course}";
                    studentFound = true;
                    break;
                }
            }

            if (studentFound)
            {
                File.WriteAllLines(filePath, lines);
                MessageBox.Show("Student updated successfully.");
                LoadStudents();
            }
            else
            {
                MessageBox.Show("Student not found.");
            }

            ClearInputFields();
        }

        // Event Handler for Delete Button
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvStudents.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a student to delete.");
                return;
            }

            string studentID = dgvStudents.SelectedRows[0].Cells[0].Value.ToString();
            List<string> lines = File.ReadAllLines(filePath).ToList();
            lines = lines.Where(line => line.Split(',')[0] != studentID).ToList();

            File.WriteAllLines(filePath, lines);
            MessageBox.Show("Student deleted successfully.");
            LoadStudents();
        }

        // Event Handler for Generate Report Button
        private void btnGenerateReport_Click(object sender, EventArgs e)
        {
            List<string> lines = File.ReadAllLines(filePath).ToList();
            int totalStudents = lines.Count;
            int totalAge = 0;

            foreach (string line in lines)
            {
                string[] fields = line.Split(',');
                if (int.TryParse(fields[2], out int age))
                {
                    totalAge += age;
                }
            }

            double averageAge = totalStudents > 0 ? (double)totalAge / totalStudents : 0;
            lblTotalStudents.Text = "Total Students: " + totalStudents;
            lblAverageAge.Text = "Average Age: " + averageAge.ToString("F2");

            File.WriteAllText("summary.txt", $"Total Students: {totalStudents}\nAverage Age: {averageAge:F2}");
            MessageBox.Show("Report generated successfully.");
        }

        // Helper method to load students into DataGridView
        private void LoadStudents()
        {
            dgvStudents.Rows.Clear();
            if (!File.Exists(filePath)) return;

            foreach (string line in File.ReadAllLines(filePath))
            {
                string[] fields = line.Split(',');
                dgvStudents.Rows.Add(fields);
            }
        }

        // Helper method to clear input fields
        private void ClearInputFields()
        {
            txtStudentID.Clear();
            txtName.Clear();
            txtAge.Clear();
            txtCourse.Clear();
        }

        private void dgvStudents_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
