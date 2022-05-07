#load "setup.fsx"
open FsChess
open System.IO

let bestdictfl = @"D:\lc0\lc0black10.txt"
let codefl1 = @"D:\lc0\ChessData1.kt"
let codefl2 = @"D:\lc0\ChessData2.kt"
let codefl3 = @"D:\lc0\ChessData3.kt"

let lines = File.ReadAllLines(bestdictfl)
let dct = Best.LoadDict(lines)

let tocod (kv:string*FsChessPgn.Best.Bmresps) =
    let fen,bmrs = kv
    let bd = fen|>Board.FromStr
    let bm = bmrs.BestMove
    let nbd = if bm="" then bd else bd|>Board.PushSAN bm
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

let start1 =
    [|
    "package com.example.bestblackand"
    ""
    "class ChessData1(){"
    "    val lines = arrayOf("
    |]
let start2 =
    [|
    "package com.example.bestblackand"
    ""
    "class ChessData2(){"
    "    val lines = arrayOf("
    |]
let start3 =
    [|
    "package com.example.bestblackand"
    ""
    "class ChessData3(){"
    "    val lines = arrayOf("
    |]
let nd = 
    [|
    "    )"
    "}"
    |]

let mid = arrlines.Length/3
let mid2 = 2*mid
let arrlines1 = arrlines.[0..mid-1]
let arrlines2 = arrlines.[mid..mid2-1]
let arrlines3 = arrlines.[mid2..]

let codelines1 = Array.append start1 (Array.append arrlines1 nd)
File.WriteAllLines(codefl1,codelines1)
let codelines2 = Array.append start2 (Array.append arrlines2 nd)
File.WriteAllLines(codefl2,codelines2)
let codelines3 = Array.append start3 (Array.append arrlines3 nd)
File.WriteAllLines(codefl3,codelines3)