using GeoSensePlus.Cli.Commands.Shared;
using GeoSensePlus.Firestore.Models;
using GeoSensePlus.Firestore.Repositories.Common;
using System;
using Microsoft.Extensions.DependencyInjection;
using GeoSensePlus.Firestore;
using GeoSensePlus.Firestore.Services;
using GeoSensePlus.Mqtt;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using NetCoreUtils.Database.InfluxDb;
using System.Text.Json;
using System.Text;

namespace GeoSensePlus.Cli.Commands
{
    public class TestCommand : CommandBase
    { 
        readonly IRepository<AssetData> _repo;
        readonly IMqttService _mqttService;
        readonly IMqttClientService _mqttClient;

        public TestCommand(
            IRepository<AssetData> repo
            , IMqttService mqttService
            , IMqttClientService mqttClient
        ){
            _repo = repo;
            _mqttService = mqttService;
            _mqttClient = mqttClient;
        }

        /// <summary>
        /// Attach asset between 2 edges to mimic moving asset between 2 edges
        /// </summary>
        public void ChangeAssetEdges(string assetId, string edgeId1, string edgeId2)
        {
            this.Execute(() => {
                var assetSvc = sp.GetService<IAssetService>();
                var edgeRepo = sp.GetService<IRepository<EdgeData>>();

                string targetEdgeId = edgeId1;
                var batch = sp.GetService<IFirebaseClient>().GetFirestoreDb().StartBatch();
                do
                {
                    Console.WriteLine($"Moving asset ({assetId}) to edge ({targetEdgeId}) ...");
                    assetSvc.UpdateEdgeAsync(batch, _repo.GetDocument(assetId), edgeRepo.GetDocument(targetEdgeId)).Wait();
                    batch.CommitAsync().Wait();
                    targetEdgeId = targetEdgeId == edgeId1 ? edgeId2 : edgeId1;
                    Console.WriteLine("Input 'q' to quit ...");
                }
                while (Console.ReadLine() != "q") ;
            });
        }

        public async Task UpdateTemperature()
        {
            const string urlRoot = "https://localhost:5001/api/metrics";
            HttpClient _httpClient;

            // bypass the certificate
            _httpClient = new HttpClient(
                new HttpClientHandler { ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true }
            );

            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "GeoSense+ CLI");

            // data
            Random value = new Random();
            PointModel<double> model = new PointModel<double>();
            model.Measurement = "TestMeasurement";
            model.Tags["TestTag1"] = "SomeTag1";
            model.Tags["TestTag2"] = "SomeTag2";
            model.Tags["TestTag3"] = "SomeTag3";
            model.Fields["TestValu1"] = value.NextDouble() * 100;
            model.Fields["TestValu2"] = value.NextDouble() * 100;

            string json = JsonSerializer.Serialize(model);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            // post
            await _httpClient.PostAsync(urlRoot, data);
        }

        public async Task MqttService()
        {
            await _mqttService.StartAsync();

            int count = 1;
            do
            {
                await _mqttService.PublishAsync("hello", $"test message {count++}");
            }
            while (Console.ReadKey().KeyChar != 'q');
            //while (Console.KeyAvailable);
        }

        public async Task MqttClient()
        {
            await _mqttClient.ConnectAsync();
            await _mqttClient.SubscribeTopic("hello");
            Console.ReadLine();
        }
    }
}
