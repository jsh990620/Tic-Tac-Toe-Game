using System;
using System.Threading;

/*
 목표: 틱택토 게임 2단계까지 구현
 반복문이나 조건문, 함수에 주석을 꼼꼼히 달아 설명하기
 */

Random random = new Random();
// 재시작용 반복
while (true)
{
    Console.Clear();
    char[,] board = new char[3, 3];
    char turn = 'X';
    int count = 0;
    int mode;
    // 보드 초기화
    for (int i = 0; i < 3; i++)
    {
        for (int j = 0; j < 3; j++)
        {
            board[i, j] = '.';
        }
    }
    // 게임 모드 선택
    while (true)
    {
        Console.WriteLine("=== 틱택토 게임 ===");
        Console.WriteLine("1. 2인 대전");
        Console.WriteLine("2. 컴퓨터 대전");
        Console.Write("모드 선택: ");
        string select = Console.ReadLine();
        if (int.TryParse(select, out mode)
            && (mode == 1 || mode == 2))
        {
            break;
        }
        Console.WriteLine("1 또는 2를 입력하세요.");
        Console.WriteLine();
    }
    Console.Clear();
    PrintGuide();
    PrintBoard(board);

    // 게임 진행
    while (true)
    {
        int position;
        // 컴퓨터 차례일때 조건(mode가 2이고 turn이 O일때)
        if (mode == 2 && turn == 'O')
        {
            Console.WriteLine("컴퓨터가 생각 중...");
            Thread.Sleep(2000);
            // 빈칸을 무작위로 선택
            while (true)
            {
                position = random.Next(1, 10);
                int row = (position - 1) / 3;
                int col = (position - 1) % 3;
                if (board[row, col] == '.')
                {
                    break;
                }
            }
        }
        else
        {
            // 사람 차례
            while (true)
            {
                Console.Write($"플레이어 {turn} 위치 입력(1~9): ");
                string input = Console.ReadLine();
                // 숫자 검사
                if (!int.TryParse(input, out position))
                {
                    Console.WriteLine("숫자를 입력하세요.");
                    continue;
                }
                // 경계 검사
                if (position < 1 || position > 9)
                {
                    Console.WriteLine("1부터 9까지 입력하세요.");
                    continue;
                }
                int row = (position - 1) / 3;
                int col = (position - 1) % 3;
                // 중복 검사
                if (board[row, col] != '.')
                {
                    Console.WriteLine("이미 사용한 칸입니다.");
                    continue;
                }
                break;
            }
        }
        // 선택한 위치에 표시
        int x = (position - 1) / 3;
        int y = (position - 1) % 3;
        board[x, y] = turn;
        count++;
        // 화면 초기화 및 보드 출력
        Console.Clear();
        PrintGuide();
        PrintBoard(board);

        // 컴퓨터가 선택한 위치 출력
        if (mode == 2 && turn == 'O')
        {
            Console.WriteLine($"컴퓨터가 {position}번에 두었습니다.");
        }
        // 매 턴마다 승리 판정
        if (CheckWin(board, turn))
        {
            if (mode == 2 && turn == 'O')
            {
                Console.WriteLine("컴퓨터 승리!");
            }
            else
            {
                Console.WriteLine($"플레이어 {turn} 승리!");
            }
            break;
        }
        // count가 9가 되었을시에 무승부 판정
        if (count == 9)
        {
            Console.WriteLine("무승부입니다!");
            break;
        }
        // 차례 변경
        if (turn == 'X')
        {
            turn = 'O';
        }
        else
        {
            turn = 'X';
        }
    }

    bool restart = false;
    // 재시작을 입력받을때까지 반복
    while (true)
    {
        Console.Write("다시 플레이하시겠습니까? (Y/N): ");
        string answer = Console.ReadLine();
        if (string.Equals(answer, "Y", StringComparison.OrdinalIgnoreCase))
        {
            restart = true;
            break;
        }
        else if (string.Equals(answer, "N", StringComparison.OrdinalIgnoreCase))
        {
            break;
        }
        else
        {
            Console.WriteLine("Y 또는 N을 입력하세요.");
        }
    }
    if (!restart)
    {
        Console.WriteLine("게임을 종료합니다.");
        break;
    }
}
// 택택토 위치 번호 안내 함수
void PrintGuide()
{
    Console.WriteLine("=== 틱택토 게임 ===");
    Console.WriteLine();
    for (int i = 0; i < 3; i++)
    {
        for (int j = 0; j < 3; j++)
        {
            Console.Write($"{i * 3 + j + 1}");
            if (j < 2)
            {
                Console.Write(" | ");
            }
        }
        Console.WriteLine();
        if (i < 2)
        {
            Console.WriteLine("---------");
        }
    }
    Console.WriteLine();
}
// 틱택토 보드 출력 함수
void PrintBoard(char[,] board)
{
    Console.WriteLine("현재 보드:");
    for (int i = 0; i < 3; i++)
    {
        for (int j = 0; j < 3; j++)
        {
            Console.Write(board[i, j]);
            if (j < 2)
            {
                Console.Write(" | ");
            }
        }
        Console.WriteLine();
        if (i < 2)
        {
            Console.WriteLine("---------");
        }
    }
    Console.WriteLine();
}
// 승리 판정 함수
bool CheckWin(char[,] board, char turn)
{
    // 가로 검사
    for (int i = 0; i < 3; i++)
    {
        if (board[i, 0] == turn &&
            board[i, 1] == turn &&
            board[i, 2] == turn)
        {
            return true;
        }
    }
    // 세로 검사
    for (int i = 0; i < 3; i++)
    {
        if (board[0, i] == turn &&
            board[1, i] == turn &&
            board[2, i] == turn)
        {
            return true;
        }
    }
    // 왼쪽 위 → 오른쪽 아래 대각선
    if (board[0, 0] == turn &&
        board[1, 1] == turn &&
        board[2, 2] == turn)
    {
        return true;
    }
    // 오른쪽 위 → 왼쪽 아래 대각선
    if (board[0, 2] == turn &&
        board[1, 1] == turn &&
        board[2, 0] == turn)
    {
        return true;
    }
    return false;
}
