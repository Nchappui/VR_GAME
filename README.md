# VR_GAME

## How to launch the game

Once the computer is turned on, you must also turn on:
1. the VR headset along with the base stations
2. The treadmill

Three applications must be turned on:<br/>
1. Steam VR (this should look like that)<br/>
  ![image](https://user-images.githubusercontent.com/33152136/172951295-5fc0d69f-86b7-47c2-944b-61868b09d72e.png)<br/>
2. VIVE Wireless (this should look like that)<br/>
![screen1](https://user-images.githubusercontent.com/33152136/172950907-e49f7c82-0cac-44f1-83dc-4e0b52dde3ea.png)<br/>


3. Infinadeck application: <br/>
4.  ![image](https://user-images.githubusercontent.com/33152136/173403895-8865c71b-8642-445b-a3be-fa9a7d0c058a.png)

Once everything is turned on, you need to open Unity Hub and select VR GAME
![image](https://user-images.githubusercontent.com/33152136/172952063-af18fe1a-1f4e-4798-8809-da8566beb549.png)

Then go to Scenes and The Escape V.2:

![image](https://user-images.githubusercontent.com/33152136/172952145-5d96dd0a-d1bb-4936-83b0-597f5e3508b1.png)

Now Uniy is set up and the only things you need to do is to have the player on the treadmill, attach him to the ceilling using the harness and turn on one of the captors in his back (the light of the captor should turn green) and make sure that it is detected in the InfinaDeck application, to do so, in the Infinadeck application, you must go to settings->select user tracker->press user tracker(an error should appear, just dismiss it), then go back to the "run" tab on the InfinaDeck application and it should look like that:

![image](https://user-images.githubusercontent.com/33152136/175543867-c4f0b799-d8d8-4bd4-9d8e-c4b06437bb1d.png)


Once everything is done, you can click start on the InfinaDeck application and hit play on Unity

If the player is moving on the treadmill but his chararcter isn't moving in game, you can restart all the softwares mentionned above and retry.


## To launch the OpenWorld directly
Instead of opening the scene The Escape V.2, open the scene OpenWorld: 

![image](https://user-images.githubusercontent.com/33152136/173404411-ad76778e-5ad4-4b5e-8a61-9f7798ff75e8.png)

Make sur the position of the physical_player is Vector3(0.624000013,1.449,-2.03299999), to do so, just click on the physical_player component on the left and you can see his position on the right:

![image](https://user-images.githubusercontent.com/33152136/175544570-2061e78d-186e-413e-8f00-74a13e0a26a8.png)

## To skip directly to the last part of the last level

Open the scene OpenWorld like previously but set the position of the physical player to: Vector3(-5.68900013,1.449,34.7439995) (see image below)

![image](https://user-images.githubusercontent.com/33152136/175545253-edba5df3-ef54-4d3b-8b2f-d347aae99fc6.png)


## Video of the entire game

https://www.youtube.com/watch?v=JJMp278OOT0
