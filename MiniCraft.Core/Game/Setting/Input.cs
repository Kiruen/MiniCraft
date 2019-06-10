using CSharpGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCraft.Core.Game
{
    public enum SpecialInput
    {
        Wheel = 1,

    }

    public class InputState
    {
        public int TimeEachPause = 1;
        public int PauseRemainingTime;
        public bool Released;
        public bool Pressed { get => PauseRemainingTime > 0; }
        public int LastingTimeElapsed;
        //用户锁，用于某些复杂条件下的锁定动作，由用户锁定，用户自行释放
        public bool UserLocked;
        //自动锁，用于粗粒度地防止连续触发，由用户锁定，释放时自动解锁
        public bool AutoLocked;
        //public int ContiniousTapCount;

        public InputState(int timeEachPause)
        {
            PauseRemainingTime = 0;
            TimeEachPause = timeEachPause;
        }

        public void TryHandleOnce(Action<InputState> action)
        {
            TryHandle(action);
            if(Pressed)
                AutoLocked = true;
        }

        public void TryHandle(Action<InputState> action)
        {
            TryHandle<InputState>(action);
        }

        public bool TryHandle<T>(Action<T> action) where T : InputState
        {
            bool res = false;
            //可能还没释放掉按键，就已经再次触发、处理了。因为有时候帧率大于键盘扫描的频率
            if(LastingTimeElapsed > 0)
            {
                PauseRemainingTime--;
                if(!AutoLocked && PauseRemainingTime <= 0)
                {
                    action(this as T);
                    PauseRemainingTime = TimeEachPause;
                    res = true;
                }
                if (Released)
                {
                    HandleRelease();
                }
            }
            return res;
        }

        protected virtual void HandleRelease()
        {
            Released = false;
            AutoLocked = false;
            LastingTimeElapsed = 0;
            PauseRemainingTime = 0;
        }
    }

    public class MouseInputState : InputState
    {
        public ivec2 Position;
        public ivec2 Delta;

        public MouseInputState(int timeEachPause) : base(timeEachPause)
        {

        }
    }

    public static class Input
    {
        private static Dictionary<string, GLKeys> keySettings = new Dictionary<string, GLKeys>(128)
        {
            { "LEFT", GLKeys.A },
            { "RIGHT", GLKeys.D },
            { "FRONT", GLKeys.W },
            { "UP", GLKeys.Q },
            { "BACK", GLKeys.S },
            { "DOWN", GLKeys.E },
            { "JUMP", GLKeys.Space },
            { "FLY", GLKeys.F },
        };

        private static Dictionary<string, GLMouseButtons> mouseSettings = new Dictionary<string, GLMouseButtons>(128)
        {
            { "LEFT", GLMouseButtons.Left },
            { "RIGHT", GLMouseButtons.Right },
            { "WHEEL_SCROLL", GLMouseButtons.Middle },
            { "WHEEL", GLMouseButtons.Middle },
            { "SIDE_FRONT", GLMouseButtons.XButton1 },
            { "SIDE_BACK", GLMouseButtons.XButton2 },
        };

        //public static bool mousePressed;
        //public static float mousePressedTime;

        private static Dictionary<GLKeys, InputState> keyStates = new Dictionary<GLKeys, InputState>(128);
        private static Dictionary<GLMouseButtons, MouseInputState> mouseButtonStates = new Dictionary<GLMouseButtons, MouseInputState>(128);


        static Input()
        {
            foreach (var key in Enum.GetValues(typeof(GLKeys)))
            {
                if (!keyStates.ContainsKey((GLKeys)key))
                    //keyState.Add(key, new KeyState());
                    keyStates.Add((GLKeys)key, new InputState(1));
            }
            foreach (var key in Enum.GetValues(typeof(GLMouseButtons)))
            {
                if (!mouseButtonStates.ContainsKey((GLMouseButtons)key))
                    //keyState.Add(key, new KeyState());
                    mouseButtonStates.Add((GLMouseButtons)key, new MouseInputState(3));
            }
        }

        public static void MouseWheel(GLMouseEventArgs e)
        {
            var state = GetMouseState(GLMouseButtons.Middle);
            state.LastingTimeElapsed += 1;
            //state.Position = e.Location;
            state.Delta = new ivec2(0, e.Delta);
        }

        public static void MouseDown(GLMouseEventArgs e)
        {
            var state = GetMouseState(e.Button);
            state.LastingTimeElapsed += 1;
            //state.PauseRemainingTime -= 1;
            state.Position = e.Location;
            //state.Delta = new ivec2(0);
            //mousePressed = true;
        }

        public static void MouseUp(GLMouseEventArgs e)
        {
            var state = GetMouseState(e.Button);
            //state.LastingTime -= 1;
            state.Released = true;
            state.Position = e.Location;
            //mousePressed = false;
        }

        public static void KeyDown(GLKeyEventArgs e)
        {
            var state = GetKeyState(e.KeyCode);
            state.LastingTimeElapsed += 1;
            //state.PauseRemainingTime -= 1;
        }

        public static void KeyUp(GLKeyEventArgs e)
        {
            //keyStates[e.KeyCode].LastingTime = 0;
            GetKeyState(e.KeyCode).Released = true;
        }

        //public static void KeyDown(string button)
        //{
        //    button = button.ToUpper();
        //    KeyDown(keySettings[button]);
        //}

        //public static void KeyUp(string button)
        //{
        //    button = button.ToUpper();
        //    KeyUp(keySettings[button]);
        //}

        public static InputState GetKeyState(string button)
        {
            button = button.ToUpper();
            return GetKeyState(keySettings[button]);
        }

        public static MouseInputState GetMouseState(string button)
        {
            button = button.ToUpper();
            return GetMouseState(mouseSettings[button]);
        }

        public static InputState GetKeyState(GLKeys enumKey)
        {
            return keyStates[enumKey];
        }

        public static MouseInputState GetMouseState(GLMouseButtons enumButton)
        {
            return mouseButtonStates[enumButton];
        }

        public static void HanleInputs()
        {
            TryHandleMouseButtons();
            TryHandleMovingButtons();
            TryHandleNumButtons();
            //GetKeyState(GLKeys.C).TryHandle((state) => Commander.TP(GameState.player, new vec3(2, 42, 2)));
        }

        public static void TryHandleMouseButtons()
        {
            foreach (var enumButton in new[] { GLMouseButtons.Left, GLMouseButtons.Right })
            {
                GetMouseState(enumButton).TryHandle<MouseInputState>((state) =>
                {
                    var obj = GameState.ChosingObj;
                    if (obj != VoxelBase.None)
                    {
                        obj.OnMouseClicked(new MMouseEventArgs() { Button = enumButton });
                    }
                });
            }
            GetMouseState("WHEEL_SCROLL").TryHandle<MouseInputState>((state) =>
            {
                GameState.player.Bag.SelectedIndex += state.Delta.y > 0 ? -1 : 1;
                state.Released = true;
            });
        }

        public static void TryHandleNumButtons()
        {
            for (int i = 0; i < 10; i++)
            {
                GetKeyState(GLKeys.D0 + i).TryHandle(state => GameState.player.Bag.SelectedIndex = i);
            }
        }

        public static void TryHandleMovingButtons()
        {
            var manipulater = GameState.manipulater;
            GetKeyState("JUMP").TryHandle((state) =>
            {
                if (!state.UserLocked)
                {
                    PhysicsEngine.AddForce(GameState.player, new vec3(0, 1, 0));
                    state.UserLocked = true;
                }
            });
            GetKeyState("FLY").TryHandleOnce((state) =>
            {
                PhysicsEngine.GravityOn = !PhysicsEngine.GravityOn;
                var bindingPhysics = GameState.player.Physics;
                if(PhysicsEngine.GravityOn)
                    bindingPhysics.Velocity = new vec3(0);
                else
                    bindingPhysics.Velocity = new vec3(1, 0, 1);
            });
            GetKeyState("FRONT").TryHandle((state) => manipulater.MoveCamera(Direction3D.Front));
            GetKeyState("BACK").TryHandle((state) => manipulater.MoveCamera(Direction3D.Back));
            GetKeyState("LEFT").TryHandle((state) => manipulater.MoveCamera(Direction3D.Left));
            GetKeyState("RIGHT").TryHandle((state) => manipulater.MoveCamera(Direction3D.Right));
            GetKeyState("UP").TryHandle((state) => manipulater.MoveCamera(Direction3D.Up));
            GetKeyState("DOWN").TryHandle((state) => manipulater.MoveCamera(Direction3D.Down));

            GetKeyState(GLKeys.X).TryHandleOnce((state) =>
            {
                var pos = GameState.ChosingObj == VoxelBase.None ?
                    GameState.currPosition : GameState.ChosingPosition;
                CompoundCreator.SamplePosition(pos);
            });
            GetKeyState(GLKeys.C).TryHandleOnce((state) => CompoundCreator.CopyVoxels(GameState.currWorld));
            GetKeyState(GLKeys.V).TryHandleOnce((state) =>
            {
                var pos = GameState.currPosition;
                if (GameState.ChosingObj != VoxelBase.None)
                    pos = GameState.ChosingObj.GlobalPosition;
                CompoundCreator.PasteVoxels(GameState.currWorld, pos);
            });
            //GetKeyState(GLKeys.Z).TryHandleOnce((state) => CompoundCreator.SaveCompound(DateTime.Now.ToLongDateString()));
            //GetKeyState(GLKeys.L).TryHandleOnce((state) => CompoundCreator.LoadCompound("tree"));
        }
    }
}
