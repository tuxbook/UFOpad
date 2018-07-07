# UFOpad
LED light controller for the Dynatrace UFO made in Unity 3D.

Watch the demo to see it in action:

[![UFOpad Demo](https://img.youtube.com/vi/9oTJtqW05AU/0.jpg)](https://www.youtube.com/watch?v=9oTJtqW05AU)

Intended to run on an iOS device in Hotspot mode with the UFO configured to connect to the hotspot as the second client.

In this case the UFO's IP address will be 172.20.10.2. If this isn't the case for you, look at changing the UFO IP in Unity.

The app itself loads up a circle comprised of 15 triangles, each representing one of the 15 LED sections on the UFO. Touching and dragging will change the color of the selected regions to the next color in the Paint Colors list set in Unity.

The logo colors are set to the average color of the nearest LEDs.

Developed in a few hours on a public holiday just for fun.
