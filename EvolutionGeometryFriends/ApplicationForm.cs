﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using EvolutionGeometryFriends.Properties;

namespace EvolutionGeometryFriends {
    public partial class ApplicationForm : Form {
        
        public enum State
        {
            Stopped,
            Stopping,
            Starting,
            Running,
            Crashed
        }

        private State curretState = State.Stopped;

        public State CurrentState
        {
            get { return curretState; }
            set
            {
                curretState = value;
                switch (value)
                {
                    case State.Stopped:
                        label_status.Text = "STOPPED";
                        label_status.ForeColor = Color.DarkRed;
                    break;
                    case State.Stopping:
                        label_status.Text = "STOPPING";
                        label_status.ForeColor = Color.OrangeRed;
                        break;
                    case State.Starting:
                        label_status.Text = "STARTING";
                        label_status.ForeColor = Color.Gold;
                        break;
                    case State.Running:
                        label_status.Text = "RUNNING";
                        label_status.ForeColor = Color.Green;
                        break;
                    case State.Crashed:
                        label_status.Text = "CRASHED";
                        label_status.ForeColor = Color.MidnightBlue;
                        break;
                         
                }

                button_StopEvolution.Enabled = (curretState == State.Running);
                button_StartEvolution.Enabled = (curretState == State.Stopped);
            }
        }

        private string selectedProjectPath = "";
        private Dictionary<int, float> tableData = new Dictionary<int, float>();

        public ApplicationForm() {
            InitializeComponent();
        }

        private void LoadProject(string path) {
            selectedProjectPath = path;
            label_loadedProject.Text = Path.GetFileName(path);
        }

        private void ApplicationForm_Load(object sender, EventArgs e)
        {
            CurrentState = State.Stopped;
        }

        private void button_SelectLevel_Click(object sender, EventArgs e) {
            SelectLevel selectLevel = new SelectLevel();
            selectLevel.ShowDialog(this);
        }

        private void AddRow(int index, float fitness)
        {
            individualTable.RowCount = individualTable.RowCount + 1;
            individualTable.Controls.Add(new Label() { Text = index.ToString() }, 0, individualTable.RowCount - 1);
            individualTable.Controls.Add(new Label() { Text = fitness.ToString() }, 1, individualTable.RowCount - 1);
        }

        private void button_StartEvolution_Click(object sender, EventArgs e) {
            CurrentState = State.Starting;
            label_status.Update();
            Thread.Sleep(2000);
            CurrentState = State.Running;
        }

        private void button_StopEvolution_Click(object sender, EventArgs e) {
            CurrentState = State.Stopping;
            label_status.Update();
            Thread.Sleep(2000);
            CurrentState = State.Stopped;
        }

        private void button_LoadProject_Click(object sender, EventArgs e) {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            if (folderBrowser.ShowDialog() == DialogResult.OK) {
                LoadProject(folderBrowser.SelectedPath);
            }
        }

        private void button_RunIndividual_Click(object sender, EventArgs e) {

        }

        public void UpdateTable(Dictionary<int, float> tableData)
        {
            individualTable.Controls.Clear();
            this.tableData = tableData;
            foreach (KeyValuePair<int, float> pair in tableData)
            {
                AddRow(pair.Key, pair.Value);
            }
        }

    }
}
