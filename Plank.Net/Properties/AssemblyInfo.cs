using log4net.Config;
using System.Reflection;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Plank.Net")]
[assembly: AssemblyDescription("A framework for quickly building Api's using .NET")]
[assembly: AssemblyCompany("BiterSoftware")]
[assembly: AssemblyProduct("Plank.Net")]
[assembly: AssemblyCopyright("Copyright ©  2018")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("0285aeb8-31f5-49f5-8260-9363168f65f7")]

// log4net
[assembly: XmlConfigurator(ConfigFile = "plank.net.log4net.config")]

// Version
[assembly: AssemblyVersion("0.0.*")]