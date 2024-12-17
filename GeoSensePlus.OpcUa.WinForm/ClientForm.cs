using System;
using System.Collections.Generic;
using System.Windows.Forms;

using System.Threading;

namespace GeoSensePlus.OpcUa.WinForm;

public partial class ClientForm : Form
{
    //creating a object that encapsulates the entire OPC UA Server related work
    OpcUaServerConnector myOPCUAServer;

    //creating a dictionary of Tags that would be captured from the OPC UA Server
    Dictionary<String, TagObject> TagList = new Dictionary<String, TagObject>();

    public ClientForm()
    {
        InitializeComponent();


        TagList.Add("TBC0401", new TagObject("RLTest2", "M0401.CPU945.iBatchOutput"));

        myOPCUAServer = new OpcUaServerConnector("opc.tcp://127.0.0.1:53530/OPCUA/SimulationServer", TagList, true, 1, "3");


        //once the OPC Server has been initialized, you can easily read Tag values and even see when they were
        // updated last time
        //as an example i could read the TBC0401 tag by:

        var tagCurrentValue = TagList["TBC0401"].CurrentValue;
        var tagLastGoodValue = TagList["TBC0401"].LastGoodValue;
        var lastTimeTagupdated = TagList["TBC0401"].LastUpdatedTime;
    }
}
