# X32Client
A wrapper around the OSC protocol used to interact with the Behringer X32 family of digital mixers.

Usage:

```cs

var x32Client = new X32Client("192.168.0.109"); //Automatically defaults to port 10023

//Set a timeout. There is currently an issue where the first time an app connects to the X32 
//it may hang on listening to the response. This makes sure you at least get a timeout semi quickly;
x32Client.ReceiveTimeout = 100;

//Get an object representing channel 1
var chan1 = x32Client.GetChannel(1);

//Getters lazy load the properties from the mixer. The underlying UDP calls are synchronous and blocking
Console.WriteLine($"Channel 1: - {chan1.Name} | {chan1.Color.GetName()} | {chan1.Source.GetName()}");

//Setters auto send OSC command to update the console
chan1.Name = "Hello";
chan1.Color = X32Color.InvertedRed;
chan1.Icon = 3;
chan1.Source = X32Source.In02;

```

