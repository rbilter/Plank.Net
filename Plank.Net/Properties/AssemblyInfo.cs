using log4net.Config;
using System.Reflection;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Plank.Net")]

// log4net 
[assembly: XmlConfigurator(ConfigFile = "plank.net.log4net.config", Watch = true)]
