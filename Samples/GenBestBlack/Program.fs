open FsChess
open System.IO

let splitfol = @"D:\lc0\lc0black10_split"
let depth = 10
Leela.SetFol(@"D:\lc0")

let outfol = Path.Combine(splitfol,"out")
Directory.CreateDirectory(outfol)|>ignore

let rec expfls (fnl:string list) =
    if not fnl.IsEmpty then
        let fn = fnl.Head
        let outfn = Path.Combine(outfol,Path.GetFileName(fn))
        let dct = Best.GetDict(fn)
        //iterate through keys filling blanks
        printfn "Processing file: %s" fn
        let ndct = Best.Expand(dct, depth)
        Best.SaveDict(outfn,ndct)
        File.Delete(fn)
        expfls fnl.Tail

let files = Directory.GetFiles(splitfol)
let fnl = files|>List.ofArray
expfls fnl