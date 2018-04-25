# UFOpad
LED light controller for the Dynatrace UFO made in Unity 3D.

Intended to run on an iOS device in Hotspot mode with the UFO configured to connect to the hotspot as the second client.

In this case the UFO's IP address will be 172.20.10.2. If this isn't the case for you, look at changing the UFO IP in Unity.

The app itself loads up a circle comprised of 15 triangles, each representing one of the 15 LED sections on the UFO. Touching and dragging will change the color of the selected regions to the next color in the Paint Colors list set in Unity.

The logo colors are set to the average color of the nearest LEDs.
