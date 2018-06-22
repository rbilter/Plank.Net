using log4net.Config;
using System.Reflection;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Plank.Net")]
[assembly: AssemblyDescription("A framework for quickly building Api's using .NET")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("BilterSoftware")]
[assembly: AssemblyProduct("Plank.Net")]
[assembly: AssemblyCopyright("Copyright ©  2018")]
[assembly: AssemblyTrademark("")]


// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Revision and Build Numbers 
// by using the '*' as shown below:
[assembly: AssemblyVersion("0.1.0")]
[assembly: AssemblyFileVersion("0.1.0.0")]

// log4net 
[assembly: XmlConfigurator(ConfigFile = "plank.net.log4net.config", Watch = true)]
