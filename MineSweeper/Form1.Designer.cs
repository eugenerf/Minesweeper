﻿namespace MineSweeper
{
    partial class FormMineSweeper
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMineSweeper));
            this.msMenu = new System.Windows.Forms.MenuStrip();
            this.tsmiGame = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiNewGame = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiParameters = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiExit = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.gbMineField = new System.Windows.Forms.GroupBox();
            this.butNewGame = new System.Windows.Forms.Button();
            this.ilIconsHeader = new System.Windows.Forms.ImageList(this.components);
            this.lMines = new System.Windows.Forms.Label();
            this.lTime = new System.Windows.Forms.Label();
            this.tTime = new System.Windows.Forms.Timer(this.components);
            this.lMineIco = new System.Windows.Forms.Label();
            this.lTimeIco = new System.Windows.Forms.Label();
            this.ilIconsField = new System.Windows.Forms.ImageList(this.components);
            this.msMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // msMenu
            // 
            resources.ApplyResources(this.msMenu, "msMenu");
            this.msMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiGame,
            this.tsmiAbout});
            this.msMenu.Name = "msMenu";
            // 
            // tsmiGame
            // 
            resources.ApplyResources(this.tsmiGame, "tsmiGame");
            this.tsmiGame.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsmiGame.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiNewGame,
            this.toolStripSeparator1,
            this.tsmiParameters,
            this.toolStripSeparator2,
            this.tsmiExit});
            this.tsmiGame.Name = "tsmiGame";
            // 
            // tsmiNewGame
            // 
            resources.ApplyResources(this.tsmiNewGame, "tsmiNewGame");
            this.tsmiNewGame.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsmiNewGame.Name = "tsmiNewGame";
            this.tsmiNewGame.Click += new System.EventHandler(this.tsmiNewGame_Click);
            // 
            // toolStripSeparator1
            // 
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            // 
            // tsmiParameters
            // 
            resources.ApplyResources(this.tsmiParameters, "tsmiParameters");
            this.tsmiParameters.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsmiParameters.Name = "tsmiParameters";
            this.tsmiParameters.Click += new System.EventHandler(this.tsmiParameters_Click);
            // 
            // toolStripSeparator2
            // 
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            // 
            // tsmiExit
            // 
            resources.ApplyResources(this.tsmiExit, "tsmiExit");
            this.tsmiExit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsmiExit.Name = "tsmiExit";
            this.tsmiExit.Click += new System.EventHandler(this.tsmiExit_Click);
            // 
            // tsmiAbout
            // 
            resources.ApplyResources(this.tsmiAbout, "tsmiAbout");
            this.tsmiAbout.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsmiAbout.Name = "tsmiAbout";
            this.tsmiAbout.Click += new System.EventHandler(this.tsmiAbout_Click);
            // 
            // gbMineField
            // 
            resources.ApplyResources(this.gbMineField, "gbMineField");
            this.gbMineField.Name = "gbMineField";
            this.gbMineField.TabStop = false;
            // 
            // butNewGame
            // 
            resources.ApplyResources(this.butNewGame, "butNewGame");
            this.butNewGame.ImageList = this.ilIconsHeader;
            this.butNewGame.Name = "butNewGame";
            this.butNewGame.TabStop = false;
            this.butNewGame.UseVisualStyleBackColor = true;
            this.butNewGame.Click += new System.EventHandler(this.butNewGame_Click);
            // 
            // ilIconsHeader
            // 
            this.ilIconsHeader.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilIconsHeader.ImageStream")));
            this.ilIconsHeader.TransparentColor = System.Drawing.Color.Transparent;
            this.ilIconsHeader.Images.SetKeyName(0, "mine_big.png");
            this.ilIconsHeader.Images.SetKeyName(1, "time.png");
            this.ilIconsHeader.Images.SetKeyName(2, "smiley_new.png");
            this.ilIconsHeader.Images.SetKeyName(3, "smiley_loose.png");
            this.ilIconsHeader.Images.SetKeyName(4, "smiley_win.png");
            // 
            // lMines
            // 
            resources.ApplyResources(this.lMines, "lMines");
            this.lMines.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lMines.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lMines.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lMines.Name = "lMines";
            // 
            // lTime
            // 
            resources.ApplyResources(this.lTime, "lTime");
            this.lTime.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lTime.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lTime.Name = "lTime";
            // 
            // tTime
            // 
            this.tTime.Interval = 1000;
            this.tTime.Tick += new System.EventHandler(this.tTime_Tick);
            // 
            // lMineIco
            // 
            resources.ApplyResources(this.lMineIco, "lMineIco");
            this.lMineIco.ImageList = this.ilIconsHeader;
            this.lMineIco.Name = "lMineIco";
            // 
            // lTimeIco
            // 
            resources.ApplyResources(this.lTimeIco, "lTimeIco");
            this.lTimeIco.ImageList = this.ilIconsHeader;
            this.lTimeIco.Name = "lTimeIco";
            // 
            // ilIconsField
            // 
            this.ilIconsField.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilIconsField.ImageStream")));
            this.ilIconsField.TransparentColor = System.Drawing.Color.Transparent;
            this.ilIconsField.Images.SetKeyName(0, "mine_big.png");
            this.ilIconsField.Images.SetKeyName(1, "flag.png");
            this.ilIconsField.Images.SetKeyName(2, "question.png");
            // 
            // FormMineSweeper
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.lTimeIco);
            this.Controls.Add(this.lMineIco);
            this.Controls.Add(this.lTime);
            this.Controls.Add(this.lMines);
            this.Controls.Add(this.butNewGame);
            this.Controls.Add(this.gbMineField);
            this.Controls.Add(this.msMenu);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.msMenu;
            this.MaximizeBox = false;
            this.Name = "FormMineSweeper";
            this.Load += new System.EventHandler(this.FormMineSweeper_Load);
            this.msMenu.ResumeLayout(false);
            this.msMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip msMenu;
        private System.Windows.Forms.ToolStripMenuItem tsmiGame;
        private System.Windows.Forms.ToolStripMenuItem tsmiNewGame;
        private System.Windows.Forms.ToolStripMenuItem tsmiParameters;
        private System.Windows.Forms.ToolStripMenuItem tsmiExit;
        private System.Windows.Forms.ToolStripMenuItem tsmiAbout;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.GroupBox gbMineField;
        private System.Windows.Forms.Button butNewGame;
        private System.Windows.Forms.Label lMines;
        private System.Windows.Forms.Label lTime;
        private System.Windows.Forms.ImageList ilIconsHeader;
        private System.Windows.Forms.Timer tTime;
        private System.Windows.Forms.Label lMineIco;
        private System.Windows.Forms.Label lTimeIco;
        private System.Windows.Forms.ImageList ilIconsField;
    }
}
