public static class MoveSystem {
    /// <returns>下降不可能な場合、null</returns>    
    public static Piece? FallPieceOn(this Piece piece, in BlockSet blocks)
    => MoveOn(piece, blocks, 0, -1);

    public static Piece? HardFallOn(this Piece piece, in BlockSet blocks) {
        Piece lastOk = new Piece(piece);
        for (var i = 1; i < 10; i++) {
            var fallen = MoveOn(piece, blocks, 0, -i);
            if (fallen is Piece p) {
                lastOk = p;
            } else break;
        }
        return lastOk;
    }

    public static Piece? MoveRightOn(this Piece piece, in BlockSet blocks)
    => MoveOn(piece, blocks, 1, 0);
    public static Piece? MoveLeftOn(this Piece piece, in BlockSet blocks)
    => MoveOn(piece, blocks, -1, 0);

    static Piece? MoveOn(Piece piece, BlockSet blocks, int x, int y) {
        var result = piece.GetMoved(y, x);
        if (!blocks.IsCrashing(result)) {
            return result;
        }
        return null;
    }
}
