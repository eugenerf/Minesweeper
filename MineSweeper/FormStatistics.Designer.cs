namespace MineSweeper
{
    partial class FormStatistics
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormStatistics));
            this.lbPreset = new System.Windows.Forms.ListBox();
            this.lPreset = new System.Windows.Forms.Label();
            this.lTop5Name = new System.Windows.Forms.Label();
            this.cbStatsType = new System.Windows.Forms.CheckBox();
            this.dgvTop5 = new System.Windows.Forms.DataGridView();
            this.dgvTop10 = new System.Windows.Forms.DataGridView();
            this.gbPresetStats = new System.Windows.Forms.GroupBox();
            this.lMaxLooseStreak = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.lMaxWinStreak = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.lLooseStreak = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lWinStreak = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.lMinGameTime = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lMaxGameTime = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lWinPercent = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lWinGames = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lTotalGames = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.butClearStats = new System.Windows.Forms.Button();
            this.butClose = new System.Windows.Forms.Button();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cPreset = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cGameTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cWinTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cLevel3BV = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTop5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTop10)).BeginInit();
            this.gbPresetStats.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbPreset
            // 
            this.lbPreset.Items.AddRange(new object[] {
            resources.GetString("lbPreset.Items"),
            resources.GetString("lbPreset.Items1"),
            resources.GetString("lbPreset.Items2")});
            resources.ApplyResources(this.lbPreset, "lbPreset");
            this.lbPreset.Name = "lbPreset";
            this.lbPreset.TabStop = false;
            this.lbPreset.SelectedIndexChanged += new System.EventHandler(this.lbPreset_SelectedIndexChanged);
            // 
            // lPreset
            // 
            resources.ApplyResources(this.lPreset, "lPreset");
            this.lPreset.Name = "lPreset";
            // 
            // lTop5Name
            // 
            resources.ApplyResources(this.lTop5Name, "lTop5Name");
            this.lTop5Name.Name = "lTop5Name";
            // 
            // cbStatsType
            // 
            resources.ApplyResources(this.cbStatsType, "cbStatsType");
            this.cbStatsType.Name = "cbStatsType";
            this.cbStatsType.TabStop = false;
            this.cbStatsType.UseVisualStyleBackColor = true;
            this.cbStatsType.CheckedChanged += new System.EventHandler(this.cbStatsType_CheckedChanged);
            // 
            // dgvTop5
            // 
            this.dgvTop5.AllowUserToAddRows = false;
            this.dgvTop5.AllowUserToDeleteRows = false;
            this.dgvTop5.AllowUserToResizeRows = false;
            this.dgvTop5.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvTop5.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvTop5.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.dgvTop5.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvTop5.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cGameTime,
            this.cName,
            this.cWinTime,
            this.cLevel3BV});
            this.dgvTop5.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            resources.ApplyResources(this.dgvTop5, "dgvTop5");
            this.dgvTop5.MultiSelect = false;
            this.dgvTop5.Name = "dgvTop5";
            this.dgvTop5.ReadOnly = true;
            this.dgvTop5.RowHeadersVisible = false;
            this.dgvTop5.ShowCellErrors = false;
            this.dgvTop5.ShowCellToolTips = false;
            this.dgvTop5.ShowEditingIcon = false;
            this.dgvTop5.ShowRowErrors = false;
            this.dgvTop5.TabStop = false;
            // 
            // dgvTop10
            // 
            this.dgvTop10.AllowUserToAddRows = false;
            this.dgvTop10.AllowUserToDeleteRows = false;
            this.dgvTop10.AllowUserToResizeRows = false;
            this.dgvTop10.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvTop10.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvTop10.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.dgvTop10.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvTop10.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.cPreset});
            this.dgvTop10.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            resources.ApplyResources(this.dgvTop10, "dgvTop10");
            this.dgvTop10.MultiSelect = false;
            this.dgvTop10.Name = "dgvTop10";
            this.dgvTop10.ReadOnly = true;
            this.dgvTop10.RowHeadersVisible = false;
            this.dgvTop10.ShowCellErrors = false;
            this.dgvTop10.ShowCellToolTips = false;
            this.dgvTop10.ShowEditingIcon = false;
            this.dgvTop10.ShowRowErrors = false;
            this.dgvTop10.TabStop = false;
            // 
            // gbPresetStats
            // 
            this.gbPresetStats.Controls.Add(this.lMaxLooseStreak);
            this.gbPresetStats.Controls.Add(this.label11);
            this.gbPresetStats.Controls.Add(this.lMaxWinStreak);
            this.gbPresetStats.Controls.Add(this.label13);
            this.gbPresetStats.Controls.Add(this.lLooseStreak);
            this.gbPresetStats.Controls.Add(this.label7);
            this.gbPresetStats.Controls.Add(this.lWinStreak);
            this.gbPresetStats.Controls.Add(this.label9);
            this.gbPresetStats.Controls.Add(this.lMinGameTime);
            this.gbPresetStats.Controls.Add(this.label6);
            this.gbPresetStats.Controls.Add(this.lMaxGameTime);
            this.gbPresetStats.Controls.Add(this.label5);
            this.gbPresetStats.Controls.Add(this.lWinPercent);
            this.gbPresetStats.Controls.Add(this.label4);
            this.gbPresetStats.Controls.Add(this.lWinGames);
            this.gbPresetStats.Controls.Add(this.label3);
            this.gbPresetStats.Controls.Add(this.lTotalGames);
            this.gbPresetStats.Controls.Add(this.label1);
            resources.ApplyResources(this.gbPresetStats, "gbPresetStats");
            this.gbPresetStats.Name = "gbPresetStats";
            this.gbPresetStats.TabStop = false;
            // 
            // lMaxLooseStreak
            // 
            resources.ApplyResources(this.lMaxLooseStreak, "lMaxLooseStreak");
            this.lMaxLooseStreak.Name = "lMaxLooseStreak";
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // lMaxWinStreak
            // 
            resources.ApplyResources(this.lMaxWinStreak, "lMaxWinStreak");
            this.lMaxWinStreak.Name = "lMaxWinStreak";
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.Name = "label13";
            // 
            // lLooseStreak
            // 
            resources.ApplyResources(this.lLooseStreak, "lLooseStreak");
            this.lLooseStreak.Name = "lLooseStreak";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // lWinStreak
            // 
            resources.ApplyResources(this.lWinStreak, "lWinStreak");
            this.lWinStreak.Name = "lWinStreak";
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // lMinGameTime
            // 
            resources.ApplyResources(this.lMinGameTime, "lMinGameTime");
            this.lMinGameTime.Name = "lMinGameTime";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // lMaxGameTime
            // 
            resources.ApplyResources(this.lMaxGameTime, "lMaxGameTime");
            this.lMaxGameTime.Name = "lMaxGameTime";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // lWinPercent
            // 
            resources.ApplyResources(this.lWinPercent, "lWinPercent");
            this.lWinPercent.Name = "lWinPercent";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // lWinGames
            // 
            resources.ApplyResources(this.lWinGames, "lWinGames");
            this.lWinGames.Name = "lWinGames";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // lTotalGames
            // 
            resources.ApplyResources(this.lTotalGames, "lTotalGames");
            this.lTotalGames.Name = "lTotalGames";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // butClearStats
            // 
            resources.ApplyResources(this.butClearStats, "butClearStats");
            this.butClearStats.Name = "butClearStats";
            this.butClearStats.UseVisualStyleBackColor = true;
            this.butClearStats.Click += new System.EventHandler(this.butClearStats_Click);
            // 
            // butClose
            // 
            resources.ApplyResources(this.butClose, "butClose");
            this.butClose.Name = "butClose";
            this.butClose.UseVisualStyleBackColor = true;
            this.butClose.Click += new System.EventHandler(this.butClose_Click);
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            resources.ApplyResources(this.dataGridViewTextBoxColumn4, "dataGridViewTextBoxColumn4");
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            resources.ApplyResources(this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            resources.ApplyResources(this.dataGridViewTextBoxColumn2, "dataGridViewTextBoxColumn2");
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            resources.ApplyResources(this.dataGridViewTextBoxColumn3, "dataGridViewTextBoxColumn3");
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cPreset
            // 
            this.cPreset.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            resources.ApplyResources(this.cPreset, "cPreset");
            this.cPreset.Name = "cPreset";
            this.cPreset.ReadOnly = true;
            this.cPreset.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cGameTime
            // 
            this.cGameTime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            resources.ApplyResources(this.cGameTime, "cGameTime");
            this.cGameTime.Name = "cGameTime";
            this.cGameTime.ReadOnly = true;
            this.cGameTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // cName
            // 
            this.cName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            resources.ApplyResources(this.cName, "cName");
            this.cName.Name = "cName";
            this.cName.ReadOnly = true;
            this.cName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cWinTime
            // 
            this.cWinTime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            resources.ApplyResources(this.cWinTime, "cWinTime");
            this.cWinTime.Name = "cWinTime";
            this.cWinTime.ReadOnly = true;
            this.cWinTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cLevel3BV
            // 
            this.cLevel3BV.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            resources.ApplyResources(this.cLevel3BV, "cLevel3BV");
            this.cLevel3BV.Name = "cLevel3BV";
            this.cLevel3BV.ReadOnly = true;
            this.cLevel3BV.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // FormStatistics
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.butClose);
            this.Controls.Add(this.butClearStats);
            this.Controls.Add(this.gbPresetStats);
            this.Controls.Add(this.dgvTop10);
            this.Controls.Add(this.dgvTop5);
            this.Controls.Add(this.cbStatsType);
            this.Controls.Add(this.lTop5Name);
            this.Controls.Add(this.lPreset);
            this.Controls.Add(this.lbPreset);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormStatistics";
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormStatistics_FormClosing);
            this.Load += new System.EventHandler(this.FormStatistics_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTop5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTop10)).EndInit();
            this.gbPresetStats.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lbPreset;
        private System.Windows.Forms.Label lPreset;
        private System.Windows.Forms.Label lTop5Name;
        private System.Windows.Forms.CheckBox cbStatsType;
        private System.Windows.Forms.DataGridView dgvTop5;
        private System.Windows.Forms.DataGridView dgvTop10;
        private System.Windows.Forms.GroupBox gbPresetStats;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lTotalGames;
        private System.Windows.Forms.Label lWinGames;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lWinPercent;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lMaxGameTime;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lMinGameTime;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lMaxLooseStreak;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lMaxWinStreak;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label lLooseStreak;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lWinStreak;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button butClearStats;
        private System.Windows.Forms.Button butClose;
        private System.Windows.Forms.DataGridViewTextBoxColumn cGameTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn cName;
        private System.Windows.Forms.DataGridViewTextBoxColumn cWinTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn cLevel3BV;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn cPreset;
    }
}