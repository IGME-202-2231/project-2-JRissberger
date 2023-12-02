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
The fish wander around, avoiding obstacles and generally staying near other small fish. It also flees from jellyfish if
it's being chased by one.

#### Steering Behaviors

- Wander
- Cohesion
- Flee (from jellyfish)
- Obstacles - Avoids seaweed and rocks
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
- Flee (jellyfish)
- Obstacles - _List all obstacle types this state avoids_
- Seperation - Separates from other small fish
   
#### State Transistions
When the timer tracking how long since the fish has eaten reaches a certain amount, it enters this state. 
It exits the state once it's eaten.

## Jellyfish
This jellyfish avoids others of its kind. It wanders around, and isn't interested in the 
food for the smaller fish. Every so often, if a small fish is close enough, it will chase 
the small fish for a short amount of time.

### Wander and avoid
The jellyfish wanders the tank, avoiding obstacles and other jellyfish.

#### Steering Behaviors

- Wander
- Obstacles - avoids seaweed and rocks
- Seperation - this agent separates from other jellyfish.
   
#### State Transistions
This is the default state, and the large fish will be in it whenever it is far away from 
any smaller fish.
   
### Chasing
A smaller fish has gotten too near to the jellyfish, and the jellyfish is 
now chasing it at a higher speed than it normally moves at. The chase continues until the 
jellyfish loses interest (chase timer runs out).

#### Steering Behaviors

- Seek (small fish)
- Obstacles - avoids seaweed and rocks
- Seperation - separates from other jellyfish
   
#### State Transistions
This state is entered when a small fish is within a certain distace of the large fish.
If there are multiple small fish meeting this criteria, the large fish prioritizes the first one.

## Sources
I created all the assets for this project.

## Make it Your Own
Creating own assets (environment, obstacles, fish)

## Known Issues

_List any errors, lack of error checking, or specific information that I need to know to run your program_

### Requirements not completed

_If you did not complete a project requirement, notate that here_

