using System;
using Debug = UnityEngine.Debug;

public static class MinimaxAIController
{
    public static (int row, int col)? GetBestMove(GameManager.PlayerType[,] board) //가상 시뮬레이터는 이때부터 시작된다!
    {
        float bestScore = -1000;
        (int row, int col)? bestMove = null;
        
        for (var row = 0; row < board.GetLength(0); row++)
        {
            for (var col = 0; col < board.GetLength(1); col++)
            {
                if (board[row, col] == GameManager.PlayerType.None) //베스트 스코어 생성
                {
                    Debug.Log($"row : {row} / col : {col}");
                    board[row, col] = GameManager.PlayerType.PlayerB; //임시의 시뮬레이터 AI 마크를 할당, 첫 번째 임시의 변수를 놓는다.
                    var score = DoMinimax(board, 0, false); //이 false는 자기 턴일 때를 알리는 느낌..?
                    board[row, col] = GameManager.PlayerType.None; //그리고 그 값에 따른 첫 번째 임시의 변수를 비교한다.
                    
                    Debug.Log(score);
                    
                    if (score > bestScore) //그래서 0! 무승부로 결정이 
                    {
                        bestScore = score;
                        bestMove = (row, col);
                    }
                }
            }
        }
        return bestMove;
    }

    private static float DoMinimax(GameManager.PlayerType[,] board, int depth, bool isMaximizing) //이때부터 a,b 점점 뎁스로 돌아가면서 수를 놓기 시작한다.
    {
        //재귀함수가 끝나는 구간 (시뮬레이션을 돌릴 때, 모든 빈칸이 다 채워졌을 때임, 재귀함수가 끝나는 구간)
        if (CheckGameWin(GameManager.PlayerType.PlayerA, board)) // 플레이어 A가 이길 거 같다면 기존 depth에서 10
            return -10 + depth;
        if (CheckGameWin(GameManager.PlayerType.PlayerB, board)) // 플레이어 B가 이길 거 같다면 기존 depth에서 10
            return 10 - depth;
        if (IsAllBlocksPlaced(board)) //무승부로 결정이 나면 0
            return 0;

        if (isMaximizing) //뎁스 0, + 1
        {
            //여기서 말하는 스코어는 재귀함수가 끝날 때 결정이 된다.
            var bestScore = float.MinValue;
            for (var row = 0; row < board.GetLength(0); row++)
            {
                for (var col = 0; col < board.GetLength(1); col++)
                {
                    if (board[row, col] == GameManager.PlayerType.None)
                    {
                        board[row, col] = GameManager.PlayerType.PlayerB; //2,0이 저장
                        var score = DoMinimax(board, depth + 1, false);
                        board[row, col] = GameManager.PlayerType.None;
                        bestScore = Math.Max(bestScore, score);
                    }
                }
            }
            return bestScore;
        }
        else
        {
            var bestScore = float.MaxValue;
            for (var row = 0; row < board.GetLength(0); row++)
            {
                for (var col = 0; col < board.GetLength(1); col++)
                {
                    if (board[row, col] == GameManager.PlayerType.None)
                    {
                        board[row, col] = GameManager.PlayerType.PlayerA;
                        var score = DoMinimax(board, depth + 1, true); //턴이 계속 바뀐다.
                        board[row, col] = GameManager.PlayerType.None;
                        bestScore = Math.Min(bestScore, score);
                    }
                }
            }
            return bestScore;
        }
    }
    
    public static bool IsAllBlocksPlaced(GameManager.PlayerType[,] board)
    {
        for (var row = 0; row < board.GetLength(0); row++)
        {
            for (var col = 0; col < board.GetLength(1); col++)
            {
                if (board[row, col] == GameManager.PlayerType.None)
                    return false;
            }
        }
        return true;
    }
    
    private static bool CheckGameWin(GameManager.PlayerType playerType, GameManager.PlayerType[,] board) //임시의 수를 놓았을 때 이길 수 있는지 확인
    {
        // 가로로 마커가 일치하는지 확인
        for (var row = 0; row < board.GetLength(0); row++)
            if (board[row, 0] == playerType && board[row, 1] == playerType && board[row, 2] == playerType)
                return true;
        
        // 세로로 마커가 일치하는지 확인
        for (var col = 0; col < board.GetLength(1); col++)
            if (board[0, col] == playerType && board[1, col] == playerType && board[2, col] == playerType)
                return true;
        
        // 대각선 마커 일치하는지 확인
        if (board[0, 0] == playerType && board[1, 1] == playerType && board[2, 2] == playerType)
            return true;
        if (board[0, 2] == playerType && board[1, 1] == playerType && board[2, 0] == playerType)
            return true;
        
        return false;
    }
}
