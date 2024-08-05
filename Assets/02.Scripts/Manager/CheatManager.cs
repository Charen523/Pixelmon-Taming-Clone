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
                    NextBossStage();
                    break;
                case "g":
                case "G":
                    SaveManager.Instance.SetFieldData("gold", 10000, true);
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

    private void NextBossStage()
    {
        StageManager.Instance.stageNum = 10;
    }
}