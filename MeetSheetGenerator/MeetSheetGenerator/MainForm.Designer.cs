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
            this.labelProgress = new System.Windows.Forms.Label();
            this.progressBarFileRead = new System.Windows.Forms.ProgressBar();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.buttonSelectNone = new System.Windows.Forms.Button();
            this.buttonSelectAll = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.checkBoxExtraSlot = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.radioButtonLastInitalFirst = new System.Windows.Forms.RadioButton();
            this.radioButtonFirstInitalLast = new System.Windows.Forms.RadioButton();
            this.radioButtonLastFirst = new System.Windows.Forms.RadioButton();
            this.radioButtonFirstLast = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.listBoxEvents = new System.Windows.Forms.ListBox();
            this.comboBoxSchools = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(6, 19);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 0;
            this.btnBrowse.Text = "Open File";
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
            // labelProgress
            // 
            this.labelProgress.AutoSize = true;
            this.labelProgress.Location = new System.Drawing.Point(107, 24);
            this.labelProgress.Name = "labelProgress";
            this.labelProgress.Size = new System.Drawing.Size(80, 13);
            this.labelProgress.TabIndex = 4;
            this.labelProgress.Text = "Read Progress:";
            // 
            // progressBarFileRead
            // 
            this.progressBarFileRead.Location = new System.Drawing.Point(193, 18);
            this.progressBarFileRead.Name = "progressBarFileRead";
            this.progressBarFileRead.Size = new System.Drawing.Size(133, 23);
            this.progressBarFileRead.TabIndex = 3;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.buttonSelectNone);
            this.groupBox2.Controls.Add(this.buttonSelectAll);
            this.groupBox2.Controls.Add(this.groupBox4);
            this.groupBox2.Controls.Add(this.groupBox3);
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.listBoxEvents);
            this.groupBox2.Controls.Add(this.comboBoxSchools);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Enabled = false;
            this.groupBox2.Location = new System.Drawing.Point(18, 65);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(326, 320);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Customize";
            // 
            // buttonSelectNone
            // 
            this.buttonSelectNone.Location = new System.Drawing.Point(9, 290);
            this.buttonSelectNone.Name = "buttonSelectNone";
            this.buttonSelectNone.Size = new System.Drawing.Size(120, 23);
            this.buttonSelectNone.TabIndex = 12;
            this.buttonSelectNone.Text = "Unselect All";
            this.buttonSelectNone.UseVisualStyleBackColor = true;
            this.buttonSelectNone.Click += new System.EventHandler(this.buttonSelectNone_Click);
            // 
            // buttonSelectAll
            // 
            this.buttonSelectAll.Location = new System.Drawing.Point(9, 261);
            this.buttonSelectAll.Name = "buttonSelectAll";
            this.buttonSelectAll.Size = new System.Drawing.Size(120, 23);
            this.buttonSelectAll.TabIndex = 11;
            this.buttonSelectAll.Text = "Select All";
            this.buttonSelectAll.UseVisualStyleBackColor = true;
            this.buttonSelectAll.Click += new System.EventHandler(this.buttonSelectAll_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.checkBoxExtraSlot);
            this.groupBox4.Location = new System.Drawing.Point(141, 113);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(179, 100);
            this.groupBox4.TabIndex = 10;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Other Options";
            // 
            // checkBoxExtraSlot
            // 
            this.checkBoxExtraSlot.AutoSize = true;
            this.checkBoxExtraSlot.Location = new System.Drawing.Point(7, 20);
            this.checkBoxExtraSlot.Name = "checkBoxExtraSlot";
            this.checkBoxExtraSlot.Size = new System.Drawing.Size(133, 17);
            this.checkBoxExtraSlot.TabIndex = 0;
            this.checkBoxExtraSlot.Text = "Inluce Extra Blank Slot";
            this.checkBoxExtraSlot.UseVisualStyleBackColor = true;
            this.checkBoxExtraSlot.CheckedChanged += new System.EventHandler(this.checkBoxExtraSlot_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.radioButtonLastInitalFirst);
            this.groupBox3.Controls.Add(this.radioButtonFirstInitalLast);
            this.groupBox3.Controls.Add(this.radioButtonLastFirst);
            this.groupBox3.Controls.Add(this.radioButtonFirstLast);
            this.groupBox3.Location = new System.Drawing.Point(135, 40);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(185, 65);
            this.groupBox3.TabIndex = 9;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Output Name Options";
            // 
            // radioButtonLastInitalFirst
            // 
            this.radioButtonLastInitalFirst.AutoSize = true;
            this.radioButtonLastInitalFirst.Location = new System.Drawing.Point(81, 42);
            this.radioButtonLastInitalFirst.Name = "radioButtonLastInitalFirst";
            this.radioButtonLastInitalFirst.Size = new System.Drawing.Size(94, 17);
            this.radioButtonLastInitalFirst.TabIndex = 3;
            this.radioButtonLastInitalFirst.TabStop = true;
            this.radioButtonLastInitalFirst.Text = "Last Inital/First";
            this.radioButtonLastInitalFirst.UseVisualStyleBackColor = true;
            this.radioButtonLastInitalFirst.CheckedChanged += new System.EventHandler(this.radioButtonLastInitalFirst_CheckedChanged);
            // 
            // radioButtonFirstInitalLast
            // 
            this.radioButtonFirstInitalLast.AutoSize = true;
            this.radioButtonFirstInitalLast.Location = new System.Drawing.Point(81, 19);
            this.radioButtonFirstInitalLast.Name = "radioButtonFirstInitalLast";
            this.radioButtonFirstInitalLast.Size = new System.Drawing.Size(94, 17);
            this.radioButtonFirstInitalLast.TabIndex = 2;
            this.radioButtonFirstInitalLast.TabStop = true;
            this.radioButtonFirstInitalLast.Text = "First Inital/Last";
            this.radioButtonFirstInitalLast.UseVisualStyleBackColor = true;
            this.radioButtonFirstInitalLast.CheckedChanged += new System.EventHandler(this.radioButtonFirstInitalLast_CheckedChanged);
            // 
            // radioButtonLastFirst
            // 
            this.radioButtonLastFirst.AutoSize = true;
            this.radioButtonLastFirst.Location = new System.Drawing.Point(6, 42);
            this.radioButtonLastFirst.Name = "radioButtonLastFirst";
            this.radioButtonLastFirst.Size = new System.Drawing.Size(69, 17);
            this.radioButtonLastFirst.TabIndex = 1;
            this.radioButtonLastFirst.TabStop = true;
            this.radioButtonLastFirst.Text = "Last/First";
            this.radioButtonLastFirst.UseVisualStyleBackColor = true;
            this.radioButtonLastFirst.CheckedChanged += new System.EventHandler(this.radioButtonLastFirst_CheckedChanged);
            // 
            // radioButtonFirstLast
            // 
            this.radioButtonFirstLast.AutoSize = true;
            this.radioButtonFirstLast.Checked = true;
            this.radioButtonFirstLast.Location = new System.Drawing.Point(6, 19);
            this.radioButtonFirstLast.Name = "radioButtonFirstLast";
            this.radioButtonFirstLast.Size = new System.Drawing.Size(69, 17);
            this.radioButtonFirstLast.TabIndex = 0;
            this.radioButtonFirstLast.TabStop = true;
            this.radioButtonFirstLast.Text = "First/Last";
            this.radioButtonFirstLast.UseVisualStyleBackColor = true;
            this.radioButtonFirstLast.CheckedChanged += new System.EventHandler(this.radioButtonFirstLast_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(169, 219);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(112, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "Open In Word";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
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
            // listBoxEvents
            // 
            this.listBoxEvents.FormattingEnabled = true;
            this.listBoxEvents.Location = new System.Drawing.Point(9, 56);
            this.listBoxEvents.Name = "listBoxEvents";
            this.listBoxEvents.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.listBoxEvents.Size = new System.Drawing.Size(120, 199);
            this.listBoxEvents.TabIndex = 6;
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "School Name:";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(361, 390);
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
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
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
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton radioButtonLastInitalFirst;
        private System.Windows.Forms.RadioButton radioButtonFirstInitalLast;
        private System.Windows.Forms.RadioButton radioButtonLastFirst;
        private System.Windows.Forms.RadioButton radioButtonFirstLast;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox checkBoxExtraSlot;
        private System.Windows.Forms.Button buttonSelectNone;
        private System.Windows.Forms.Button buttonSelectAll;
    }
}

