using GeoSensePlus.Mongo;
using NetCoreUtils.Database.MongoDb;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GeoSensePlus.Cli.Commands
{
    public class MongoCommand
    {
        IMongoDocWriter<Channel> writer;

        public MongoCommand(IMongoDocWriter<Channel> writer)
        {
            this.writer = writer;
        }

        public async Task Add()
        {
            await writer.InsertOneAsync(new Channel { Name = "Test Channel 1", Description = "Test Channel 1's description" });
        }
    }
}
