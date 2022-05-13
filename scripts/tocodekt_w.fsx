#load "setup.fsx"
open FsChess
open System.IO

let bestdictfl = @"D:\lc0\lc0white10.txt"
//let bestdictfl = @"D:\lc0\lc0white10 - Copy.txt"
let codefl1 = @"D:\lc0\ChessData1.kt"
let codefl2 = @"D:\lc0\ChessData2.kt"
let codefl3 = @"D:\lc0\ChessData3.kt"
let codefl4 = @"D:\lc0\ChessData4.kt"

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

let start1 =
    [|
    "package com.example.bestwhiteand"
    ""
    "class ChessData1(){"
    "    val lines = arrayOf("
    |]
let start2 =
    [|
    "package com.example.bestwhiteand"
    ""
    "class ChessData2(){"
    "    val lines = arrayOf("
    |]
let start3 =
    [|
    "package com.example.bestwhiteand"
    ""
    "class ChessData3(){"
    "    val lines = arrayOf("
    |]
let start4 =
    [|
    "package com.example.bestwhiteand"
    ""
    "class ChessData4(){"
    "    val lines = arrayOf("
    |]
let nd = 
    [|
    "    )"
    "}"
    |]

let mid = arrlines.Length/4
let mid2 = 2*mid
let mid3 = 3*mid
let arrlines1 = arrlines.[0..mid-1]
let arrlines2 = arrlines.[mid..mid2-1]
let arrlines3 = arrlines.[mid2..mid3-1]
let arrlines4 = arrlines.[mid3..]

let codelines1 = Array.append start1 (Array.append arrlines1 nd)
File.WriteAllLines(codefl1,codelines1)
let codelines2 = Array.append start2 (Array.append arrlines2 nd)
File.WriteAllLines(codefl2,codelines2)
let codelines3 = Array.append start3 (Array.append arrlines3 nd)
File.WriteAllLines(codefl3,codelines3)
let codelines4 = Array.append start4 (Array.append arrlines4 nd)
File.WriteAllLines(codefl4,codelines4)