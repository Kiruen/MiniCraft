using CSharpGL;
using MiniCraft.Core.Game;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCraft.Core
{
    /// <summary>
    /// Rendering something using multiple GLSL shader programs and VBO(VAO).
    /// </summary>
    /// <summary>
    /// 渲染功能单元。
    /// </summary>
   [Editor(typeof(PropertyGridEditor), typeof(UITypeEditor))]    
    public partial class MRenderUnit : IDisposable
    {
        protected const string strRenderUnit = "RenderUnit";

        /// <summary>
        /// 渲染单元的模型顶点数据。同时支持两种渲染方式：normal和instancing
        /// </summary>
        public MeshBase Mesh { get; set; }


        protected Dictionary<string, RenderMethodBuilder> builders
                    = new Dictionary<string, RenderMethodBuilder>();

        public Dictionary<string, RenderMethod> Methods { get; private set; } 
                    = new Dictionary<string, RenderMethod>();


        public MRenderUnit() //MeshBase mesh_data
        {
            //Mesh = mesh_data;
        }

        public void AddBuilder(string name, RenderMethodBuilder builder)
        {
            if (!builders.ContainsKey(name))
                builders.Add(name, builder);
            else
                builders[name] = builder;
        }

        public void AddBuilder(RenderMethodBuilder builder)
        {
            AddBuilder(RenderStrategy.DEFAULT, builder);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        } // end sub

        /// <summary>
        /// Destruct instance of the class.
        /// </summary>
        ~MRenderUnit()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// Backing field to track whether Dispose has been called.
        /// </summary>
        private bool disposedValue = false;

        /// <summary>
        /// Dispose managed and unmanaged resources of this instance.
        /// </summary>
        /// <param name="disposing">If disposing equals true, managed and unmanaged resources can be disposed. If disposing equals false, only unmanaged resources can be disposed. </param>
        private void Dispose(bool disposing)
        {
            if (this.disposedValue == false)
            {
                if (disposing)
                {
                    // Dispose managed resources.
                } // end if

                // Dispose unmanaged resources.
                var methods = this.Methods;
                if (methods != null)
                {
                    foreach (var item in methods.Values)
                    {
                        if (item != null)
                        {
                            item.Dispose();
                        }
                    }
                }
            } // end if

            this.disposedValue = true;
        } // end sub


        #region Initialize()

        protected static object synObj = new object();

        protected bool isInitialized = false;
        /// <summary>
        /// Already initialized.
        /// </summary>
        [Category(strRenderUnit)]
        [Description("Is this node initialized or not?")]
        public bool IsInitialized { get { return isInitialized; } }

        /// <summary>
        /// Initialize all stuff related to OpenGL.
        /// </summary>
        public void Initialize()
        {
            if (!isInitialized)
            {
                lock (synObj)
                {
                    if (!isInitialized)
                    {
                        //initializing = true;
                        DoInitialize();
                        //initializing = false;
                        isInitialized = true;
                    }
                }
            }
        }

        /// <summary>
        /// This method should only be invoked once.
        /// </summary>
        protected virtual void DoInitialize()
        {
            foreach (var key in this.builders.Keys)
            {
                Mesh.InstancingMode = key == RenderStrategy.INSTANCING ? true : false;
                RenderMethod method = this.builders[key].ToRenderMethod(Mesh);
                //if (Methods.ContainsKey(key))
                this.Methods.Add(key, method);
            }
        }

        #endregion Initialize()

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="index"></param>
        ///// <param name="controlMode">index buffer is accessable randomly or only by frame.</param>
        ///// <param name="transformFeedbackObj"></param>
        //public void Render(int index, ControlMode controlMode, TransformFeedbackObject transformFeedbackObj = null)
        //{
        //    if (index < 0 || this.Methods.Length <= index) { throw new System.IndexOutOfRangeException(); }

        //    this.Methods[index].Render(controlMode, transformFeedbackObj);
        //}
    }

    public partial class MRenderUnit
    {
        //TODO:想办法弄一个拷贝函数，让体素可以动态修改数据
    }
}
