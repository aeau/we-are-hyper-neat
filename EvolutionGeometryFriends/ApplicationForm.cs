using System;
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
        private List<string[]> tableData = new List<string[]>();

        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new ApplicationForm());
        }

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
            runSpeed.Value = 75;
        }


        private void AddRow(string index, string fitness)
        {
            individualTable.RowCount = individualTable.RowCount + 1;
            individualTable.Controls.Add(new Label() { Text = index }, 0, individualTable.RowCount - 1);
            individualTable.Controls.Add(new Label() { Text = fitness }, 1, individualTable.RowCount - 1);
        }

        private void button_StartEvolution_Click(object sender, EventArgs e) {
            CurrentState = State.Starting;
            (new Thread(() => {
                Program.SetProjectPath(Environment.CurrentDirectory + "/../../../neural_network_params");
                Program.RunEvolution((int)runSpeed.Value, (int)nGenerations.Value);
            })).Start();
            CurrentState = State.Running;
        }

        private void button_StopEvolution_Click(object sender, EventArgs e) {
            Program.StopEvolution();
            CurrentState = State.Stopping;
        }

        private void button_LoadProject_Click(object sender, EventArgs e) {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            if (folderBrowser.ShowDialog() == DialogResult.OK) {
                LoadProject(folderBrowser.SelectedPath);
            }
        }

        private void button_RunIndividual_Click(object sender, EventArgs e)
        {
            MessageBox.Show(individualNumber.Value.ToString());
            for (int i = 0; i < tableData.Count; i++)
            {
                if (tableData[i][0] == individualNumber.Value.ToString())
                {
                    //Program.RunIndividual(i);
                    return;
                }
            }
        }

        public void UpdateTable(List<string[]> tableData)
        {
            individualTable.Controls.Clear();
            this.tableData = tableData;
            foreach (var row in tableData)
            {
                AddRow(row[0], row[1]);
            }
        }

    }
}
