namespace MiniCraft.Core
{
    partial class FormMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.winGLCanvas1 = new CSharpGL.WinGLCanvas();
            this.panel_inGame = new System.Windows.Forms.Panel();
            this.button_resume = new System.Windows.Forms.Button();
            this.button_saveAndExit = new System.Windows.Forms.Button();
            this.bagUIBox = new System.Windows.Forms.PictureBox();
            this.panel_mainMenu = new System.Windows.Forms.Panel();
            this.button_singlePlayer = new System.Windows.Forms.Button();
            this.button_exit = new System.Windows.Forms.Button();
            this.button_multiPlayer = new System.Windows.Forms.Button();
            this.panel_singlePlayer = new System.Windows.Forms.Panel();
            this.panel_mapSelect = new System.Windows.Forms.Panel();
            this.listView_mapSelector = new System.Windows.Forms.ListView();
            this.columnHeader_mapName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_size = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_modTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.button_startGame = new System.Windows.Forms.Button();
            this.panel_command = new System.Windows.Forms.Panel();
            this.cmd_Text = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.winGLCanvas1)).BeginInit();
            this.panel_inGame.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bagUIBox)).BeginInit();
            this.panel_mainMenu.SuspendLayout();
            this.panel_singlePlayer.SuspendLayout();
            this.panel_mapSelect.SuspendLayout();
            this.panel_command.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Interval = 30;
            // 
            // winGLCanvas1
            // 
            this.winGLCanvas1.AccumAlphaBits = ((byte)(0));
            this.winGLCanvas1.AccumBits = ((byte)(0));
            this.winGLCanvas1.AccumBlueBits = ((byte)(0));
            this.winGLCanvas1.AccumGreenBits = ((byte)(0));
            this.winGLCanvas1.AccumRedBits = ((byte)(0));
            this.winGLCanvas1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.winGLCanvas1.Location = new System.Drawing.Point(0, 0);
            this.winGLCanvas1.Margin = new System.Windows.Forms.Padding(4);
            this.winGLCanvas1.Name = "winGLCanvas1";
            this.winGLCanvas1.RenderTrigger = CSharpGL.RenderTrigger.TimerBased;
            this.winGLCanvas1.Size = new System.Drawing.Size(1165, 510);
            this.winGLCanvas1.StencilBits = ((byte)(0));
            this.winGLCanvas1.TabIndex = 4;
            this.winGLCanvas1.TimerTriggerInterval = 40;
            this.winGLCanvas1.UpdateContextVersion = true;
            // 
            // panel_inGame
            // 
            this.panel_inGame.Controls.Add(this.panel_command);
            this.panel_inGame.Controls.Add(this.button_resume);
            this.panel_inGame.Controls.Add(this.button_saveAndExit);
            this.panel_inGame.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_inGame.Location = new System.Drawing.Point(0, 0);
            this.panel_inGame.Name = "panel_inGame";
            this.panel_inGame.Size = new System.Drawing.Size(1165, 510);
            this.panel_inGame.TabIndex = 5;
            this.panel_inGame.Visible = false;
            // 
            // button_resume
            // 
            this.button_resume.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_resume.Location = new System.Drawing.Point(525, 209);
            this.button_resume.Name = "button_resume";
            this.button_resume.Size = new System.Drawing.Size(133, 49);
            this.button_resume.TabIndex = 0;
            this.button_resume.Text = "继续游戏";
            this.button_resume.UseVisualStyleBackColor = true;
            this.button_resume.Click += new System.EventHandler(this.button_resume_Click);
            // 
            // button_saveAndExit
            // 
            this.button_saveAndExit.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_saveAndExit.Location = new System.Drawing.Point(525, 299);
            this.button_saveAndExit.Name = "button_saveAndExit";
            this.button_saveAndExit.Size = new System.Drawing.Size(133, 49);
            this.button_saveAndExit.TabIndex = 0;
            this.button_saveAndExit.Text = "保存并退出";
            this.button_saveAndExit.UseVisualStyleBackColor = true;
            this.button_saveAndExit.Click += new System.EventHandler(this.button_saveAndExit_Click);
            // 
            // bagUIBox
            // 
            this.bagUIBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.bagUIBox.Location = new System.Drawing.Point(1006, 472);
            this.bagUIBox.Margin = new System.Windows.Forms.Padding(2);
            this.bagUIBox.Name = "bagUIBox";
            this.bagUIBox.Size = new System.Drawing.Size(433, 39);
            this.bagUIBox.TabIndex = 3;
            this.bagUIBox.TabStop = false;
            this.bagUIBox.Visible = false;
            // 
            // panel_mainMenu
            // 
            this.panel_mainMenu.Controls.Add(this.button_singlePlayer);
            this.panel_mainMenu.Controls.Add(this.button_exit);
            this.panel_mainMenu.Controls.Add(this.button_multiPlayer);
            this.panel_mainMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_mainMenu.Location = new System.Drawing.Point(0, 0);
            this.panel_mainMenu.Name = "panel_mainMenu";
            this.panel_mainMenu.Size = new System.Drawing.Size(1165, 510);
            this.panel_mainMenu.TabIndex = 6;
            // 
            // button_singlePlayer
            // 
            this.button_singlePlayer.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_singlePlayer.Location = new System.Drawing.Point(525, 182);
            this.button_singlePlayer.Name = "button_singlePlayer";
            this.button_singlePlayer.Size = new System.Drawing.Size(133, 48);
            this.button_singlePlayer.TabIndex = 0;
            this.button_singlePlayer.Text = "单人游戏";
            this.button_singlePlayer.UseVisualStyleBackColor = true;
            this.button_singlePlayer.Click += new System.EventHandler(this.button_singlePlayer_Click);
            // 
            // button_exit
            // 
            this.button_exit.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_exit.Location = new System.Drawing.Point(525, 344);
            this.button_exit.Name = "button_exit";
            this.button_exit.Size = new System.Drawing.Size(133, 44);
            this.button_exit.TabIndex = 0;
            this.button_exit.Text = "退出";
            this.button_exit.UseVisualStyleBackColor = true;
            // 
            // button_multiPlayer
            // 
            this.button_multiPlayer.Enabled = false;
            this.button_multiPlayer.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_multiPlayer.Location = new System.Drawing.Point(525, 264);
            this.button_multiPlayer.Name = "button_multiPlayer";
            this.button_multiPlayer.Size = new System.Drawing.Size(133, 46);
            this.button_multiPlayer.TabIndex = 0;
            this.button_multiPlayer.Text = "多人游戏";
            this.button_multiPlayer.UseVisualStyleBackColor = true;
            // 
            // panel_singlePlayer
            // 
            this.panel_singlePlayer.Controls.Add(this.panel_mapSelect);
            this.panel_singlePlayer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_singlePlayer.Location = new System.Drawing.Point(0, 0);
            this.panel_singlePlayer.Name = "panel_singlePlayer";
            this.panel_singlePlayer.Size = new System.Drawing.Size(1165, 510);
            this.panel_singlePlayer.TabIndex = 7;
            this.panel_singlePlayer.Visible = false;
            // 
            // panel_mapSelect
            // 
            this.panel_mapSelect.Controls.Add(this.listView_mapSelector);
            this.panel_mapSelect.Controls.Add(this.button_startGame);
            this.panel_mapSelect.Location = new System.Drawing.Point(362, 86);
            this.panel_mapSelect.Margin = new System.Windows.Forms.Padding(2);
            this.panel_mapSelect.Name = "panel_mapSelect";
            this.panel_mapSelect.Size = new System.Drawing.Size(363, 305);
            this.panel_mapSelect.TabIndex = 2;
            // 
            // listView_mapSelector
            // 
            this.listView_mapSelector.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader_mapName,
            this.columnHeader_size,
            this.columnHeader_modTime});
            this.listView_mapSelector.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView_mapSelector.Location = new System.Drawing.Point(0, 0);
            this.listView_mapSelector.Name = "listView_mapSelector";
            this.listView_mapSelector.Size = new System.Drawing.Size(363, 268);
            this.listView_mapSelector.TabIndex = 3;
            this.listView_mapSelector.UseCompatibleStateImageBehavior = false;
            this.listView_mapSelector.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader_mapName
            // 
            this.columnHeader_mapName.Text = "地图名称";
            this.columnHeader_mapName.Width = 161;
            // 
            // columnHeader_size
            // 
            this.columnHeader_size.Text = "大小";
            this.columnHeader_size.Width = 109;
            // 
            // columnHeader_modTime
            // 
            this.columnHeader_modTime.Text = "最近保存时间";
            this.columnHeader_modTime.Width = 197;
            // 
            // button_startGame
            // 
            this.button_startGame.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.button_startGame.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_startGame.Location = new System.Drawing.Point(0, 268);
            this.button_startGame.Name = "button_startGame";
            this.button_startGame.Size = new System.Drawing.Size(363, 37);
            this.button_startGame.TabIndex = 2;
            this.button_startGame.Text = "进入所选择的的世界";
            this.button_startGame.UseVisualStyleBackColor = true;
            this.button_startGame.Click += new System.EventHandler(this.button_startGame_Click);
            // 
            // panel_command
            // 
            this.panel_command.Controls.Add(this.cmd_Text);
            this.panel_command.Location = new System.Drawing.Point(648, 396);
            this.panel_command.Margin = new System.Windows.Forms.Padding(2);
            this.panel_command.Name = "panel_command";
            this.panel_command.Size = new System.Drawing.Size(443, 27);
            this.panel_command.TabIndex = 8;
            this.panel_command.Visible = false;
            // 
            // cmd_Text
            // 
            this.cmd_Text.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmd_Text.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmd_Text.Location = new System.Drawing.Point(0, 0);
            this.cmd_Text.Margin = new System.Windows.Forms.Padding(2);
            this.cmd_Text.Name = "cmd_Text";
            this.cmd_Text.Size = new System.Drawing.Size(443, 30);
            this.cmd_Text.TabIndex = 5;
            this.cmd_Text.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyUp);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1165, 510);
            this.Controls.Add(this.bagUIBox);
            this.Controls.Add(this.panel_mainMenu);
            this.Controls.Add(this.panel_inGame);
            this.Controls.Add(this.winGLCanvas1);
            this.Controls.Add(this.panel_singlePlayer);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "FormMain";
            this.Text = "MiniCraft Alpha";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.FormMain_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.winGLCanvas1)).EndInit();
            this.panel_inGame.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bagUIBox)).EndInit();
            this.panel_mainMenu.ResumeLayout(false);
            this.panel_singlePlayer.ResumeLayout(false);
            this.panel_mapSelect.ResumeLayout(false);
            this.panel_command.ResumeLayout(false);
            this.panel_command.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer timer1;
        private CSharpGL.WinGLCanvas winGLCanvas1;
        private System.Windows.Forms.Panel panel_inGame;
        private System.Windows.Forms.Button button_saveAndExit;
        private System.Windows.Forms.Panel panel_mainMenu;
        private System.Windows.Forms.Button button_singlePlayer;
        private System.Windows.Forms.Button button_exit;
        private System.Windows.Forms.Button button_multiPlayer;
        private System.Windows.Forms.Panel panel_singlePlayer;
        private System.Windows.Forms.Button button_resume;
        private System.Windows.Forms.PictureBox bagUIBox;
        private System.Windows.Forms.Panel panel_mapSelect;
        private System.Windows.Forms.Button button_startGame;
        private System.Windows.Forms.Panel panel_command;
        private System.Windows.Forms.TextBox cmd_Text;
        private System.Windows.Forms.ListView listView_mapSelector;
        private System.Windows.Forms.ColumnHeader columnHeader_mapName;
        private System.Windows.Forms.ColumnHeader columnHeader_size;
        private System.Windows.Forms.ColumnHeader columnHeader_modTime;
    }
}

