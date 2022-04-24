open System.IO

let bestdictfl = @"D:\lc0\lc0white10.txt"
let outfol = Path.Combine(Path.GetDirectoryName(bestdictfl), Path.GetFileNameWithoutExtension(bestdictfl)+"_split")
Directory.CreateDirectory(outfol)

let rec dosplit i (rlns:string[]) =
    let outfl = Path.Combine(outfol,i.ToString() + ".txt")
    if rlns.Length<11 then
        File.WriteAllLines(outfl,rlns)
    else
        File.WriteAllLines(outfl,rlns.[0..9])
        dosplit (i+1) rlns.[10..]

let lns = File.ReadAllLines(bestdictfl)
dosplit 1 lns