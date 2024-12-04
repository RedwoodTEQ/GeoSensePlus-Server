using Opc.Ua;   // Install-Package OPCFoundation.NetStandard.Opc.Ua
using Opc.Ua.Client;
using Opc.Ua.Configuration;

namespace GeoSensePlus.OpcUa.WinForm;

public class OpcUaServerConnector
{
    public string ServerAddress { get; set; }
    public string ServerPortNumber { get; set; }
    public bool SecurityEnabled { get; set; }
    public string MyApplicationName { get; set; }
    public Session OpcUaSession { get; set; }
    public string OpcUaNameSpace { get; set; }
    public Dictionary<string, TagObject> TagList { get; set; }

    public bool SessionRenewalRequired { get; set; }
    public double SessionRenewalPeriodMins { get; set; }
    public DateTime LastTimeSessionRenewed { get; set; }
    public DateTime LastTimeOPCServerFoundAlive { get; set; }
    public bool ClassDisposing { get; set; }
    public bool InitialisationCompleted { get; set; }
    private Thread RenewerThread { get; set; }
    public OpcUaServerConnector(string serverAddres, string serverport, Dictionary<string, TagObject> taglist, bool sessionrenewalRequired, double sessionRenewalMinutes, string nameSpace)
    {
        ServerAddress = serverAddres;
        ServerPortNumber = serverport;
        MyApplicationName = "MyApplication";
        TagList = taglist;
        SessionRenewalRequired = sessionrenewalRequired;
        SessionRenewalPeriodMins = sessionRenewalMinutes;
        OpcUaNameSpace = nameSpace;
        LastTimeOPCServerFoundAlive = DateTime.Now;
        InitializeOPCUAClient();

        if (SessionRenewalRequired)
        {
            LastTimeSessionRenewed = DateTime.Now;
            RenewerThread = new Thread(renewSessionThread);
            RenewerThread.Start();
        }
    }

    //class destructor
    ~OpcUaServerConnector()
    {
        ClassDisposing = true;
        try
        {
            OpcUaSession.Close();
            OpcUaSession.Dispose();
            OpcUaSession = null;
            RenewerThread.Abort();
        }
        catch { }
    }

    private void renewSessionThread()
    {
        while (!ClassDisposing)
        {
            if ((DateTime.Now - LastTimeSessionRenewed).TotalMinutes > SessionRenewalPeriodMins
                || (DateTime.Now - LastTimeOPCServerFoundAlive).TotalSeconds > 60)
            {
                Console.WriteLine("Renewing Session");
                try
                {
                    OpcUaSession.Close();
                    OpcUaSession.Dispose();
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
        var config = new Opc.Ua.ApplicationConfiguration()
        {
            ApplicationName = MyApplicationName,
            ApplicationUri = Utils.Format(@"urn:{0}:" + MyApplicationName + "", ServerAddress),
            ApplicationType = ApplicationType.Client,
            SecurityConfiguration = new SecurityConfiguration
            {
                ApplicationCertificate = new CertificateIdentifier { StoreType = @"Directory", StorePath = @"%CommonApplicationData%\OPC Foundation\CertificateStores\MachineDefault", SubjectName = Utils.Format(@"CN={0}, DC={1}", MyApplicationName, ServerAddress) },
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
            config.CertificateValidator.CertificateValidation += (s, e) => { e.Accept = (e.Error.StatusCode == StatusCodes.BadCertificateUntrusted); };
        }

        var application = new ApplicationInstance
        {
            ApplicationName = MyApplicationName,
            ApplicationType = ApplicationType.Client,
            ApplicationConfiguration = config
        };
        application.CheckApplicationInstanceCertificate(false, 2048).GetAwaiter().GetResult();


        //string serverAddress = Dns.GetHostName();
        string serverAddress = ServerAddress;
        var selectedEndpoint = CoreClientUtils.SelectEndpoint("opc.tcp://" + serverAddress + ":" + ServerPortNumber + "", useSecurity: SecurityEnabled, discoverTimeout: 15000);

        // Console.WriteLine($"Step 2 - Create a session with your server: {selectedEndpoint.EndpointUrl} ");
        OpcUaSession = Session.Create(config, new ConfiguredEndpoint(null, selectedEndpoint, EndpointConfiguration.Create(config)), false, "", 60000, null, null).GetAwaiter().GetResult();
        {
            //Console.WriteLine("Step 4 - Create a subscription. Set a faster publishing interval if you wish.");
            var subscription = new Subscription(OpcUaSession.DefaultSubscription) { PublishingInterval = 1000 };

            //Console.WriteLine("Step 5 - Add a list of items you wish to monitor to the subscription.");
            var list = new List<MonitoredItem> { };
            //list.Add(new MonitoredItem(subscription.DefaultItem) { DisplayName = "M0404.CPU945.iBatchOutput", StartNodeId = "ns=2;s=M0404.CPU945.iBatchOutput" });

            list.Add(new MonitoredItem(subscription.DefaultItem) { DisplayName = "ServerStatusCurrentTime", StartNodeId = "i=2258" });

            foreach (KeyValuePair<string, TagObject> td in TagList)
            {
                list.Add(new MonitoredItem(subscription.DefaultItem) { DisplayName = td.Value.DisplayName, StartNodeId = "ns=" + OpcUaNameSpace + ";s=" + td.Value.NodeID + "" });
            }

            list.ForEach(i => i.Notification += OnTagValueChange);
            subscription.AddItems(list);

            //Console.WriteLine("Step 6 - Add the subscription to the session.");
            OpcUaSession.AddSubscription(subscription);
            subscription.Create();
        }
    }

    public void OnTagValueChange(MonitoredItem item, MonitoredItemNotificationEventArgs e)
    {

        foreach (var value in item.DequeueValues())
        {

            if (item.DisplayName == "ServerStatusCurrentTime")
            {
                LastTimeOPCServerFoundAlive = value.SourceTimestamp.ToLocalTime();
            }
            else
            {
                if (value.Value != null)
                    Console.WriteLine("{0}: {1}, {2}, {3}", item.DisplayName, value.Value.ToString(), value.SourceTimestamp.ToLocalTime(), value.StatusCode);
                else
                    Console.WriteLine("{0}: {1}, {2}, {3}", item.DisplayName, "Null Value", value.SourceTimestamp, value.StatusCode);

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
