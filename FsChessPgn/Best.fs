namespace FsChessPgn

open FsChess
open System.IO

module Best =
    type Bmresps = {BestMove:string;Replies:string[]}
    ///get dictionary given location
    let GetDict(fl:string) = 
        let ans = new System.Collections.Generic.Dictionary<string,Bmresps>()
        if File.Exists(fl) then
            //TODO
            ans
        else
            ans
    ///save dictionary given location
    let SaveDict(fl:string, dct:System.Collections.Generic.Dictionary<string,Bmresps>) =
        ()

    ///Add to dictionary given fen, best move and list of responses
    let Add(dct:System.Collections.Generic.Dictionary<string,Bmresps>, fen:string, bm: string, sans:string[]) =
        let bmrs = {BestMove=bm;Replies=sans}
        if not (dct.ContainsKey(fen)) then dct.Add(fen,bmrs)
        ()
    