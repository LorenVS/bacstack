using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BACnet.Explorer.Core.Models
{
    public class SessionsStore
    {
        /// <summary>
        /// The singleton instance of the session store
        /// </summary>
        public static SessionsStore Instance { get { return _instance; } }

        /// <summary>
        /// The singleton instance of the session store
        /// </summary>
        private static SessionsStore _instance = new SessionsStore();

        /// <summary>
        /// The xml serializer to use for sessions
        /// </summary>
        private XmlSerializer _serializer = new XmlSerializer(typeof(Session),
            AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(d => d.GetTypes())
                .Where(t => typeof(Process).IsAssignableFrom(t))
                .ToArray());
        
        /// <summary>
        /// Constructs a new sessions store
        /// </summary>
        private SessionsStore() { }

        /// <summary>
        /// Returns the folder to put session objects in
        /// </summary>
        /// <returns></returns>
        private string _getSessionsFolder()
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var folder = Path.Combine(appData, Constants.SessionsFolder);
            Directory.CreateDirectory(folder);
            return folder;
        }

        /// <summary>
        /// Gets the list of session
        /// </summary>
        /// <returns></returns>
        public List<Session> GetSessions()
        {
            List<Session> ret = new List<Session>();
            var folder = _getSessionsFolder();
            foreach(var file in Directory.EnumerateFiles(folder, "*.xml", SearchOption.TopDirectoryOnly))
            {
                try
                {
                    using (var stream = File.OpenRead(file))
                    {
                        Session session = (Session)_serializer.Deserialize(stream);
                        ret.Add(session);
                    }
                }
                catch
                {
                    File.Delete(file);
                }
            }
            return ret;
        }

        /// <summary>
        /// Saves a session
        /// </summary>
        /// <param name="session">The session to save</param>
        public void SaveSession(Session session)
        {
            var folder = _getSessionsFolder();
            using (var stream = File.OpenWrite(Path.Combine(folder, session.Name + ".xml")))
            {
                _serializer.Serialize(stream, session);
            }
        }
    }
}
