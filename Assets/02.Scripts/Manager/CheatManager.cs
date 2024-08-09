using System.Numerics;
using UnityEngine;

public class CheatManager : Singleton<CheatManager>
{
    private void Update()
    {
        if (Input.anyKeyDown)
        {
            switch(Input.inputString)
            {
                case "s":
                case "S":
                    NextStage();
                    break;
                case "g":
                case "G":
                    BigInteger money = 33348332500000;
                    SaveManager.Instance.SetFieldData("gold", money, true);
                    break;
                case "d":
                case "D":
                    SaveManager.Instance.SetFieldData(nameof(SaveManager.Instance.userData.diamond), 10000, true);
                    break;                        
                default:
                    break;
            }
        }
    }

    private void NextStage()
    {
        StageManager.Instance.killCount = StageManager.Instance.data.nextStageCount;
    }
}