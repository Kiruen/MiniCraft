using CSharpGL;
using System;
using System.ComponentModel;
using System.Drawing.Design;

namespace MiniCraft.Core
{
    public abstract partial class MNodeBase
    {
        private float roll;
        public float Roll
        {
            get { return roll; }
            set
            {
                if (roll != value)
                {
                    roll = value;
                    worldSpacePropertyUpdated = true;
                }
            }
        }

        private float yaw;
        public float Yaw
        {
            get { return yaw; }
            set
            {
                if (yaw != value)
                {
                    yaw = value;
                    worldSpacePropertyUpdated = true;
                }
            }
        }

        private float pitch;
        public float Pitch
        {
            get { return pitch; }
            set
            {
                if (pitch != value)
                {
                    pitch = value;
                    worldSpacePropertyUpdated = true;
                }
            }
        }

        /// <summary>
        /// 是否会动态改变
        /// </summary>
        public bool Active { get; set; }
        public mat4 multipleMat;
        private bool multiCached = false;
        MNodeBase lastParent;
        /// <summary>
        /// 获取将该对象内部空间的顶点变换到根世界的模型变换。注意，应使用该对象内部的顶点进行变换，而不是worldposition属性
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public mat4 GetModelMatrixFromParent()
        {
            //TODO:解决parent断层情况
            MNodeBase currParent = this.Parent as MNodeBase;
            //if (parent == null)
            //    parent = this.parent;
            if (currParent == null)
            {
                if (this.worldSpacePropertyUpdated)
                    return this.GetModelMatrix(mat4.identity());
                else
                    return cascadeModelMatrix;
            }

            bool parentUpdated = currParent.worldSpacePropertyUpdated;
            bool thisUpdated = this.worldSpacePropertyUpdated;
            bool parentChanged = lastParent != currParent;
            if (Active || !multiCached || parentUpdated || parentChanged || thisUpdated)
            {
                //一定要用一次GetModelMatrix(mat)来更新cascade mat
                var parentMat = currParent.GetModelMatrixFromParent();
                multipleMat = this.GetModelMatrix(parentMat);
                multiCached = true;
            }

            lastParent = currParent;
            return multipleMat;
        }


        /// <summary>
        /// 【Update】 and Get cascade model matrix.
        /// </summary>
        /// <param name="parentCascadeModelMatrix"></param>
        /// <returns></returns>
        public override mat4 GetModelMatrix(mat4 parentCascadeModelMatrix)
        {
            if (this.worldSpacePropertyUpdated)
            {
                mat4 matrix = glm.translate(mat4.identity(), this.WorldPosition);
                matrix = glm.translate(matrix, this.ScaleCenter);
                matrix = glm.scale(matrix, this.Scale);
                matrix = glm.translate(matrix, -this.ScaleCenter);
                //matrix = glm.translate(matrix, this.RotationCenter);
                //matrix = glm.rotate(matrix, this.RotationAngle, this.RotationAxis);
                matrix = matrix * MathExt.YPRToRotation(Roll, Pitch, Yaw);

                //matrix = glm.translate(matrix, -this.RotationCenter);

                this.thisModelMatrix = matrix;
                this.worldSpacePropertyUpdated = false;

                this.cascadeModelMatrix = parentCascadeModelMatrix * this.thisModelMatrix;
            }
            else
            {
                this.cascadeModelMatrix = parentCascadeModelMatrix * this.thisModelMatrix;
            }

            return this.cascadeModelMatrix;
        }
        //private mat4 inverseMat;
        //bool inverseCached = true;
        ///// <summary>
        ///// 获取该对象在另一个对象空间中的变换矩阵。不同于旧的版本，该版本进行了优化，避免了重复的矩阵运算
        ///// </summary>
        ///// <param name="parent"></param>
        ///// <returns></returns>
        //public mat4 GetInverseModelMatrixFromParent()
        //{
        //    MNodeBase parent = this.parent as MNodeBase;
        //    if (parent == null)
        //        return glm.inverse(this.GetModelMatrix());

        //    bool parentUpdated = parent.worldSpacePropertyUpdated;
        //    if (!inverseCached || parentUpdated || this.worldSpacePropertyUpdated)
        //    {
        //        inverseMat = glm.inverse(this.GetModelMatrix(parent.GetModelMatrixFromParent()));
        //        inverseCached = true;
        //    }
        //    //if (parentUpdated)
        //    //    parent.worldSpacePropertyUpdated = false;
        //    //if (this.worldSpacePropertyUpdated)
        //    //    this.worldSpacePropertyUpdated = false;

        //    return inverseMat;
        //}
    }
}