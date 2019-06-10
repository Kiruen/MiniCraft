using CSharpGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace MiniCraft.Core.Game
{

    public static partial class PhysicsEngine
    {
        public static void Initialize()
        {
            //PhysicsTimer = new Timer(50);
        }

        /// <summary>
        /// 检测v是否在以ABC为三个顶点的三角形中
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="C"></param>
        /// <param name="V"></param>
        /// <returns></returns>
        public static bool IsInnerTriangle(vec3 A, vec3 B, vec3 C, vec3 V)
        {
            vec3 v0 = C - A;
            vec3 v1 = B - A;
            vec3 v2 = V - A;
            double a = v0.dot(v0);
            double b = v1.dot(v1);
            double c = v0.dot(v1);
            double d = v2.dot(v0);
            double e = v2.dot(v1);

            double f = a * b - c * c;
            double x = b * d - c * e;
            double y = a * e - c * d;
            return (x >= 0) && (y >= 0) && (x + y - f <= 0);
        }

        /// <summary>
        /// 检测ray是否与三角形相交
        /// </summary>
        /// <param name="ray"></param>
        /// <param name="v0"></param>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static bool IntersectTriangle(Ray ray, vec3 v0, vec3 v1, vec3 v2, out float t)
        {
            var normal = (v1 - v0).cross(v2 - v0).normalize();
            t = default(float);
            if (normal.length() != 0) // && (eo = ray.Direction.dot(normal)) < 0
            {
                float eo = ray.Direction.dot(normal); //距离 a b cos
                t = (v0 - ray.Start).dot(normal) / eo;
                if (t < 0.001f || t >= ray.MaxDistance)
                    return false;

                //var V = S * ray.Direction + ray.Start;
                var V = ray.Extend(t);
                if (IsInnerTriangle(v0, v1, v2, V))
                {
                    //result->Shape = const_cast<Triangle*>(this);
                    //result->Normal = normal;
                    //result->Distance = S;
                    return true;
                }
            }
            return false;
        }

        // Determine whether a ray intersect with a triangle
        // Parameters
        // orig: origin of the ray
        // dir: direction of the ray
        // v0, v1, v2: vertices of triangle
        // t(out): weight of the intersection for the ray
        // u(out), v(out): barycentric coordinate of intersection，交点的纹理坐标值
        public static bool IntersectTriangle1(Ray ray, vec3 v0, vec3 v1, vec3 v2, out vec3 uvt)
        {
            uvt = default(vec3);
            //ray.Direction = ray.Direction.normalize();
            var e1 = (v1 - v0).normalize();
            var e2 = (v2 - v0).normalize();
            var p = ray.Direction.cross(e2);
            var a = e1.dot(p);

            if (a == 0)
                return false;

            float f = 1.0f / a;

            var s = ray.Start - v0;
            var u = f * s.dot(p);
            if (u < 0.0f || u > 1.0f)
                return false;

            var q = s.cross(e1);
            var v = f * ray.Direction.dot(q);

            if (v < 0.0f || u + v > 1.0f)
                return false;

            var t = f * e2.dot(q);
            if(t >= 0)
            {
                uvt.x = u;
                uvt.y = v;
                uvt.z = t;
                return true;
            }
            return false;
        }

//        // E1
//        vec3 E1 = v1 - v0;
//        // E2
//        vec3 E2 = v2 - v0;
//        // P
//        vec3 P = ray.Direction.cross(E2);
//        // determinant
//        float det = E1.dot(P);

//        // keep det > 0, modify T accordingly
//        vec3 T;
//            if (det > 0)
//            {
//                T = ray.Position - v0;
//            }
//            else
//            {
//                T = v0 - ray.Position;
//                det = -det;
//            }
//            // If determinant is near zero, ray lies in plane of triangle
//            if (det< 0.0001f)
//                return false;

//            float u, v, t;
//// Calculate u and make sure u <= 1
//u = T.dot(P);
//            if (u< 0.0f || u> det)
//                return false;

//            // Q
//            vec3 Q = T.cross(E1);
//// Calculate v and make sure u + v <= 1
//v = ray.Position.dot(Q);
//            if (v< 0.0f || u + v> det)
//                return false;
//            // Calculate t, scale parameters, ray intersects triangle
//            t = E2.dot(Q);

//            float fInvDet = 1.0f / det;
//t *= fInvDet;
//            u *= fInvDet;
//            v *= fInvDet;

//            uvt.x = u; uvt.y = v; uvt.z = t;


        /// <summary>
        /// 检测射线是否与AABB碰撞和相交
        /// </summary>
        /// <param name="orig"></param>
        /// <param name="dir"></param>
        /// <param name="box"></param>
        /// <returns></returns>
        public static bool IntersectAABB(Ray ray, AABB box, out uint[] triangle, out vec3 interPoint, bool needInterPoint = true)
        {
            triangle = new uint[3];
            interPoint = default(vec3);
            bool res = false;
            var mesh = box.Mesh;
            int len = mesh.Triangles.Length;
            var modelMat = box.GetModelMatrixFromParent();
            vec3 lastInterPoint = new vec3(65535);
            //var inv = glm.inverse(model);
            //ray.Start = (inv * ray.Start.ToVec4()).ToVec3();
            //ray.Direction = (inv * ray.Direction.ToVec4()).ToVec3();
            for (int i = 0; i < len; i += 3)
            {
                //TODO:根据模型面的固定顺序，确定与哪个方向相交
                vec3 v0 = (modelMat * mesh.Vertices[mesh.Triangles[i + 0]].ToVec4()).ToVec3();
                vec3 v1 = (modelMat * mesh.Vertices[mesh.Triangles[i + 1]].ToVec4()).ToVec3();
                vec3 v2 = (modelMat * mesh.Vertices[mesh.Triangles[i + 2]].ToVec4()).ToVec3();
                //vec3 v0 = mesh.Vertices[mesh.Triangles[i + 0]];
                //vec3 v1 = mesh.Vertices[mesh.Triangles[i + 1]];
                //vec3 v2 = mesh.Vertices[mesh.Triangles[i + 2]];
                if (IntersectTriangle(ray, v0, v1, v2, out float t))  //
                {
                    if (!needInterPoint)
                        return true;
                    res = true;
                    interPoint = ray.Start + t * ray.Direction; //(1 - uvt.x - uvt.y) * v0 + uvt.x * v1 + v2
                    //如果此时的交点比之前的交点距离射线射点近，则替换掉最近的交点
                    if ((interPoint - ray.Start).length() < (lastInterPoint - ray.Start).length())
                    {
                        triangle[0] = mesh.Triangles[i + 0];
                        triangle[1] = mesh.Triangles[i + 1];
                        triangle[2] = mesh.Triangles[i + 2];
                    }
                    lastInterPoint = interPoint;
                    if (i % 2 == 0)
                        i += 3;
                }
            }
            return res;
        }

        public static bool IntersectAABB(Ray ray, AABB box)
        {
            return IntersectAABB(ray, box, out uint[] array, out vec3 vec, false);
        }

        static float COLLIDE_THRESHOLD = -0.1F;
        public static bool CollideAABB(AABB a, AABB b)
        {
            vec3 aMax = a.Max, aMin = a.Min,
                bMax = b.Max, bMin = b.Min;
            float xOver = TestAxisCollide(aMax.x, aMin.x, bMax.x, bMin.x);
            float yOver = TestAxisCollide(aMax.y, aMin.y, bMax.y, bMin.y);
            float zOver = TestAxisCollide(aMax.z, aMin.z, bMax.z, bMin.z);
            return xOver > COLLIDE_THRESHOLD && yOver > COLLIDE_THRESHOLD && zOver > COLLIDE_THRESHOLD;
        }

        private static float TestAxisCollide(float a1, float a2, float b1, float b2)
        {
            float alft = a1 < a2 ? a1 : a2,
                art = a1 > a2 ? a1 : a2,
                blft = b1 < b2 ? b1 : b2,
                brt = b1 > b2 ? b1 : b2;

            float lrt = 0;
            float rlft = 0;
            if (alft < blft) //a物体在b物体左侧
            {
                lrt = art;
                rlft = blft;
            }
            else //a物体在b物体右侧
            {
                lrt = brt;
                rlft = alft;
            }
            return lrt - rlft;
            //if (lrt > rlft)
            //{
            //    return lrt - rlft;
            //}
            //else return 0;
        }
    }
}
