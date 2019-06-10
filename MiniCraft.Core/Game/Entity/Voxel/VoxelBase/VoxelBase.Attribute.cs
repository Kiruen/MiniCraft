using CSharpGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCraft.Core.Game
{
    //定义体素对象的诸多属性
    public abstract partial class VoxelBase
    {
        /// <summary>
        /// 分别对应前右上后左下的面的可见状态
        /// </summary>
        public BitSet FaceState { get; private set; } = new BitSet(0b111111);

        //TODO:注意可能误操作，导致超过最高位的数位发生变化
        private bool visible = true;
        public override bool Visible
        {
            get => visible;
            set => visible = value;
        }
        //public override bool Visible
        //{
        //    get => FaceState.StateValue != 0;
        //    set => FaceState.StateValue = 0;
        //}
    }
}
