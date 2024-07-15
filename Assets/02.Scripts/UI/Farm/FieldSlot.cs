using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class FieldSlot : SerializedMonoBehaviour
{
    public FieldState currentFieldState;
    //수확하는데 걸리는 시간 -> 작물 종류 늘어나면 거기에 변수추가?
    //현재 남은 시간
    //패시브로 성장속도가 증가한다면 남은시간 표시가 빨리 줄어들도록 하기.
    //다른 픽셀몬으로 바뀐 순간 성장속도만 다시 느려지도록.

    
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