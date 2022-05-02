open System.IO

let bestdictfl = @"D:\lc0\lc0black10.txt"
let outfol = Path.Combine(Path.Combine(Path.GetDirectoryName(bestdictfl), Path.GetFileNameWithoutExtension(bestdictfl)+"_split"),"out")
let lns = File.ReadAllLines(bestdictfl)
let len0 = lns.Length
//create copy of file
File.Copy(bestdictfl,bestdictfl + "." + len0.ToString() + ".txt")
//get files to be merged
let fls = Directory.GetFiles(outfol)
let flslen = fls.Length
let addlns = fls|>Array.map(File.ReadAllLines)|>Array.concat
let addlen = addlns.Length
//add to original
let newlns = Array.append lns addlns
let newlen = newlns.Length
//remove dups
let unlns = newlns|>Set.ofArray|>Set.toArray
let unlen = unlns.Length
//save expanded
File.WriteAllLines(bestdictfl,unlns)