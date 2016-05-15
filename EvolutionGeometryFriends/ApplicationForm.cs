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
using System.Xml;
using EvolutionGeometryFriends.Properties;
using SharpNeat.Genomes.Neat;

namespace EvolutionGeometryFriends {
    public partial class ApplicationForm : Form {

        private static volatile ApplicationForm instance;
        public static ApplicationForm Instance
        {
            get
            {
                if (instance == null)
                    instance = new ApplicationForm();
                return instance;
            }
        }

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
                button_RunIndividual.Enabled = (curretState == State.Stopped);
            }
        }

        private string selectedProjectPath = "None";
        private List<string[]> tableData = new List<string[]>();

        private Thread evolutionThread;

        public void ChangeState()
        {
            CurrentState = State.Stopped;
        }

        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(Instance);
        }

        public ApplicationForm() {
            InitializeComponent();
        }

        private void LoadProject(string path) {
            selectedProjectPath = path;
            label_loadedProject.Text = Path.GetFileName(path);
            Program.SetProjectPath(path);
            LoadNeuralNetworkFile();
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
            if (selectedProjectPath == "None")
            {
                MessageBox.Show("Please select a project folder.");
                return;
            }
            CurrentState = State.Starting;

            BackgroundWorker bw = new BackgroundWorker();
            bw.WorkerReportsProgress = true;
            bw.DoWork += new DoWorkEventHandler(
            delegate(object o, DoWorkEventArgs args) {
                BackgroundWorker b = o as BackgroundWorker;

                Program.SetProjectPath(selectedProjectPath);
                Program.RunEvolution((int)runSpeed.Value, (int)nGenerations.Value - 1, this);
            });

            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(
            delegate(object o, RunWorkerCompletedEventArgs args) {
                OnStop();
            });
            bw.RunWorkerAsync();
            CurrentState = State.Running;
        }

        private void button_StopEvolution_Click(object sender, EventArgs e) {
            Program.StopEvolution();
            CurrentState = State.Stopping;
        }

        public void OnStop()
        {
            LoadNeuralNetworkFile();
            CurrentState = State.Stopped;
        }

        private void button_LoadProject_Click(object sender, EventArgs e) {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            if (folderBrowser.ShowDialog() == DialogResult.OK)
            {
                LoadProject(folderBrowser.SelectedPath);
            }
            else
            {
                Console.WriteLine(":;(");
            }
        }

        private void button_RunIndividual_Click(object sender, EventArgs e)
        {
            if (selectedProjectPath == "None") {
                MessageBox.Show("Please select a project folder.");
                return;
            }
            if (tableData.Count == 0)
            {
                MessageBox.Show("There is currently not project data to run individual from.\nSelect another project path or run evolution first.");
                return;
            }
            for (int i = 0; i < tableData.Count; i++)
            {
                if (tableData[i][0] == individualNumber.Value.ToString())
                {
                    Program.RunIndividual(i, (int)runSpeed.Value);
                    break;
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

        private void LoadNeuralNetworkFile()
        {
            var NEURAL_NETWORK_FILE = selectedProjectPath + "/circle_neural_network.xml";
            List<string[]> individuals = new List<string[]>();
            using (var reader = new XmlTextReader(NEURAL_NETWORK_FILE))
            {
                try
                {
                    while (reader.Read())
                    {
                        switch (reader.NodeType)
                        {
                            case XmlNodeType.Element: // The node is an element.
                                if (reader.Name == "Network")
                                {
                                    individuals.Add(new string[]
                                    {reader.GetAttribute("id"), reader.GetAttribute("fitness")});
                                }
                                break;

                        }
                    }
                }
                catch (Exception e)
                {
                    individualTable.Controls.Clear();
                    return;
                }
            }

            UpdateTable(individuals);
        }
    }
}
