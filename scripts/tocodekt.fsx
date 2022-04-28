#load "setup.fsx"
open FsChess
open System.IO

//let bestdictfl = @"D:\lc0\lc0white10.txt"
let bestdictfl = @"D:\lc0\lc0white10 - Copy.txt"
let codefl = @"D:\lc0\ChessData.kt"

let lines = File.ReadAllLines(bestdictfl)
let dct = Best.LoadDict(lines)


let tocod (kv:string*FsChessPgn.Best.Bmresps) =
    let fen,bmrs = kv
    let bd = fen|>Board.FromStr
    let bm = bmrs.BestMove
    let nbd = bd|>Board.PushSAN bm
    let bmfen = nbd|>Board.ToStr
    let rsa = bmrs.Replies
    let rbda = rsa|>Array.map (fun r -> nbd|>Board.PushSAN r)|>Array.map (Board.ToStr)
    let nln =
        fen + "|" +
        bm + ":" + bmfen + ";" +
        (if rsa.Length=0 then "" else rsa|>Array.reduce(fun a b -> a + "," + b)) + ";" +
        (if rbda.Length=0 then "" else rbda|>Array.reduce(fun a b -> a + "," + b)) 
    "        \"" + nln + "\","

let kva = dct|>Seq.map(fun (KeyValue(k,v)) -> k,v)|>Seq.toArray


let arrlines =
    kva
    |>Array.map tocod

let start =
    [|
    "package com.example.bestwhiteand"
    ""
    "class ChessData(){"
    "    val lines = arrayOf("
    |]
let nd = 
    [|
    "    )"
    "}"
    |]
let codelines = Array.append start (Array.append arrlines nd)
File.WriteAllLines(codefl,codelines)