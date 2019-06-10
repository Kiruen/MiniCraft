namespace MiniCraft.Core
{
    partial class HelperForm
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
            this.trvScene = new System.Windows.Forms.TreeView();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.trvSceneGUI = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // trvScene
            // 
            this.trvScene.Dock = System.Windows.Forms.DockStyle.Left;
            this.trvScene.Location = new System.Drawing.Point(308, 0);
            this.trvScene.Name = "trvScene";
            this.trvScene.Size = new System.Drawing.Size(404, 605);
            this.trvScene.TabIndex = 7;
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Left;
            this.propertyGrid1.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(308, 605);
            this.propertyGrid1.TabIndex = 6;
            // 
            // trvSceneGUI
            // 
            this.trvSceneGUI.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trvSceneGUI.Font = new System.Drawing.Font("宋体", 12F);
            this.trvSceneGUI.Location = new System.Drawing.Point(712, 0);
            this.trvSceneGUI.Name = "trvSceneGUI";
            this.trvSceneGUI.Size = new System.Drawing.Size(330, 605);
            this.trvSceneGUI.TabIndex = 8;
            // 
            // HelperForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1042, 605);
            this.Controls.Add(this.trvSceneGUI);
            this.Controls.Add(this.trvScene);
            this.Controls.Add(this.propertyGrid1);
            this.Name = "HelperForm";
            this.Text = "HelperForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView trvScene;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.TreeView trvSceneGUI;
    }
}