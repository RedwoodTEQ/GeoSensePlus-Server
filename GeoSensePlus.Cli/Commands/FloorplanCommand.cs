using System;
using System.Collections.Generic;
using GeoSensePlus.Cli.Commands.Shared;
using GeoSensePlus.Firestore.Models;
using GeoSensePlus.Firestore.Repositories;
using GeoSensePlus.Firestore.Repositories.Common;
using Google.Cloud.Firestore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace GeoSensePlus.Cli.Commands
{
    public class FloorplanCommand : CommandBase
    {
        readonly IRepository<FloorplanData> _repoPlan;

        public FloorplanCommand()
        {
            _repoPlan = sp.GetService<IRepository<FloorplanData>>();
        }

        public void Display(string planId)
        {
            this.Execute(() =>
            {
                FloorplanData obj = _repoPlan.RetrieveAsync(planId).Result;

                if (obj == null)
                {
                    Console.WriteLine($"Can not find any plan by given id {planId}");
                }
                else
                {
                    FloorplanDataJson jsonData = new FloorplanDataJson(obj);
                    Console.WriteLine(
                        JsonConvert.SerializeObject(jsonData, Formatting.Indented, new JsonSerializerSettings
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        })
                    );
                }
            });
        }
        
        public void ListEdgeMarkers(string planId)
        {
            this.Execute(() =>
            {
                var edgeMarkerRepo = sp.GetService<IEdgeMarkerRepository>();
                edgeMarkerRepo.FloorplanId = planId;
                var documents = edgeMarkerRepo.ListAllAsync();
                if (documents.Result.Count == 0)
                {
                    Console.WriteLine($"Can not find any edge maker by given floorplan id {planId}");
                }
                else
                {
                    foreach (var snapshot in documents.Result)
                    {
                        Console.WriteLine($"Edge maker id: {snapshot.Id}");
                        var obj = edgeMarkerRepo.RetrieveAsync(snapshot.Id).Result;
                        if (obj == null)
                        {
                            Console.WriteLine($"Can not find edge marker by given id {snapshot.Id}");
                        }
                        else
                        {
                            var jsonData = new EdgeMarkerDataJson(obj);
                            Console.WriteLine(
                                JsonConvert.SerializeObject(jsonData, Formatting.Indented, new JsonSerializerSettings
                                {
                                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                                })
                            );
                        }
                    }
                }
            });
        }
    }
}