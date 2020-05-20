# Chess.AF

This is a simple chess engine. Because it is an engine, I have not put to much effort in the UI, so I only created a console UI
just for testing purposes. Later maybe I will add a web application or windows form application to use the engine.

At this point it is possible to play chess against another person or yourself in the console.

## Classes

1. Position

    Heart of the engine is this class, it knows everything about a chess position.
    Internally it uses bitmaps (ulong) to represent where all the pieces are.
    
        When you look at a ulong in hexadecomal format 0x0102030405060708 it contains 8 bytes of 8 bits, our 64 squares
        on a board. From left to right, the first byte is row a8 - h8, second byte is a7 - h7, etc ...
        
        If you look at the enum SquareEnum, this enum can be directly cast to an int representing the correct index into
        a bitmap (ulong). Example a8 = 0, b8 = 1, h8 = 7, a7 = 8, etc ...
        
    **Note:** I do not allow state changes on a Position, If you look at the private Move method, it is used after the copy ctor.
    
2. PiecesIterator

    Generic iterator where T is either a PieceEnum or PiecesEnum. PieceEnum can be converted to a PiecesEnum, dependent on who's
    turn it is to move, and select only the pieces for either black or white. This iterator also accomodates iteration over
    pawns that can be promoted.
