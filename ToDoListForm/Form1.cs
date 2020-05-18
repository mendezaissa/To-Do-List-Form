using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ToDoListForm.Model; //need to add the reference to models

namespace ToDoListForm
{
    public partial class Form1 : Form
    {
        private ToDoListDBContext toDoListDBContext;

        public Form1()
        {
            InitializeComponent();

            toDoListDBContext = new ToDoListDBContext();

            var statuses = toDoListDBContext.Statuses.ToList();

            foreach (Status s in statuses)
            {
                cboStatus.Items.Add(s);
            }

            refreshData();
        }

        private void refreshData() //refreshes the list of tasks
        {
            BindingSource bi = new BindingSource();

            var query = from t in toDoListDBContext.Tasks
                        orderby t.DueDate
                        select new { t.Id, TaskName = t.Name, StatusName = t.Status.Name, t.DueDate };

            bi.DataSource = query.ToList();

            dataGridView1.DataSource = bi;
            dataGridView1.Refresh();
        }

        private void cmdCreateTask_Click(object sender, EventArgs e) //create task button
        {
            if(cboStatus.SelectedItem != null && txtTask.Text != String.Empty)
            {
                var newTask = new Model.Task
                {
                    Name = txtTask.Text,
                    StatusId = (cboStatus.SelectedItem as Model.Status).Id,
                    DueDate = dateTimePicker1.Value
                };

                toDoListDBContext.Tasks.Add(newTask);
                toDoListDBContext.SaveChanges(); //saves it to the database
                MessageBox.Show("Task Added Successfully");
                refreshData();

                cmdUpdateTask.Text = "Update";
                txtTask.Text = string.Empty;
                dateTimePicker1.Value = DateTime.Now;
                cboStatus.Text = "Please Select...";
            }
            else //gives error if left blank
            {
                MessageBox.Show("Please make sure all inputs have been entered");
            }
        }

        
        private void cmdDeleteTask_Click(object sender, EventArgs e) //removes tasks from list
        {
            var t = toDoListDBContext.Tasks.Find((int)dataGridView1.SelectedCells[0].Value);
            toDoListDBContext.Tasks.Remove(t);
            toDoListDBContext.SaveChanges();
            MessageBox.Show("Deletion Completed");
            refreshData();
        }

        private void cmdUpdateTask_Click(object sender, EventArgs e) //update button
        {
            if(cmdUpdateTask.Text == "Update")
            {
                txtTask.Text = dataGridView1.SelectedCells[1].Value.ToString(); //populates the name of the task
                dateTimePicker1.Value = (DateTime)dataGridView1.SelectedCells[3].Value; //populates the due date of the task

                foreach(Status s in cboStatus.Items) //populating status of the task
                {
                    if(s.Name == dataGridView1.SelectedCells[2].Value.ToString())
                    {
                        cboStatus.SelectedItem = s;
                    }
                }

                cmdUpdateTask.Text = "Save"; //replaces update with save
            }

            else if (cmdUpdateTask.Text == "Save") //if button equals to save then do this
            {
                var t = toDoListDBContext.Tasks.Find((int)dataGridView1.SelectedCells[0].Value);

                t.Name = txtTask.Text;
                t.StatusId = (cboStatus.SelectedItem as Status).Id;
                t.DueDate = dateTimePicker1.Value;

                toDoListDBContext.SaveChanges();
                MessageBox.Show("Update Complete");
                refreshData();

                cmdUpdateTask.Text = "Update";
                txtTask.Text = string.Empty;
                dateTimePicker1.Value = DateTime.Now;
                cboStatus.Text = "Please Select...";
            }
        }

        private void cmdCancel_Click(object sender, EventArgs e) //cancel button
        {
            cmdUpdateTask.Text = "Update";
            txtTask.Text = string.Empty;
            dateTimePicker1.Value = DateTime.Now;
            cboStatus.Text = "Please Select...";
        }
    }
}
