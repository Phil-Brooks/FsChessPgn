namespace FsChessPgn.Test

open FsChess
open FsChessPgn

open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type MoveUtilTest()=
    let brd1 = Board.Start
    let mv1 = "e4"|>MoveUtil.fromSAN brd1

    [<TestMethod>]
    member this.MoveUtil_Parse() =
        Assert.AreEqual<Square>(E2,mv1|>Move.From)
        Assert.AreEqual<Square>(E4,mv1|>Move.To)

    [<TestMethod>]
    member this.MoveUtil_Desc() =
        Assert.AreEqual<string>("e2e4",mv1|>MoveUtil.toUci)

    [<TestMethod>]
    member this.MoveUtil_DescBd() =
        Assert.AreEqual<string>("e4",mv1|>MoveUtil.toPgn brd1)

    [<TestMethod>]
    member this.MoveUtil_Descs() =
        Assert.AreEqual<string>("1. e4 ",MoveUtil.Descs [mv1] brd1 true)

    [<TestMethod>]
    member this.MoveUtil_FindMv() =
        let mv = "e2e4"|>MoveUtil.FindMv brd1
        Assert.AreEqual<Square>(E2,mv.Value|>Move.From)
        Assert.AreEqual<Square>(E4,mv.Value|>Move.To)
