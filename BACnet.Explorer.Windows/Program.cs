using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BACnet.Explorer.Core;

namespace BACnet.Explorer.Windows
{
    public static class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            foreach(var dll in Directory.EnumerateFiles(Environment.CurrentDirectory, "*.dll", SearchOption.TopDirectoryOnly))
            {
                Assembly.LoadFile(dll);
            }

            var app = App.Create(Eto.Platforms.Wpf);
            app.Platform.Add<Eto.OxyPlot.Plot.IHandler>(() => new Eto.OxyPlot.Wpf.PlotHandler());

            app.Run();

            var session = BACnetSession.Current;
            if (session != null)
            {
                session.Dispose();
                BACnetSession.Current = null;
            }

            app.Dispose();

        }
    }
}
