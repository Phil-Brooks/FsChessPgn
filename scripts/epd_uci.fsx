#load "setup.fsx"
open FsChess

let inpf = @"D:\Github\Flounder\docs\Tester\wacnew.epd"
let outpf = @"D:\Github\Flounder\docs\Tester\uciwacnew.epd"

let lns = System.IO.File.ReadAllLines(inpf)
let filt (ln:string) =
    let bits = ln.Split("bm")
    let mvbits = bits.[1].Split(";")
    not (mvbits.[0].Trim().Contains(" "))
let doln (ln:string) =
    let bits = ln.Split("bm")
    let fen = bits.[0] + "0 0"
    let pgnmv = 
        let mvbits = bits.[1].Split(";")
        mvbits.[0].Trim()
    let bd = fen|>Board.FromStr
    let mvuci = (Move.FromSan bd pgnmv)|>Move.ToUci
    bits.[0] + "bm " + mvuci
let olns = lns|>Array.filter filt|>Array.map doln
System.IO.File.WriteAllLines(outpf,olns)