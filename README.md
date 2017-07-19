# X32Client
A wrapper around the OSC protocol used to interact with the Behringer X32 family of digital mixers.

My goal is to abstract the osc messages and udp networking away, allowing development to be focus on features of the mixer.

For details about the Behringer OSC protocal, check out: 
http://behringerwiki.music-group.com/index.php?title=OSC_Remote_Protocol

## Supported Features
Currently only supports the "configuration" and "gate" commands listed in 
http://behringerwiki.music-group.com/index.php?title=Channel_(/ch)_data

Note: Testing has been pretty light. There are a couple fields that don't seem to be returning appropriate values.

## Next Steps
My first focus is to complete all /ch related commands including properties for: compressor, insert settings, EQ, mix, DCA and Mute groups

Also looking to update how various settings are defined. For example Gate Thresholds are represented as a value from 0.0 to 1.0 in relation to the dial range. I would like to represent this in the appropriate decible range ie. -80dB to 0db (unity). Please leave some comments if you think that would be worthwhile.

One day I may work on an asynchronous version but currently I'm more focused on making the library intuitive to use by all developers.

## Usage

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

