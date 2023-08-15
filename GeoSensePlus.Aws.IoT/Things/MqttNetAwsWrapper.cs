using MQTTnet;
using MQTTnet.Client;
using Oocx.ReadX509CertificateFromPem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GeoSensePlus.Aws.IoT.Things;
public class MqttNetAwsWrapper : IDisposable
{
    IMqttClient _mqttClient;

    public MqttNetAwsWrapper(IMqttClient mqttClient)
    {
        _mqttClient = mqttClient;
    }

    static public async Task<MqttNetAwsWrapper> Create(
        string brokerAddress
        , string certKeyPath
        , string certFileName
        , string privateKeyFileName
        , string clientId = "mqtt-test-client-id"
        , string amazonRootCA1FileName = "AmazonRootCA1.pem"
        , int port = 8883
    )
    {
        var mqttClient = new MqttFactory().CreateMqttClient();

        string pemString_DeviceCert = File.ReadAllText(@$"{certKeyPath}\{certFileName}");
        string pemString_DevicePriKey = File.ReadAllText(@$"{certKeyPath}\{privateKeyFileName}");
        string pemString_AmazonRootCA1 = File.ReadAllText(@$"{certKeyPath}\{amazonRootCA1FileName}");

        X509Certificate2 x509Cert_Device = new CertificateFromPemReader().LoadCertificateWithPrivateKeyFromStrings(pemString_DeviceCert, pemString_DevicePriKey);
        X509Certificate2 x509Cert_AmazonRootCA1 = new(Encoding.UTF8.GetBytes(pemString_AmazonRootCA1));

        RootCertificateTrust rootCertificateTrust = new();
        rootCertificateTrust.AddCert(x509Cert_AmazonRootCA1);

        MqttClientOptionsBuilderTlsParameters tlsOptions = new();
        tlsOptions.Certificates = new List<X509Certificate> { x509Cert_AmazonRootCA1, x509Cert_Device };
        tlsOptions.SslProtocol = System.Security.Authentication.SslProtocols.Tls12;
        tlsOptions.UseTls = true;
        tlsOptions.AllowUntrustedCertificates = true;
        tlsOptions.CertificateValidationHandler += rootCertificateTrust.VerifyServerCertificate;

        var options = new MqttClientOptionsBuilder()
            .WithTcpServer(brokerAddress, port)
            .WithClientId(clientId)
            .WithTls(tlsOptions)
            .Build();

        await mqttClient.ConnectAsync(options, CancellationToken.None);
        return new MqttNetAwsWrapper(mqttClient);
    }

    public async Task Subscribe(string topic, Func<MqttApplicationMessageReceivedEventArgs, Task> fn)
    {
        _mqttClient.ApplicationMessageReceivedAsync += fn;

        await _mqttClient.SubscribeAsync(
            new MqttFactory()
                .CreateSubscribeOptionsBuilder()
                .WithTopicFilter(f => f.WithTopic(topic))
                .Build(),
            CancellationToken.None
        );
    }

    public async Task Publish(string topic, string payload)
    {
        await _mqttClient.PublishAsync(
            new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(payload)
                .Build(),
            CancellationToken.None
        );
    }

    public void Dispose()
    {
        _mqttClient?.Dispose();
    }
}
