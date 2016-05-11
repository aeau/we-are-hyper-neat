namespace FORMTEST {
    partial class SelectLevel {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectLevel));
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.Level0 = new System.Windows.Forms.PictureBox();
            this.Level1 = new System.Windows.Forms.PictureBox();
            this.Level2 = new System.Windows.Forms.PictureBox();
            this.Level3 = new System.Windows.Forms.PictureBox();
            this.Level4 = new System.Windows.Forms.PictureBox();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Level0)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Level1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Level2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Level3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Level4)).BeginInit();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.Level0);
            this.flowLayoutPanel1.Controls.Add(this.Level1);
            this.flowLayoutPanel1.Controls.Add(this.Level2);
            this.flowLayoutPanel1.Controls.Add(this.Level3);
            this.flowLayoutPanel1.Controls.Add(this.Level4);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(9, 12);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(533, 83);
            this.flowLayoutPanel1.TabIndex = 5;
            // 
            // Level0
            // 
            this.Level0.BackColor = System.Drawing.SystemColors.Highlight;
            this.Level0.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Level0.Image = global::FORMTEST.Properties.Resources.Level0;
            this.Level0.InitialImage = ((System.Drawing.Image)(resources.GetObject("Level0.InitialImage")));
            this.Level0.Location = new System.Drawing.Point(3, 3);
            this.Level0.Name = "Level0";
            this.Level0.Size = new System.Drawing.Size(100, 75);
            this.Level0.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Level0.TabIndex = 0;
            this.Level0.TabStop = false;
            this.Level0.Click += new System.EventHandler(this.Level0_Click);
            // 
            // Level1
            // 
            this.Level1.Image = global::FORMTEST.Properties.Resources.Level1;
            this.Level1.InitialImage = ((System.Drawing.Image)(resources.GetObject("Level1.InitialImage")));
            this.Level1.Location = new System.Drawing.Point(109, 3);
            this.Level1.Name = "Level1";
            this.Level1.Size = new System.Drawing.Size(100, 75);
            this.Level1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Level1.TabIndex = 1;
            this.Level1.TabStop = false;
            this.Level1.Click += new System.EventHandler(this.Level1_Click);
            // 
            // Level2
            // 
            this.Level2.Image = global::FORMTEST.Properties.Resources.Level2;
            this.Level2.InitialImage = ((System.Drawing.Image)(resources.GetObject("Level2.InitialImage")));
            this.Level2.Location = new System.Drawing.Point(215, 3);
            this.Level2.Name = "Level2";
            this.Level2.Size = new System.Drawing.Size(100, 75);
            this.Level2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Level2.TabIndex = 2;
            this.Level2.TabStop = false;
            this.Level2.Click += new System.EventHandler(this.Level2_Click);
            // 
            // Level3
            // 
            this.Level3.Image = global::FORMTEST.Properties.Resources.Level3;
            this.Level3.InitialImage = ((System.Drawing.Image)(resources.GetObject("Level3.InitialImage")));
            this.Level3.Location = new System.Drawing.Point(321, 3);
            this.Level3.Name = "Level3";
            this.Level3.Size = new System.Drawing.Size(100, 75);
            this.Level3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Level3.TabIndex = 3;
            this.Level3.TabStop = false;
            this.Level3.Click += new System.EventHandler(this.Level3_Click);
            // 
            // Level4
            // 
            this.Level4.Image = global::FORMTEST.Properties.Resources.Level4;
            this.Level4.InitialImage = ((System.Drawing.Image)(resources.GetObject("Level4.InitialImage")));
            this.Level4.Location = new System.Drawing.Point(427, 3);
            this.Level4.Name = "Level4";
            this.Level4.Size = new System.Drawing.Size(100, 75);
            this.Level4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Level4.TabIndex = 4;
            this.Level4.TabStop = false;
            this.Level4.Click += new System.EventHandler(this.Level4_Click);
            // 
            // SelectLevel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(550, 103);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "SelectLevel";
            this.Text = "SelectLevel";
            this.flowLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Level0)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Level1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Level2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Level3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Level4)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.PictureBox Level0;
        private System.Windows.Forms.PictureBox Level1;
        private System.Windows.Forms.PictureBox Level2;
        private System.Windows.Forms.PictureBox Level3;
        private System.Windows.Forms.PictureBox Level4;
    }
}