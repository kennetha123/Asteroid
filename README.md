# Asteroid Game
This is Asteroid classic game, to run the game I already prepared 2 ways,
by using Unity Project or directly use binaries to start the game.

If you want to Build the game, make sure you have 2019.4 LTS version. (I use 2019.4.9f1)

If you want to play the game binary, use the one in folder "bin" and extract the zip file into your respective folder.

You can watch the gameplay by accessing Video folder.

For full explanation of this game, you can accessing this Google Slide : https://docs.google.com/presentation/d/1RBlYMmTFjuPQbY1ScqARlAp8A2swvey_BKf4KVQYKy8/edit?usp=sharing

A little summary from this game :

##Challenge :

- Mono Singleton pattern are not really suited with ECS structure use, even I already edit Execution Order it didn’t budge.

- Destroying Object from Laser without checking Object type / component one by one. 
   I tackled it by creating Entity object.

##Design Pattern :

- Object pooling for Asteroid spawning performance.

- Singleton Pattern without creating / checking Instance while it gone.

##Decision :

- I didn’t used Observer pattern because want to experimenting with ECS design.
To use data structure as an object to called is much more easier to used.

Thank you and Enjoy!
