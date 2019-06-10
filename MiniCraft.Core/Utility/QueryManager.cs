using CSharpGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCraft.Core
{
    public class QueryManager
    {
        private static GLDelegates.void_int_uintN glGenQueries;
        private static GLDelegates.void_uint glIsQuery;
        private static GLDelegates.void_uint_uint glBeginQuery;
        private static GLDelegates.void_uint glEndQuery;
        private static GLDelegates.void_uint_uint_intN glGetQueryiv;
        private static GLDelegates.void_uint_uint_intN glGetQueryObjectiv;
        private static GLDelegates.void_uint_uint_uintN glGetQueryObjectuiv;
        private static GLDelegates.void_uint_uint glBeginConditionalRender;
        private static GLDelegates.void_void glEndConditionalRender;
        private static GLDelegates.void_int_uintN glDeleteQueries;

        /// <summary>
        /// texture's id/name.
        /// </summary>
        protected static uint[] ids = new uint[1024 * 1024];


        static QueryManager()
        {
            glGenQueries = GL.Instance.GetDelegateFor("glGenQueries", GLDelegates.typeof_void_int_uintN) as GLDelegates.void_int_uintN;
            glIsQuery = GL.Instance.GetDelegateFor("glIsQuery", GLDelegates.typeof_void_uint) as GLDelegates.void_uint;
            glBeginQuery = GL.Instance.GetDelegateFor("glBeginQuery", GLDelegates.typeof_void_uint_uint) as GLDelegates.void_uint_uint;
            glEndQuery = GL.Instance.GetDelegateFor("glEndQuery", GLDelegates.typeof_void_uint) as GLDelegates.void_uint;
            glGetQueryiv = GL.Instance.GetDelegateFor("glGetQueryiv", GLDelegates.typeof_void_uint_uint_intN) as GLDelegates.void_uint_uint_intN;
            glBeginConditionalRender = GL.Instance.GetDelegateFor("glBeginConditionalRender", GLDelegates.typeof_void_uint_uint) as GLDelegates.void_uint_uint;
            glEndConditionalRender = GL.Instance.GetDelegateFor("glEndConditionalRender", GLDelegates.typeof_void_void) as GLDelegates.void_void;
            glGetQueryObjectiv = GL.Instance.GetDelegateFor("glGetQueryObjectiv", GLDelegates.typeof_void_uint_uint_intN) as GLDelegates.void_uint_uint_intN;
            glGetQueryObjectuiv = GL.Instance.GetDelegateFor("glGetQueryObjectuiv", GLDelegates.typeof_void_uint_uint_uintN) as GLDelegates.void_uint_uint_uintN;
            glDeleteQueries = GL.Instance.GetDelegateFor("glDeleteQueries", GLDelegates.typeof_void_int_uintN) as GLDelegates.void_int_uintN;
        }

        /// <summary>
        /// Begin query.
        /// <para>delimit the boundaries of a query object.</para>
        /// </summary>
        /// <param name="target">Specifies the target type of query object established between glBeginQuery and the subsequent glEndQuery.</param>
        public static void BeginQuery(QueryTarget target, uint id)
        {
            if(ids[0] == 0)
                glGenQueries(1024 * 1024, ids);
            glBeginQuery((uint)target, id);
        }

        /// <summary>
        /// Een query.
        /// </summary>
        /// <param name="target">Specifies the target type of query object to be concluded.s</param>
        public static void EndQuery(QueryTarget target)
        {
            glEndQuery((uint)target);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public static int SampleCount(uint id)
        {
            var result = new int[1];
            glGetQueryObjectiv(id, GL.GL_QUERY_RESULT, result);

            return result[0];
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public static bool SampleRendered(uint id)
        {
            var result = new int[1];
            int count = 1000;
            while (result[0] == 0 && count-- > 0)
            {
                glGetQueryObjectiv(id, GL.GL_QUERY_RESULT_AVAILABLE, result);
            }

            if (result[0] != 0)
            {
                glGetQueryObjectiv(id, GL.GL_QUERY_RESULT, result);
            }
            else
            {
                result[0] = 1;
            }

            return result[0] != 0;
        }

        // TODO: need demo!
        /// <summary>
        /// Begin conditional rendering.
        /// </summary>
        public static void BeginConditionalRender(ConditionalRenderMode mode, uint id)
        {
            glBeginConditionalRender(id, (uint)mode);
        }

        /// <summary>
        /// End conditional rendering.
        /// </summary>
        public static void EndConditionalRender()
        {
            glEndConditionalRender();
        }

    }
}
