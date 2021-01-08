using System;
using System.Collections.Generic;
using Xunit;

namespace Ray.AutoTask.LiWo.Test
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            dic["test"] = "ts";

            Assert.True(dic.Count == 1);
        }
    }
}
