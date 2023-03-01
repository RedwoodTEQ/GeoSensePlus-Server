using Amazon;
using Amazon.IoT;
using Amazon.IoT.Model;
using Amazon.IotData;
using Amazon.IotData.Model;
using Amazon.Runtime;
using CoreCmd;
using GeoSensePlus.Cli.ConfigModels;
using Google.Api.Gax;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MQTTnet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Oocx.ReadX509CertificateFromPem;
using System.Net.Security;
using MQTTnet.Client;
using System.Threading;
using System.Text.Json;

namespace GeoSensePlus.Cli.Things;

public interface IThingService
{
    Task MqttPubAsync();
    Task GetThingShadowAsync();
    Task ListThingsAsync();
    Task CreateKeysAndCertificateAsync();
    Task MqttSubAsync();
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

    public async Task MqttSubAsync()
    {
        // Create a new MQTT client.
        var factory = new MqttFactory();
        using var mqttClient = factory.CreateMqttClient();
        mqttClient.ApplicationMessageReceivedAsync += e =>
        {
            Console.WriteLine("Received application message.");
            //e.DumpToConsole();
            try
            {
                string topic = e.ApplicationMessage.Topic;
                if (string.IsNullOrWhiteSpace(topic) == false)
                {
                    string payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                    Console.WriteLine($"Topic: {topic}. Message Received: {payload}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, ex);
            }

            return Task.CompletedTask;
        };

        var broker = "a30gbmt0ucy3ut-ats.iot.ap-southeast-2.amazonaws.com";
        var port = 8883;

        string deviceCertPEMString = File.ReadAllText(@"d:\sync_src\aws_cert_keys\ba74-certificate.pem.crt");
        string devicePrivateCertPEMString = File.ReadAllText(@"d:\sync_src\aws_cert_keys\ba74-private.pem.key");
        string amazonRootCA1 = File.ReadAllText(@"d:\sync_src\aws_cert_keys\AmazonRootCA1.pem");

        var certBytes = Encoding.UTF8.GetBytes(amazonRootCA1);
        var signingcert = new X509Certificate2(certBytes);

        var reader = new CertificateFromPemReader();
        X509Certificate2 deviceCertificate = reader.LoadCertificateWithPrivateKeyFromStrings(deviceCertPEMString, devicePrivateCertPEMString);

        RootCertificateTrust rootCertificateTrust = new RootCertificateTrust();
        rootCertificateTrust.AddCert(signingcert);

        List<X509Certificate> certs = new List<X509Certificate> { signingcert, deviceCertificate };

        MqttClientOptionsBuilderTlsParameters tlsOptions = new MqttClientOptionsBuilderTlsParameters();
        tlsOptions.Certificates = certs;
        tlsOptions.SslProtocol = System.Security.Authentication.SslProtocols.Tls12;
        tlsOptions.UseTls = true;
        tlsOptions.AllowUntrustedCertificates = true;
        tlsOptions.CertificateValidationHandler += rootCertificateTrust.VerifyServerCertificate;

        var options = new MqttClientOptionsBuilder()
            .WithTcpServer(broker, port)
            .WithClientId("mqttnet-ID")
            .WithTls(tlsOptions)
            .Build();

        await mqttClient.ConnectAsync(options, CancellationToken.None);

        //var message = new MqttApplicationMessageBuilder()
        //    .WithTopic("rltest/hello")
        //    .WithPayload("Hello World")
        //    .Build();

        //await mqttClient.PublishAsync(message, CancellationToken.None);

        var mqttSubscribeOptions = new MqttFactory().CreateSubscribeOptionsBuilder()
                .WithTopicFilter( f => f.WithTopic("rltest/hello") )
                .Build();

        var response = await mqttClient.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);

        Console.WriteLine("mqttnet subscribed to topic 'rltest/hello'");
        Console.ReadKey();
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

internal static class ObjectExtensions
{
    public static TObject DumpToConsole<TObject>(this TObject @object)
    {
        var output = "NULL";
        if (@object != null)
        {
            output = System.Text.Json.JsonSerializer.Serialize(@object, new JsonSerializerOptions
            {
                WriteIndented = true
            });
        }

        Console.WriteLine($"[{@object?.GetType().Name}]:\r\n{output}");
        return @object;
    }
}

/// <summary>
/// Verifies certificates against a list of manually trusted certs.
/// If a certificate is not in the Windows cert store, this will check that it's valid per our internal code.
/// </summary>
internal class RootCertificateTrust
{

    X509Certificate2Collection certificates;
    internal RootCertificateTrust()
    {
        certificates = new X509Certificate2Collection();
    }

    /// <summary>
    /// Add a trusted certificate
    /// </summary>
    /// <param name="x509Certificate2"></param>
    internal void AddCert(X509Certificate2 x509Certificate2)
    {
        certificates.Add(x509Certificate2);
    }

    /// <summary>
    /// This matches the delegate signature expected for certificate verification for MQTTNet
    /// </summary>
    /// <param name="arg"></param>
    /// <returns></returns>
    internal bool VerifyServerCertificate(MqttClientCertificateValidationEventArgs arg) => VerifyServerCertificate(new object(), arg.Certificate, arg.Chain, arg.SslPolicyErrors);


    /// <summary>
    /// This matches the delegate signature expected for certificate verification for M2MQTT
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="certificate"></param>
    /// <param name="chain"></param>
    /// <param name="sslPolicyErrors"></param>
    /// <returns></returns>
    internal bool VerifyServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {

        if (sslPolicyErrors == SslPolicyErrors.None) return true;

        X509Chain chainNew = new X509Chain();
        var chainTest = chain;

        chainTest.ChainPolicy.ExtraStore.AddRange(certificates);

        // Check all properties
        chainTest.ChainPolicy.VerificationFlags = X509VerificationFlags.NoFlag;

        // This setup does not have revocation information
        chainTest.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;

        // Build the chain
        var buildResult = chainTest.Build(new X509Certificate2(certificate));

        //Just in case it built with trust
        if (buildResult) return true;

        //If the error is something other than UntrustedRoot, fail
        foreach (var status in chainTest.ChainStatus)
        {
            if (status.Status != X509ChainStatusFlags.UntrustedRoot)
            {
                return false;
            }
        }

        //If the UntrustedRoot is on something OTHER than the GreenGrass CA, fail
        foreach (var chainElement in chainTest.ChainElements)
        {
            foreach (var chainStatus in chainElement.ChainElementStatus)
            {
                if (chainStatus.Status == X509ChainStatusFlags.UntrustedRoot)
                {
                    var found = certificates.Find(X509FindType.FindByThumbprint, chainElement.Certificate.Thumbprint, false);
                    if (found.Count == 0) return false;
                }
            }
        }

        return true;
    }

}
