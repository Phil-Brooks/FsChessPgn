#I @"D:\GitHub\FsChessPgn\FsChessPgn\bin\Debug\net6.0"
#r "FsChessPgn.dll"
open FsChess
do
    fsi.AddPrinter<Move>(Pretty.Move)
    fsi.AddPrinter<Square>(Pretty.Square)
    fsi.AddPrinter<Brd>(Pretty.Board)
    fsi.AddPrinter<Game>(Pretty.Game)
    fsi.AddPrinter<BrdStats>(Pretty.BrdStats)

