﻿namespace FsChessPgn.Test

open FsChess
open FsChessPgn

open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type BoardTest()=
    let brd1 = Board.Start
    let mv1 = "e4"|>MoveUtil.fromSAN brd1


    [<TestMethod>]
    member this.Board_SquareAttacked() =
        let pas = brd1|>Board.SquareAttacked D2 Player.White
        Assert.AreEqual<bool>(true,pas)

    [<TestMethod>]
    member this.Board_MoveApply() =
        let brd2 = brd1|>Board.MoveApply mv1
        let str = brd2|>Board.ToStr
        Assert.AreEqual<string>("rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR b KQkq e3 0 1",str)

    [<TestMethod>]
    member this.Board_MoveApply2() =
        //test for failing castle rights
        let brd = Board.FromStr "rnbqkbnr/4pppp/8/1p6/2pPP3/8/1P3PPP/R1BQKBNR w KQkq - 0 8"
        let san = "Rxa8"
        let brd2 = brd|>Board.PushSAN san
        let str = brd2|>Board.ToStr
        Assert.AreEqual<string>("Rnbqkbnr/4pppp/8/1p6/2pPP3/8/1P3PPP/2BQKBNR b Kk - 0 8",str)

    [<TestMethod>]
    member this.Board_IsCheck() =
        let ic = brd1|>Board.IsChk
        let ic2 = brd1|>Board.IsChck Player.White
        Assert.AreEqual<bool>(false,ic)
        Assert.AreEqual<bool>(false,ic2)

    [<TestMethod>]
    member this.Board_FromFEN() =
        let fen = FEN.Parse "rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR b KQkq e3 0 1"
        let brd2 = Board.FromFEN fen
        let str = brd2|>Board.ToStr
        Assert.AreEqual<string>("rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR b KQkq e3 0 1",str)
