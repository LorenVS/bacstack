using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Explorer.Core.Controls;

namespace BACnet.Explorer.Core
{
    public static class App
    {
        public static Eto.Forms.Application Create(string platform)
        {
            var app = new Eto.Forms.Application(platform);
            
            app.Initialized += delegate
            {
                // only create new controls/forms/etc during or after this event
                app.MainForm = new MainForm();
                app.MainForm.Show();
            };

            return app;
        }
    }
}
