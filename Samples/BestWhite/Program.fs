namespace FsChess.WinForms

open System
open System.Windows.Forms
open System.Drawing
open FsChess


module Main =
    [<STAThread>]
    Application.EnableVisualStyles()
    
    type FrmMain() as this =
        inherit Form(Text = "Best White", Width = 410, Height = 700)

        let mutable board = Board.Start
        let dct = Best.LoadDict lines
        let fen = board|>Board.ToStr
        let bmrs = dct.[fen]
        let mutable bmstr = bmrs.BestMove
        let bmlbl = new Label(Text=bmstr,BackColor=Color.PaleGreen,Top = 400, Left = 36, Width=330)
        let bd = new PnlBoard()
        let rslv = new ListBox(Top = 440, Left = 36, Width=330, DataSource=bmrs.Replies, Height=200)

        let lblClick() =
            let nbd = board|>Board.PushSAN bmstr
            bd.DoMove bmstr

        let lvClick() =
            let nbd = board|>Board.PushSAN bmstr
            let san = rslv.SelectedItem.ToString()
            board <- nbd|>Board.PushSAN san
            bd.SetBoard(board)
        do
            bd|>this.Controls.Add
            bmlbl|>this.Controls.Add
            rslv|>this.Controls.Add
            //events
            bmlbl.Click.Add(fun _ -> lblClick())
            rslv.Click.Add(fun _ -> lvClick())
    
    let frm = new FrmMain()
    
    Application.Run(frm)