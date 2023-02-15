using GeoSensePlus.App.AssetTracking;
using GeoSensePlus.App.AssetTracking.Handlers;
using GeoSensePlus.App.AssetTracking.MessageProcessors;
using GeoSensePlus.App.AssetTracking.Messages;
using GeoSensePlus.App.ProgressTracking.Messages;
using GeoSensePlus.Core.Codec;
using GeoSensePlus.Core.CommandProcessing.MessageHandlers;
using GeoSensePlus.Core.MessageProcessing;
using GeoSensePlus.Core.UnitTest.TestEnv;
using System;
using System.Collections.Generic;
using Xunit;

namespace GeoSensePlus.Core.UnitTest
{
    class TestChannelContext : ChannelContext<string> { }

    public class MesssageHandlerTests : IClassFixture<GlobalUtilFixture>
    {
        GlobalUtilFixture _util;

        public MesssageHandlerTests(GlobalUtilFixture util) => _util = util;

        [Fact]
        public void Test_IndoorArrivalHandler()
        {
            var ctx = new TestChannelContext();
            new IndoorArrivalHandler().Handle(@"
                {
                   timestamp: '2019-06-19T17:33:22Z',
                   user: {
                       id: 'abcd_1234', 
                       props: {
                           battery: 0.81,
                           device_type: 1
                       }
                   },
                   tag: {
                       id: 'efgh_5678', 
                       props: {
                           signal: 122,
                           battery: 0.93
                       }
                   },
                   additional_info: 'xyz'
                }", ctx);

            var data = (IndoorArrivalMessage)ctx.MessageObject;

            Assert.NotNull(data);
            Assert.NotNull(ctx.RawMessage);

            Assert.Equal(DateTime.Parse("2019-06-19T17:33:22Z"), data.TimeStamp);
            Assert.Equal("abcd_1234", data.UserDeviceId);
            Assert.Equal(0.81, data.UserDeviceBatteryLevel);
            Assert.Equal(IndoorArrivalMessage.UserDeviceTypes.ePhoneApp, data.UserDeviceType);

            Assert.Equal("efgh_5678", data.TagId);
            Assert.Equal(122, data.TagSignal);
            Assert.Equal(0.93, data.TagBatteryLevel);
        }

        class AssetReportServiceMock : IMessageExecutor<IndoorAssetReportMessage>
        {
            public void Execute(IndoorAssetReportMessage message)
            { }
        }

        [Fact]
        public void IndoorAssetReportHandler_WithValidPayload()
        {
            var payloadDecoder = _util.GetService<IPayloadDecoder<List<IndoorTagPayloadInfo>>>();
            var ctx = new TestChannelContext();
            new IndoorAssetReportHandler(payloadDecoder, new AssetReportServiceMock()).Handle(@"
                {
                  'app_id': 'my-app-id',
                  'dev_id': 'my-dev-id',
                  'hardware_serial': '0102030405060708',
                  'port': 1,
                  'counter': 2,
                  'is_retry': false,
                  'confirmed': false,
                  'payload_raw': 'ff010100820002611918FC01F04DC7C51918FC01F242B9C5',
                  'payload_fields': {},
                  'metadata': {
                    'time': '1970-01-01T02:13:16Z',
                    'frequency': 868.1,
                    'modulation': 'LORA',
                    'data_rate': 'SF7BW125',
                    'bit_rate': 50000,
                    'coding_rate': '4/5',
                    'gateways': [
                      {
                        'gtw_id': 'ttn-herengracht-ams',
                        'timestamp': 12345,
                        'time': '1970-01-01T01:02:10Z',
                        'channel': 0,
                        'rssi': -25,
                        'snr': 5,
                        'rf_chain': 0,
                        'latitude': 52.1234,
                        'longitude': 6.1234,
                        'altitude': 6
                      },
                    ],
                    'latitude': 52.2345,              // Latitude of the device
                    'longitude': 6.2345,              // Longitude of the device
                    'altitude': 2                     // Altitude of the device
                  },
                  'downlink_url': 'https://integrations.thethingsnetwork.org/ttn-eu/api/v2/down/my-app-id/my-process-id?key=ttn-account-v2.secret'
                }", ctx);

            var data = (IndoorAssetReportMessage)ctx.MessageObject;

            Assert.NotNull(data);
            Assert.NotNull(ctx.RawMessage);

            Assert.Equal(DateTime.Parse("1970-01-01T02:13:16Z"), data.TimeStamp);
            Assert.Equal("0102030405060708", data.HardwareSerial);

            Assert.Equal(2, data.IndoorTagPayloadInfo.Count);

            Assert.Equal("1918FC01F04D", data.IndoorTagPayloadInfo[0].MacAddress);
            Assert.NotEqual("1918fc01f04d", data.IndoorTagPayloadInfo[0].MacAddress);    // must be upper cases
            Assert.Equal(new BinaryDecoder("c7").DecodeNextInt(2), data.IndoorTagPayloadInfo[0].Rss);
            Assert.Equal(new BinaryDecoder("c5").DecodeNextInt(2), data.IndoorTagPayloadInfo[0].BatteryLevel);

            Assert.Equal("1918FC01F242", data.IndoorTagPayloadInfo[1].MacAddress);
            Assert.Equal(new BinaryDecoder("b9").DecodeNextInt(2), data.IndoorTagPayloadInfo[1].Rss);
            Assert.Equal(new BinaryDecoder("c5").DecodeNextInt(2), data.IndoorTagPayloadInfo[1].BatteryLevel);
        }

        [Fact]
        public void IndoorAssetReportHandler_WithInvalidPayload()
        {
            var payloadDecoder = _util.GetService<IPayloadDecoder<List<IndoorTagPayloadInfo>>>();
            var ctx = new TestChannelContext();
            IndoorAssetReportHandler handler = new IndoorAssetReportHandler(payloadDecoder, new AssetReportServiceMock());
            // use invalid 'payload_raw': '1918FC01F04DC7C51918FC01F242B9C5FFFFFFFFFFFF006',
            handler.Handle(@"
                {
                  'app_id': 'my-app-id',
                  'dev_id': 'my-dev-id',
                  'hardware_serial': '0102030405060708',
                  'port': 1,
                  'counter': 2,
                  'is_retry': false,
                  'confirmed': false,
                  'payload_raw': '1918FC01F04DC7C51918FC01F242B9C5FFFFFFFFFFFF006',
                  'payload_fields': {},
                  'metadata': {
                    'time': '1970-01-01T02:13:16Z',
                    'frequency': 868.1,
                    'modulation': 'LORA',
                    'data_rate': 'SF7BW125',
                    'bit_rate': 50000,
                    'coding_rate': '4/5',
                    'gateways': [],
                    'latitude': 52.2345,              // Latitude of the device
                    'longitude': 6.2345,              // Longitude of the device
                    'altitude': 2                     // Altitude of the device
                  },
                  'downlink_url': 'https://integrations.thethingsnetwork.org/ttn-eu/api/v2/down/my-app-id/my-process-id?key=ttn-account-v2.secret'
                }", ctx);

            Assert.Null(ctx.MessageObject);
            Assert.NotNull(ctx.RawMessage);
        }
    }
}
