﻿namespace EvolutionGeometryFriends {
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
            this.button_SelectLevel = new System.Windows.Forms.Button();
            this.SelectedLevelImage = new System.Windows.Forms.PictureBox();
            this.button_StopEvolution = new System.Windows.Forms.Button();
            this.button_StartEvolution = new System.Windows.Forms.Button();
            this.button_LoadProject = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label_loadedProject = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label_status = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.SelectedLevelImage)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
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
            // button_SelectLevel
            // 
            this.button_SelectLevel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.button_SelectLevel.Location = new System.Drawing.Point(410, 191);
            this.button_SelectLevel.Name = "button_SelectLevel";
            this.button_SelectLevel.Size = new System.Drawing.Size(130, 32);
            this.button_SelectLevel.TabIndex = 5;
            this.button_SelectLevel.Text = "Select Level";
            this.button_SelectLevel.UseVisualStyleBackColor = true;
            this.button_SelectLevel.Click += new System.EventHandler(this.button_SelectLevel_Click);
            // 
            // SelectedLevelImage
            // 
            this.SelectedLevelImage.Location = new System.Drawing.Point(410, 106);
            this.SelectedLevelImage.Name = "SelectedLevelImage";
            this.SelectedLevelImage.Size = new System.Drawing.Size(130, 79);
            this.SelectedLevelImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.SelectedLevelImage.TabIndex = 6;
            this.SelectedLevelImage.TabStop = false;
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
            // numericUpDown1
            // 
            this.numericUpDown1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F);
            this.numericUpDown1.Location = new System.Drawing.Point(432, 71);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(56, 27);
            this.numericUpDown1.TabIndex = 16;
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
            this.label6.Location = new System.Drawing.Point(13, 36);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(45, 16);
            this.label6.TabIndex = 19;
            this.label6.Text = "Index";
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
            // ApplicationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(557, 342);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label_loadedProject);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.button_LoadProject);
            this.Controls.Add(this.button_StartEvolution);
            this.Controls.Add(this.button_StopEvolution);
            this.Controls.Add(this.SelectedLevelImage);
            this.Controls.Add(this.button_SelectLevel);
            this.Controls.Add(this.button_RunIndividual);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.individualTable);
            this.Name = "ApplicationForm";
            this.Text = "ApplicationForm";
            this.Load += new System.EventHandler(this.ApplicationForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.SelectedLevelImage)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel individualTable;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_RunIndividual;
        private System.Windows.Forms.Button button_SelectLevel;
        private System.Windows.Forms.PictureBox SelectedLevelImage;
        private System.Windows.Forms.Button button_StopEvolution;
        private System.Windows.Forms.Button button_StartEvolution;
        private System.Windows.Forms.Button button_LoadProject;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label_loadedProject;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label_status;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
    }
}