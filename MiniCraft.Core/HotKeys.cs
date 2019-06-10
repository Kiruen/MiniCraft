using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniCraft.Windows
{
    //组合控制键
    public enum HKModifiers : byte
    {
        None = 0,
        Alt = 1,
        Control = 2,
        Shift = 4,
        Win = 8,
    }

    public class HotKeys
    {
        //引入系统API
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int modifiers, Keys vk);
        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private Dictionary<int, Action> keys = new Dictionary<int, Action>();
        private int Id { get; set; } = 10;
        private IntPtr hWnd;

        public HotKeys(IntPtr handle)
        {
            hWnd = handle;
        }

        public void Register(HKModifiers modifiers, Keys vk, Action action)
        {
            keys.Add(Id, action);
            RegisterHotKey(hWnd, Id++, (int)modifiers, vk);
        }

        public void UnRegister(int id)
        {
            UnregisterHotKey(hWnd, id);
        }

        public bool Execute(Message msg)
        {
            const int WM_HOTKEY = 0x0312;
            if (msg.Msg == WM_HOTKEY)
            {
                Action callback = null;
                if (keys.TryGetValue(msg.WParam.ToInt32(), out callback))
                {
                    callback();
                    return true;
                }
            }
            return false;
        }
    }
}
