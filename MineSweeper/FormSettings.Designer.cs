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
            this.rbNewbie = new System.Windows.Forms.RadioButton();
            this.rbAmateur = new System.Windows.Forms.RadioButton();
            this.rbProfessional = new System.Windows.Forms.RadioButton();
            this.rbCustom = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.nudHeight = new System.Windows.Forms.NumericUpDown();
            this.nudWidth = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.nudMines = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.gbParameters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMines)).BeginInit();
            this.SuspendLayout();
            // 
            // butClose
            // 
            this.butClose.Location = new System.Drawing.Point(167, 176);
            this.butClose.Name = "butClose";
            this.butClose.Size = new System.Drawing.Size(75, 23);
            this.butClose.TabIndex = 0;
            this.butClose.TabStop = false;
            this.butClose.Text = "Close";
            this.butClose.UseVisualStyleBackColor = true;
            this.butClose.Click += new System.EventHandler(this.butClose_Click);
            // 
            // gbParameters
            // 
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
            this.gbParameters.Location = new System.Drawing.Point(0, 0);
            this.gbParameters.Name = "gbParameters";
            this.gbParameters.Size = new System.Drawing.Size(250, 170);
            this.gbParameters.TabIndex = 1;
            this.gbParameters.TabStop = false;
            // 
            // rbNewbie
            // 
            this.rbNewbie.AutoSize = true;
            this.rbNewbie.Location = new System.Drawing.Point(6, 12);
            this.rbNewbie.Name = "rbNewbie";
            this.rbNewbie.Size = new System.Drawing.Size(68, 17);
            this.rbNewbie.TabIndex = 0;
            this.rbNewbie.Text = "Новичок";
            this.rbNewbie.UseVisualStyleBackColor = true;
            this.rbNewbie.CheckedChanged += new System.EventHandler(this.rbNewbie_CheckedChanged);
            // 
            // rbAmateur
            // 
            this.rbAmateur.AutoSize = true;
            this.rbAmateur.Location = new System.Drawing.Point(6, 64);
            this.rbAmateur.Name = "rbAmateur";
            this.rbAmateur.Size = new System.Drawing.Size(76, 17);
            this.rbAmateur.TabIndex = 1;
            this.rbAmateur.Text = "Любитель";
            this.rbAmateur.UseVisualStyleBackColor = true;
            this.rbAmateur.CheckedChanged += new System.EventHandler(this.rbAmateur_CheckedChanged);
            // 
            // rbProfessional
            // 
            this.rbProfessional.AutoSize = true;
            this.rbProfessional.Location = new System.Drawing.Point(6, 117);
            this.rbProfessional.Name = "rbProfessional";
            this.rbProfessional.Size = new System.Drawing.Size(101, 17);
            this.rbProfessional.TabIndex = 2;
            this.rbProfessional.Text = "Профессионал";
            this.rbProfessional.UseVisualStyleBackColor = true;
            this.rbProfessional.CheckedChanged += new System.EventHandler(this.rbProfessional_CheckedChanged);
            // 
            // rbCustom
            // 
            this.rbCustom.AutoSize = true;
            this.rbCustom.Location = new System.Drawing.Point(177, 12);
            this.rbCustom.Name = "rbCustom";
            this.rbCustom.Size = new System.Drawing.Size(65, 17);
            this.rbCustom.TabIndex = 3;
            this.rbCustom.Text = "Особый";
            this.rbCustom.UseVisualStyleBackColor = true;
            this.rbCustom.CheckedChanged += new System.EventHandler(this.rbCustom_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Поле: 9 х 9 ячеек";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Мин: 10";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 94);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Мин: 40";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(23, 81);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(106, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Поле: 16 х 16 ячеек";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(23, 147);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Мин: 99";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(23, 134);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(106, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "Поле: 16 х 30 ячеек";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(150, 29);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(48, 13);
            this.label7.TabIndex = 10;
            this.label7.Text = "Высота:";
            // 
            // nudHeight
            // 
            this.nudHeight.Enabled = false;
            this.nudHeight.Location = new System.Drawing.Point(202, 27);
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
            this.nudHeight.Size = new System.Drawing.Size(40, 20);
            this.nudHeight.TabIndex = 11;
            this.nudHeight.TabStop = false;
            this.nudHeight.Value = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.nudHeight.ValueChanged += new System.EventHandler(this.nudHeight_ValueChanged);
            // 
            // nudWidth
            // 
            this.nudWidth.Enabled = false;
            this.nudWidth.Location = new System.Drawing.Point(202, 53);
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
            this.nudWidth.Size = new System.Drawing.Size(40, 20);
            this.nudWidth.TabIndex = 13;
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
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(149, 55);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(49, 13);
            this.label8.TabIndex = 12;
            this.label8.Text = "Ширина:";
            // 
            // nudMines
            // 
            this.nudMines.Enabled = false;
            this.nudMines.Location = new System.Drawing.Point(202, 79);
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
            this.nudMines.Size = new System.Drawing.Size(40, 20);
            this.nudMines.TabIndex = 15;
            this.nudMines.TabStop = false;
            this.nudMines.Value = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.nudMines.ValueChanged += new System.EventHandler(this.nudMines_ValueChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(159, 81);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(39, 13);
            this.label9.TabIndex = 14;
            this.label9.Text = "Мины:";
            // 
            // FormSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(252, 208);
            this.Controls.Add(this.gbParameters);
            this.Controls.Add(this.butClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSettings";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Параметры";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormSettings_FormClosing);
            this.Load += new System.EventHandler(this.FormSettings_Load);
            this.gbParameters.ResumeLayout(false);
            this.gbParameters.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMines)).EndInit();
            this.ResumeLayout(false);

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
    }
}