namespace FsChess.WinForms

open System.Drawing
open System.Windows.Forms
open FsChess

[<AutoOpen>]
module Library1 =
    
    let private img nm =
        let thisExe = System.Reflection.Assembly.GetExecutingAssembly()
        let file = thisExe.GetManifestResourceStream("BestWhite.Images." + nm)
        Image.FromStream(file)

    type PnlBoard() as bd =
        inherit Panel(Width = 400, Height = 400)

        let mutable board = Board.Start
        let mutable sqTo = -1
        let bdpnl = new Panel(Dock = DockStyle.Top, Height = 400)
        let sqpnl = new Panel(Width = 420, Height = 420, Left = 29, Top = 13)
        
        let edges =
            [ new Panel(BackgroundImage = img "Back.jpg", Width = 342, 
                        Height = 8, Left = 24, Top = 6)
              
              new Panel(BackgroundImage = img "Back.jpg", Width = 8, 
                        Height = 350, Left = 24, Top = 8)
              
              new Panel(BackgroundImage = img "Back.jpg", Width = 8, 
                        Height = 350, Left = 366, Top = 6)
              
              new Panel(BackgroundImage = img "Back.jpg", Width = 342, 
                        Height = 8, Left = 32, Top = 350) ]
        
        let sqs : PictureBox [] = Array.zeroCreate 64
        let flbls : Label [] = Array.zeroCreate 8
        let rlbls : Label [] = Array.zeroCreate 8

        //events
        let mvEvt = new Event<_>()
        let bdEvt = new Event<_>()

        //functions
 
        /// get image given char
        let getim c =
            match c with
            | "P" -> img "WhitePawn.png"
            | "B" -> img "WhiteBishop.png"
            | "N" -> img "WhiteKnight.png"
            | "R" -> img "WhiteRook.png"
            | "K" -> img "WhiteKing.png"
            | "Q" -> img "WhiteQueen.png"
            | "p" -> img "BlackPawn.png"
            | "b" -> img "BlackBishop.png"
            | "n" -> img "BlackKnight.png"
            | "r" -> img "BlackRook.png"
            | "k" -> img "BlackKing.png"
            | "q" -> img "BlackQueen.png"
            | _ -> failwith "invalid piece"

        ///set pieces on squares
        let setpcsmvs () =
            let setpcsmvs() =
                board.PieceAt
                |>List.map Piece.ToStr
                |> List.iteri (fun i c -> sqs.[i].Image <- if c = " " then null else getim c)
            if (bd.InvokeRequired) then 
                try 
                    bd.Invoke(MethodInvoker(setpcsmvs)) |> ignore
                with _ -> ()
            else setpcsmvs()

        ///orient board
        let orient isw =
            let ori() =
                let possq i (sq : PictureBox) =
                    let r = i / 8
                    let f = i % 8
                    if not isw then 
                        sq.Top <- 7 * 42 - r * 42 + 1
                        sq.Left <- 7 * 42 - f * 42 + 1
                    else 
                        sq.Left <- f * 42 + 1
                        sq.Top <- r * 42 + 1
                sqs |> Array.iteri possq
                flbls
                |> Array.iteri (fun i l -> 
                       if isw then l.Left <- i * 42 + 30
                       else l.Left <- 7 * 42 - i * 42 + 30)
                rlbls
                |> Array.iteri (fun i l -> 
                       if isw then l.Top <- 7 * 42 - i * 42 + 16
                       else l.Top <- i * 42 + 16)
            if (bd.InvokeRequired) then 
                try 
                    bd.Invoke(MethodInvoker(ori)) |> ignore
                with _ -> ()
            else ori()

        ///highlight squares
        let highlightsqs () =
            sqs
            |> Array.iteri (fun i sq -> 
                   sqs.[i].BackColor <- if (i % 8 + i / 8) % 2 = 1 then 
                                            Color.Green
                                        else Color.PaleGreen)
             
            sqs.[sqTo].BackColor <- if (sqTo % 8 + sqTo / 8) % 2 = 1 then 
                                            Color.YellowGreen
                                        else Color.Yellow

        /// creates file label
        let flbl i lbli =
            let lbl = new Label()
            lbl.Text <- FILE_NAMES.[i]
            lbl.Font <- new Font("Arial", 12.0F, FontStyle.Bold, 
                                 GraphicsUnit.Point, byte (0))
            lbl.ForeColor <- Color.Green
            lbl.Height <- 21
            lbl.Width <- 42
            lbl.TextAlign <- ContentAlignment.MiddleCenter
            lbl.Left <- i * 42 + 30
            lbl.Top <- 8 * 42 + 24
            flbls.[i] <- lbl

        /// creates rank label
        let rlbl i lbli =
            let lbl = new Label()
            lbl.Text <- (i + 1).ToString()
            lbl.Font <- new Font("Arial", 12.0F, FontStyle.Bold, 
                                 GraphicsUnit.Point, byte (0))
            lbl.ForeColor <- Color.Green
            lbl.Height <- 42
            lbl.Width <- 21
            lbl.TextAlign <- ContentAlignment.MiddleCenter
            lbl.Left <- 0
            lbl.Top <- 7 * 42 - i * 42 + 16
            rlbls.[i] <- lbl
       
        ///set board colours and position of squares
        let setsq i sqi =
            let r = i / 8
            let f = i % 8
            let sq =
                new PictureBox(Height = 42, Width = 42, 
                               SizeMode = PictureBoxSizeMode.CenterImage)
            sq.BackColor <- if (f + r) % 2 = 1 then Color.Green
                            else Color.PaleGreen
            sq.Left <- f * 42 + 1
            sq.Top <- r * 42 + 1
            sq.Tag <- i
            //events
            sqs.[i] <- sq
        
        do 
            sqs |> Array.iteri setsq
            sqs |> Array.iter sqpnl.Controls.Add
            setpcsmvs()
            edges |> List.iter bdpnl.Controls.Add
            flbls |> Array.iteri flbl
            flbls |> Array.iter bdpnl.Controls.Add
            rlbls |> Array.iteri rlbl
            rlbls |> Array.iter bdpnl.Controls.Add
            sqpnl |> bdpnl.Controls.Add
            bdpnl |> bd.Controls.Add

        ///Sets the Board to be displayed
        member bd.SetBoard(ibd:Brd) =
            board<-ibd
            setpcsmvs()

        ///Orients the Board depending on whether White
        member bd.Orient(isw:bool) =
            isw|>orient

        ///Sets the board given a new move in SAN format
        member bd.DoMove(san:string) =
            let mv = san|>Move.FromSan board
            board <- board|>Board.Push mv
            sqTo <- mv|>Move.To|>int
            highlightsqs ()
            setpcsmvs()


