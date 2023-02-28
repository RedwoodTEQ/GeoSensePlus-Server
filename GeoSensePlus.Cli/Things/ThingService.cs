using Amazon;
using Amazon.IoT;
using Amazon.IoT.Model;
using Amazon.IotData;
using Amazon.IotData.Model;
using Amazon.Runtime;
using CoreCmd;
using GeoSensePlus.Cli.ConfigModels;
using Google.Api.Gax;
using InfluxDB.Client.Api.Domain;
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
    Task CreateKeysAndCertificateAsync();
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

        /***** TODO: test the following methods: *****/
        //_iotClient.CreateThingAsync()
        //_iotClient.CreateStreamAsync()    // transfer file via mqtt
        //_iotClient.CreateProvisioningTemplateAsync()
    }

    public async Task CreateKeysAndCertificateAsync()
    {
        var createKeysAndCertificateResponse = await _iotClient.CreateKeysAndCertificateAsync(new CreateKeysAndCertificateRequest() { SetAsActive = true });
        string cert_id = createKeysAndCertificateResponse.CertificateId;
        string cert_arn = createKeysAndCertificateResponse.CertificateArn;
        string cert_pem = createKeysAndCertificateResponse.CertificatePem;
        string prikey = createKeysAndCertificateResponse.KeyPair.PrivateKey;
        string pubkey = createKeysAndCertificateResponse.KeyPair.PublicKey;

        Console.WriteLine($"cert_id:\n{cert_id}");
        Console.WriteLine($"cert_arn:\n{cert_arn}");
        Console.WriteLine($"cert_pem:\n{cert_pem}");
        Console.WriteLine($"prikey:\n{prikey}");        // TODO: need to save to file
        Console.WriteLine($"pubkey:\n{pubkey}");        // TODO: need to save to file

        await _iotClient.AttachPolicyAsync(new AttachPolicyRequest()    // see also: _iotClient.CreatePolicyAsync();
        {
            Target = createKeysAndCertificateResponse.CertificateArn,
            PolicyName = "prod_mqtt__all_things_237624127646"           // preconfigured in aws iot core console page
        });
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
