using CommandLine;
using System.Collections.Generic;

namespace portKnockingServer
{
    class Options
    {
        
        [Option('p', "port", Required = true, HelpText = "Ports sequence", Min = 2, SetName = "config")]
        public IEnumerable<string> Ports { get; set; }

        [Option('e', "exec",
        Required = true,
        HelpText = "command to execute after the knock", SetName = "config")]
        public string Exec { get; set; }

        [Option('t', "timeout",
        Required = true,
        HelpText = "sequence timeout", SetName= "config")]
        public int SeqTimeout { get; set; }


        [Option('i', "interface, number",
        Required = false,
        HelpText = "interface name (number), get all interfaces by -l option", SetName = "config")]
        public int InterfaceIdx { get; set; }


        [Option("protocol",
          Required =false,
          Default = "tcp",
          HelpText = "tcp or udp", SetName = "config")]
        public string protocol { get; set; }

        [Option('l', "list",
        Required = true,
        HelpText = "list all availaible interfaces", SetName = "help")]
        public bool ListInterfaces { get; set; }


    }
}
