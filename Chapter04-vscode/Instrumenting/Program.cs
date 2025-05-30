﻿using System.Diagnostics; // To use Debug and Trace
using System.Text;
using Microsoft.Extensions.Configuration; // To use ConfigurationBuilder.

string logPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "log.txt");

Console.WriteLine($"Writing to: {logPath}");

FileStream logStream = File.OpenWrite(logPath);
logStream.Seek(0, SeekOrigin.End);
//logStream.Write(Encoding.ASCII.GetBytes(Environment.NewLine));

TextWriterTraceListener logFile = new(logStream);


Trace.Listeners.Add(logFile);

#if DEBUG
// Text writer is buffered, so this option calls Flush() on all listeners after writing.
Trace.AutoFlush = true;
#endif

Debug.WriteLine("Debug says, I am watching!");
Trace.WriteLine("Trace says, I am watching!");

string settingsFile = "appsettings.json";
string settingsPath = Path.Combine(Directory.GetCurrentDirectory(), settingsFile);

Console.WriteLine("Processing: {0}", settingsFile);

Console.WriteLine("---{0} contents--", settingsFile);
Console.WriteLine(File.ReadAllText(settingsPath));
Console.WriteLine("----");

ConfigurationBuilder builder = new();
builder.SetBasePath(Directory.GetCurrentDirectory());

// Add the settings file to the processed configuration and make it mandatory
// so an exccceptionwill be thrown if the file is not found.
builder.AddJsonFile(settingsFile, optional: false, reloadOnChange: true);

IConfigurationRoot configuration = builder.Build();

TraceSwitch ts = new(
    displayName: "PacketSwitch",
    description: "This switch is set via a JSON config");

configuration.GetSection("PacktSwitch").Bind(ts);

Console.WriteLine($"Trace switch value: {ts.Value}");
Console.WriteLine($"Trace switch level: {ts.Level}");

Trace.WriteLineIf(ts.TraceError, "Trace error");
Trace.WriteLineIf(ts.TraceWarning, "Trace warning");
Trace.WriteLineIf(ts.TraceInfo, "Trace Information");
Trace.WriteLineIf(ts.TraceVerbose, "Trace verbose");

int unitsInStock = 12;
LogSourceDetails(unitsInStock > 10);

// Close the text file (also flushes) and release resources
Debug.Close();
Trace.Close();

Console.WriteLine("Press enter to exit.");
Console.ReadLine();