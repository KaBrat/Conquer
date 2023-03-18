// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public abstract class GamePhase
// {
//     public abstract void Handle(Context context);
// }

// public class Preparing : GamePhase
// {
//     public override void Handle(Context context)
//     {
//         Game.CreateTile(context.Players, context.TilePrefab);
//         context.State = new Running();
//     }
// }

// public class Running : GamePhase
// {
//     public override void Handle(Context context)
//     {
//         context.State = new Preparing();
//     }
// }

// public class Context
// {
//     GamePhase state;
//     public Player[] Players;
//     public GameObject TilePrefab;
//     public Context(GamePhase state)
//     {
//         this.State = state;
//     }
//     public GamePhase State
//     {
//         get { return state; }
//         set
//         {
//             state = value;
//         }
//     }
//     public void NextGamePhase()
//     {
//         state.Handle(this);
//     }
// }
