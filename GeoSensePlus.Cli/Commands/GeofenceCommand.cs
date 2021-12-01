using System;
using System.Collections.Generic;
using System.Text;
using GeoSensePlus.Cli.Commands.Shared;
using GeoSensePlus.Firestore.Models;
using GeoSensePlus.Firestore.Repositories;
using GeoSensePlus.Firestore.Repositories.Common;
using Microsoft.Extensions.DependencyInjection;

namespace GeoSensePlus.Cli.Commands
{
    public class GeofenceCommand : CommandBase
    {
        readonly IRepository<GeofenceData> _svc;

        public GeofenceCommand()
        {
            _svc = sp.GetService<IRepository<GeofenceData>>();
        }

        //public void Add(string docId)
        //{
        //    this.Execute(()=> { 
        //        Console.WriteLine($"geofence add {docId}");
        //        _svc.AddAsync(docId, new GeofenceData {
        //            Name = "test name",
        //            Radius = 668.2370932609019
        //        }).Wait();
        //    });
        //}

        public void Remove(string docId)
        {
            this.Execute(() => {
                Console.WriteLine($"geofence remove {docId}");
                _svc.RemoveAsync(docId).Wait();
            });
        }

        public void List()
        {
            this.Execute(() => {
                Console.WriteLine("geofence list");
                var documents = _svc.ListAllAsync();
                
                if (documents.Result.Count == 0)
                {
                    Console.WriteLine($"No geofence.");
                }
                else
                {
                    foreach (var snapshot in documents.Result)
                    {
                        Console.WriteLine($"Geofence id: {snapshot.Id}");
                    }
                }
            });
        }
    }
}
