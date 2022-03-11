public static class PieceDirectionUtil{
    public static PieceDirection Rotated(this PieceDirection dir, RotateDirection rot) {
        return rot switch {
            RotateDirection.R => dir switch {
                PieceDirection.U => PieceDirection.R,
                PieceDirection.R => PieceDirection.D,
                PieceDirection.D => PieceDirection.L,
                PieceDirection.L => PieceDirection.U,
                _ => default
            },
            RotateDirection.L => dir switch {
                PieceDirection.U => PieceDirection.L,
                PieceDirection.L => PieceDirection.D,
                PieceDirection.D => PieceDirection.R,
                PieceDirection.R => PieceDirection.U,
                _ => default
            },
            _ => default
        };
    }
}