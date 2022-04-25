namespace BestWhiteAnd

open System

open Android.App
open Android.Content
open Android.OS
open Android.Runtime
open Android.Views
open Android.Widget
open FsChess

type Resources = BestWhiteAnd.Resource

[<Activity (Label = "Best White", MainLauncher = true, Icon = "@mipmap/icon")>]
type MainActivity () =
    inherit Activity ()

    let mutable count:int = 1
    let mutable board = Board.Start
    let mutable gm = GameEMP
    let dct = Best.LoadDict lines
    let fen = board|>Board.ToStr
    let bmrs = dct.[fen]
    let mutable bmstr = bmrs.BestMove
        

    override this.OnCreate (bundle) =

        base.OnCreate (bundle)

        // Set our view from the "main" layout resource
        this.SetContentView (Resources.Layout.Main)

        // Get our button from the layout resource, and attach an event to it
        let button = this.FindViewById<Button>(Resources.Id.myButton)
        let rslv = this.FindViewById<ListView>(BestWhiteAnd.Resource.Id.)
        button.Text <- bmstr
        button.Click.Add (fun args -> 
            button.Text <- sprintf "%d clicks!" count
            count <- count + 1
        )
        ()
        //rslv.ItemsSource

