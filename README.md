# TDA572-Shard-and-RushB
The Shard and RushB is implemented in TDA572 Game engine Architecture at Chalmers University of Technology. Based on the Shard v1.0, some new systems and features have been added. Additionally, implementing a feature complete example game to show the function of the engine.

# RushB
It is a normal turn-based game. Players and the AI take turns controlling their soldiers, and those who have already acted need to wait until the next turn.

# User Interface
The user interface system has been implemented into Shard. It contains several common UI elements, including text, button, image, panel and scroll window. All UI elements inherit from GameObject and can control their own rendering process, which means developers just need to create the UI element. Additionally, they can also handle the input events and update their information in the real time. The UIButton allow developers to execute their own function via C\# delegates. The UIPanel element also support the attachment of other components, which just need to set the relative position.
## Feature List
| | Feature | Location|
| --- | --- | --- |
|1|UI Base class|Shard\UIBase.cs|
|2|Text|Shard\UIText.cs|
|3|Button|Shard\UIButton.cs|
|4|Image|Shard\UIImage.cs|
|5|Panel|Shard\UIPanel.cs|
|6|Scroll Window|Shard\UIScrollWindow.cs|

# Turn Base System
Technically, the turn-based system is not a standard game engine system, but a gameplay system. Since the state of the turn is limited and hard to be controlled in one game manager class, I decide to build it by finite state machine. The machine use to control initiate the state list and the transition between them. Different FSM states control their own transition logic.

## Feature List
| | Feature | Location|
| --- | --- | --- |
|1|Finite State Machine|RushB\FiniteStateMachine.cs|
|2|FSM State|RushB\FSMState.cs|
| | |RushB\StateWaitPlayerSelect.cs|
| | |RushB\StatePlayerSelected.cs|
| | |RushB\StatePlayerSelectAction.cs|
| | |RushB\StatePlayerSelectTarget.cs|
| | |RushB\StateAI.cs|
|3|FSM Transition|RushB\FSMTransition.cs|

# Artificial Intelligence
Basically, it is a Astar search algorithm path finder based on the graph. Developers need to build a map for the algorithm than it will work.

## Feature List
| | Feature | Location|
| --- | --- | --- |
|1|AstarSearch algorithm|RushB\AstarSearch.cs|
| | |RushB\PriorityQueue.cs|
|2|Graph|RushB\Grid.cs|
| | |RushB\WeightedGraph.cs|
| | |RushB\SquareGraph.cs|

# Sound
It is a simple sound system, with play and pause/resume function, and allows the music and sound effect to be played simultaneously.

## Feature List
| | Feature | Location|
| --- | --- | --- |
|1|Sound|Shard\Sound.cs|

# Sprite
The sprite extension allow developers to set animation for the game object. It should be added to the object as a field to work and processed every 100ms in the loop. It support the loop animation and one time animation.

## Feature List
| | Feature | Location|
| --- | --- | --- |
|1|Sprite|RushB\Sprite.cs|