public class StepModel
{
    public int score;
    public int bestScore;
    public int[][] numbers;

    /// <summary>
    /// 保存数据
    /// </summary>
    /// <param name="score"></param>
    /// <param name="bestScore"></param>
    /// <param name="tiles"></param>
    public void UpdateData(int score, int bestScore, Tile[][] tiles) {
        this.score = score;
        this.bestScore = bestScore;

        if (numbers == null) {
            numbers = new int[tiles.Length][];
        }

        for (int i = 0; i < tiles.Length; i++) {
            for (int j = 0; j < tiles.Length; j++) {
                if (numbers[i] == null) {
                    numbers[i] = new int[tiles[i].Length];
                }
                numbers[i][j] = tiles[i][j].HaveNumber() ? tiles[i][j].GetNumber().GetNumberValue() : 0;
            }
        }

    }
}
