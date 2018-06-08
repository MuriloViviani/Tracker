using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Tracker
{
    public partial class Form1 : Form
    {
        private int seconds = 0, minutes = 0, hours = 0;
        private bool running = false;

        // Task Management variables
        string[] task = new string[4];
        List<string[]> tasks = new List<string[]>();

        public Form1()
        {
            InitializeComponent();

            btnStop.Enabled = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtTaskName.Focus();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (running)
            {
                if (txtTaskName.Text.Trim().Equals(""))
                {
                    MessageBox.Show("You Need to inform the task name first");
                    txtTaskName.Focus();

                    return;
                }
                else
                    SaveTask();
            }
            else
            {
                if (txtTaskName.Text.Trim().Equals(""))
                {
                    MessageBox.Show("You Need to inform the task name first");
                    txtTaskName.Focus();

                    return;
                }
                else
                {
                    lsvHistoric.Items.Clear();

                    running = true;
                    btnStart.Text = "New Task";

                    btnStop.Enabled = true;
                    SecondsTimer.Enabled = true;
                }
            }

            // Get new task information
            task[0] = txtTaskName.Text;
            task[1] = DateTime.Now.ToString("HH:mm:ss");

            Reset();

            lblCurrentTask.Text = task[0];
        }

        private void SecondsTimer_Tick(object sender, EventArgs e)
        {
            if (seconds >= 59)
            {
                seconds = 0;
                if (minutes >= 59)
                {
                    minutes = 0;
                    hours++;
                }
                else { minutes++; }
            }
            else { seconds++; }

            UpdateTimerLabels();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            SaveTask();

            SecondsTimer.Enabled = false;

            Reset();

            btnStart.Text = "Start";
            btnStop.Enabled = false;
            running = false;

            var path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Run_" + DateTime.Now.ToString("dd/MM/yyyy").Replace("/", "_") + "_" + Guid.NewGuid() + ".txt";
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(path))
            {
                file.WriteLine("Task Block from - " + DateTime.Now.ToString());
                file.WriteLine("-----------------------------------------------------------------------");
                file.WriteLine("-----------------------------------------------------------------------");
                file.WriteLine("Task name - Start time - End time - Spent time");
                file.WriteLine("-----------------------------------------------------------------------");
                foreach (var t in tasks)
                {
                    file.WriteLine(t[0] + " - " + t[1] + " - " + t[2] + " - " + t[3]);
                }
            }
        }

        private void SaveTask()
        {
            var initialTime = DateTime.Parse(task[1]);
            var finalTime = DateTime.Now;

            // Get the final time
            task[2] = finalTime.ToString("HH:mm:ss");

            task[3] = finalTime.Subtract(initialTime).ToString();

            // Save current task in the List View
            var listViewItem = new ListViewItem(task);
            lsvHistoric.Items.Add(listViewItem);

            tasks.Add(new string[] { task[0], task[1], task[2], task[3] });
        }

        private void UpdateTimerLabels()
        {
            lblSeconds.Text = seconds.ToString().PadLeft(2, '0');
            lblMinutes.Text = minutes.ToString().PadLeft(2, '0');
            lblHours.Text = hours.ToString().PadLeft(2, '0');
        }

        private void Reset()
        {
            seconds = 0;
            minutes = 0;
            hours = 0;

            txtTaskName.Text = "";
            txtTaskName.Focus();

            lblCurrentTask.Text = "No Task running";

            UpdateTimerLabels();
        }
    }
}
