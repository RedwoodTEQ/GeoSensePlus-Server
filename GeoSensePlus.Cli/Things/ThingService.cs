using Amazon;
using Amazon.IotData;
using Amazon.IotData.Model;
using Amazon.Runtime;
using CoreCmd;
using GeoSensePlus.Cli.ConfigModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
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
    Task GetThingShadowAsync();
}
public class ThingService : IThingService
{
    private readonly AmazonIotDataClient client;

    public ThingService(IOptions<aws_credentials> option)
    {
        string accessKey = option.Value.rwd_access_key_id;
        string secretKey = option.Value.rwd_secret_access_key;

        Console.WriteLine($"accessKey = {accessKey}; secretKey = {secretKey}");
        AWSCredentials credentials = new BasicAWSCredentials(accessKey, secretKey);
        //RegionEndpoint endpoint = RegionEndpoint.APSoutheast2;      // The Asia Pacific (Sydney) endpoint
        client = new AmazonIotDataClient("https://a30gbmt0ucy3ut-ats.iot.ap-southeast-2.amazonaws.com", credentials);
    }

    public async Task UpdateThingShadow(string name, MemoryStream payLoad)
    {
        var updateThingShadowRequest = new UpdateThingShadowRequest() { ThingName = name, Payload = payLoad };
        var result = await this.client.UpdateThingShadowAsync(updateThingShadowRequest);
        if (result.HttpStatusCode != HttpStatusCode.OK)
        {
            throw new Exception($"Received HTTP {result.HttpStatusCode}");
        }
    }

    public async Task GetThingShadowAsync()
    {
        var req = new GetThingShadowRequest() { ThingName = "OTA_DEMO_3" };
        var res = await this.client.GetThingShadowAsync(req);
        var json = Encoding.UTF8.GetString(res.Payload.ToArray());
        Console.WriteLine($"json = {json}");
    }
}
