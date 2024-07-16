using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class FieldSlot : SerializedMonoBehaviour
{
    #region data화 필요
    private FieldState currentFieldState; //저장 데이터.
    private PixelmonData pixelmonSetted;
    #endregion

    #region properties
    public FieldState CurrentFieldState 
    {
        get => currentFieldState;
        private set
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
    [SerializeField] private Button pixelmonBtn;
    [SerializeField] private Button FieldBtn;
    
    [SerializeField] private Sprite[] fieldStatusImgs;
    [SerializeField] private Image currentSprite;

    [SerializeField] private Sprite[] Icons;
    [SerializeField] private Image FieldIcon;

    [SerializeField] private TextMeshProUGUI timeTxt;
    #endregion

    public float leftTime; //시간 단위.

    private void Start()
    {
        CurrentFieldAction(currentFieldState);
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
                //관련 함수`
                currentSprite.sprite = fieldStatusImgs[0];
                FieldIcon.sprite = Icons[1];
                break;
            case FieldState.Empty: //빈 밭.
                FieldBtn.interactable = true;
                //관련 함수
                currentSprite.sprite = fieldStatusImgs[1];
                FieldIcon.sprite = Icons[2];
                break;
            case FieldState.Seeded: //작물이 심긴 밭.
                FieldBtn.interactable = false;
                currentSprite.sprite = fieldStatusImgs[2];
                FieldIcon.sprite = Icons[3];
                break;
            case FieldState.Harvest: //수확 준비된 밭.
                //관련 함수
                FieldBtn.interactable = true;
                currentSprite.sprite = fieldStatusImgs[3];
                FieldIcon.sprite = Icons[4];
                break;
            default:
                break;
        }
    }


    private void OnBuyFieldClicked()
    {

    }

    private void OnSeedFieldClicked()
    {

    }

    private void OnHarvestFieldClicked()
    {

    }
    
    //패시브로 성장속도가 증가한다면 남은시간 표시가 빨리 줄어들도록 하기.
    
    //pixelmon 배치된 후 농장 작업 들어가면 더 이상 못바꾸도록 버튼 비활성화 필요.

    //픽셀몬의 패시브 효과를 넣는걸 도와줄 메서드
}


//public class ItemSlot : MonoBehaviour
//{
//    public ItemData item;

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