#load "setup.fsx"
open FsChess
open System.IO

let bestdictfl = @"D:\lc0\lc0white10.txt"
let fixfen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1"
let fixbmstr = "e3"
let dct = Best.GetDict(bestdictfl)

//STEP 1 - Replace the entry for the fixfen
let fixkey fen bmstr =
    let ce = dct.[fen]
    
    let bd = Board.FromStr fen
    let bm = bmstr|>Move.FromSan bd
    let nbd = bd|>Board.Push bm
    let sans = OpenExp.GetMoves(nbd)
    let ndct0 = dct.Remove(fen)
    let ndct = Best.Add(ndct0,fen,bmstr,sans)

    let nce = ndct.[fen]

    //create copy of file
    let lns = File.ReadAllLines(bestdictfl)
    let len0 = lns.Length
    File.Copy(bestdictfl,bestdictfl + "." + len0.ToString() + ".txt")
    //update
    Best.SaveDict(bestdictfl,ndct)

//fixkey fixfen fixbmstr


//STEP 2 - remove all redundant entries
let lns = File.ReadAllLines(bestdictfl)
let len0 = lns.Length
let kv2fens (kv:System.Collections.Generic.KeyValuePair<string,FsChessPgn.Best.Bmresps>) =
    let bd = Board.FromStr kv.Key
    let nbd = 
        if kv.Value.BestMove="" then bd
        else
            let bm = kv.Value.BestMove|>Move.FromSan bd
            bd|>Board.Push bm
    let obds = kv.Value.Replies|>Array.map(fun m -> nbd|>Board.PushSAN m)
    let fens = obds|>Array.map(Board.ToStr)
    fens

let kvl = dct|>Seq.toArray
let ofens = kvl|>Array.map kv2fens|>Array.concat
let len1 = ofens.Length
let oset = Set.ofArray ofens
let iset = kvl|>Array.map(fun kv -> kv.Key)|>Set.ofArray

let ilen = iset.Count
let olen = oset.Count

let orphans = iset-oset
let orplen = orphans.Count

let mutable odct = dct

let remov o =
    let fenstar = Board.Start|>Board.ToStr
    if o<>fenstar then odct <- odct.Remove(o)


do orphans|>Set.iter remov

Best.SaveDict(bestdictfl,odct)
//TODO

Leela.SetFol(@"D:\lc0")
let depth = 10



