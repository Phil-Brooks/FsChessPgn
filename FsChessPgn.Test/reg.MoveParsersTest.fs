﻿#nowarn "25"
namespace FsChessPgn.Test

open FsChess
open FsChessPgn

open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type MoveRegParserTest() =

    [<TestMethod>]
    member this.parse_simple_piece_move() =
        let s = "Qf5"
        let gml = Games.ReadFromString s
        Assert.AreEqual<int>(1, gml.Length)
        let gm = gml.Head
        let mt = gm.MoveText
        Assert.AreEqual<int>(1, mt.Length)
        let (HalfMoveEntry (_,_,mv,_)) = mt.Head
        Assert.AreEqual<MoveType>(MoveType.Simple,mv.Mtype)
        Assert.AreEqual(PieceType.Queen, mv.Piece.Value)
        Assert.AreEqual<Square>(F5, mv.TargetSquare)
        Assert.AreEqual<string>(s,mv|>PgnWrite.MoveStr)

    [<TestMethod>]
    member this.parse_simple_pawn_move() =
        let s = "d5"
        let gml = Games.ReadFromString s
        Assert.AreEqual<int>(1, gml.Length)
        let gm = gml.Head
        let mt = gm.MoveText
        Assert.AreEqual<int>(1, mt.Length)
        let (HalfMoveEntry (_,_,mv,_)) = mt.Head
        Assert.AreEqual<MoveType>(MoveType.Simple,mv.Mtype)
        Assert.AreEqual(PieceType.Pawn, mv.Piece.Value)
        Assert.AreEqual<Square>(D5, mv.TargetSquare)
        Assert.AreEqual<string>(s,mv|>PgnWrite.MoveStr)

    [<TestMethod>]
    member this.parse_castleK() =
        let s = "O-O"
        let gml = Games.ReadFromString s
        Assert.AreEqual<int>(1, gml.Length)
        let gm = gml.Head
        let mt = gm.MoveText
        Assert.AreEqual<int>(1, mt.Length)
        let (HalfMoveEntry (_,_,mv,_)) = mt.Head
        Assert.AreEqual<MoveType>(MoveType.CastleKingSide,mv.Mtype)
        Assert.AreEqual<Square>(OUTOFBOUNDS, mv.TargetSquare)
        Assert.AreEqual<string>(s,mv|>PgnWrite.MoveStr)

    [<TestMethod>]
    member this.parse_castleQ() =
        let s = "O-O-O"
        let gml = Games.ReadFromString s
        Assert.AreEqual<int>(1, gml.Length)
        let gm = gml.Head
        let mt = gm.MoveText
        Assert.AreEqual<int>(1, mt.Length)
        let (HalfMoveEntry (_,_,mv,_)) = mt.Head
        Assert.AreEqual<MoveType>(MoveType.CastleQueenSide,mv.Mtype)
        Assert.AreEqual<Square>(OUTOFBOUNDS, mv.TargetSquare)
        Assert.AreEqual<string>(s,mv|>PgnWrite.MoveStr)

    [<TestMethod>]
    member this.parse_pawn_capture() =
        let s = "cxd4"
        let gml = Games.ReadFromString s
        Assert.AreEqual<int>(1, gml.Length)
        let gm = gml.Head
        let mt = gm.MoveText
        Assert.AreEqual<int>(1, mt.Length)
        let (HalfMoveEntry (_,_,mv,_)) = mt.Head
        Assert.AreEqual<MoveType>(MoveType.Capture,mv.Mtype)
        Assert.AreEqual<Square>(D4, mv.TargetSquare)
        Assert.AreEqual<File>(FileC, mv.OriginFile.Value)
        Assert.AreEqual<string>(s,mv|>PgnWrite.MoveStr)

    [<TestMethod>]
    member this.parse_piece_capture() =
        let s = "Bxf3"
        let gml = Games.ReadFromString s
        Assert.AreEqual<int>(1, gml.Length)
        let gm = gml.Head
        let mt = gm.MoveText
        Assert.AreEqual<int>(1, mt.Length)
        let (HalfMoveEntry (_,_,mv,_)) = mt.Head
        Assert.AreEqual<MoveType>(MoveType.Capture,mv.Mtype)
        Assert.AreEqual<Square>(F3, mv.TargetSquare)
        Assert.AreEqual(PieceType.Bishop, mv.Piece.Value)
        Assert.AreEqual<string>(s,mv|>PgnWrite.MoveStr)

    [<TestMethod>]
    member this.parse_ambiguous_file() =
        let s = "Nfd7"
        let gml = Games.ReadFromString s
        Assert.AreEqual<int>(1, gml.Length)
        let gm = gml.Head
        let mt = gm.MoveText
        Assert.AreEqual<int>(1, mt.Length)
        let (HalfMoveEntry (_,_,mv,_)) = mt.Head
        Assert.AreEqual<MoveType>(MoveType.Simple,mv.Mtype)
        Assert.AreEqual<Square>(D7, mv.TargetSquare)
        Assert.AreEqual(PieceType.Knight, mv.Piece.Value)
        Assert.AreEqual<File>(FileF, mv.OriginFile.Value)
        Assert.AreEqual<string>(s,mv|>PgnWrite.MoveStr)
        
    [<TestMethod>]
    member this.parse_ambiguous_rank() =
        let s = "R3d5"
        let gml = Games.ReadFromString s
        Assert.AreEqual<int>(1, gml.Length)
        let gm = gml.Head
        let mt = gm.MoveText
        Assert.AreEqual<int>(1, mt.Length)
        let (HalfMoveEntry (_,_,mv,_)) = mt.Head
        Assert.AreEqual<MoveType>(MoveType.Simple,mv.Mtype)
        Assert.AreEqual<Square>(D5, mv.TargetSquare)
        Assert.AreEqual(PieceType.Rook, mv.Piece.Value)
        Assert.AreEqual<Rank>(Rank3, mv.OriginRank.Value)
        Assert.AreEqual<string>(s,mv|>PgnWrite.MoveStr)

    [<TestMethod>]
    member this.parse_prom() =
        let s = "d8=Q"
        let gml = Games.ReadFromString s
        Assert.AreEqual<int>(1, gml.Length)
        let gm = gml.Head
        let mt = gm.MoveText
        Assert.AreEqual<int>(1, mt.Length)
        let (HalfMoveEntry (_,_,mv,_)) = mt.Head
        Assert.AreEqual<MoveType>(MoveType.Simple,mv.Mtype)
        Assert.AreEqual(PieceType.Pawn, mv.Piece.Value)
        Assert.AreEqual<Square>(D8, mv.TargetSquare)
        Assert.AreEqual(PieceType.Queen, mv.PromotedPiece.Value)
        Assert.AreEqual<string>(s,mv|>PgnWrite.MoveStr)

    [<TestMethod>]
    member this.parse_prom_cap() =
        let s = "cxd8=Q"
        let gml = Games.ReadFromString s
        Assert.AreEqual<int>(1, gml.Length)
        let gm = gml.Head
        let mt = gm.MoveText
        Assert.AreEqual<int>(1, mt.Length)
        let (HalfMoveEntry (_,_,mv,_)) = mt.Head
        Assert.AreEqual<MoveType>(MoveType.Capture,mv.Mtype)
        Assert.AreEqual(PieceType.Pawn, mv.Piece.Value)
        Assert.AreEqual<Square>(D8, mv.TargetSquare)
        Assert.AreEqual(PieceType.Queen, mv.PromotedPiece.Value)
        Assert.AreEqual<string>(s,mv|>PgnWrite.MoveStr)

    [<TestMethod>]
    member this.parse_simple_check_move() =
        let s = "Qf5+"
        let gml = Games.ReadFromString s
        Assert.AreEqual<int>(1, gml.Length)
        let gm = gml.Head
        let mt = gm.MoveText
        Assert.AreEqual<int>(1, mt.Length)
        let (HalfMoveEntry (_,_,mv,_)) = mt.Head
        Assert.AreEqual<MoveType>(MoveType.Simple,mv.Mtype)
        Assert.AreEqual(PieceType.Queen, mv.Piece.Value)
        Assert.AreEqual<Square>(F5, mv.TargetSquare)
        Assert.AreEqual<string>(s,mv|>PgnWrite.MoveStr)

    [<TestMethod>]
    member this.parse_simple_doublecheck_move() =
        let s = "Qf5+"
        let gml = Games.ReadFromString s
        Assert.AreEqual<int>(1, gml.Length)
        let gm = gml.Head
        let mt = gm.MoveText
        Assert.AreEqual<int>(1, mt.Length)
        let (HalfMoveEntry (_,_,mv,_)) = mt.Head
        Assert.AreEqual<MoveType>(MoveType.Simple,mv.Mtype)
        Assert.AreEqual(PieceType.Queen, mv.Piece.Value)
        Assert.AreEqual<Square>(F5, mv.TargetSquare)
        Assert.AreEqual<string>(s,mv|>PgnWrite.MoveStr)

    [<TestMethod>]
    member this.parse_simple_mate_move() =
        let s = "Qf5#"
        let gml = Games.ReadFromString s
        Assert.AreEqual<int>(1, gml.Length)
        let gm = gml.Head
        let mt = gm.MoveText
        Assert.AreEqual<int>(1, mt.Length)
        let (HalfMoveEntry (_,_,mv,_)) = mt.Head
        Assert.AreEqual<MoveType>(MoveType.Simple,mv.Mtype)
        Assert.AreEqual(PieceType.Queen, mv.Piece.Value)
        Assert.AreEqual<Square>(F5, mv.TargetSquare)
        Assert.AreEqual<string>(s,mv|>PgnWrite.MoveStr)

    [<TestMethod>]
    member this.parse_simple_good_move() =
        let s = "Qf5"
        let gml = Games.ReadFromString s
        Assert.AreEqual<int>(1, gml.Length)
        let gm = gml.Head
        let mt = gm.MoveText
        Assert.AreEqual<int>(1, mt.Length)
        let (HalfMoveEntry (_,_,mv,_)) = mt.Head
        Assert.AreEqual<MoveType>(MoveType.Simple,mv.Mtype)
        Assert.AreEqual(PieceType.Queen, mv.Piece.Value)
        Assert.AreEqual<Square>(F5, mv.TargetSquare)
        Assert.AreEqual<string>(s,mv|>PgnWrite.MoveStr)

    [<TestMethod>]
    member this.parse_simple_blunder_move() =
        let s = "Qf5"
        let gml = Games.ReadFromString s
        Assert.AreEqual<int>(1, gml.Length)
        let gm = gml.Head
        let mt = gm.MoveText
        Assert.AreEqual<int>(1, mt.Length)
        let (HalfMoveEntry (_,_,mv,_)) = mt.Head
        Assert.AreEqual<MoveType>(MoveType.Simple,mv.Mtype)
        Assert.AreEqual(PieceType.Queen, mv.Piece.Value)
        Assert.AreEqual<Square>(F5, mv.TargetSquare)
        let mvstr = mv|>PgnWrite.MoveStr
        Assert.AreEqual<string>(s,mvstr)

    [<TestMethod>]
    member this.parse_simple_interesting_move() =
        let s = "Qf5"
        let gml = Games.ReadFromString s
        Assert.AreEqual<int>(1, gml.Length)
        let gm = gml.Head
        let mt = gm.MoveText
        Assert.AreEqual<int>(1, mt.Length)
        let (HalfMoveEntry (_,_,mv,_)) = mt.Head
        Assert.AreEqual<MoveType>(MoveType.Simple,mv.Mtype)
        Assert.AreEqual(PieceType.Queen, mv.Piece.Value)
        Assert.AreEqual<Square>(F5, mv.TargetSquare)
        let mvstr = mv|>PgnWrite.MoveStr
        Assert.AreEqual<string>(s,mvstr)
