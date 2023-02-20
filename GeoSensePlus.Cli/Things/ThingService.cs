using Amazon;
using Amazon.IoT;
using Amazon.IotData;
using Amazon.IotData.Model;
using Amazon.Runtime;
using CoreCmd;
using GeoSensePlus.Cli.ConfigModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GeoSensePlus.Cli.Things;

public interface IThingService
{
    Task MqttPubAsync();
    Task GetThingShadowAsync();
    Task ListThingsAsync();
}
public class ThingService : IThingService
{
    AmazonIotDataClient _iotDataClient;
    AmazonIoTClient _iotClient;

    public ThingService(IOptions<aws_credentials> option)
    {
        string accessKey = option.Value.rwd_access_key_id;
        string secretKey = option.Value.rwd_secret_access_key;
        AWSCredentials credentials = new BasicAWSCredentials(accessKey, secretKey);

        _iotDataClient = new AmazonIotDataClient("https://a30gbmt0ucy3ut-ats.iot.ap-southeast-2.amazonaws.com", credentials);
        _iotClient = new AmazonIoTClient(credentials, RegionEndpoint.APSoutheast2);
    }

    public async Task ListThingsAsync()
    {
        var things = await _iotClient.ListThingsAsync();

        Console.WriteLine("All things:");
        foreach(var thing in things.Things)
        {
            Console.WriteLine(thing.ThingName);
        }
    }

    public async Task UpdateThingShadow(string name, MemoryStream payLoad)
    {
        var updateThingShadowRequest = new UpdateThingShadowRequest() { ThingName = name, Payload = payLoad };
        var result = await this._iotDataClient.UpdateThingShadowAsync(updateThingShadowRequest);
        if (result.HttpStatusCode != HttpStatusCode.OK)
        {
            throw new Exception($"Received HTTP {result.HttpStatusCode}");
        }
    }

    public async Task MqttPubAsync()
    {
        var measureData = new
        {
            Id = DateTime.Now.Ticks,
            Presures = new Random().Next(0, 100)
        };

        var jsonStr = JsonConvert.SerializeObject(measureData);
        var payloadStream = new MemoryStream(Encoding.UTF8.GetBytes(jsonStr ?? string.Empty));

        PublishRequest publishRequest = new PublishRequest()
        {
            //Topic = "$aws/things/Test_thing_001/shadow/get/accepted",
            Topic = "$aws/things/Test_thing_002/shadow/name/rl_test/get/accepted",
            Qos = 0,
            Payload = payloadStream
        };
        await _iotDataClient.PublishAsync(publishRequest);
    }

    public async Task GetThingShadowAsync()
    {
        var req = new GetThingShadowRequest() { ThingName = "OTA_DEMO_3" };
        var res = await this._iotDataClient.GetThingShadowAsync(req);
        var json = Encoding.UTF8.GetString(res.Payload.ToArray());
        Console.WriteLine($"json = {json}");
    }
}
