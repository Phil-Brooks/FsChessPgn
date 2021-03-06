#load "setup.fsx"
open FsChess

let bestdictfl = @"D:\lc0\lc0white10.txt"
let dct = Best.GetDict(bestdictfl)
let depth = 10

Leela.SetFol(@"D:\lc0")

let initialise() =
    let bd = Board.Start
    let bm = Leela.GetBestMove(bd,depth)

    let nbd = bd|>Board.Push bm

    let fen = bd|>Board.ToStr
    let bmstr = bm|>Move.ToSan bd
    let sans = OpenExp.GetMoves(nbd)

    let ndct = Best.Add(dct,fen,bmstr,sans)
    Best.SaveDict(bestdictfl,ndct)

if dct.Count=0 then initialise()
else
    //iterate through keys filling blanks
    let ndct = Best.Expand(dct, depth)
    Best.SaveDict(bestdictfl,ndct)
    ()
