using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace portKnockingServer
{
    class Options
    {
        [Option('p', "port", Required = true, HelpText = "Ports sequence", Min = 2)]
        public IEnumerable<string> Ports { get; set; }

        [Option('e', "exec",
        Required = true,
        HelpText = "command to execute after the knock")]
        public string exec { get; set; }

        [Option('t', "timeout",
        Required = true,
        HelpText = "sequence timeout")]
        public int seq_timeout { get; set; }


        [Option('i', "interface",
        Required = false,
        HelpText = "interface name")]
        public string Interface { get; set; }

        [Option('l', "list",
        Required = false,
        HelpText = "list all availaible interfaces", Group = "hello")]
        public bool listInterfaces { get; set; }

        [Option("protocol",
          Default = "tcp",
          HelpText = "tcp or udp", Group = "hello")]
        public string protocol { get; set; }
    }
}
