using CSharpGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCraft.Core.Game
{
    public abstract partial class GameObject : MNodeBase
    {
        /// <summary>
        /// 注册并添加事件处理函数到总的事件机构中。
        /// 注意父类可能会提供一些必要的事件处理函数，
        /// 因此最好在覆盖方法的一开始调用该方法的base版本
        /// </summary>
        protected abstract void InitEventHandlers();


        public event MBasicEventHandler OnSelectEvent;
        public virtual void OnSelect(GameObject sender, MEventArgs args)
        {
            OnSelectEvent?.Invoke(sender, args);
        }


        public event MMouseEventHandler OnMouseClickedEvent;
        public virtual void OnMouseClicked(MMouseEventArgs args)
        {
            OnMouseClickedEvent?.Invoke(args);
        }


        public event MMouseEventHandler OnCollidedEvent;
        public virtual void OnCollided(MMouseEventArgs args)
        {
            OnCollidedEvent?.Invoke(args);
        }

        public event MBasicEventHandler OnDestroyEvent;
        /// <summary>
        /// 当对象被销毁时调用
        /// </summary>
        /// <param name="args"></param>
        public virtual void OnDestroy(GameObject sender, MEventArgs args)
        {
            OnDestroyEvent?.Invoke(sender, args);
        }


        public event MBasicEventHandler OnObjDestroyEvent;
        /// <summary>
        /// 在和当前体素相关联的对象被销毁时会调用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void OnObjDestroy(GameObject sender, MEventArgs args)
        {
            OnObjDestroyEvent?.Invoke(sender, args);
        }
    }
}
