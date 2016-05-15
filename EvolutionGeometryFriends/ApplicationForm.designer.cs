namespace EvolutionGeometryFriends {
    partial class ApplicationForm {
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
            this.individualTable = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.button_RunIndividual = new System.Windows.Forms.Button();
            this.button_StopEvolution = new System.Windows.Forms.Button();
            this.button_StartEvolution = new System.Windows.Forms.Button();
            this.button_LoadProject = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label_loadedProject = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label_status = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.individualNumber = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.runSpeed = new System.Windows.Forms.NumericUpDown();
            this.nGenerations = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.individualNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.runSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nGenerations)).BeginInit();
            this.SuspendLayout();
            // 
            // individualTable
            // 
            this.individualTable.AutoScroll = true;
            this.individualTable.BackColor = System.Drawing.Color.White;
            this.individualTable.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.individualTable.ColumnCount = 2;
            this.individualTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.individualTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.individualTable.Location = new System.Drawing.Point(12, 55);
            this.individualTable.Name = "individualTable";
            this.individualTable.RowCount = 1;
            this.individualTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.individualTable.Size = new System.Drawing.Size(392, 168);
            this.individualTable.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(8, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Individuals";
            // 
            // button_RunIndividual
            // 
            this.button_RunIndividual.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.button_RunIndividual.Location = new System.Drawing.Point(491, 68);
            this.button_RunIndividual.Name = "button_RunIndividual";
            this.button_RunIndividual.Size = new System.Drawing.Size(49, 32);
            this.button_RunIndividual.TabIndex = 3;
            this.button_RunIndividual.Text = "Run";
            this.button_RunIndividual.UseVisualStyleBackColor = true;
            this.button_RunIndividual.Click += new System.EventHandler(this.button_RunIndividual_Click);
            // 
            // button_StopEvolution
            // 
            this.button_StopEvolution.Enabled = false;
            this.button_StopEvolution.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.button_StopEvolution.Location = new System.Drawing.Point(12, 281);
            this.button_StopEvolution.Name = "button_StopEvolution";
            this.button_StopEvolution.Size = new System.Drawing.Size(143, 51);
            this.button_StopEvolution.TabIndex = 7;
            this.button_StopEvolution.Text = "Stop Evolution";
            this.button_StopEvolution.UseVisualStyleBackColor = true;
            this.button_StopEvolution.Click += new System.EventHandler(this.button_StopEvolution_Click);
            // 
            // button_StartEvolution
            // 
            this.button_StartEvolution.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.button_StartEvolution.Location = new System.Drawing.Point(393, 281);
            this.button_StartEvolution.Name = "button_StartEvolution";
            this.button_StartEvolution.Size = new System.Drawing.Size(143, 51);
            this.button_StartEvolution.TabIndex = 8;
            this.button_StartEvolution.Text = "Start Evolution";
            this.button_StartEvolution.UseVisualStyleBackColor = true;
            this.button_StartEvolution.Click += new System.EventHandler(this.button_StartEvolution_Click);
            // 
            // button_LoadProject
            // 
            this.button_LoadProject.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.button_LoadProject.Location = new System.Drawing.Point(12, 229);
            this.button_LoadProject.Name = "button_LoadProject";
            this.button_LoadProject.Size = new System.Drawing.Size(143, 32);
            this.button_LoadProject.TabIndex = 10;
            this.button_LoadProject.Text = "Load/Create Project";
            this.button_LoadProject.UseVisualStyleBackColor = true;
            this.button_LoadProject.Click += new System.EventHandler(this.button_LoadProject_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(161, 237);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(118, 16);
            this.label5.TabIndex = 11;
            this.label5.Text = "Loaded Project:";
            // 
            // label_loadedProject
            // 
            this.label_loadedProject.AutoSize = true;
            this.label_loadedProject.Location = new System.Drawing.Point(285, 240);
            this.label_loadedProject.Name = "label_loadedProject";
            this.label_loadedProject.Size = new System.Drawing.Size(0, 13);
            this.label_loadedProject.TabIndex = 12;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(240, 281);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 20);
            this.label3.TabIndex = 13;
            this.label3.Text = "Status:";
            // 
            // label_status
            // 
            this.label_status.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_status.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_status.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label_status.Location = new System.Drawing.Point(0, 0);
            this.label_status.Name = "label_status";
            this.label_status.Size = new System.Drawing.Size(225, 34);
            this.label_status.TabIndex = 14;
            this.label_status.Text = "Stopped";
            this.label_status.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label_status);
            this.panel1.Location = new System.Drawing.Point(162, 298);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(225, 34);
            this.panel1.TabIndex = 15;
            // 
            // individualNumber
            // 
            this.individualNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F);
            this.individualNumber.Location = new System.Drawing.Point(432, 71);
            this.individualNumber.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.individualNumber.Name = "individualNumber";
            this.individualNumber.Size = new System.Drawing.Size(56, 27);
            this.individualNumber.TabIndex = 16;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.label2.Location = new System.Drawing.Point(411, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(129, 24);
            this.label2.TabIndex = 17;
            this.label2.Text = "Run Individual";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.label4.Location = new System.Drawing.Point(406, 70);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(20, 24);
            this.label4.TabIndex = 18;
            this.label4.Text = "#";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(12, 36);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(21, 16);
            this.label6.TabIndex = 19;
            this.label6.Text = "Id";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(209, 36);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(58, 16);
            this.label7.TabIndex = 20;
            this.label7.Text = "Fitness";
            // 
            // runSpeed
            // 
            this.runSpeed.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F);
            this.runSpeed.Location = new System.Drawing.Point(480, 153);
            this.runSpeed.Maximum = new decimal(new int[] {
            75,
            0,
            0,
            0});
            this.runSpeed.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.runSpeed.Name = "runSpeed";
            this.runSpeed.Size = new System.Drawing.Size(56, 27);
            this.runSpeed.TabIndex = 21;
            this.runSpeed.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // nGenerations
            // 
            this.nGenerations.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F);
            this.nGenerations.Location = new System.Drawing.Point(480, 248);
            this.nGenerations.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.nGenerations.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.nGenerations.Name = "nGenerations";
            this.nGenerations.Size = new System.Drawing.Size(56, 27);
            this.nGenerations.TabIndex = 22;
            this.nGenerations.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(420, 158);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(54, 16);
            this.label8.TabIndex = 23;
            this.label8.Text = "Speed";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(351, 253);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(123, 16);
            this.label9.TabIndex = 24;
            this.label9.Text = "# Of Generations";
            // 
            // ApplicationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(557, 342);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.nGenerations);
            this.Controls.Add(this.runSpeed);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.individualNumber);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label_loadedProject);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.button_LoadProject);
            this.Controls.Add(this.button_StartEvolution);
            this.Controls.Add(this.button_StopEvolution);
            this.Controls.Add(this.button_RunIndividual);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.individualTable);
            this.Name = "ApplicationForm";
            this.Text = "ApplicationForm";
            this.Load += new System.EventHandler(this.ApplicationForm_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.individualNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.runSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nGenerations)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private volatile System.Windows.Forms.TableLayoutPanel individualTable;
        private System.Windows.Forms.Label label1;
        private volatile System.Windows.Forms.Button button_RunIndividual;
        private volatile System.Windows.Forms.Button button_StopEvolution;
        private volatile System.Windows.Forms.Button button_StartEvolution;
        private volatile System.Windows.Forms.Button button_LoadProject;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label_loadedProject;
        private System.Windows.Forms.Label label3;
        private volatile System.Windows.Forms.Label label_status;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.NumericUpDown individualNumber;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown runSpeed;
        private System.Windows.Forms.NumericUpDown nGenerations;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
    }
}