namespace FsChess

module Board =

    ///The starting Board at the beginning of a game
    let Start = FsChessPgn.Board.Start

    ///Create a FEN string from this Board 
    let ToStr = FsChessPgn.Board.ToStr


module Best=

    ///load dictionary given array
    let LoadDict = FsChessPgn.Best.LoadDict 
