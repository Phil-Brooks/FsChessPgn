namespace FsChessPgn.Test

open FsChess
open FsChessPgn

open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type MoveTest()=
    let brd1 = Board.Start
    let mv1 = "e4"|>MoveUtil.fromSAN brd1

    [<TestMethod>]
    member this.Move_Utils() =
        Assert.AreEqual<Square>(E2,mv1|>Move.From)
        Assert.AreEqual<Square>(E4,mv1|>Move.To)
        Assert.AreEqual<Piece>(Piece.WPawn,mv1|>Move.MovingPiece)
        Assert.AreEqual<bool>(true,mv1|>Move.IsW)
        Assert.AreEqual<PieceType>(PieceType.Pawn,mv1|>Move.MovingPieceType)
        Assert.AreEqual<Player>(Player.White,mv1|>Move.MovingPlayer)
        Assert.AreEqual<bool>(false,mv1|>Move.IsCapture)
        Assert.AreEqual<Piece>(Piece.EMPTY,mv1|>Move.CapturedPiece)
        Assert.AreEqual<PieceType>(PieceType.EMPTY,mv1|>Move.CapturedPieceType)
        Assert.AreEqual<bool>(false,mv1|>Move.IsPromotion)
        Assert.AreEqual<PieceType>(PieceType.EMPTY,mv1|>Move.PromoteType)
        Assert.AreEqual<Piece>(Piece.EMPTY,mv1|>Move.Promote)
        Assert.AreEqual<bool>(false,mv1|>Move.IsEnPassant)
        Assert.AreEqual<bool>(false,mv1|>Move.IsCastle)
        Assert.AreEqual<bool>(true,mv1|>Move.IsPawnDoubleJump)

    