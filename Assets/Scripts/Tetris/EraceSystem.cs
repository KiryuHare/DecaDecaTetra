
using System.Collections.Generic;
using System.Linq;

public static class EraceSystem {
    public static (BlockSet newBlocks, int erased) EraseLine(this BlockSet blocks) {
        var res_Blocks = new BlockSet(blocks);
        int res_erased = 0;
        for (var i = 0; i < res_Blocks.rowCount; i++) {
            if (blocks.data[i].All(b => b.state == BlockState.Exist)) {
                for (var j = 0; j < res_Blocks.columnCount; j++) res_Blocks.SetBlock(j, i, new Block { state = BlockState.Erace });
                res_erased++;
            }
        }
        return (new BlockSet(res_Blocks), res_erased);
    }
    public static BlockSet OrganizeLine(this BlockSet blocks) {
        var existLines = blocks.data.Select((line, i) =>
           line.All(b => b.state == BlockState.Erace) ? -1 : i
        ).Where(i => i != -1).ToList();
        return new BlockSet(Enumerable.Range(0, blocks.rowCount)
        .Select(i => i < existLines.Count ? blocks.data[existLines[i]] : new Block[10])
        .ToArray());
    }
}