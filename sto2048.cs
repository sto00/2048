using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sto2048 : MonoBehaviour
{
    // 통상적인(?) 필드의 위치
    public class Background
    {
        public readonly int garo = 4;
        public readonly int sero = 4;
        public readonly int space = 16;
    }

    // 하나의 블록에 대해서 자료형
    public class NumberBlock
    {
        public int NumberSize = 4096;
        public int NumberPositionX = 1;
        public int NumberPositionY = 1;
    }

    // 게임 내 존재하는 임의의 개수의 블록들의 데이터 
    List<NumberBlock> numberBlocks = null;


    // 게임 더미 출력의 예시
    // 0 0 0 2
    // 4 0 0 2
    // 0 0 0 0
    // 2 0 4 0

    void Start()
    {
        //랜덤한 위치 지정
        //위치는 가로좌표값과 세로좌표값으로 구성된 데이터
        //int a = Random.Range(0, 12);
        int NumberPositionX1 = Random.Range(0, 4);
        int NumberPositionY1 = Random.Range(0, 4);

        //위에서 지정된 위치 제외 후 랜덤한 위치 지정
        int NumberPositionX2 = Random.Range(0, 4);
        int NumberPositionY2 = Random.Range(0, 4);

        //만일 포지션 1과 포지션 2의 결과값이 같다면 다른 값이 나올 때까지 다시 무작위 위치 지정 실행
        while (NumberPositionY1 == NumberPositionY2
            && NumberPositionX1 == NumberPositionX2) // ()조건이 만족되는 한 , 즉 조건이 false될 때의 직전까지
        {
            NumberPositionX2 = Random.Range(0, 4);
            NumberPositionY2 = Random.Range(0, 4);
        }
        //-----------------------------
        //지정된 위치 확인
        //위치에 블록 생성
        // 쁠록이 생성되는 위치가 배경이므로, 배경을 준비한다

        //블록이 없는 상태 
        numberBlocks = new List<NumberBlock>();

        // 2개의 넘버블록 데이터 객체를 만든다.
        NumberBlock numberBlockFirst = new NumberBlock();
        NumberBlock numberBlockSecond = new NumberBlock();

        numberBlockFirst.NumberPositionX = NumberPositionX1;
        numberBlockFirst.NumberPositionY = NumberPositionY1;
        numberBlockFirst.NumberSize = 2; //* Random.Range(1, 3);

        numberBlockSecond.NumberPositionX = NumberPositionX2;
        numberBlockSecond.NumberPositionY = NumberPositionY2;
        numberBlockSecond.NumberSize = 2;//* Random.Range(1, 3);

        //numberBlocks.Add();
        numberBlocks.Add(numberBlockFirst);
        numberBlocks.Add(numberBlockSecond);
        // 배경에 블록 데이터를 추가한다.
        // [규칙] 블록은 같은 위치에 겹칠 수 없다.

        // -----------------------------

    }
    /*
    블록이 이동하는 방법 구현
    1. 블록이 이동하는 방향 지시
    2. 지시된 방향으로 블록이 전체가 이동
   
    2-1. 블록은 이동을 지시한 방향으로 이동한다. 만약 이동 후 앞에 여전히 0이 존재하거나 앞에 존재하는 것이 자기 자신과 같은 숫자일 경우
    한칸 더 이동을 반복한다.
    2-2 만약 같은 숫자와 합쳐질 경우 그 즉시 더 이상의 이동을 중지한다. 자기자신과 0이 아닌 다른 숫자가 앞에 있을 경우에도 이동을 중지한다.
    2-3 이동의 순서는 진행 방향으로 순차적으로 실행한다. (일단 구현 패스 후 원하는 대로 진행되지 않을 경우 할 부분)
  
   
    블록의 이동한 후 상태변화 구현
    1. 블록이 이동한 후 블록이 없는 곳에서 새로운 블록 출현
    2. 새로운 블록이 출현 후 두가지 조건을 만족하는지 체크
    1) 빈 공간이 없는지
    2) 합쳐질 수 있는 블록이 없는지
    합쳐질 수 있는 블록이 있는지는 연속되는 y좌표값과 x좌표값에 동일한 둘 이상의 숫자가 존재하는지 확인하는 것으로 확인한다.
    만일 존재하지 않는다면 합쳐질 수 있는 블록은 없는 것으로 취급한다.
    1), 2)의 조건이 모두 만족할 시 게임 종료. 
    */



    void MoveLeft()
    {
        //선언 , 조건(끝나는), 반복되는 내용
        for (int i = 0; i < numberBlocks.Count; i++)
        {
        here:
            int xToMove = numberBlocks[i].NumberPositionX;
            int yToMove = numberBlocks[i].NumberPositionY;

            bool isBlock = IsThereBlock(xToMove - 1, yToMove);


            if (numberBlocks[i].NumberPositionX <= 0)
            {
                Debug.Log("문제1");
                continue;
            }

            if (isBlock == false)
            {
                while ((isBlock == false) && numberBlocks[i].NumberPositionX > 0)
                {
                    numberBlocks[i].NumberPositionX = numberBlocks[i].NumberPositionX - 1;
                    isBlock = IsThereBlock(xToMove - 1, yToMove);
                    Debug.Log("문제2");
                }
                goto here;
            }
            else
            {
                // 이동해야 하는 곳에 있는 블록이 같은 사이즈인 경우
                NumberBlock targetBlock = GetBlockOrNull(xToMove - 1, yToMove);
                int size = numberBlocks[i].NumberSize;
                if (targetBlock.NumberSize == size)
                {
                    numberBlocks[i].NumberPositionX = numberBlocks[i].NumberPositionX - 1;
                    numberBlocks[i].NumberSize = numberBlocks[i].NumberSize * 2;
                    numberBlocks.Remove(targetBlock);
                    // 기존에 있던 블록을 삭제시키고 한칸 더 이동한다. 크기를 두배로 한다.
                    Debug.Log("문제3");
                }
                else
                {
                    continue;
                }
            }
        }
        Debug.Log("문제5");
        CheckGameOver();
        Debug.Log("문제6");
        MakeBlocks();
    }

    void MoveRight()
    {
        //선언 , 조건(끝나는), 반복되는 내용
        for (int i = 0; i < numberBlocks.Count; i++)
        {
        here:
            int xToMove = numberBlocks[i].NumberPositionX;
            int yToMove = numberBlocks[i].NumberPositionY;

            bool isBlock = IsThereBlock(xToMove + 1, yToMove);


            if (numberBlocks[i].NumberPositionX >= 3)
            {
                continue;
            }

            if (isBlock == false)
            {
                while ((isBlock == false) && numberBlocks[i].NumberPositionX < 3)
                {
                    numberBlocks[i].NumberPositionX = numberBlocks[i].NumberPositionX + 1;
                    isBlock = IsThereBlock(xToMove + 1, yToMove);
                }
                goto here;
            }
            else
            {
                // 이동해야 하는 곳에 있는 블록이 같은 사이즈인 경우
                NumberBlock targetBlock = GetBlockOrNull(xToMove + 1, yToMove);
                int size = numberBlocks[i].NumberSize;
                if (targetBlock.NumberSize == size)
                {
                    numberBlocks[i].NumberPositionX = numberBlocks[i].NumberPositionX + 1;
                    numberBlocks[i].NumberSize = numberBlocks[i].NumberSize * 2;
                    numberBlocks.Remove(targetBlock);
                    // 기존에 있던 블록을 삭제시키고 한칸 더 이동한다. 크기를 두배로 한다.
                }
                else
                {
                    continue;
                }
            }
        }
        CheckGameOver();
        MakeBlocks();
    }

    void MoveUp()
    {
        //선언 , 조건(끝나는), 반복되는 내용
        for (int i = 0; i < numberBlocks.Count; i++)
        {
        here:
            int xToMove = numberBlocks[i].NumberPositionX;
            int yToMove = numberBlocks[i].NumberPositionY;

            bool isBlock = IsThereBlock(xToMove, yToMove - 1);


            if (numberBlocks[i].NumberPositionY <= 0)
            {
                continue;
            }

            if (isBlock == false)
            {
                while ((isBlock == false) && numberBlocks[i].NumberPositionY >= 0)
                {
                    numberBlocks[i].NumberPositionY = numberBlocks[i].NumberPositionY - 1;
                    isBlock = IsThereBlock(xToMove, yToMove - 1);
                }
                goto here;
            }
            else
            {
                // 이동해야 하는 곳에 있는 블록이 같은 사이즈인 경우
                NumberBlock targetBlock = GetBlockOrNull(xToMove, yToMove - 1);
                int size = numberBlocks[i].NumberSize;
                if (targetBlock.NumberSize == size)
                {
                    numberBlocks[i].NumberPositionY = numberBlocks[i].NumberPositionY - 1;
                    numberBlocks[i].NumberSize = numberBlocks[i].NumberSize * 2;
                    numberBlocks.Remove(targetBlock);
                    // 기존에 있던 블록을 삭제시키고 한칸 더 이동한다. 크기를 두배로 한다.
                }
                else
                {
                    continue;
                }
                //이동을 수행하지 않고 이동 종료
            }
        }
        CheckGameOver();
        MakeBlocks();
    }

    void MoveDown()
    {
        //선언 , 조건(끝나는), 반복되는 내용
        for (int i = 0; i < numberBlocks.Count; i++)
        {
        here:
            int xToMove = numberBlocks[i].NumberPositionX;
            int yToMove = numberBlocks[i].NumberPositionY;

            bool isBlock = IsThereBlock(xToMove, yToMove + 1);


            if (numberBlocks[i].NumberPositionY >= 3)
            {
                continue;
            }

            if (isBlock == false)
            {
                while ((isBlock == false) && numberBlocks[i].NumberPositionY < 3)
                {
                    numberBlocks[i].NumberPositionY = numberBlocks[i].NumberPositionY + 1;
                    isBlock = IsThereBlock(xToMove, yToMove + 1);
                }
                goto here;
            }
            else
            {
                // 이동해야 하는 곳에 있는 블록이 같은 사이즈인 경우
                NumberBlock targetBlock = GetBlockOrNull(xToMove, yToMove + 1);
                int size = numberBlocks[i].NumberSize;
                if (targetBlock.NumberSize == size)
                {
                    numberBlocks[i].NumberPositionY = numberBlocks[i].NumberPositionY + 1;
                    numberBlocks[i].NumberSize = numberBlocks[i].NumberSize * 2;
                    numberBlocks.Remove(targetBlock);
                    // 기존에 있던 블록을 삭제시키고 한칸 더 이동한다. 크기를 두배로 한다.
                }
                else
                {
                    continue;
                }
                //이동을 수행하지 않고 이동 종료
            }
        }
        CheckGameOver();
        MakeBlocks();
    }


    // 검사 : 가려는 곳에 블록이 있는 지  없는 지 => bool (출력)
    // 가려는곳 = input , x , y => bool

    bool IsThereBlock(int x, int y)
    {
        // x,y 좌표에 해당하는 블록이 있는지 ?
        // 모든 블록을 한번씩 순회하면서 x,y가 입력값과 같은지 검사한다.
        for (int i = 0; i < numberBlocks.Count; i++)
        {
            // 반복문에서 한 블록의 좌표들 
            int xPos = numberBlocks[i].NumberPositionX;
            int yPos = numberBlocks[i].NumberPositionY;
            if (x == xPos && y == yPos)
            {
                return true;
            }
        }
        return false;
    }

    NumberBlock GetBlockOrNull(int x, int y)
    {
        // x,y 좌표에 해당하는 블록이 있는지 ?
        // 모든 블록을 한번씩 순회하면서 x,y가 입력값과 같은지 검사한다.
        for (int i = 0; i < numberBlocks.Count; i++)
        {
            // 반복문에서 한 블록의 좌표들 
            int xPos = numberBlocks[i].NumberPositionX;
            int yPos = numberBlocks[i].NumberPositionY;
            if (x == xPos && y == yPos)
            {
                return numberBlocks[i];
            }
        }
        return null;
    }


    //새로운 블록 생성과정

    //16개의 좌표값을 모두 확인한다. 만약 그 좌표값이 bool IsThereBlocks를 만족한다면 선택 대상에서 제외한다
    //나머지 좌표값에서 랜덤하게 하나의 대상을 선택하여 2 혹은 4중 랜덤한 사이즈를 가진 블록 하나를 출현시킨다.


    void MakeBlocks()
    {
        //임의의 x y 좌표의 값을 설정한다.
        int x = Random.Range(0, 4);
        int y = Random.Range(0, 4);
    //그 x y좌표에 블록이 있는지 확인한다.
    ReRoll:
        bool isBlock = IsThereBlock(x, y);
        if (isBlock == true)
        {
            //블록이 같다면 다시 돌린다.
            while (isBlock == true)
            {
                x = Random.Range(0, 4);
                y = Random.Range(0, 4);
                isBlock = IsThereBlock(x, y);
            }
            goto ReRoll;
        }
        else
        //아니라면 그 장소에 블럭을 하나 만든
        {
            NumberBlock numberBlockMaded = new NumberBlock();

            numberBlockMaded.NumberPositionX = x;
            numberBlockMaded.NumberPositionY = y;
            numberBlockMaded.NumberSize = 2 * Random.Range(1, 3);

            numberBlocks.Add(numberBlockMaded);
        }
    }

    void CheckGameOver()
    {
        bool TAAB = CheckTAAB();
        bool TCM = CheckTCM();

        if ((TAAB == true && TCM == false))
        {
            GameOver();
        }
    }

    void GameOver()
    {
        numberBlocks.Clear();
        Debug.LogError("Game Over");
        Start();
    }


    //0,0칸부터 x좌표가 3이 될 때까지 isblock이 true를 만족한다면 y를 1 더한 뒤 반복한다.
    //y=3일 때까지 이를 반복한다. 이 과정에서 단 한번도 false가 나오지 않는다면 만족한다.


    bool CheckTAAB()//ThereAreAllBlocks
    {
        for (int i = 0; i < 4; i++)
        {
            bool isBlock1 = IsThereBlock(i, 0);
            bool isBlock2 = IsThereBlock(i, 1);
            bool isBlock3 = IsThereBlock(i, 2);
            bool isBlock4 = IsThereBlock(i, 3);

            if ((isBlock1 == false) || (isBlock2 == false) || (isBlock3 == false) || (isBlock4 == false))
            {
                return false;
            }
        }
        return true;
    }



    //2) 합쳐질 수 있는 블록이 없는지 체크한다.



    bool CheckTCM()//TheyCanMove
    {
        for (int i = 0; i < numberBlocks.Count; i++)
        {
            int xPos = numberBlocks[i].NumberPositionX;
            int yPos = numberBlocks[i].NumberPositionY;
            int size = numberBlocks[i].NumberSize;

            NumberBlock targetBlockX = GetBlockOrNull(xPos + 1, yPos);
            NumberBlock targetBlockY = GetBlockOrNull(xPos, yPos + 1);


            for (xPos = 0; xPos > 3; xPos++)
            {
                if (targetBlockX.NumberSize == size)
                {
                    return true;
                }
            }
            for (yPos = 0; yPos > 3; yPos++)
            {
                if (targetBlockY.NumberSize == size)
                {
                    return true;
                }
            }
        }
        return false;
    }



    // Update is called once per frame
    void Update()
    {
        RenderGame(numberBlocks);

        if (Input.GetKeyUp(KeyCode.LeftArrow) == true)
        {
            MoveLeft();
        }
        if (Input.GetKeyUp(KeyCode.RightArrow) == true)
        {
            MoveRight();
        }
        if (Input.GetKeyUp(KeyCode.UpArrow) == true)
        {
            MoveUp();
        }
        if (Input.GetKeyUp(KeyCode.DownArrow) == true)
        {
            MoveDown();
        }
    }

    void RenderGame(List<NumberBlock> blocks)
    {
        var slotParent = gameObject.transform.GetChild(0);


        ////TODO 
        //// 4줄을 그리면 된다.
        //string[] lines = new string[4];

        //lines[0] = "0,0,0,0";
        //lines[1] = "0,0,0,0";
        //lines[2] = "0,0,0,0";
        //lines[3] = "0,0,0,0";

        // TODO 초기화
        for (int i = 0; i < 16; i++)
        {
            slotParent.GetChild(i).GetChild(0).gameObject.GetComponent<Text>().text = "";
        }

        // 블록 하나마다 반복한다. 
        for (int i = 0; i < blocks.Count; i++)
        {
            var block = blocks[i];
            var xPos = block.NumberPositionX;
            var yPos = block.NumberPositionY;

            var thisSlotIndex = 4 * yPos + xPos;
            slotParent.GetChild(thisSlotIndex).GetChild(0).gameObject.GetComponent<Text>().text = "" + block.NumberSize;
            // ----- legacy
            //var targetLine = lines[yPos];
            //    var targetLineArray = targetLine.Split(',');
            //    targetLineArray[xPos] = block.NumberSize.ToString();

            //    Debug.LogWarning("index{" + i + "} " + xPos + "," + yPos);
            //    lines[yPos] = targetLineArray[0] + "," + targetLineArray[1] + "," + targetLineArray[2] + "," + targetLineArray[3];
        }

        //Debug.Log("block count : " + blocks.Count);
        //Debug.Log(lines[0]);
        //Debug.Log(lines[1]);
        //Debug.Log(lines[2]);
        //Debug.Log(lines[3]);
    }
}
