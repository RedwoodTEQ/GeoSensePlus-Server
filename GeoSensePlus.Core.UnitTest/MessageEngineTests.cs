using GeoSensePlus.Core.MessageProcessing.Interfaces;
using GeoSensePlus.Core.UnitTest.TestEnv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace GeoSensePlus.Core.UnitTest
{
    public class MessageEngineTests : IClassFixture<GlobalUtilFixture>
    {
        GlobalUtilFixture _util;

        public MessageEngineTests(GlobalUtilFixture util) => _util = util;

        [Fact]
        public void Test_PipelineSize()
        {
            var jsonPipeline = _util.GetService<IEnumerable<IMessageHandler<string>>>();
            Assert.Equal(2, jsonPipeline.Count());

            var bytePipeline = _util.GetService<IEnumerable<IMessageHandler<byte[]>>>();
            Assert.Empty(bytePipeline);
        }
    }
}
