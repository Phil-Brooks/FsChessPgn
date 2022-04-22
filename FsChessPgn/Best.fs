namespace FsChessPgn

open FsChess
open System.IO

module Best =
    type Bmresps = {BestMove:string;Replies:string[]}
    ///get dictionary given location
    let GetDict(fl:string) = 
        let ans = new System.Collections.Generic.Dictionary<string,Bmresps>()
        if File.Exists(fl) then
            let doln (ln:string) =
                let bits = ln.Split([|'|'|])
                let fen = bits.[0]
                let bm = bits.[1]
                let rs = bits.[2].Split([|','|])
                let bmrs = {BestMove=bm;Replies=rs}
                ans.Add(fen,bmrs)
            let lns = File.ReadAllLines(fl)
            lns|>Array.iter doln
            ans
        else
            ans
    ///save dictionary given location
    let SaveDict(fl:string, dct:System.Collections.Generic.Dictionary<string,Bmresps>) =
        let toln k (v:Bmresps) =
            k + "|" + v.BestMove + "|" + 
            (v.Replies|>Array.reduce(fun a b -> a + "," + b))
        let lns = dct|>Seq.map(fun (KeyValue(k,v)) -> toln k v)|>Seq.toArray
        File.WriteAllLines(fl,lns)

    ///Add to dictionary given fen, best move and list of responses
    let Add(dct:System.Collections.Generic.Dictionary<string,Bmresps>, fen:string, bm: string, sans:string[]) =
        let bmrs = {BestMove=bm;Replies=sans}
        if not (dct.ContainsKey(fen)) then dct.Add(fen,bmrs)
        ()
    