Sorry I couldn't make the meeting today, but you can play around with what I've thrown together so far in the thielmann_test scene. 
The player controller script is on the object labeled "player".

This is using the new unity input system, it's a little less intuitive than the old one to get set up but I think
if we want to change the control scheme later it should hopefully make that easier to do without having to dig into the code.

Controls:
WASD for activating thrusters on the xy plane
Q and E for rotational thrust, counterclockwise and clockwise respectively
SHIFT to hit the brakes, stops both movement and rotation (i chose to make using this a little slower than manually coming to a stop)

Feel free to mess around with the thrust settings in the editor if it feels like the values need tweaking.

There's a toggle on the player controller script to switch from global to relative movement, I still have not decided which 
to use yet. I think relative would be a bit more realistic and would add a bit more of a fun skill ramp, and maybe we would
want to remove or severely weaken the horizontal thrusters as well. I think also if we went the relative thrust route I would probably
switch the "yawing" to be on A and D and have Q and E be the horizontal thrust. I'll probably tool around with this a little more but
I would love to hear people's feedback/ideas. 


Also I created an issue on github for designing/programming the camera. I don't think the player ought to be able to control it
directly, probably just a camera that follows you around. Not sure how much time should be spent on this right now since the camera
behavior depends a little bit on how the level design shakes out, but it's something to think about. 