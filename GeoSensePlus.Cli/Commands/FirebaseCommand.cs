using GeoSensePlus.Cli.Commands.Shared;
using GeoSensePlus.Firestore.ConfigUtils;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace GeoSensePlus.Cli.Commands
{
    public class FirebaseCommand : CommandBase
    {
        readonly IConfigOperator _configOperator;

        public FirebaseCommand()
        {
            _configOperator = sp.GetService<IConfigOperator>();
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
