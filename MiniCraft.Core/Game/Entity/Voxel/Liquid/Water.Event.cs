using CSharpGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCraft.Core.Game
{
    public partial class Water
    {
        public event MVoxelEventHandler OnFlowEvent;
        public virtual void OnFlow(GameObject sender, MVoxelEventArgs args)
        {
            OnFlowEvent?.Invoke(sender, args);
        }

        protected override void InitEventHandlers()
        {
            base.InitEventHandlers();
            OnStepInEvent += Water_OnStepInEvent;
        }

        private void Water_OnStepInEvent(GameObject sender, MVoxelEventArgs args)
        {
            //sender.WorldPosition += new vec3(0, 5, 0);
        }
    }
}
