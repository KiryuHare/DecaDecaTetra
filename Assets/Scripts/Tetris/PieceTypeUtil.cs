using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class PieceTypeUtil {
    public static (int x, int y) Center(this PieceType type) {
        return type switch {
            PieceType.I => (1, 2),
            PieceType.O => (1, 0),
            _ => (1, 1)
        };
    }
}
