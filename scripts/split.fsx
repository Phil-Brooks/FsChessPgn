#load "setup.fsx"
open FsChess
open FsChess.Pgn
open System.IO

let bestdictfl = @"D:\lc0\lc0white10.txt"

// filter out those lines that need expanding
// do rec thaa goes through the responses and see if any are not in the dct.
// if any aren't then set to true as filter function
let lns = File.ReadAllLines(bestdictfl)
let dct = Best.LoadDict(lns)

let doln (iln:string) =
    let bits = iln.Split([|'|'|])
    let fen = bits.[0]
    let bm = bits.[1]
    let rs = if bits.[2]= "" then [||] else bits.[2].Split([|','|])
    let bd = fen|>Board.FromStr
    let nbd = bd|>Board.PushSAN bm

    let rec dor (irl:string list) =
        if irl.IsEmpty then false
        else
            let r = irl.Head
            let rbd = nbd|>Board.PushSAN r
            let rfen = rbd|>Board.ToStr
            if dct.ContainsKey(rfen) then
                dor irl.Tail    
            else true
    let rl = rs|>List.ofArray
    let filt = dor rl
    filt

let outfol = Path.Combine(Path.GetDirectoryName(bestdictfl), Path.GetFileNameWithoutExtension(bestdictfl)+"_split")
Directory.CreateDirectory(outfol)

let rec dosplit i (rlns:string[]) =
    let outfl = Path.Combine(outfol,i.ToString() + ".txt")
    if rlns.Length<11 then
        File.WriteAllLines(outfl,rlns)
    else
        File.WriteAllLines(outfl,rlns.[0..9])
        dosplit (i+1) rlns.[10..]

let nlns = lns|>Array.filter doln
let lnslen = lns.Length
let nlnslen = nlns.Length

dosplit 1 nlns