namespace FsChessPgn

module Best =
    type Bmresps = {BestMove:string;Replies:string[]}
    ///load dictionary given array
    let LoadDict(lns:string[]) = 
        let mutable ans = Map.empty
        let doln (ln:string) =
            let bits = ln.Split([|'|'|])
            let fen = bits.[0]
            let bm = bits.[1]
            let rs = if bits.[2]= "" then [||] else bits.[2].Split([|','|])
            let bmrs = {BestMove=bm;Replies=rs}
            ans <- ans.Add(fen,bmrs)
        lns|>Array.iter doln
        ans

 