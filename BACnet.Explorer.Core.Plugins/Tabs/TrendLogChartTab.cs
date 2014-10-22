using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eto;
using Eto.Drawing;
using Eto.Forms;
using Eto.OxyPlot;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using BACnet.Ashrae;
using BACnet.Ashrae.Objects;
using BACnet.Core.App;
using BACnet.Client;
using BACnet.Client.Descriptors;
using BACnet.Types;
using BACnet.Explorer.Core.Controls;
using BACnet.Explorer.Core.Extensibility;

namespace BACnet.Explorer.Core.Plugins.Tabs
{
    public class TrendLogChartTab : IObjectTab
    {
        /// <summary>
        /// The order in which the tab should appear
        /// </summary>
        public int Order
        {
            get { return 5; }
        }

        /// <summary>
        /// Determines whether this page should be created for an object
        /// </summary>
        /// <param name="objectInfo">The object info</param>
        /// <returns>True if the page should be created, false otherwise</returns>
        public bool Active(ObjectInfo objectInfo)
        {
            return objectInfo.ObjectIdentifier.Type == (ushort)ObjectType.TrendLog;
        }

        /// <summary>
        /// Creates the tab page
        /// </summary>
        /// <param name="objectInfo">The object info to create the tab for</param>
        /// <returns>The tab page instance</returns>
        public TabPage Create(ObjectInfo objectInfo)
        {
            var page = new TabPage();
            page.Text = Constants.TrendLogChartTabText;
            page.Content = new Panel(objectInfo);
            return page;
        }

        private enum TrendType
        {
            Real,
            Binary,
            Unknown
        }

        public class Panel : BACnetPanel
        {
            /// <summary>
            /// The object into that the panel is displaying
            /// information for
            /// </summary>
            private ObjectInfo _info;

            private Eto.OxyPlot.Plot _plot;

            public Panel(ObjectInfo info)
            {
                this._info = info;
                this._plot = new Plot();
                this.Content = this._plot;
                this.Padding = new Padding(10);
            }

            /// <summary>
            /// Determines the trend type for a trend log based
            /// on the object that it is monitoring
            /// </summary>
            /// <param name="reference">The reference to the monitored object</param>
            /// <returns>The trend type</returns>
            private TrendType _getTrendType(DeviceObjectPropertyReference reference)
            {
                switch ((ObjectType)reference.ObjectIdentifier.Type)
                {
                    case ObjectType.AnalogInput:
                    case ObjectType.AnalogOutput:
                    case ObjectType.AnalogValue:
                        return TrendType.Real;
                    case ObjectType.BinaryInput:
                    case ObjectType.BinaryOutput:
                    case ObjectType.BinaryValue:
                    case ObjectType.Schedule:
                        return TrendType.Binary;
                }
                return TrendType.Unknown;
            }

            /// <summary>
            /// Refreshes the data on the panel
            /// </summary>
            /// <param name="client">The client to use for refreshing</param>
            /// <returns>The asynchronous task</returns>
            public async override Task Refresh(Client.Client client)
            {
                var tl = client.With<ITrendLog>(_info.DeviceInstance, _info.ObjectIdentifier);
                var props = await tl.ReadPropertiesAsync(
                    obj => obj.ObjectName,
                    obj => obj.LogDeviceObjectProperty);
                var name = props.Item1;
                var reference = props.Item2;
                var records = await tl.ReadRangeAsync(obj => obj.LogBuffer);

                if(!reference.HasValue)
                {
                    _plot.Model = null;
                    return;
                }

                var type = _getTrendType(reference.Value);
                if(type == TrendType.Unknown)
                {
                    _plot.Model = null;
                    return;
                }
                else if (records.Length > 0)
                {
                    var minDate = records.Length > 0 ? records[0].Timestamp.ToDateTime() : DateTime.MinValue;
                    var maxDate = records.Length > 0 ? records[records.Length - 1].Timestamp.ToDateTime() : DateTime.MaxValue;
                    var model = this._plot.Model;

                    if (model == null)
                    {
                        model = new PlotModel();
                        model.Title = name;

                        model.Axes.Add(new DateTimeAxis()
                        {
                            Position = AxisPosition.Bottom,
                            IsPanEnabled = true,
                            IsZoomEnabled = true
                        });

                        model.Axes.Add(new LinearAxis()
                        {
                            Position = AxisPosition.Left,
                            IsPanEnabled = false,
                            IsZoomEnabled = false
                        });

                    }

                    var dateAxis = model.Axes.OfType<DateTimeAxis>().First();
                    dateAxis.AbsoluteMinimum = DateTimeAxis.ToDouble(minDate);
                    dateAxis.AbsoluteMaximum = DateTimeAxis.ToDouble(maxDate);

                    model.Series.Clear();

                    if(type == TrendType.Real)
                    {
                        model.Series.Add(new LineSeries()
                        {
                            Color = OxyColors.Red,
                            ItemsSource = records.Where(rec => rec.LogDatum.IsRealValue)
                                .Select(rec => DateTimeAxis.CreateDataPoint(
                                    rec.Timestamp.ToDateTime(),
                                    rec.LogDatum.AsRealValue
                                )).ToList()
                        });
                    }
                    else if(type == TrendType.Binary)
                    {
                        model.Series.Add(new StairStepSeries()
                        {
                            Color = OxyColors.Red,
                            ItemsSource = records.Where(rec => rec.LogDatum.IsEnumValue)
                            .Select(rec => DateTimeAxis.CreateDataPoint(
                                rec.Timestamp.ToDateTime(),
                                rec.LogDatum.AsEnumValue.Value
                            )).ToList()
                        });
                    }


                    _plot.Model = model;

                }

            }
        }
    }
}
