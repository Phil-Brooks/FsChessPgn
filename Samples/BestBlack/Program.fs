namespace FsChess.WinForms

open System
open System.Windows.Forms
open System.Drawing
open FsChess


module Main =
    [<STAThread>]
    Application.EnableVisualStyles()
    
    type FrmMain() as this =
        inherit Form(Text = "Best Black", Width = 410, Height = 730)

        let mutable board = Board.Start
        let mutable gm = GameEMP
        let dct = Best.LoadDict lines
        let fen = board|>Board.ToStr
        let bmrs = dct.[fen]
        let mutable bmstr = bmrs.BestMove
        let bmlbl = new Label(Text=bmstr,BackColor=Color.PaleGreen,Top = 400, Left = 36, Width=330)
        let bd = new PnlBoard()
        let mutable resps = bmrs.Replies
        let rslv = new ListBox(Top = 440, Left = 36, Width=330, DataSource=resps, Height=200)
        let bkbtn = new Button(Text="Back", Left=36, Top=660)
        let fenbtn = new Button(Text="Copy FEN", Left=115, Top=660)
        let pgnbtn = new Button(Text="Copy PGN", Left=195, Top=660)

        let lblClick() =
            bd.DoMove bmstr

        let lvClick() =
            let nbd = if bmstr="" then board else board|>Board.PushSAN bmstr
            let ngm = if bmstr="" then gm else gm|>Game.PushSAN bmstr
            bd.SetBoard(nbd)
            let san = rslv.SelectedItem.ToString()
            board <- nbd|>Board.PushSAN san
            gm <- ngm|>Game.PushSAN san
            bd.DoMove san
            let fen = board|>Board.ToStr
            if dct.ContainsKey(fen) then
                let bmrs = dct.[fen]
                bmstr <- bmrs.BestMove 
                resps <- bmrs.Replies
            else
                bmstr <- "NOT DONE"
                resps <- [||]
            bmlbl.Text <- bmstr
            rslv.DataSource <- resps

        let bkClick() =
            if (gm.MoveText.Length>1) then
                gm <- gm|>Game.GetaMoves
                gm <- gm|>Game.Pop|>Game.Pop
                if gm.MoveText.Length=0 then 
                    board<-Board.Start
                    bd.SetBoard(board)
                else
                    let mte = gm.MoveText.[gm.MoveText.Length-1]
                    match mte with
                    |HalfMoveEntry(_,_,_,amv) ->
                        if amv.IsNone then failwith "should have valid aMove"
                        else
                            board <- amv.Value.PostBrd
                            bd.SetBoard(amv.Value.PreBrd)
                            bd.DoMove(amv.Value.Mv|>Move.ToSan amv.Value.PreBrd)
                    |_ -> failwith "should have valid HalfMove"
                let fen = board|>Board.ToStr
                let bmrs = dct.[fen]
                bmstr <- bmrs.BestMove
                bmlbl.Text <- bmstr
                resps <- bmrs.Replies
                rslv.DataSource <- resps

        let fenClick() =
            let fen = board|>Board.ToStr
            Clipboard.SetText(fen)

        let pgnClick() =
            let pgn = FsChessPgn.PgnWrite.GameStr(gm)
            Clipboard.SetText(pgn)

        do
            bd|>this.Controls.Add
            bmlbl|>this.Controls.Add
            rslv|>this.Controls.Add
            bkbtn|>this.Controls.Add
            fenbtn|>this.Controls.Add
            pgnbtn|>this.Controls.Add
            //events
            bmlbl.Click.Add(fun _ -> lblClick())
            rslv.Click.Add(fun _ -> lvClick())
            bkbtn.Click.Add(fun _ -> bkClick())
            fenbtn.Click.Add(fun _ -> fenClick())
            pgnbtn.Click.Add(fun _ -> pgnClick())
    
    let frm = new FrmMain()
    
    Application.Run(frm)