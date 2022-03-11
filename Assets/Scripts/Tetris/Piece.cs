
public struct Piece {
    /// <summary>
    /// 中心位置：(1,1)
    /// </summary>
    public int x;
    /// <summary>
    /// 中心位置：(1,1)
    /// </summary>
    public int y;
    public PieceType type;
    public PieceDirection dir;
    public Piece(Piece p_) : this(p_.x, p_.y, p_.type, p_.dir) { }
    public Piece(int x_, int y_, PieceType type_, PieceDirection dir_ = PieceDirection.U) {
        x = x_;
        y = y_;
        type = type_;
        dir = dir_;
    }
    public void Rotate(RotateDirection rot) {
        dir = dir.Rotated(rot);
    }

    public Piece GetRotated(RotateDirection rot) {
        return new Piece(x, y, type, dir.Rotated(rot));
    }

    public void Move(int y_, int x_) {
        x += x_;
        y += y_;
    }

    public Piece GetMoved(int y_, int x_) {
        return new Piece(x + x_, y + y_, type, dir);
    }

}
