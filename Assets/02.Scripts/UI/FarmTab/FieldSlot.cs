using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using System.Threading;

public class FieldSlot : SerializedMonoBehaviour
{
    public FarmTab farmTab;
    public int myIndex;

    #region data화 필요
    //public FarmData farmData;
    [SerializeField] private FieldState currentFieldState; //저장 데이터.
    private PixelmonData farmer;
    #endregion

    #region properties
    public FieldState CurrentFieldState 
    {
        get => currentFieldState;
        set
        {
            if (currentFieldState != value)
            {
                currentFieldState = value;
                CurrentFieldAction(value);
            }
        }
    }
    #endregion

    #region UI
    [SerializeField] private Button pxmBtn;
    [SerializeField] private Button FieldBtn;
    
    [SerializeField] private Sprite[] fieldStatusImgs;
    [SerializeField] private Image currentSprite;

    [SerializeField] private Sprite[] Icons;
    [SerializeField] private Image FieldIcon;

    [SerializeField] private TextMeshProUGUI timeTxt;
    [SerializeField] private TextMeshProUGUI priceTxt;
    #endregion

    public int harvestHour;
    private float leftTime;
    private const string lastSaveTimeKey = "LastSaveTime";    
    
    public int yield; //수확량
    [SerializeField] private int price; //밭 가격
    

    private void Start()
    {
        CurrentFieldAction(currentFieldState);

        if (CurrentFieldState == FieldState.Seeded)
        {
            //CalculateRemainingTime();
        }
    }

    private void OnApplicationQuit()
    {
        if (CurrentFieldState == FieldState.Seeded)
        {
            
        }
    }

    private void CurrentFieldAction(FieldState state)
    {
        currentFieldState = state;

        switch(currentFieldState)
        {
            case FieldState.Locked: //잠김.
                FieldBtn.interactable = false;
                currentSprite.sprite = fieldStatusImgs[0];
                FieldIcon.sprite = Icons[0];
                break;
            case FieldState.Buyable: //구매가능
                FieldBtn.interactable = true;
                FieldBtn.onClick.AddListener(OnBuyFieldClicked);
                currentSprite.sprite = fieldStatusImgs[1];
                FieldIcon.sprite = Icons[1];
                priceTxt.text = $"가격: {price}다이아";
                break;
            case FieldState.Empty: //빈 밭.
                FieldBtn.interactable = true;
                FieldBtn.onClick.RemoveAllListeners();
                FieldBtn.onClick.AddListener(OnSeedFieldClicked);
                currentSprite.sprite = fieldStatusImgs[1];
                FieldIcon.sprite = Icons[2];
                priceTxt.gameObject.SetActive(false);
                break;
            case FieldState.Seeded: //작물이 심긴 밭.
                pxmBtn.interactable = false;
                FieldBtn.interactable = false;
                FieldBtn.onClick.RemoveAllListeners();
                currentSprite.sprite = fieldStatusImgs[2];
                FieldIcon.sprite = Icons[3];
                priceTxt.gameObject.SetActive(false);
                break;
            case FieldState.Harvest: //수확 준비된 밭.
                //관련 함수
                pxmBtn.interactable = true;
                FieldBtn.interactable = true;
                FieldBtn.onClick.AddListener(OnHarvestFieldClicked);
                currentSprite.sprite = fieldStatusImgs[3];
                FieldIcon.sprite = Icons[4];
                priceTxt.gameObject.SetActive(false);
                break;
            default:
                break;
        }
    }


    private void OnBuyFieldClicked()
    {
        //TODO: 구매 팝업 띄우는 것으로 대체하기
        if (price <= InventoryManager.Instance.userData.diamond)
        {
            InventoryManager.Instance.SetDeltaData("diamond", -price);
            CurrentFieldState = FieldState.Empty;
        }
        else
        {
            Debug.LogWarning("다이아 없음 Popup");
        }
    }

    private void OnSeedFieldClicked()
    {
        if (farmTab.PlantSeed())
        {
            CalculatePassiveEffect();
            CurrentFieldState = FieldState.Seeded;
            leftTime = harvestHour * 3600f;
            StartCoroutine(plantGrowing(leftTime));
        }
        else
        {
            //TODO: 씨앗 없음을 알리는 popup
            Debug.LogWarning("씨앗 없음 Popup");
        }
    }

    private void OnHarvestFieldClicked()
    {

    }

    private void CalculatePassiveEffect()
    {
        if (farmer == null)
        {
            harvestHour = 1; //2, 4, 6 중 랜덤.
            yield = 1; //1, 3, 10 중 랜덤.
        }
    }

    private IEnumerator plantGrowing(float countDown)
    {
        timeTxt.gameObject.SetActive(true);
        while (countDown > 0)
        {
            countDown -= Time.deltaTime;
            int hours = Mathf.FloorToInt(countDown / 3600f);
            int minutes = Mathf.FloorToInt((countDown % 3600f) / 60f);
            int seconds = Mathf.FloorToInt(countDown % 60f);
            timeTxt.text = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
            yield return null;
        }
        timeTxt.gameObject.SetActive(false);
        CurrentFieldState = FieldState.Harvest;
    }

    private int RandomNumGenerator()
    {
        return Random.Range(0, 3);
    }

    
    //pixelmon 배치된 후 농장 작업 들어가면 더 이상 못바꾸도록 버튼 비활성화 필요.

    //픽셀몬의 패시브 효과를 넣는걸 도와줄 메서드
}

//    public UIInventory inventory;
//    public Button button;
//    public Image icon;
//    public TextMeshProUGUI quatityText;
//    private Outline outline;

//    public int index;
//    public bool equipped;
//    public int quantity;

//    private void Awake()
//    {
//        outline = GetComponent<Outline>();
//    }

//    private void OnEnable()
//    {
//        outline.enabled = equipped;
//    }

//    public void Set()
//    {
//        icon.gameObject.SetActive(true);
//        icon.sprite = item.icon;
//        quatityText.text = quantity > 1 ? quantity.ToString() : string.Empty;

//        if (outline != null)
//        {
//            outline.enabled = equipped;
//        }
//    }

//    public void Clear()
//    {
//        item = null;
//        icon.gameObject.SetActive(false);
//        quatityText.text = string.Empty;
//    }

//    public void OnClickButton()
//    {
//        inventory.SelectItem(index);
//    }
//}