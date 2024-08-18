using UnityEngine;

public enum OpenTabType
{
    Skill,
    Farm,
    Dungeon
}

public class GuideManager : Singleton<GuideManager>
{
    public GameObject[] Locks;
    public GameObject GuideArrow;
    public GameObject PxmToggle;
    public int guideNum = 0;
    private UserData userData => SaveManager.Instance.userData;

    #region Tutorial Start Indexes
    public readonly int equipPixelmon = 1;
    public readonly int setAllPixelmon = 3;
    public readonly int nestLvUp = 6;
    public readonly int feedPixelmon = 10;
    public readonly int skillGatcha = 21;
    public readonly int skillEquip = 23;
    public readonly int seedFarm = 35;
    public readonly int harvestFarm = 45;
    public readonly int goldDg = 51;
    #endregion

    #region Unlock Indexes: 해금될 Q 번호
    private readonly int tabUpgr = 8;
    private readonly int tabSkill = 20;
    private readonly int tabFarm = 34;
    private readonly int tabFarm2 = 45;
    private readonly int tabDg = 50;
    #endregion

    protected override void Awake()
    {
        base.Awake();        
    }

    public void SetBottomLock()
    {
        Locks[0].SetActive(false);

        if (guideNum > tabUpgr)
        {
            Locks[1].SetActive(false);
        }

        if (guideNum > tabSkill)
        {
            Locks[2].SetActive(false);
            Locks[5].SetActive(false);
        }

        if (guideNum > tabFarm)
        {
            Locks[3].SetActive(false);
        }

        if (guideNum > tabDg)
        {
            Locks[4].SetActive(false);
        }
        
        if (SaveManager.Instance.userData.isSetArrowOnEgg)
            GuideArrow.SetActive(false);
    }

    public void SetArrow(GameObject obj, float addYPos = 0)
    {
        Vector3 currentPosition = obj.transform.position;
        Vector3 newPosition = new Vector3(currentPosition.x, currentPosition.y + 20f + addYPos, currentPosition.z);
        GuideArrow.transform.position = newPosition;
    }
}
