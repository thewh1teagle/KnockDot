# KnockDot
Port knocking server in .NET especially for Windows


require .NET 5 runtime (windows 10 and above contains it by default)

## usage
```
C:\> KnockDot.exe --help
KnockDot 1.0.0
Copyright (C) 2021 KnockDot

  -p, --port                 Required. Ports sequence

  -e, --exec                 Required. command to execute after the knock

  -t, --timeout              Required. sequence timeout

  -i, --interface, number    interface name (number), get all interfaces by -l option

  --protocol                 (Default: tcp) tcp or udp

  -l, --list                 Required. list all availaible interfaces

  --help                     Display this help screen.

  --version                  Display version information.

```
## tested on windows 10 with [android portKnocker app](https://play.google.com/store/apps/details?id=com.xargsgrep.portknocker)
