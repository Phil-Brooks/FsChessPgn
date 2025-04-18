﻿namespace FsChessPgn.Test

open FsChess
open FsChessPgn

open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type TagRegParserTests() =

    [<TestMethod>]
    member this.pTag_should_accept_tags_which_are_prefixes_of_others() =
        let gml = Games.ReadFromString "[WhiteSomethingFoo \"\"]"
        Assert.AreEqual<int>(1, gml.Length)
        let gm = gml.Head
        Assert.AreEqual<int>(1, gm.AdditionalInfo.Count)
        let k,v = gm.AdditionalInfo|>Map.toList|>List.head
        Assert.AreEqual<string>("WhiteSomethingFoo",k)
        Assert.AreEqual<string>("",v)

    [<TestMethod>]
    member this.pTag_should_create_a_PgnDateTag_object_from_a_valid_tag() =
        let gml = Games.ReadFromString "[Date \"2013.05.15\"]"
        Assert.AreEqual<int>(1, gml.Length)
        let gm = gml.Head
        Assert.AreEqual<int>(2013, gm.Year.Value)
        Assert.AreEqual<int>(5, gm.Month.Value)
        Assert.AreEqual<int>(15, gm.Day.Value)

    [<TestMethod>]
    member this.pTag_should_accept_only_the_year_as_date() =
        let gml = Games.ReadFromString "[Date \"2013\"]"
        Assert.AreEqual<int>(1, gml.Length)
        let gm = gml.Head
        Assert.AreEqual<int>(2013, gm.Year.Value)
        Assert.AreEqual<bool>(false, gm.Month.IsSome)
        Assert.AreEqual<bool>(false, gm.Day.IsSome)

    [<TestMethod>]
    member this.pTag_should_create_a_PgnRoundTag_object_from_a_valid_tag() =
        let gml = Games.ReadFromString "[Round \"13\"]"
        Assert.AreEqual<int>(1, gml.Length)
        let gm = gml.Head
        Assert.AreEqual<string>("13",gm.Round)

    [<TestMethod>]
    member this.pTag_should_create_PgnRoundTag_object_from_two_tags_in_sequence() =
        let gml = Games.ReadFromString @"[Round ""?""][White ""Tarrasch, Siegbert""]"
        Assert.AreEqual<int>(1, gml.Length)
        let gm = gml.Head
        Assert.AreEqual<string>("?",gm.Round)
        Assert.AreEqual<string>("Tarrasch, Siegbert",gm.WhitePlayer)

    [<TestMethod>]
    member this.pTag_should_accept_non_numeric_rounds() =
        let gml = Games.ReadFromString "[Round \"4.1\"]"
        Assert.AreEqual<int>(1, gml.Length)
        let gm = gml.Head
        Assert.AreEqual<string>("4.1",gm.Round)

    [<TestMethod>]
    member this.pTag_should_create_a_PgnResultTag_object_from_a_valid_tag() =
        let gml = Games.ReadFromString "[Result \"1-0\"]"
        Assert.AreEqual<int>(1, gml.Length)
        let gm = gml.Head
        Assert.AreEqual(GameResult.WhiteWins,gm.Result)

    [<TestMethod>]
    member this.pTag_should_create_a_FenTag_object_from_a_valid_tag() =
        let gml = Games.ReadFromString "[FEN \"rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1\"]"
        Assert.AreEqual<int>(1, gml.Length)
        let gm = gml.Head
        Assert.AreEqual<bool>(true,gm.BoardSetup.IsSome)
        let setup = gm.BoardSetup.Value
        Assert.AreEqual(Piece.BRook, setup.PieceAt.[Sq(FileA,Rank8)|>int])
        Assert.AreEqual(Piece.WKnight, setup.PieceAt.[Sq(FileB,Rank1)|>int])
        Assert.AreEqual(Piece.BBishop, setup.PieceAt.[Sq(FileC,Rank8)|>int])
        Assert.AreEqual(Piece.EMPTY, setup.PieceAt.[Sq(FileC,Rank3)|>int])
        Assert.AreEqual(Piece.WKing, setup.PieceAt.[Sq(FileE,Rank1)|>int])

        Assert.AreEqual(Player.White, setup.WhosTurn)

        Assert.AreEqual(CstlFlgs.All, setup.CastleRights)

        Assert.AreEqual<Square>(OUTOFBOUNDS, setup.EnPassant)

        Assert.AreEqual<int>(0, setup.Fiftymove)
        Assert.AreEqual<int>(1, setup.Fullmove)


    [<TestMethod>]
    member this.pTag_should_create_a_FenTag_object_from_another_valid_tag() =
        let gml = Games.ReadFromString "[FEN \"rnbqkbnr/pp1ppppp/8/2p5/4P3/8/PPPP1PPP/RNBQKBNR b Kq c6 1 2\"]"
        Assert.AreEqual<int>(1, gml.Length)
        let gm = gml.Head
        Assert.AreEqual<bool>(true,gm.BoardSetup.IsSome)
        let setup = gm.BoardSetup.Value
        Assert.AreEqual(Piece.BRook, setup.PieceAt.[Sq(FileA,Rank8)|>int])
        Assert.AreEqual(Piece.BPawn, setup.PieceAt.[Sq(FileB,Rank7)|>int])
        Assert.AreEqual(Piece.BPawn, setup.PieceAt.[Sq(FileC,Rank5)|>int])
        Assert.AreEqual(Piece.WPawn, setup.PieceAt.[Sq(FileE,Rank4)|>int])
        Assert.AreEqual(Piece.EMPTY, setup.PieceAt.[Sq(FileE,Rank2)|>int])
        Assert.AreEqual(Piece.WKing, setup.PieceAt.[Sq(FileE,Rank1)|>int])

        Assert.AreEqual(Player.Black, setup.WhosTurn)

        Assert.AreEqual(CstlFlgs.WhiteShort|||CstlFlgs.BlackLong, setup.CastleRights)

        Assert.AreEqual<Square>(C6, setup.EnPassant)

        Assert.AreEqual<int>(1, setup.Fiftymove)
        Assert.AreEqual<int>(2, setup.Fullmove)
