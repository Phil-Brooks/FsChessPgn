#load "setup.fsx"
open FsChess

let bestdictfl = @"D:\lc0\fsharp\lc0black10.txt"
let dct = Best.GetDict(bestdictfl)
let depth = 10

Leela.SetFol(@"D:\lc0")

let initialise_b() =
    let bd = Board.Start
    
    let fen = bd|>Board.ToStr
    let bmstr = ""
    let sans = OpenExp.GetMoves(bd)

    let ndct = Best.Add(dct,fen,bmstr,sans)
    Best.SaveDict(bestdictfl,ndct)

if dct.Count=0 then initialise_b()
else
    //iterate through keys filling blanks
    let ndct = Best.Expand(dct, depth)
    Best.SaveDict(bestdictfl,ndct)
    ()
