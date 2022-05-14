namespace FsChess.WinForms

open System
open System.Windows.Forms
open System.Drawing
open System.IO
open FsChess


module Main =
    [<STAThread>]
    Application.EnableVisualStyles()
    
    type FrmMain() as this =
        inherit Form(Text = "Best White", Width = 410, Height = 730)

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
        let addbtn = new Button(Text="+", Left=275, Top=660, Width=25)


        let lblClick() =
            bd.DoMove bmstr

        let lvClick() =
            let nbd = board|>Board.PushSAN bmstr
            let ngm = gm|>Game.PushSAN bmstr
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

        let addClick() =
            if bmstr <> "NOT DONE" then
                MessageBox.Show("Can only add missing positions!","Error",MessageBoxButtons.OK,MessageBoxIcon.Warning)|>ignore
            else
                let fen = board|>Board.ToStr
                let addfl = @"D:\lc0\lc0white10_add.txt"
                File.AppendAllText(addfl,fen + System.Environment.NewLine)
                MessageBox.Show("Position added to : " + addfl,"Add Position",MessageBoxButtons.OK,MessageBoxIcon.Information)|>ignore
        
        do
            bd|>this.Controls.Add
            bmlbl|>this.Controls.Add
            rslv|>this.Controls.Add
            bkbtn|>this.Controls.Add
            fenbtn|>this.Controls.Add
            pgnbtn|>this.Controls.Add
            addbtn|>this.Controls.Add
            //events
            bmlbl.Click.Add(fun _ -> lblClick())
            rslv.Click.Add(fun _ -> lvClick())
            bkbtn.Click.Add(fun _ -> bkClick())
            fenbtn.Click.Add(fun _ -> fenClick())
            pgnbtn.Click.Add(fun _ -> pgnClick())
            addbtn.Click.Add(fun _ -> addClick())
    
    let frm = new FrmMain()
    
    Application.Run(frm)