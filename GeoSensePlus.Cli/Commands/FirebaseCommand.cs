using GeoSensePlus.Cli.Commands.Shared;
using GeoSensePlus.Firestore.ConfigUtils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using GeoSensePlus.Firestore.Repositories.Common;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GeoSensePlus.Cli.Commands
{
    public class FirebaseCommand : CommandBase
    {
        readonly IConfigOperator _configOperator;
        private readonly IManagerRepository _manageRepository;

        public FirebaseCommand()
        {
            _configOperator = sp.GetService<IConfigOperator>();
            _manageRepository = sp.GetService<IManagerRepository>();
        }

        public void Export()
        {
            this.Execute(() =>
            {
               var result = _manageRepository.ExportDatabaseDictionary().Result;
            });
        }

        /// <summary>
        /// Export db as json file. Store it by given absolute file path.
        /// </summary>
        /// <param name="outputPath">Absolute output file path</param>
        public void ExportJson(string outputPath)
        {
            this.Execute(() =>
            {
               var outputExt = Path.GetExtension(outputPath);
               
               if (outputExt != ".json" && outputExt != ".txt")
               {
                   Console.ForegroundColor = ConsoleColor.Red;
                   Console.WriteLine($"ERROR: Output path must contains file name, and file extension must be either .json or .txt");
                   Console.ResetColor();
                   return;
               }
               else if (!Directory.Exists(Path.GetDirectoryName(outputPath)))
               {
                   Console.ForegroundColor = ConsoleColor.Red;
                   Console.WriteLine($"ERROR: Output path directory is not exist. Ensure to set an absolute path.");
                   Console.ResetColor();
                   return;
               }
               else if (File.Exists(outputPath))
               {
                   Console.ForegroundColor = ConsoleColor.Red;
                   Console.WriteLine($"ERROR: Output file is exist.");
                   Console.ResetColor();
                   return;
               }
                
               var result = _manageRepository.ExportDatabaseJsonDictionary().Result;
               
               if ( result == null)
               {
                   Console.ForegroundColor = ConsoleColor.Red;
                   Console.WriteLine($"No data.");
                   Console.ResetColor();
               }
               else
               {
                   var json = JsonConvert.SerializeObject(result, Formatting.Indented);
                   System.IO.File.WriteAllText(outputPath, json);
                   
                   Console.ForegroundColor = ConsoleColor.DarkGreen;
                   Console.WriteLine($"\nExported to {outputPath}.");
                   Console.ResetColor();
               }
            });
        }

        public void Default()
        {
            Console.WriteLine("Current firebase key file:");
            var key = _configOperator.GetFirebaseKey();
            Console.WriteLine(_configOperator.GetFirebaseKey());
        }

        /// <summary>
        /// Set the current firebase key file. The specified file must exist under the current directory.
        /// </summary>
        public void Set(string keyFileName)
        {
            try
            {
                _configOperator.SetFirebaseKey(keyFileName);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        /// <summary>
        /// Register a firebase key file to the specified alias name.
        /// </summary>
        /// <param name="name">The relevant alias name.</param>
        /// <param name="keyFileName">The firebase key file name which must exist.</param>
        public void Register(string name, string keyFileName)
        {
            try
            {
                _configOperator.RegisterFirebaseKey(name, keyFileName);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void Unregister(string name)
        {
            try
            {
                _configOperator.UnregisterFirebaseKey(name);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void Switch(string name)
        {
            try
            {
                _configOperator.SwitchFirebaseKey(name);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void List()
        {
            try
            {
                _configOperator.ListFirebaseKey();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
