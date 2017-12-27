# FMV Event Baker

Adds a simple window to Unity allowing you to bake method names with parameters into video clips. This data is then is invoked at 
run time if possible.

## Getting Started
This project provides a sample set up with a video already provided in the scene. 
There is also a package called VideoEventBaker.package
which allows you to import the assets into your own project.
### Prerequisites

Requires Unity 5.6.0f or above for new Video player support.
## Primer

### Implementation Flow

Add a Video Player to the scene.
Add a Video Clip to the Video Player.
A VideoClipEventController component is added to the Video Player object 
(this component is hidden in editor for neatness)


Once a VideoClip has been added, right-click on the video clip in Assets. 
At the bottom of the list will be an option called "Edit Video".
Selecting this option will open up a Video Editor Window for attaching events.

The top slider represents the current point in the videos progression when it is playing.
The bottom slider represents at what point in time you would want to add an event.

The "Add Event" button is there to reveal fields that can be changed to add an event.

You can provide an event name, 
the time of the event in the video, and a parameter consisting of float, int, string or object.

Any methods added to a video clip need to be present on the object the video clip is playing from.
This includes children objects.

This allows you have a component that controls audio with a method called PlaySound() and
another component that spawns objects with a method called CreateLamp(int).

This can be added with their necessary data to the clip as long as the video player with the clip in on
the same Game Object or a parent Game Object.

## Built With

* [Unity3d](https://unity3d.com/) - Game Engine

## Authors

* **Tim Flusk** - [VQZX](https://github.com/VQZX)

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details

