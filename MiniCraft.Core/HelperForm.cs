using CSharpGL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniCraft.Core
{
    public partial class HelperForm : Form
    {
        public HelperForm()
        {
            InitializeComponent();
            this.trvScene.AfterSelect += trvScene_AfterSelect;
            this.trvSceneGUI.AfterSelect += trvScene_AfterSelect;
        }

        void trvScene_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.propertyGrid1.SelectedObject = e.Node.Tag;
        }

        public void Match(GLControl nodeBase)
        {
            var treeView = this.trvSceneGUI;
            treeView.Nodes.Clear();
            var node = new TreeNode(nodeBase.ToString()) { Tag = nodeBase };
            treeView.Nodes.Add(node);
            Match(node, nodeBase);
        }

        public void Match(TreeNode node, GLControl nodeBase)
        {
            foreach (var item in nodeBase.Children)
            {
                var child = new TreeNode(item.ToString()) { Tag = item };
                node.Nodes.Add(child);
                Match(child, item);
            }
        }

        public void Match(SceneNodeBase nodeBase)
        {
            var treeView = this.trvScene;
            treeView.Nodes.Clear();
            var node = new TreeNode(nodeBase.ToString()) { Tag = nodeBase };
            treeView.Nodes.Add(node);

            Match(node, nodeBase);
            this.trvScene.ExpandAll();

        }

        private void Match(TreeNode node, SceneNodeBase nodeBase)
        {
            foreach (var item in nodeBase.Children)
            {
                var child = new TreeNode(item.ToString()) { Tag = item };
                node.Nodes.Add(child);
                Match(child, item);
            }
        }
    }
}
