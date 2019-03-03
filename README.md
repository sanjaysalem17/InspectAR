# inspectAR
##How it Works
*InspectAR is an AR mobile app that uses Vuforia's image tracking, Unity's engine, and Firebase to show you analytics of various devices in your home without having to activate those devices at all. We used Firebase as a database for all registered device information and used unique QR links to get to each device's information. This information is viewable on an app we built with Android Studio, and uses Augmented Reality to display the information.

##Challenges
*We were more limited by time than expected. We were unable to accurately create 3-D models for everyone's personal devices using Vuforia's Model Creator app. Instead we created QR code placeholders to identify devices. Certain aesthetic features such as thermostat models were not properly importable from Blender to Unity as well as we'd hoped. Unity's database access functions often resulted in segfaults, which limited development for longer than we thought.

##What We Learned
*The Vuforia AR APIs are well implemented into Unity. However, this ease of access is traded off by a certain inflexibility of a finished product. Connecting databases and trying to reprocess information in the AR apps took much more effort than expected. In addition, Unity does not utilize Cycles Render (a render engine for Blender), so adding models' materials became more difficult than expected.

##Future Steps
*The first step is to add a better UI to the AR info display from proximity based labels to a interactive selection function, to create a truly immersive experience. We plan to update the image recognition of the AR from QR codes to model recognition. This step would also include creating a 'registration' function where users can scan their devices into the database.

##Contributors
* [Ian Kuo] - Back-End Firebase server, data acquisition, Front-end data display
* [Brian Hu] - Back-End Firebase server, data synchronization, data forwarding
* [Jeff Kim] - Front-End Unity/Vuforia data display, Android Studio mobile app building
* [Sanjay Salem] - Front-End Unity/Vuforia data display, UI/graphics, data display, project lead
