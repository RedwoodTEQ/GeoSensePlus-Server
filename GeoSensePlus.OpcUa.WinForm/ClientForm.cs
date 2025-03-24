using System;
using System.Collections.Generic;
using System.Windows.Forms;

using System.Threading;

namespace GeoSensePlus.OpcUa.WinForm;

public partial class ClientForm : Form
{
    OpcUaServerConnector myOPCUAServer;

    Dictionary<String, TagObject> TagList = new Dictionary<String, TagObject>();

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        base.OnFormClosing(e);
        if (myOPCUAServer != null)
        {
            myOPCUAServer.Dispose();
        }
    }

    public ClientForm()
    {
        InitializeComponent();

        TagList.Add("TBC0401", new TagObject("RLTest2", "M0401.CPU945.iBatchOutput"));

        myOPCUAServer = new OpcUaServerConnector("opc.tcp://127.0.0.1:53530/OPCUA/SimulationServer", TagList, true, 1, "3");

        var tagCurrentValue = TagList["TBC0401"].CurrentValue;
        var tagLastGoodValue = TagList["TBC0401"].LastGoodValue;
        var lastTimeTagupdated = TagList["TBC0401"].LastUpdatedTime;
    }
}
