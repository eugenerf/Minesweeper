namespace MineSweeper
{
    partial class FormSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSettings));
            this.butClose = new System.Windows.Forms.Button();
            this.gbParameters = new System.Windows.Forms.GroupBox();
            this.nudMines = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.nudWidth = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.nudHeight = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.rbCustom = new System.Windows.Forms.RadioButton();
            this.rbProfessional = new System.Windows.Forms.RadioButton();
            this.rbAmateur = new System.Windows.Forms.RadioButton();
            this.rbNewbie = new System.Windows.Forms.RadioButton();
            this.cbUseQuestionMarks = new System.Windows.Forms.CheckBox();
            this.cbUseStdDBClick = new System.Windows.Forms.CheckBox();
            this.gbParameters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMines)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHeight)).BeginInit();
            this.SuspendLayout();
            // 
            // butClose
            // 
            resources.ApplyResources(this.butClose, "butClose");
            this.butClose.Name = "butClose";
            this.butClose.TabStop = false;
            this.butClose.UseVisualStyleBackColor = true;
            this.butClose.Click += new System.EventHandler(this.butClose_Click);
            // 
            // gbParameters
            // 
            resources.ApplyResources(this.gbParameters, "gbParameters");
            this.gbParameters.Controls.Add(this.nudMines);
            this.gbParameters.Controls.Add(this.label9);
            this.gbParameters.Controls.Add(this.nudWidth);
            this.gbParameters.Controls.Add(this.label8);
            this.gbParameters.Controls.Add(this.nudHeight);
            this.gbParameters.Controls.Add(this.label7);
            this.gbParameters.Controls.Add(this.label5);
            this.gbParameters.Controls.Add(this.label6);
            this.gbParameters.Controls.Add(this.label3);
            this.gbParameters.Controls.Add(this.label4);
            this.gbParameters.Controls.Add(this.label2);
            this.gbParameters.Controls.Add(this.label1);
            this.gbParameters.Controls.Add(this.rbCustom);
            this.gbParameters.Controls.Add(this.rbProfessional);
            this.gbParameters.Controls.Add(this.rbAmateur);
            this.gbParameters.Controls.Add(this.rbNewbie);
            this.gbParameters.Name = "gbParameters";
            this.gbParameters.TabStop = false;
            // 
            // nudMines
            // 
            resources.ApplyResources(this.nudMines, "nudMines");
            this.nudMines.Maximum = new decimal(new int[] {
            668,
            0,
            0,
            0});
            this.nudMines.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudMines.Name = "nudMines";
            this.nudMines.TabStop = false;
            this.nudMines.Value = new decimal(new int[] {
            9,
            0,
            0,
            0});
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // nudWidth
            // 
            resources.ApplyResources(this.nudWidth, "nudWidth");
            this.nudWidth.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.nudWidth.Minimum = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.nudWidth.Name = "nudWidth";
            this.nudWidth.TabStop = false;
            this.nudWidth.Value = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.nudWidth.ValueChanged += new System.EventHandler(this.nudWidth_ValueChanged);
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // nudHeight
            // 
            resources.ApplyResources(this.nudHeight, "nudHeight");
            this.nudHeight.Maximum = new decimal(new int[] {
            24,
            0,
            0,
            0});
            this.nudHeight.Minimum = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.nudHeight.Name = "nudHeight";
            this.nudHeight.TabStop = false;
            this.nudHeight.Value = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.nudHeight.ValueChanged += new System.EventHandler(this.nudHeight_ValueChanged);
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // rbCustom
            // 
            resources.ApplyResources(this.rbCustom, "rbCustom");
            this.rbCustom.Name = "rbCustom";
            this.rbCustom.UseVisualStyleBackColor = true;
            this.rbCustom.CheckedChanged += new System.EventHandler(this.rbCustom_CheckedChanged);
            // 
            // rbProfessional
            // 
            resources.ApplyResources(this.rbProfessional, "rbProfessional");
            this.rbProfessional.Name = "rbProfessional";
            this.rbProfessional.UseVisualStyleBackColor = true;
            // 
            // rbAmateur
            // 
            resources.ApplyResources(this.rbAmateur, "rbAmateur");
            this.rbAmateur.Name = "rbAmateur";
            this.rbAmateur.UseVisualStyleBackColor = true;
            // 
            // rbNewbie
            // 
            resources.ApplyResources(this.rbNewbie, "rbNewbie");
            this.rbNewbie.Name = "rbNewbie";
            this.rbNewbie.UseVisualStyleBackColor = true;
            // 
            // cbUseQuestionMarks
            // 
            resources.ApplyResources(this.cbUseQuestionMarks, "cbUseQuestionMarks");
            this.cbUseQuestionMarks.Name = "cbUseQuestionMarks";
            this.cbUseQuestionMarks.TabStop = false;
            this.cbUseQuestionMarks.UseVisualStyleBackColor = true;
            // 
            // cbUseStdDBClick
            // 
            resources.ApplyResources(this.cbUseStdDBClick, "cbUseStdDBClick");
            this.cbUseStdDBClick.Name = "cbUseStdDBClick";
            this.cbUseStdDBClick.TabStop = false;
            this.cbUseStdDBClick.UseVisualStyleBackColor = true;
            // 
            // FormSettings
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cbUseStdDBClick);
            this.Controls.Add(this.cbUseQuestionMarks);
            this.Controls.Add(this.gbParameters);
            this.Controls.Add(this.butClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSettings";
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormSettings_FormClosing);
            this.Load += new System.EventHandler(this.FormSettings_Load);
            this.gbParameters.ResumeLayout(false);
            this.gbParameters.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMines)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHeight)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button butClose;
        private System.Windows.Forms.GroupBox gbParameters;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rbCustom;
        private System.Windows.Forms.RadioButton rbProfessional;
        private System.Windows.Forms.RadioButton rbAmateur;
        private System.Windows.Forms.RadioButton rbNewbie;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown nudHeight;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown nudMines;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown nudWidth;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox cbUseQuestionMarks;
        private System.Windows.Forms.CheckBox cbUseStdDBClick;
    }
}