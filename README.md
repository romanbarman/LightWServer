# LightWServer

*LightWServer* is HTTP server for static files. In the current implementation, it supports files with the extension: .gif, .jpeg, .png, .svg, .css, .csv, .html, .js, .pdf, .jpg, .html and .txt.

## Requirements

.NET 6

## Usage


You should provide the root folder with files for *LightWServer*. To get a file, send a **GET** request to ```http://<hostname>/<filename>```. If the file is in a subfolder, you should specify the subfolder in the request ```http://<hostname>/<subfoldername>/<filename>```. Example: ```http://localhost:80/menu/index.html```

### Default configuration

```csharp
using LightWServer.Core;

var serverHost = new ServerBuilder().Build();
await serverHost.RunAsync();
```

Default configuration sets:
1.  Root folder with files - ```www```.
2.  Port - 80.
3.  Log - ```SimpleConsoleLog```. 

### Custom configuration

```csharp
using LightWServer.Core;

var serverHost = new ServerBuilder()
    .SetPort(5001)
    .SetStaticFileRequestHandler("root")
    .SetLogger(new UserLogger())
    .Build();
await serverHost.RunAsync();
```

The ```SetPort``` method sets the port on which the server will run.

The ```SetStaticFileRequestHandler``` method sets the root folder with files.

The ```SetLogger``` method sets a custom logger that implements the ```ILog``` interface.
