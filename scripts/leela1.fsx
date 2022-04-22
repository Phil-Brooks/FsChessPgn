#load "setup.fsx"
open FsChess
open FsChess.Pgn

let bestdictfl = @"D:\lc0\bestdict.txt"
let dct = Best.GetDict(bestdictfl)

Leela.SetFol(@"D:\lc0")
let bd = Board.Start
let bm = Leela.GetBestMove(bd,10)

let nbd = bd|>Board.Push bm

let fen = bd|>Board.ToStr
let bmstr = bm|>Move.ToSan bd
let sans = OpenExp.GetMoves(nbd)

Best.Add(dct,fen,bmstr,sans)
dct
Best.SaveDict(bestdictfl,dct)