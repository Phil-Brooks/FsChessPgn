#load "setup.fsx"
open FsChess

let board = Board.FromStr("r1bqkb1r/pppp1Qpp/2n2n2/4p3/2B1P3/8/PPPP1PPP/RNB1K1NR b KQkq - 0 4")
let chk1 = board|>Board.IsCheck
let chk2 = board|>Board.SquareAttacked E8 Player.White
let attackers = board|>Board.SquareAttackers F3 Player.White

