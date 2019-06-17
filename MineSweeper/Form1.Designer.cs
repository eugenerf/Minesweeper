namespace MineSweeper
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
            this.msMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiGame,
            this.tsmiAbout});
            this.msMenu.Location = new System.Drawing.Point(0, 0);
            this.msMenu.Name = "msMenu";
            this.msMenu.Size = new System.Drawing.Size(209, 24);
            this.msMenu.TabIndex = 0;
            this.msMenu.Text = "Main menu";
            // 
            // tsmiGame
            // 
            this.tsmiGame.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsmiGame.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiNewGame,
            this.toolStripSeparator1,
            this.tsmiParameters,
            this.toolStripSeparator2,
            this.tsmiExit});
            this.tsmiGame.Name = "tsmiGame";
            this.tsmiGame.Size = new System.Drawing.Size(46, 20);
            this.tsmiGame.Text = "Игра";
            // 
            // tsmiNewGame
            // 
            this.tsmiNewGame.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsmiNewGame.Name = "tsmiNewGame";
            this.tsmiNewGame.Size = new System.Drawing.Size(138, 22);
            this.tsmiNewGame.Text = "Новая игра";
            this.tsmiNewGame.Click += new System.EventHandler(this.tsmiNewGame_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(135, 6);
            // 
            // tsmiParameters
            // 
            this.tsmiParameters.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsmiParameters.Name = "tsmiParameters";
            this.tsmiParameters.Size = new System.Drawing.Size(138, 22);
            this.tsmiParameters.Text = "Параметры";
            this.tsmiParameters.Click += new System.EventHandler(this.tsmiParameters_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(135, 6);
            // 
            // tsmiExit
            // 
            this.tsmiExit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsmiExit.Name = "tsmiExit";
            this.tsmiExit.Size = new System.Drawing.Size(138, 22);
            this.tsmiExit.Text = "Выход";
            this.tsmiExit.Click += new System.EventHandler(this.tsmiExit_Click);
            // 
            // tsmiAbout
            // 
            this.tsmiAbout.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsmiAbout.Name = "tsmiAbout";
            this.tsmiAbout.Size = new System.Drawing.Size(63, 20);
            this.tsmiAbout.Text = "Об игре";
            this.tsmiAbout.Click += new System.EventHandler(this.tsmiAbout_Click);
            // 
            // gbMineField
            // 
            this.gbMineField.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.gbMineField.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.gbMineField.Location = new System.Drawing.Point(5, 62);
            this.gbMineField.Name = "gbMineField";
            this.gbMineField.Size = new System.Drawing.Size(200, 200);
            this.gbMineField.TabIndex = 1;
            this.gbMineField.TabStop = false;
            // 
            // butNewGame
            // 
            this.butNewGame.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.butNewGame.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.butNewGame.ImageIndex = 2;
            this.butNewGame.ImageList = this.ilIconsHeader;
            this.butNewGame.Location = new System.Drawing.Point(90, 30);
            this.butNewGame.Name = "butNewGame";
            this.butNewGame.Size = new System.Drawing.Size(30, 30);
            this.butNewGame.TabIndex = 2;
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
            this.lMines.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lMines.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lMines.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lMines.Font = new System.Drawing.Font("Candara", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lMines.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lMines.Location = new System.Drawing.Point(45, 30);
            this.lMines.Name = "lMines";
            this.lMines.Size = new System.Drawing.Size(40, 30);
            this.lMines.TabIndex = 4;
            this.lMines.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lTime
            // 
            this.lTime.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lTime.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lTime.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lTime.Font = new System.Drawing.Font("Candara", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lTime.Location = new System.Drawing.Point(160, 30);
            this.lTime.Name = "lTime";
            this.lTime.Size = new System.Drawing.Size(40, 30);
            this.lTime.TabIndex = 5;
            this.lTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tTime
            // 
            this.tTime.Interval = 1000;
            this.tTime.Tick += new System.EventHandler(this.tTime_Tick);
            // 
            // lMineIco
            // 
            this.lMineIco.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lMineIco.ImageIndex = 0;
            this.lMineIco.ImageList = this.ilIconsHeader;
            this.lMineIco.Location = new System.Drawing.Point(10, 30);
            this.lMineIco.Name = "lMineIco";
            this.lMineIco.Size = new System.Drawing.Size(30, 30);
            this.lMineIco.TabIndex = 6;
            // 
            // lTimeIco
            // 
            this.lTimeIco.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lTimeIco.ImageIndex = 1;
            this.lTimeIco.ImageList = this.ilIconsHeader;
            this.lTimeIco.Location = new System.Drawing.Point(125, 30);
            this.lTimeIco.Name = "lTimeIco";
            this.lTimeIco.Size = new System.Drawing.Size(30, 30);
            this.lTimeIco.TabIndex = 7;
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
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(209, 266);
            this.Controls.Add(this.lTimeIco);
            this.Controls.Add(this.lMineIco);
            this.Controls.Add(this.lTime);
            this.Controls.Add(this.lMines);
            this.Controls.Add(this.butNewGame);
            this.Controls.Add(this.gbMineField);
            this.Controls.Add(this.msMenu);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.msMenu;
            this.MaximizeBox = false;
            this.Name = "FormMineSweeper";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MineSweeper";
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

