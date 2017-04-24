namespace MeetSheetGenerator
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnBrowse = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxSchools = new System.Windows.Forms.ComboBox();
            this.progressBarFileRead = new System.Windows.Forms.ProgressBar();
            this.labelProgress = new System.Windows.Forms.Label();
            this.listBoxEvents = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(6, 19);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 0;
            this.btnBrowse.Text = "Open PDF";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.labelProgress);
            this.groupBox1.Controls.Add(this.progressBarFileRead);
            this.groupBox1.Controls.Add(this.btnBrowse);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(332, 47);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Generate a New Meet Sheet";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.listBoxEvents);
            this.groupBox2.Controls.Add(this.comboBoxSchools);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Enabled = false;
            this.groupBox2.Location = new System.Drawing.Point(18, 65);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(326, 265);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Customize";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "School Name:";
            // 
            // comboBoxSchools
            // 
            this.comboBoxSchools.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSchools.FormattingEnabled = true;
            this.comboBoxSchools.Location = new System.Drawing.Point(86, 13);
            this.comboBoxSchools.Name = "comboBoxSchools";
            this.comboBoxSchools.Size = new System.Drawing.Size(121, 21);
            this.comboBoxSchools.Sorted = true;
            this.comboBoxSchools.TabIndex = 5;
            // 
            // progressBarFileRead
            // 
            this.progressBarFileRead.Location = new System.Drawing.Point(193, 18);
            this.progressBarFileRead.Name = "progressBarFileRead";
            this.progressBarFileRead.Size = new System.Drawing.Size(133, 23);
            this.progressBarFileRead.TabIndex = 3;
            // 
            // labelProgress
            // 
            this.labelProgress.AutoSize = true;
            this.labelProgress.Location = new System.Drawing.Point(107, 24);
            this.labelProgress.Name = "labelProgress";
            this.labelProgress.Size = new System.Drawing.Size(80, 13);
            this.labelProgress.TabIndex = 4;
            this.labelProgress.Text = "Read Progress:";
            // 
            // listBoxEvents
            // 
            this.listBoxEvents.FormattingEnabled = true;
            this.listBoxEvents.Location = new System.Drawing.Point(9, 56);
            this.listBoxEvents.Name = "listBoxEvents";
            this.listBoxEvents.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.listBoxEvents.Size = new System.Drawing.Size(120, 199);
            this.listBoxEvents.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Events:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(173, 94);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(361, 342);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Meet Sheet Generator";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxSchools;
        private System.Windows.Forms.ProgressBar progressBarFileRead;
        private System.Windows.Forms.Label labelProgress;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox listBoxEvents;
        private System.Windows.Forms.Button button1;
    }
}

