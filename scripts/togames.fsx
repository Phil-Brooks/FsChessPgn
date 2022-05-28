#load "setup.fsx"
open FsChess

let bestdictfl = @"D:\lc0\lc0white10.txt"
let dct = Best.GetDict(bestdictfl)

let rec getgames cbd cgm igml =
    let fen = cbd|>Board.ToStr
    if not (dct.ContainsKey fen) then igml
    else
        let ce = dct.[fen]
        let nbd1 = cbd|>Board.PushSAN ce.BestMove
        let ngm1 = cgm |>Game.PushSAN ce.BestMove
        if ce.Replies.Length=0 then ngm1::igml
        else
            let nbdgmss = ce.Replies|>Array.map(fun m -> nbd1|>Board.PushSAN m, ngm1|>Game.PushSAN m)
            let gmla = nbdgmss|>Array.map(fun (b,g) -> getgames b g igml)
            gmla|>List.concat

let bd = Board.Start
let gm = GameEMP
let gml = getgames bd gm []

do gml|>Pgn.Games.WriteFile @"D:\lc0\lc0white10.pgn"