using CSharpGL;
using IrrKlang;
using MiniCraft.Core.Game;
using MiniCraft.Core.GUI;
using MiniCraft.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniCraft.Core
{
    public partial class FormMain : Form
    {
        private HelperForm helper;
        private CompoundForm compoundForm;
        //private MLabel label;

        public FormMain()
        {
            InitializeComponent();
            this.winGLCanvas1.OpenGLDraw += winGLCanvas1_OpenGLDraw;
            this.winGLCanvas1.AfterRenderingDraw += WinGLCanvas1_AfterRenderingDraw;
            this.winGLCanvas1.Resize += winGLCanvas1_Resize;
            this.winGLCanvas1.MouseMove += winGLCanvas1_MouseMove;

            this.bagUIBox.Paint += BagUIBox_Paint;

            //this.panel_inGame.BackColor = Color.FromArgb(100, 200, 200, 200);

            //helper = new HelperForm();
            //helper.Show();

            compoundForm = new CompoundForm();
            Controls.Add(compoundForm);

            //SetFormHotKey();
        }

        //重写按键事件，只控制UI模块，不涉及到游戏内的控制
        const int WM_KEYDOWN = 0x100;
        const int WM_KEYUP = 0x101;
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (msg.Msg == WM_KEYDOWN)
            {
                if (keyData == Keys.Escape)
                {
                    //GameState.manipulater.Clip = new Rectangle(Location, Size);
                    SwitchPausePanel();
                }
                else if (!cmd_Text.Visible && (keyData == Keys.T || keyData == Keys.Oem2))
                {
                    SwitchCommandLine();
                }
                else if (!cmd_Text.Visible && keyData == Keys.K)
                {
                    compoundForm.Switch();
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void WinGLCanvas1_AfterRenderingDraw(object sender, PaintEventArgs e)
        {
            if (!canRender) return;
            //helper.Text = ";
            //Text = $"{GameState.chunkInFrustumCount} {GameState.visibleVoxelCount}";
            TextBoard.Draw(e.Graphics,
                $@"FPS: {Math.Round(winGLCanvas1.FPS, 2)}", //Timer:{winGLCanvas1.TimerTriggerInterval}",
                $@"Choosing: {GameState.ChosingObj}",
                $@"Pos: {GameState.currPosition} / Standing: { GameState.currChunk }",
                $@"Looking: {GameState.chosingChunk}",
                $@"ChunkCount:{ GameState.chunkInFrustumCount} BlockCount:{ GameState.visibleVoxelCount}"
            );
            bagUIBox.Invalidate();
            //if(winGLCanvas1.FPS >= 30)
            //    Thread.Sleep(33 - (int)(Time.DeltaTime * 1000));

            //e.Graphics.DrawImage(WorldNode.tex.GetImage(Width, Height), 0, 0);
            //WorldNode.depthFrame.Dispose();
            //WorldNode.tex.Dispose();
        }

        private void BagUIBox_Paint(object sender, PaintEventArgs e)
        {
            GameState.player?.Bag.Render(e.Graphics, e.ClipRectangle);
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            GameState.InitializeBase(this, winGLCanvas1);
            GameState.InitializeGameContainer();
            GameState.InitializeActions();
            GameState.InitializEventHandlers();
            UpdateMapsSelector();
            UpdateAll();
        }

        bool canRender = false;
        object drawSyncObj = new object();
        int cmdCount = 0;
        private void winGLCanvas1_OpenGLDraw(object sender, PaintEventArgs e)
        {
            //不需要加锁！此Timer是单线程的！
            if (!canRender) return;
            Time.MainTimeLinePropel();

            ActionList list = GameState.actionList;
            if (list != null)
            {
                vec4 clearColor = GameState.scene.ClearColor;
                GL.Instance.ClearColor(clearColor.x, clearColor.y, clearColor.z, clearColor.w);
                GL.Instance.Clear(GL.GL_COLOR_BUFFER_BIT | GL.GL_DEPTH_BUFFER_BIT | GL.GL_STENCIL_BUFFER_BIT);

                list.Act(new ActionParams(Viewport.GetCurrent()));
            }
            //if (winGLCanvas1.FPS >= 25)
            //    Thread.Sleep(40 - (int)(Time.DeltaTime * 1000));

            Time.MainTimeLinePause();
        }

        //void control_MouseUp(object sender, GLMouseEventArgs e)
        //{
        //    MessageBox.Show(string.Format("This is a message from {0}!", sender));
        //}

        void winGLCanvas1_Resize(object sender, EventArgs e)
        {
            UpdateAll();
        }

        private void CenterizeControls()
        {
            try
            {
                int width = winGLCanvas1.Width, height = winGLCanvas1.Height;
                GameState.scene.Camera.AspectRatio = this.winGLCanvas1.Width / this.winGLCanvas1.Height;
                var crossHair = GameState.gameContainer.crossHair;
                if (crossHair != null)
                {
                    crossHair.Parent.Width = width;
                    crossHair.Parent.Height = height;
                    crossHair.Location = new GUIPoint((width - crossHair.Width) / 2, (height - crossHair.Height) / 2);
                }
                //bagUIBox.Location = new Point((width - bagUIBox.Width) / 2, height - bagUIBox.Height);
                CenterizeControl(bagUIBox, winGLCanvas1, 0.5f, 1);
                CenterizeControl(panel_mapSelect, winGLCanvas1);
            }
            catch { }
        }

        protected void CenterizeControl(Control ctrl, Control _base, float u = 0.5f, float v = 0.5f)
        {
            ctrl.Location = new Point((int)((_base.Width - ctrl.Width) * u), (int)((_base.Height - ctrl.Height) * v));
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            GameState.OnGameExit();
            base.OnClosing(e);
        }

        private void winGLCanvas1_MouseMove(object sender, MouseEventArgs e)
        {
            //int clipWidth = (int)(Width / 1.1f), clipHeight = (int)(Height / 1.1f);
            //Cursor.Clip = new Rectangle((Width - clipWidth) / 2, (Height - clipHeight) / 2, clipWidth, clipHeight);
            Cursor.Clip = new Rectangle(Location, Size);
            //Cursor.Clip = new Rectangle(Location.X + (Width - clipWidth) / 2, Location.Y + (Height - clipHeight) / 2, Width / 2, Height / 2);
            Cursor.Hide();
        }

        private void button_saveAndExit_Click(object sender, EventArgs e)
        {
            GameState.OnGameExit();
            Application.Exit();
        }

        private async void button_startGame_Click(object sender, EventArgs e)
        {
            if (listView_mapSelector.SelectedItems.Count == 0)
            {
                MessageBox.Show("请选择地图！");
                return;
            }
            AssetManager.LoadMapFile(listView_mapSelector.SelectedItems[0].Text);

            GameState.InitializePlayer();
            
            panel_singlePlayer.Visible = false;

            //Cursor.Position = new Point(0, 0);
            winGLCanvas1.TimerTriggerInterval = int.MaxValue;
            //winGLCanvas1.RenderTrigger = RenderTrigger.Manual;
            await GameState.gameContainer.TestCreate();

            //PhysicsEngine.PhysicsTimer.Start();

            //helper.Match(GameState.scene.RootNode);
            //helper.Match(GameState.scene.RootControl);
            winGLCanvas1.TimerTriggerInterval = 40;

            canRender = true;
            bagUIBox.Visible = true;
            bagUIBox.Parent = winGLCanvas1;
            //winGLCanvas1.RenderTrigger = RenderTrigger.TimerBased;
            //MessageBox.Show(GL.Instance.GetString(GL.GL_VERSION));
        }

        private void button_singlePlayer_Click(object sender, EventArgs e)
        {
            panel_mainMenu.Visible = false;
            panel_singlePlayer.Visible = true;
            panel_singlePlayer.BringToFront();
        }

        private void UpdateAll()
        {
            UpdateCommandLine();
            CenterizeControls();
        }

        private void UpdateMapsSelector()
        {
            listView_mapSelector.Items.Clear();
            listView_mapSelector.Items.AddRange(
                Directory.GetFiles(AssetManager.GetNBTFilePath())
            .Select(f =>
            {
                var info = new FileInfo(f);
                return new ListViewItem(new[] { info.Name.Replace(info.Extension, ""), $"{info.Length / 1024.0f} kb", info.LastWriteTime.ToString() });
            })
            .ToArray());
        }

        private void UpdateCommandLine()
        {
            panel_command.Width = winGLCanvas1.Width;
            CenterizeControl(panel_command, winGLCanvas1, 0.5f, 1);
        }

        private void button_resume_Click(object sender, EventArgs e)
        {
            SwitchPausePanel();
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Commander.Parse(cmd_Text.Text);
                SwitchCommandLine();
            }
        }

        private void SwitchCommandLine()
        {
            //panel_command.TopLevelControl
            panel_command.Visible = !panel_command.Visible;
            if (panel_command.Visible)
            {
                panel_command.BringToFront();
                cmd_Text.Focus();
            }
            else
            {
                cmd_Text.Clear();
            }
        }

        private void SwitchPausePanel()
        {
            panel_inGame.Visible = !panel_inGame.Visible;
            if (panel_inGame.Visible)
                Cursor.Show();
            else
                Cursor.Hide();
        }

        private void FormMain_KeyUp(object sender, KeyEventArgs e)
        {

        }

        //HotKeys hotkeys;
        //private void SetFormHotKey()
        //{
        //    hotkeys = new HotKeys(Handle);
        //    //TODO:写入配置文件中,封装此类操作
        //    hotkeys.Register(HKModifiers.None, Keys.Escape, SwitchPausePanel);
        //    hotkeys.Register(HKModifiers.None, Keys.Oem2, SwitchCommandLine);
        //    hotkeys.Register(HKModifiers.None, Keys.T, SwitchCommandLine);
        //    hotkeys.Register(HKModifiers.None, Keys.K, compoundForm.Switch);
        //}
    }
}
