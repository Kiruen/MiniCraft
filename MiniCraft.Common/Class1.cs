using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

//必须4.6.1才能引用standard2.0
namespace MiniCraft.Common
{
    public class TestClass : ISerializable
    {
        public int Data { get; set; }

        public void Foo(Action<string> act)
        {
            var list = new List<string>() { "1", "2"};
            list.ForEach(n => Console.WriteLine(n));
            var c = from a in list
                    select a.ToString();

            act(Data.ToString());
            Task.Run(() =>
            {
                act("你好啊:" + Thread.CurrentThread);
                Thread.Sleep(5000);
                Point p = new Point();
                p.Offset(1, 2);
                act("你好啊:" + p);
            });

        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            
            throw new NotImplementedException();
        }
    }
}
