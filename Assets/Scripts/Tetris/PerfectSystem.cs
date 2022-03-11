public static class PerfectSystem {
    public static bool IsPerfect(this BlockSet bs) {
        var flag = true;
        //capture: flag
        bs.ForEachBlock((x, y, b) => {
            if (b.state == BlockState.Exist) {
                flag = false;
            }
        });
        return flag;
    }
}
