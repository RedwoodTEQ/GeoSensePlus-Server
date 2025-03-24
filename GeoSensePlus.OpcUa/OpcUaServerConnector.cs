using Opc.Ua;   // Install-Package OPCFoundation.NetStandard.Opc.Ua
using Opc.Ua.Client;
using Opc.Ua.Configuration;
using System.Threading;

namespace GeoSensePlus.OpcUa;

public class OpcUaServerConnector : IDisposable
{
    public bool SecurityEnabled { get; set; }
    public string MyApplicationName { get; set; }
    public Session? OpcUaSession { get; set; }
    public string OpcUaNameSpaceIndex { get; set; }
    public Dictionary<string, TagObject> TagList { get; set; }
    private string DiscoveryUrl { get; set; }
    public bool SessionRenewalRequired { get; set; }
    public double SessionRenewalPeriodMins { get; set; }
    public DateTime LastTimeSessionRenewed { get; set; }
    public DateTime LastTimeOPCServerFoundAlive { get; set; }
    public bool ClassDisposing { get; set; }
    public bool InitialisationCompleted { get; set; }


    /// <summary>
    /// The session can be closed from the Server side, so it's better to allow the class to
    /// reinitiate session periodically
    /// </summary>
    private Thread? RenewerThread { get; set; }

    private CancellationTokenSource tokenSource = new();

    /// <param name="discoveryUrl">
    /// If use "Prosys OPC UA Simulation Server", discoveryUrl is the value of "Connection Address (UA TCP)"
    /// on the "Status" tab.
    /// </param>
    /// <param name="nameSpaceIndex">
    /// For example, if NodeId is "ns=7;i=1001",the nameSpaceIndex should be assigned as "7".
    /// </param>
    public OpcUaServerConnector(string discoveryUrl, Dictionary<string, TagObject> taglist, bool sessionrenewalRequired, double sessionRenewalMinutes, string nameSpaceIndex)
    {
        DiscoveryUrl = discoveryUrl;
        MyApplicationName = "MyApplication";
        TagList = taglist;
        SessionRenewalRequired = sessionrenewalRequired;
        SessionRenewalPeriodMins = sessionRenewalMinutes;
        OpcUaNameSpaceIndex = nameSpaceIndex;
        LastTimeOPCServerFoundAlive = DateTime.Now;
        InitializeOPCUAClient();

        if (SessionRenewalRequired)
        {
            LastTimeSessionRenewed = DateTime.Now;
            RenewerThread = new(() => RenewSessionThread(tokenSource.Token));
            RenewerThread.Start();
        }
    }

    public void Dispose()
    {
        ClassDisposing = true;
        try
        {
            if (OpcUaSession != null)
            {
                OpcUaSession.Close();
                OpcUaSession.Dispose();
            }

            // Stop thread
            if (tokenSource != null)
            {
                tokenSource.Cancel();
                if (RenewerThread != null)
                {
                    RenewerThread.Join(); // wait thread to finish
                }
                tokenSource.Dispose();
            }
        }
        catch
        {
            // TODO: log exception
        }
        GC.SuppressFinalize(this);
    }

    ~OpcUaServerConnector()
    {
        Dispose();
    }

    private void RenewSessionThread(CancellationToken token)
    {
        while (!token.IsCancellationRequested && !ClassDisposing)
        {
            if ((DateTime.Now - LastTimeSessionRenewed).TotalMinutes > SessionRenewalPeriodMins
            || (DateTime.Now - LastTimeOPCServerFoundAlive).TotalSeconds > 60)
            {
                Console.WriteLine("Renewing Session");
                try
                {
                    OpcUaSession?.Close();
                    OpcUaSession?.Dispose();
                }
                catch { }
                InitializeOPCUAClient();
                LastTimeSessionRenewed = DateTime.Now;
            }
            Thread.Sleep(2000);
        }
    }

    public void InitializeOPCUAClient()
    {
        //Console.WriteLine("Step 1 - Create application configuration and certificate.");
        var config = new ApplicationConfiguration()
        {
            ApplicationName = MyApplicationName,
            //ApplicationUri = Utils.Format(@"urn:{0}:" + MyApplicationName + "", ServerAddress),
            ApplicationType = ApplicationType.Client,

            // TODO: refactor to enable this certificate configuration
            SecurityConfiguration = new SecurityConfiguration
            {
                //ApplicationCertificate = new CertificateIdentifier { StoreType = @"Directory", StorePath = @"%CommonApplicationData%\OPC Foundation\CertificateStores\MachineDefault", SubjectName = Utils.Format(@"CN={0}, DC={1}", MyApplicationName, ServerAddress) },
                ApplicationCertificate = new CertificateIdentifier { StoreType = @"Directory", StorePath = @"%CommonApplicationData%\OPC Foundation\CertificateStores\MachineDefault", SubjectName = "TestSubject1" },
                TrustedIssuerCertificates = new CertificateTrustList { StoreType = @"Directory", StorePath = @"%CommonApplicationData%\OPC Foundation\CertificateStores\UA Certificate Authorities" },
                TrustedPeerCertificates = new CertificateTrustList { StoreType = @"Directory", StorePath = @"%CommonApplicationData%\OPC Foundation\CertificateStores\UA Applications" },
                RejectedCertificateStore = new CertificateTrustList { StoreType = @"Directory", StorePath = @"%CommonApplicationData%\OPC Foundation\CertificateStores\RejectedCertificates" },
                AutoAcceptUntrustedCertificates = true,
                AddAppCertToTrustedStore = true
            },
            TransportConfigurations = new TransportConfigurationCollection(),
            TransportQuotas = new TransportQuotas { OperationTimeout = 15000 },
            ClientConfiguration = new ClientConfiguration { DefaultSessionTimeout = 60000 },
            TraceConfiguration = new TraceConfiguration()
        };
        config.Validate(ApplicationType.Client).GetAwaiter().GetResult();
        if (config.SecurityConfiguration.AutoAcceptUntrustedCertificates)
        {
            config.CertificateValidator.CertificateValidation += (s, e) =>
            {
                e.Accept = e.Error.StatusCode == StatusCodes.BadCertificateUntrusted;
            };
        }

        var application = new ApplicationInstance
        {
            ApplicationName = MyApplicationName,
            ApplicationType = ApplicationType.Client,
            ApplicationConfiguration = config
        };
        application.CheckApplicationInstanceCertificate(false, 2048).GetAwaiter().GetResult();


        var selectedEndpoint = CoreClientUtils.SelectEndpoint(DiscoveryUrl, useSecurity: SecurityEnabled, discoverTimeout: 15000);

        OpcUaSession = Session.Create(config, new ConfiguredEndpoint(null, selectedEndpoint, EndpointConfiguration.Create(config)), false, "", 60000, null, null).GetAwaiter().GetResult();

        var subscription = new Subscription(OpcUaSession.DefaultSubscription) { PublishingInterval = 1000 };

        var list = new List<MonitoredItem> { };


        // TODO: refactor this list logic
        //list.Add(new MonitoredItem(subscription.DefaultItem) { DisplayName = "RLTest1", StartNodeId = "ns=3;i=1001" });

        foreach (KeyValuePair<string, TagObject> td in TagList)
        {
            var item = new MonitoredItem(subscription.DefaultItem)
            {
                DisplayName = td.Value.DisplayName,
                StartNodeId = "ns=" + OpcUaNameSpaceIndex + ";s=" + td.Value.Identifier,
            };
            item.Notification += OnTagValueChange;
            list.Add(item);
        }

        subscription.AddItems(list);
        OpcUaSession.AddSubscription(subscription);
        subscription.Create();
    }

    public void OnTagValueChange(MonitoredItem item, MonitoredItemNotificationEventArgs e)
    {

        foreach (var value in item.DequeueValues())
        {
            // TODO: what's the purpose of this if block?
            if (item.DisplayName == "ServerStatusCurrentTime")
            {
                LastTimeOPCServerFoundAlive = value.SourceTimestamp.ToLocalTime();
            }
            else
            {
                if (value.Value != null)
                {
                    string info = $"{item.DisplayName}: {value.Value.ToString()}, {value.SourceTimestamp.ToLocalTime()}, {value.StatusCode}";
                    Console.WriteLine(info);
                    System.Diagnostics.Debug.WriteLine(info);
                }
                else
                    Console.WriteLine($"{item.DisplayName}: Null Value, {value.SourceTimestamp}, {value.StatusCode}");

                // TODO: refactor to use dictionary
                if (TagList.ContainsKey(item.DisplayName))
                {
                    if (value.Value != null)
                    {
                        TagList[item.DisplayName].LastGoodValue = value.Value.ToString();
                        TagList[item.DisplayName].CurrentValue = value.Value.ToString();
                        TagList[item.DisplayName].LastUpdatedTime = DateTime.Now;
                        TagList[item.DisplayName].LastSourceTimeStamp = value.SourceTimestamp.ToLocalTime();
                        TagList[item.DisplayName].StatusCode = value.StatusCode.ToString();
                    }
                    else
                    {
                        TagList[item.DisplayName].StatusCode = value.StatusCode.ToString();
                        TagList[item.DisplayName].CurrentValue = null;
                    }
                }
            }
        }
        InitialisationCompleted = true;
    }
}
