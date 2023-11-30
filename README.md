# Project _NAME_

[Markdown Cheatsheet](https://github.com/adam-p/markdown-here/wiki/Markdown-Here-Cheatsheet)

_REPLACE OR REMOVE EVERYTING BETWEEN "\_"_

### Student Info

-   Name: Julia Rissberger
-   Section: 5

## Simulation Design
A fishtank with two different types of fish. One type tends to flock together, and must be fed by the user. The other type tends to avoid
others of its kind, and will occasionally chase away other fish that get too close.

### Controls

-   _List all of the actions the player can have in your simulation_
    -   _Include how to preform each action ( keyboard, mouse, UI Input )_
    -   _Include what impact an action has in the simulation ( if is could be unclear )_

## Small Fish

A small, peaceful fish. It sticks together with others, and must be 
fed by the user.

### Wander and flock
The fish wander around, flocking together with others. It will move in the general
direction of food that it spots, and avoid larger fish.

#### Steering Behaviors

- Wander
- Cohesion
- Seek (food)
- Flee (from larger fish)
- Obstacles - _List all obstacle types this state avoids_
- Seperation - Separates from other small fish
   
#### State Transistions

-This is the default state
-The fish will be in this state until it hasn't eaten in a 
certain amount of time.
   
## Starving

This state occurs when the fish hasn't eaten in a long time. 
Its movement is slowed, it no longer follows other fish, and 
it prioritizes finding food.

#### Steering Behaviors

- Wander
- Seek (Food) (The weight applied to this force is higher than in the default state)
- Flee (large fish)
- Obstacles - _List all obstacle types this state avoids_
- Seperation - Separates from larger fish
   
#### State Transistions
When the timer tracking how long since the fish has eaten reaches a certain amount, it enters this state. 
It exits the state once it's eaten.

## Large Fish
This fish is larger than the other fish in the tank, and avoids others of its kind. It
will eat food if it sees it, but isn't at risk of starving like the other fish. 
If smaller fish get near it, it will chase them.

### Wander and avoid
The fish wanders the tank, eating food it comes across and avoiding others.

#### Steering Behaviors

- Wander
- Seek (food) (low force weight)
- Flee (other large fish)
- Obstacles - _List all obstacle types this state avoids_
- Seperation - _List all agents this state seperates from_
   
#### State Transistions
This is the default state, and the large fish will be in it whenever it is far away from 
any smaller fish.
   
### Chasing
A smaller fish has gotten too near to the large fish, and the large fish is 
now chasing it. The chase continues until the small fish gets too far away, or 
the large fish loses interest (chase timer runs out).

#### Steering Behaviors

- Pursue (small fish)
- Obstacles - _List all obstacle types this state avoids_
- Seperation - _List all agents this state seperates from_
   
#### State Transistions
This state is entered when a small fish is within a certain distace of the large fish.
If there are multiple small fish meeting this criteria, the large fish prioritizes the first one.

## Sources
Small fish: https://voodoomoose.itch.io/free-fish-icons
Large fish: https://voodoomoose.itch.io/free-fish-icons

## Make it Your Own
Sound effects, creating environment art
- _List out what you added to your game to make it different for you_
- _If you will add more agents or states make sure to list here and add it to the documention above_
- _If you will add your own assets make sure to list it here and add it to the Sources section

## Known Issues

_List any errors, lack of error checking, or specific information that I need to know to run your program_

### Requirements not completed

_If you did not complete a project requirement, notate that here_

