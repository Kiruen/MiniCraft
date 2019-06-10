using CSharpGL;
using System;
using System.Drawing;

using System.Windows.Forms;

namespace MiniCraft.Core.Game
{
    /// <summary>
    /// Manipulate a camera in first-persppective's view.
    /// </summary>
    public class BasicManipulater :
        Manipulater, IMouseHandler, IKeyboardHandler
    {
        private char backKey;
        private ICamera camera;
        private IGLCanvas canvas;

        private char downKey;
        private char frontKey;
        private GLEventHandler<GLKeyPressEventArgs> keyPressEvent;
        private GLMouseButtons lastBindingMouseButtons;
        private ivec2 lastMousePosition;
        private char leftKey;
        private GLEventHandler<GLMouseEventArgs> mouseDownEvent;
        private bool mouseDownFlag = false;
        private GLEventHandler<GLMouseEventArgs> mouseMoveEvent;
        private GLEventHandler<GLMouseEventArgs> mouseUpEvent;
        private GLEventHandler<GLMouseEventArgs> mouseWheelEvent;
        private char rightKey;
        private char upcaseBackKey;
        private char upcaseDownKey;
        private char upcaseFrontKey;
        private char upcaseLeftKey;
        private char upcaseRightKey;
        private char upcaseUpKey;
        private char upKey;

        /// <summary>
        /// 限制光标所在的范围
        /// </summary>
        public Rectangle Clip { get; set; }
        public Form GameWindow { get; set; }

        //public BasicManipulater()
        //    : this(new Rectangle(0, 0, 10, 10), 1f, 0.12f, 0.12f, GLMouseButtons.Left | GLMouseButtons.Right) { }

        /// <summary>
        ///
        /// </summary>
        /// <param name="stepLength"></param>
        /// <param name="horizontalRotationSpeed"></param>
        /// <param name="verticalRotationSpeed"></param>
        /// <param name="bindingMouseButtons"></param>
        public BasicManipulater(Form gameWindow,
            float stepLength = 1f, float horizontalRotationSpeed = 0.12f,
            float verticalRotationSpeed = 0.12f,
            GLMouseButtons bindingMouseButtons = GLMouseButtons.Left | GLMouseButtons.Right)
        {
            GameWindow = gameWindow;
            int clipWidth = (int)(gameWindow.Width / 1.8f), clipHeight = (int)(gameWindow.Height / 1.8f);
            Clip = new Rectangle((gameWindow.Width - clipWidth) / 2,
                    (gameWindow.Height - clipHeight) / 2,
                    clipWidth, clipHeight);

            this.FrontKey = 'w';
            this.BackKey = 's';
            this.LeftKey = 'a';
            this.RightKey = 'd';
            this.UpKey = 'q';
            this.DownKey = 'e';

            this.StepLength = stepLength;
            this.HorizontalRotationSpeed = horizontalRotationSpeed;
            this.VerticalRotationSpeed = verticalRotationSpeed;
            this.BindingMouseButtons = bindingMouseButtons;

            this.keyPressEvent = (((IKeyboardHandler)this).canvas_KeyPress);
            this.mouseDownEvent = (((IMouseHandler)this).canvas_MouseDown);
            this.mouseMoveEvent = (((IMouseHandler)this).canvas_MouseMove);
            this.mouseUpEvent = (((IMouseHandler)this).canvas_MouseUp);
            this.mouseWheelEvent = (((IMouseHandler)this).canvas_MouseWheel);
        }

        /// <summary>
        ///
        /// </summary>
        public char BackKey
        {
            get { return backKey; }
            set
            {
                backKey = value.ToString().ToLower()[0];
                upcaseBackKey = value.ToString().ToUpper()[0];
            }
        }

        /// <summary>
        ///
        /// </summary>
        public GLMouseButtons BindingMouseButtons { get; set; }

        /// <summary>
        ///
        /// </summary>
        public char DownKey
        {
            get { return downKey; }
            set
            {
                downKey = char.ToLower(value); // value.ToString().ToLower()[0]
                upcaseDownKey = char.ToUpper(value);//value.ToString().ToUpper()[0];
            }
        }

        /// <summary>
        ///
        /// </summary>
        public char FrontKey
        {
            get { return frontKey; }
            set
            {
                frontKey = value.ToString().ToLower()[0];
                upcaseFrontKey = value.ToString().ToUpper()[0];
            }
        }

        /// <summary>
        ///
        /// </summary>
        public float HorizontalRotationSpeed { get; set; }

        /// <summary>
        ///
        /// </summary>
        public char LeftKey
        {
            get { return leftKey; }
            set
            {
                leftKey = value.ToString().ToLower()[0];
                upcaseLeftKey = value.ToString().ToUpper()[0];
            }
        }

        /// <summary>
        ///
        /// </summary>
        public char RightKey
        {
            get { return rightKey; }
            set
            {
                rightKey = value.ToString().ToLower()[0];
                upcaseRightKey = value.ToString().ToUpper()[0];
            }
        }

        /// <summary>
        ///
        /// </summary>
        public float StepLength { get; set; }

        /// <summary>
        ///
        /// </summary>
        public char UpKey
        {
            get { return upKey; }
            set
            {
                upKey = value.ToString().ToLower()[0];
                upcaseUpKey = value.ToString().ToUpper()[0];
            }
        }

        /// <summary>
        ///
        /// </summary>
        public float VerticalRotationSpeed { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="camera"></param>
        /// <param name="canvas"></param>
        public override void Bind(ICamera camera, IGLCanvas canvas)
        {
            if (camera == null || canvas == null) { throw new ArgumentNullException(); }

            this.camera = camera;
            this.canvas = canvas;

            canvas.KeyPress += this.keyPressEvent;
            canvas.MouseDown += this.mouseDownEvent;
            canvas.MouseMove += this.mouseMoveEvent;
            canvas.MouseUp += this.mouseUpEvent;
            canvas.MouseWheel += this.mouseWheelEvent;
        }

        void IKeyboardHandler.canvas_KeyPress(object sender, GLKeyPressEventArgs e)
        {
            //bool updated = false;

            //if (updated)
            //{
            //    IGLCanvas canvas = this.canvas;
            //    if (canvas.RenderTrigger == RenderTrigger.Manual)
            //    {
            //        canvas.Repaint();
            //    }
            //}
        }

        public vec3 LastCameraPosition { get; set; }
        public void MoveCameraTo(vec3 pos)
        {
            var old = this.camera.Position;
            this.camera.Position = pos;
            this.camera.Target += (pos - old);
        }

        /// <summary>
        /// 无条件移动摄像机
        /// </summary>
        /// <param name="delta">移动向量</param>
        public void MoveCamera(vec3 delta)
        {
            var bindingObj = GameState.player;
            var velocity = bindingObj.Physics.Velocity;
            velocity.x = Math.Abs(velocity.x) * delta.x.ExtractSign();
            velocity.z = Math.Abs(velocity.z) * delta.z.ExtractSign();
            bindingObj.Physics.Velocity = velocity;
            //delta += bindingObj.Physics.Velocity;
            //delta.y -= bindingObj.Physics.Velocity.y;
            var dy = new vec3(0, delta.y, 0);
            var dx = new vec3(delta.x + velocity.x, 0, 0);
            var dz = new vec3(0, 0, delta.z + velocity.z);
            PhysicsEngine.PostAction(new MoveAction(bindingObj, bindingObj.WorldPosition, dx));
            PhysicsEngine.PostAction(new MoveAction(bindingObj, bindingObj.WorldPosition, dz));
            PhysicsEngine.PostAction(new MoveAction(bindingObj, bindingObj.WorldPosition, dy));
            //this.camera.Position += delta;
            //this.camera.Target += delta;
            //StepLength = MathExt.Clamp(StepLength + 0.3f, OriginalVelocity, OriginalVelocity * 3f);
        }
        //public float OriginalVelocity;

        public bool MoveCamera(Direction3D dir)
        {
            LastCameraPosition = camera.Position;
            bool updated = false;
            switch (dir)
            {
                case Direction3D.Front:
                    {
                        vec3 right = this.camera.GetRight();
                        vec3 standardFront = this.camera.UpVector.cross(right).normalize();
                        MoveCamera(standardFront * this.StepLength);
                        updated = true;
                        break;
                    }
                case Direction3D.Back:
                    {
                        vec3 right = this.camera.GetRight();
                        vec3 standardBack = right.cross(this.camera.UpVector).normalize();
                        MoveCamera(standardBack * this.StepLength);
                        updated = true;
                        break;
                    }
                case Direction3D.Left:
                    {
                        vec3 right = this.camera.GetRight();
                        vec3 left = (-right).normalize();
                        MoveCamera(left * this.StepLength);
                        updated = true;
                        break;
                    }
                case Direction3D.Right:
                    {
                        vec3 right = this.camera.GetRight().normalize();
                        MoveCamera(right * this.StepLength);
                        updated = true;
                        break;
                    }
                case Direction3D.Up:
                    {
                        vec3 up = this.camera.UpVector.normalize();
                        MoveCamera(up * this.StepLength);
                        updated = true;
                        break;
                    }
                case Direction3D.Down:
                    {
                        vec3 down = -this.camera.UpVector.normalize();
                        MoveCamera(down * this.StepLength);
                        updated = true;
                        break;
                    }
            }
            return updated;
        }

        void IMouseHandler.canvas_MouseDown(object sender, GLMouseEventArgs e)
        {
            this.lastBindingMouseButtons = this.BindingMouseButtons;
            if ((e.Button & this.lastBindingMouseButtons) != GLMouseButtons.None)
            {
                this.mouseDownFlag = true;
                this.lastMousePosition = e.Location;
            }
        }


        public float roll, pitch;
        float sensitivity = 0.04f; //2.5
        void IMouseHandler.canvas_MouseMove(object sender, GLMouseEventArgs e)
        {
            //if (locked) return;
            //locked = true;
            if (Clip.Contains(e.X, e.Y))
            {
                if (e.X != this.lastMousePosition.x || e.Y != this.lastMousePosition.y)
                {
                    float deltaX = (e.X - this.lastMousePosition.x) * sensitivity,
                        deltaY = (this.lastMousePosition.y - e.Y) * sensitivity;
                    float deltaPitch = this.HorizontalRotationSpeed * deltaX,
                         deltaRoll = this.VerticalRotationSpeed * deltaY;

                    roll = MathExt.Clamp(roll + deltaRoll,
                            -MathExt.HalfPI + 0.1f,
                            MathExt.HalfPI - 0.1f);

                    pitch = (pitch + deltaPitch) % MathExt.TwoPI;
                    vec3 front;
                    front.y = (float)Math.Sin(roll);
                    front.x = (float)Math.Cos(roll) * (float)Math.Cos(pitch);
                    front.z = (float)Math.Cos(roll) * (float)Math.Sin(pitch);

                    //mat4 rotationMatrix = glm.rotate(deltaRoll, -this.camera.UpVector);
                    //var front = new vec4(this.camera.GetFront(), 1.0f);
                    //vec4 front1 = rotationMatrix * front;
                    //rotationMatrix = glm.rotate(deltaPitch, this.camera.GetRight());
                    //vec4 front2 = rotationMatrix * front1;
                    //front2 = front2.normalize();
                    //this.camera.Target = this.camera.Position + new vec3(front2);
                    this.camera.Target = this.camera.Position + front.normalize();

                    IGLCanvas canvas = this.canvas;
                    if (canvas != null && canvas.RenderTrigger == RenderTrigger.Manual)
                    {
                        canvas.Repaint();
                    }
                    this.lastMousePosition = e.Location;
                }
            }
            else
            {
                lastMousePosition = Clip.Center().ToIVec2();
                Cursor.Position = GameWindow.PointToScreen(lastMousePosition.ToPoint());
            }
            //locked = false;
        }


        void IMouseHandler.canvas_MouseUp(object sender, GLMouseEventArgs e)
        {
            if ((e.Button & this.lastBindingMouseButtons) != GLMouseButtons.None)
            {
                this.mouseDownFlag = false;
            }
        }

        void IMouseHandler.canvas_MouseWheel(object sender, GLMouseEventArgs e)
        {
            //TODO:可以添加缩放功能
            //this.camera.MouseWheel(e.Delta);

            IGLCanvas canvas = this.canvas;
            if (canvas != null && canvas.RenderTrigger == RenderTrigger.Manual)
            {
                canvas.Repaint();
            }
        }

        /// <summary>
        ///
        /// </summary>
        public override void Unbind()
        {
            if (this.canvas != null && (!this.canvas.IsDisposed))
            {
                this.canvas.KeyPress -= this.keyPressEvent;
                this.canvas.MouseDown -= this.mouseDownEvent;
                this.canvas.MouseMove -= this.mouseMoveEvent;
                this.canvas.MouseUp -= this.mouseUpEvent;
                this.canvas.MouseWheel -= this.mouseWheelEvent;
                this.canvas = null;
                this.camera = null;
            }
        }
    }
}