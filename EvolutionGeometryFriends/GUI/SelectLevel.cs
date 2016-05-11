using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EvolutionGeometryFriends;

namespace FORMTEST {
    public partial class SelectLevel : Form {
        public SelectLevel() {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterParent;
        }

        private void Level0_Click(object sender, EventArgs e)
        {
            var form = (ApplicationForm)Owner;
            form.LevelIndex = 1;
            Close();
        }

        private void Level1_Click(object sender, EventArgs e) {
            var form = (ApplicationForm)Owner;
            form.LevelIndex = 2;
            Close();
        }

        private void Level2_Click(object sender, EventArgs e) {
            var form = (ApplicationForm)Owner;
            form.LevelIndex = 3;
            Close();
        }

        private void Level3_Click(object sender, EventArgs e) {
            var form = (ApplicationForm)Owner;
            form.LevelIndex = 4;
            Close();
        }

        private void Level4_Click(object sender, EventArgs e) {
            var form = (ApplicationForm)Owner;
            form.LevelIndex = 5;
            Close();
        }
    }
}
