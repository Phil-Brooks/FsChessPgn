namespace FsChessPgn

open FsChess
open System.IO

module Best =
    type Bmresps = {BestMove:string;Replies:string[]}
    ///get dictionary given location
    let GetDict(fl:string) = 
        let mutable ans = Map.empty
        if File.Exists(fl) then
            let doln (ln:string) =
                let bits = ln.Split([|'|'|])
                let fen = bits.[0]
                let bm = bits.[1]
                let rs = bits.[2].Split([|','|])
                let bmrs = {BestMove=bm;Replies=rs}
                ans <- ans.Add(fen,bmrs)
            let lns = File.ReadAllLines(fl)
            lns|>Array.iter doln
            ans
        else
            ans
    ///save dictionary given location
    let SaveDict(fl:string, dct:Map<string,Bmresps>) =
        let toln k (v:Bmresps) =
            k + "|" + v.BestMove + "|" + 
            (v.Replies|>Array.reduce(fun a b -> a + "," + b))
        let lns = dct|>Seq.map(fun (KeyValue(k,v)) -> toln k v)|>Seq.toArray
        File.WriteAllLines(fl,lns)

    ///Add to dictionary given dictionary, fen, best move and list of responses
    let Add(dct:Map<string,Bmresps>, fen:string, bm: string, sans:string[]) =
        let bmrs = {BestMove=bm;Replies=sans}
        if not (dct.ContainsKey(fen)) then dct.Add(fen,bmrs)
        else dct
    
    ///Expand for one move given dictionary,board, depth and move
    let ExpandMove(dct:Map<string,Bmresps>, nbd:Brd, depth:int) (mvstr:string) =
        let mv = mvstr|>MoveUtil.fromSAN nbd
        let bd = nbd|>Board.MoveApply mv
        //need to only process if not already in dct
        let fen = bd|>Board.ToStr
        if dct.ContainsKey fen then dct
        else
            let bm = Leela.GetBestMove(bd,depth)
            let nbd = bd|>Board.MoveApply bm
            let bmstr = bm|>MoveUtil.toPgn bd
            let sans = OpenExp.GetMoves(nbd)
            Add(dct,fen,bmstr,sans)

    ///Expand for one key given dictionary, depth and key value
    let ExpandKey(dct:Map<string,Bmresps>, depth:int) (kv:System.Collections.Generic.KeyValuePair<string,Bmresps>)=
        let fen,bmrs = kv|>fun (KeyValue(k,v)) -> k,v
        let bd = fen|>Board.FromStr
        let bm = bmrs.BestMove|>MoveUtil.fromSAN bd
        let nbd = bd|>Board.MoveApply bm
        let rec expmvs idct (rl:string list) =
            if rl.IsEmpty then idct
            else
                let r = rl.Head
                let odct = ExpandMove(idct,nbd,depth) r
                expmvs odct (rl.Tail)

        expmvs dct (bmrs.Replies|>List.ofArray)

    ///Expand all given dictionary and depth
    let Expand(dct:Map<string,Bmresps>, depth:int) =
        let rec expall idct (kvl:System.Collections.Generic.KeyValuePair<string,Bmresps> list) =
            if List.isEmpty kvl then idct
            else
                let kv = kvl.Head
                let odct = ExpandKey(dct,depth) kv
                expall odct kvl.Tail
        let kvl = dct|>Seq.toList
        expall dct kvl