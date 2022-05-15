#load "setup.fsx"
open FsChess
open System.IO

let bestdictfl = @"D:\lc0\lc0black10.txt"
let addfl = @"D:\lc0\lc0black10_add.txt"
let dct = Best.GetDict(bestdictfl)
let depth = 10

Leela.SetFol(@"D:\lc0")

//create copy of file
let lns = File.ReadAllLines(bestdictfl)
let len0 = lns.Length
File.Copy(bestdictfl,bestdictfl + "." + len0.ToString() + ".txt")

//iterate through fens, designed so can stop and keeps progress
let fens = File.ReadAllLines(addfl)

let rec doadd (idct:Map<string,FsChessPgn.Best.Bmresps>) (fnl:string list) =
    if fnl.IsEmpty then
        File.Delete(addfl)
    else
        let fen = fnl.Head
        let ndct = Best.AddFen(idct,depth) fen
        let nfnl = fnl.Tail
        //save over addfl
        File.WriteAllLines(addfl,nfnl)
        //save over bestdictfl
        Best.SaveDict(bestdictfl,ndct)
        doadd ndct nfnl

doadd dct (fens|>List.ofArray)