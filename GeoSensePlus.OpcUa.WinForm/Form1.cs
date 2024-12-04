using System;
using System.Collections.Generic;
using System.Windows.Forms;

using System.Threading;

namespace GeoSensePlus.OpcUa.WinForm;

public partial class Form1 : Form
{
    //creating a object that encapsulates the entire OPC UA Server related work
    OPCUAClass myOPCUAServer;

    //creating a dictionary of Tags that would be captured from the OPC UA Server
    Dictionary<String, TagClass> TagList = new Dictionary<String, TagClass>();

    public Form1()
    {
        InitializeComponent();


        //Add tags to the Tag List, For each tag, you have to define the name of the tag and its address
        //the address can typically be found by browsing the OPC UA Server's tree. In the example below
        // The OPC Server had the following hierarchy: M0401 -> CPU945 -> IBatchOutput
        //i used TBC0401 as a name of the tag, you can use any name
        //add as many tags as you want to capture
        TagList.Add("TBC0401", new TagClass("TBC0401", "M0401.CPU945.iBatchOutput"));

        //to initialize the OPC UA Server, provide the IP Address, Port Number, the list of tags you want to capture
        //in some OPC UA servers and kepware aswell the session can be closed by the OPC UA Server, so its better to 
        //allow the class to reinitiate session periodically, before renewing current sessions are closed
        myOPCUAServer = new OPCUAClass("127.0.0.1", "49320", TagList, true, 1, "2");


        //once the OPC Server has been initialized, you can easily read Tag values and even see when they were
        // updated last time
        //as an example i could read the TBC0401 tag by:

        var tagCurrentValue = TagList["TBC0401"].CurrentValue;
        var tagLastGoodValue = TagList["TBC0401"].LastGoodValue;
        var lastTimeTagupdated = TagList["TBC0401"].LastUpdatedTime;

    }
}
