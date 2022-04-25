open System.IO

let bestdictfl = @"D:\lc0\lc0white10.txt"
let codefl = @"D:\lc0\Data.fs"

let lines = File.ReadAllLines(bestdictfl)
let arrlines =
    lines
    |>Array.map (fun l -> "        \"" + l + "\"")

let start =
    [|
    "namespace FsChess.WinForms"
    "[<AutoOpen>]"
    "module Data ="
    "    let lines ="
    "        [|"
    |]
let nd = [|"        |]"|]
let codelines = Array.append start (Array.append arrlines nd)
File.WriteAllLines(codefl,codelines)