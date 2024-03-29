﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreCmd.Attributes;
using GeoSensePlus.Cli.Commands.Shared;
using GeoSensePlus.Firestore;
using GeoSensePlus.Firestore.Models;
using GeoSensePlus.Firestore.Repositories;
using GeoSensePlus.Firestore.Repositories.Common;
using Google.Cloud.Firestore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GeoSensePlus.Cli.Commands
{
    public class EdgeCommand : CommandBase
    {
        readonly IRepository<EdgeData> _svc;

        public EdgeCommand()
        {
            _svc = sp.GetService<IRepository<EdgeData>>();
        }

        //public void Add(string edgeId, string description)
        //public void Add(string edgeId)
        //{
        //    this.Execute(() => { 
        //        Console.WriteLine($"Adding edge {edgeId} ...");
        //        _svc.AddAsync(edgeId, new EdgeData()).Wait();
        //        Console.WriteLine("Finished adding edge");
        //    });
        //}

        public void LinkMarker(string edgeId, string floorPlanId, string edgeMarkerId)
        {
            this.Execute(() => { 
                var _floorplanSvc = sp.GetService<IRepository<FloorplanData>>();
                var _edgeMarkerSvc = sp.GetService<IEdgeMarkerRepository>();
                _edgeMarkerSvc.FloorplanId = floorPlanId;

                Console.WriteLine($"link edge {edgeId} to edge mark {edgeMarkerId} of floorplan {floorPlanId}");

                var edge = _svc.GetDocument(edgeId);
                var edgeMarker = _edgeMarkerSvc.GetDocument(edgeMarkerId);

                //var floorplan = _floorplanSvc.RetrieveAsync(floorPlanId).Result;
                //FirestoreUtils.PrintObject(floorplan);
                //Console.WriteLine("");

                edge.UpdateAsync(nameof(EdgeData.EdgeMarkerRef), edgeMarker).Wait();
                edgeMarker.UpdateAsync(nameof(EdgeMarkerData.EdgeRef), edge).Wait();
            });
        }

        public void Remove(string edgeId)
        {
            this.Execute(() => { 
                Console.WriteLine($"edge remove {edgeId}");
                _svc.RemoveAsync(edgeId).Wait();
            });
        }

        public void Display(string edgeId)
        {
            this.Execute(() => {
                // EdgeData obj = _svc.RetrieveAsync(edgeId).Result;
                var dict = _svc.RetrieveJsonDictionaryAsync(edgeId).Result;

                if ( dict == null)
                {
                    Console.WriteLine($"Can not find any edge by given id {edgeId}");
                }
                else
                {
                    // Console.WriteLine(
                    //     JsonConvert.SerializeObject( dict, Formatting.Indented )
                    // );

                    JsonConverter[] converters =
                    {
                       new DocumentReferenceConverter(),
                       new CollectionReferenceConverter(),
                    };
                    
                    Console.WriteLine(
                        JsonConvert.SerializeObject( 
                            dict, 
                            Formatting.Indented, converters
                        )
                    );
                }
            });
        }

        //public void Report(string hardwareSerial, string sessionId)
        //{
        //    Console.WriteLine("subcommand: edge report");
        //}
    }
}
