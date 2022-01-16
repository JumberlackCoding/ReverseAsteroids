# ReverseAsteroids

*Because being the bad guy is more fun*

**Jumberlack Coding**



## Game Overview
It's the game of asteroids, but instead of playing as the space ship and shooting the asteroids, you get to be the evil mastermind that creates the asteroids and flings them at the space ship.

- ### Target Audiance
    Nobody. Its for practice.
- ### Genre
    Arcade Game
- ### Game Flow Summary
    Main Menu -> Game Play -> Stat Screen
- ### Look and Feel
    2D graphics of basic shapes. Should look and feel like an arcade game. 

## Gameplay

- ### Objectives
    The goal of this game is for the player to hit the AI controlled ship with asteroids. The player will have some limitations on the asteroids it can spawn and possibly have some ways of imporoving its capabilities over the course of the game.
- ### Game Progression
    No progression in the game at the moment. First implementation is just based on killing the AI ship.
- ### Play Flow
    You start right into it. No distinct stages for a flow description to make sense. 
- ### Mission Structure
    No missions in the game at the moment. First implementation is just based on killing the AI ship.
- ### Puzzle Structure
    No puzzels in this game.

## Mechanics

- ### Rules
    There is a circle around the ship inside of which the player cannot spawn asteroids. Outside of this circle the player can spawn asteroids of a limited size at a limited rate. These limitations can change as the game goes on.
- ### Model of the Game Universe
    Just a random place in space
- ### Physics
    Basic 2D kinematics with no friction (space). The ship is able to use brakes (just to make it easier to write AI rules for it). Maybe we take this out later.
    When an asteroid breaks, the pieces will fly off in random directions biased by the current velocity vector of the asteroid. 
- ### Economy
    Economy in this game corresponds to the resources the player has to make asteroids. Maybe this is how we limit the size and number of asteroids that the player can make. Each asteroid costs a certain amount based on its size and speed. So the player can make a bunch of small slow ones, or one big fast one. How these resources are recovered is unknown. Maybe it just recharges over time? Maybe the player has some meta game they are playing to get resources. 
- ### Character Movement
    The player spawns asteroids with a click.
    The AI ship moves by itself.
- ### Objects
    AI ship - The AI controlled ship
    Bullets - The bullets shot from the ship
    Asteroids - The asteroids created by the player
- ### Actions
    Spawn Asteroid
- ### Combat
    No direct player combat. Just make them asteroids
- ### Screen Flow
    Not sure what this means, but the veiw is fixed
- ### Game Options
    TODO
- ### Replaying and Saving
    No save feature. Maybe it would be good to add for practice
- ### Cheats and Easter Eggs
    TODO :)

## Story and Narrative
- ### Back Story
    No Story
- ### Plot Elements
    No Story
- ### Game Story Progression
    No Story
- ### Cut Scenes
    No Story
- ### General Look and Feel of World
    No Story
- ### Plot Elements
    No Story
- ### Game Story Progression
    No Story
- ### Cut Scenes
    No Story
   
## Levels/Scenes

- ### Totorial Level
    Not implemented but may be worth doing for practice
- ### Main Scene
    

## Interface

- ### HUD
    Pavlos is working on a UI design. Will upload an image when done.
- ### Menus
    Main Menu, Settings Menu
- ### Camera Model
    Fixed on Scene
- ### Control Layout
    Clicking to spawn asteroids
    TODO - Other keybinds for different asteroid functions

## Audio Art and Music

- ### Audio
    No sound in space so no sound... (jk)
    Ship thrust sound
    Bullet Fire sound
    Asteroid shatter sound
    Asteroid impact ship sound
- ### Art
    Cool space backgrounds and pixel art for game objects
- ### Music
    Gotta find some fancy royalty free beats
