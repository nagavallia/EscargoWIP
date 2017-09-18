# shellPlatformer
Our snail game that is gonna be huge. 

Tasks for Monday 9/18
---------------------
Nick - Movement  
Arnesh - Shell Throwing  
Jimmy - Interactables - good place to start would be a button/switch that spawns in the exit. you can reuse the exit stuff in the 1st prototype.  
Grant - Shell Spawner  
Justin + George - Level/Mechanics mockups  


Organization
-------------
Folders are cool. Put things in a Resources folder if you need to spawn it in code. Any Resources folder anywhere in Assests works. But it has to be called "Resources" exactly.  

Use the sandbox scene for messing aboot and implementing features, builder should be where actual levels are made. Controllers and UI should be added eventually but most features will live in prefabs (and scripts on those prefabs) anyway.  


Methods to Read Up On  
---------------------  
`Resources.Load("[prefab name]")` - Instantiate a prefab in code  
`GetComponent<[Component Name]>()` - Get component instance from game object  
Transform - SUPER important component attached to every GameObject, tons of useful methods  
MonoBehaviour inherited methods - Update(), Awake(), OnCollisionEnter(), etc. Methods that are invoked when certain built-in Unity things happen.  
`[SerializeField] private <var type> <var name>` - makes a field editable in Unity editor but still private (can't be changed by other classes at runtime)  
`Destroy(this.gameObject)` - Deletes gameObject attached to "this". MAKE SURE TO USE this.gameObject IF CALLING FROM COMPONENT  
`public <var type> <var name> { get; [private] set; }` - C# shortcut for creating getters/setters at variable initialization. Making setter private can be useful.  
`SetActive(<boolean>)` - Makes current GameObject/component active (true) or inactive (false).  


Making GameObjects Interact  
---------------------------  
There are three types of `GameObject` communication that should be useful to us.  
NOTES: All examples I mention can be implemented in other ways than the ones I say, these are just examples.  

1. Explicit Link  

This is the most basic, most efficient, but also most inflexible method.   
Simply attach one `GameObject` to the field of another by dragging and dropping in the Unity editor and call whatever methods you want at the appropriate time.  

Example: Button opening a door  

In the "Button" script we should have code like this  

```C#
    public class Button : MonoBehaviour {  
        [SerializeField] private GameObject door;  
        .  
        .  
        .  
    }
```
    
Attach the Button component to a `GameObject` that will represent the button and there will be a new blank field.  
Link the `GameObject` that represents the door to the empty field and now code like this will work.  

```C#
    public class Button : MonoBehaviour {  
        .  
        .  
        .  
        private void OnTriggerEnter2D(Collider2D collision) {  
            door.Open();  
        }  
        .  
        .  
        .  
    }
```

This calls the `Open()` method on the door object.  

NOTES: If doing any interactions this way, care MUST be taken to ensure everything is linked up properly.   
Or else things won't work and you'll be sad.  

2. SendMessage  

Slightly inefficient, fairly flexible method. Requires you to have a `GameObject` reference in code, but no need to muck about with the Unity editor.  

Invoke a function call in another object by doing  
    
    other.SendMessage("Foo")  
    
This causes the `GameObject` "other" to execute its (CASE SENSITIVE) method `Foo()`.  

Example: Shell collides with enemy  

We'll put some code in the Shell script that looks like   

```C#
    public class Shell : MonoBehaviour {  
        .   
        .  
        .  
        private void OnCollisionEnter2D(Collider2D collision) {  
            collision.gameObject.SendMessage("ShellCollide");  
        .  
        .  
        .  
    }
```
     
Now the enemy script just needs to implement the function `ShellCollide()` that will respond when the enemy is hit.  

This has a few advantages over explicit links:  
    1) No need to link enemy and shell in Unity editor   
    2) Shell collision code works for ANY `GameObject`, they just need to implement `ShellCollide()`  
    3) If an object is hit by the shell and doesn't have `ShellCollide()`, nothing happens  
Some cons:  
    1) Less efficient than explicit calls  
    2) Need to get object reference somehow (eg, through `OnCollisionEnter2D` function)  
    
NOTES: If `SendMessage("Foo")` is called on a `GameObject` that doesn't have `Foo()`, it will actually log a (harmless) error message to the console. Use `SendMessage("Foo", SendMessageOptions.DontRequireReceiver)` if this gets annoying  

SendMessage can also be used to invoke functions that take arguments. Google it and read about it in the Unity API.  

3. Messenger Broadcast  

This uses a 3rd party library I got off the Unity community wiki. Read through Messenger.cs for detailed information.  

Most flexible, most abstract communication method. Here's a picture I found illustrating the concept https://cms-assets.tutsplus.com/uploads/users/202/posts/25407/image/interactions_msgs.png  

Essentially, this method works by having objects broadcast when certain conditions are met. These broadcasts are intercepted and responded to by listeners.  

Broadcast: `Messenger.Broadcast("[EventString]")`   
Listener: `Messenger.AddListener("[EventString]", Foo)` 

Broadcasts/Listeners take a string as input. Instead of using actual strings in code, though, put them in GameEvent.cs.   
This will allow text editor auto-completion and easier organization.  

Look at Exit.cs and GrantsScript.cs for an in-code implementation.  

Example: Finishing a level  
We have a level exit that has an Exit script attached. Exit will broadcast when the level is finished.  

```C#
    public class Exit : MonoBehaviour {  
        .  
        .  
        .  
        private void OnTriggerEnter2D(Collider2D collision) {  
            Messenger.Broadcast(GameEvent.LEVEL_COMPLETE);  
        }  
        .  
        .  
        .  
    }
```
    
Now we just need to have anything that needs to respond to the level finishing, well, respond. This can be 0, 1, 2, up to infinite objects.  
We just need to make sure they're listening.  

```C#
    public class SceneController : MonoBehaviour {  
        private void Awake() {  
            Messenger.AddListener(GameEvent.LEVEL_COMPLETE, FinishLevel);  
        }  
          
        private void OnDestroy() {  
            Messenger.RemoveListener(GameEvent.LEVEL_COMPLETE, FinishLevel);  
        }  
          
        private void FinishLevel() {  
            //code to run on level completion  
        }  
    }
```
    
Remember we can have an arbitrary number of objects respond to `LEVEL_COMPLETE`, they just need listeners.  
They can also respond to them using any function or number of functions they want.  

The main advantages of this method is that Exit doesn't know (nor care) about what, if anything, is responding to its broadcast just as the listeners don't know (nor care) about the broadcaster.   

NOTES: Messenger broadcasts/listeners can accept 0, 1, 2, or 3 function parameters BUT NOT MORE. We could add support for more params if we need them but I doubt we'll need them.  

Broadcasts can also ask for callback functions. Read through Messenger.cs for more info.  

Adding/removing listeners can happen anywhere, not just in `Awake()`/`OnDestroy()`. These are just good "default" locations.  
Just think about when you want/don't want to be listening.   
