using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using String_Formatter;

namespace FormatterTests
{
    internal class TestCashe : ExpressionCashe
    {
        public int readTry = 0;
        public int readCnt = 0;
        public int writeTry = 0;
        public int writeCnt = 0;

        public override string? FindElement(string key, object target)
        {
            Interlocked.Increment(ref readTry);
            string? res = base.FindElement(key, target);
            if (res != null) Interlocked.Increment(ref readCnt);
            return res;
        }

        public override string? AddElement(string key, object target)
        {
            Interlocked.Increment(ref writeTry);
            string? res = base.AddElement(key, target);
            if (res != null) Interlocked.Increment(ref writeCnt);
            return res;
        }
    }
}
