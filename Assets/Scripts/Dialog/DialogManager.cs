using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{

    public static DialogManager Instance { get; private set; }

    [Header("Dialog Reterences")]
    [SerializeField] private DialogDatabaseSO dialogDatabase;

    [Header("UI References")]
    [SerializeField] private GameObject dialogPanel;

    [SerializeField] private Image portraitImage;

    [SerializeField] private TextMeshProUGUI characterNameText;
    [SerializeField] private TextMeshProUGUI dialogText;
    [SerializeField] private Button NextButton;

    [Header("Dialog Settings")]
    [SerializeField] private float typingSpeed = 0.05f;
    [SerializeField] private bool useTyperiteEffect = true;

    private bool isTypeing = false;
    private Coroutine typingCoroutine;

    private DialogSO currentDialog;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        if(dialogDatabase !=null)
        {
            dialogDatabase.Initailize();        //초기화

        }
        else
        {
            Debug.LogError("Dialog Database is not assingedto Dialog Manager");
        }

        if(NextButton !=null)
        {
            NextButton.onClick.AddListener(NextDialog);
        }
        else
        {
            Debug.LogError("Next Button is Not assigned!");
        }

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //UI초기화 후 대화 ㅣ작
        CloseDialog();
        StartDialog(1);
    }

    public void StartDialog(int dialogId)
    {
        DialogSO dialog = dialogDatabase.GetDialongById(dialogId);
        if(dialog != null)
        {
            StartDialog(dialogId);
        }
        else
        {
            Debug.LogError($"Dialog with Id {dialogId} not found");
        }

    }

    public void StartDialog(DialogSO dialog)
    {
        if (dialog == null) return;

        currentDialog = dialog;
        ShowDialog();
        dialogPanel.SetActive(true);
    }

    public void ShowDialog()
    {
        if (currentDialog == null) return;
        characterNameText.text = currentDialog.characterName;

        if(useTyperiteEffect)
        {
            StartTypingEffect(currentDialog.text);
        }
        else
        {
            dialogText.text = currentDialog.text;
        }
      

        if (currentDialog.portrait != null)
        {
            portraitImage.sprite = currentDialog.portrait;
            portraitImage.gameObject.SetActive(true);
        }
        else if (!string.IsNullOrEmpty(currentDialog.portraitPath))
        {
            Sprite portrait = Resources.Load<Sprite>(currentDialog.portraitPath);
            Debug.Log(currentDialog.portraitPath);
            if(portrait !=null)
            {
                portraitImage.sprite = portrait;
                portraitImage.gameObject.SetActive(true);
            }
            else
            {
                Debug.LogWarning($"Portrait not found at path : {currentDialog.portraitPath}");
                portraitImage.gameObject.SetActive(false);
            }

        }
        else
        {
            portraitImage.gameObject.SetActive(false);
        }
    }

    public void CloseDialog()
    {
        dialogPanel.SetActive(false);
        currentDialog = null;
        stopTypingEffect();
    }

    public void NextDialog()
    {
        if(isTypeing)
        {
            stopTypingEffect();
            dialogText.text = currentDialog.text;
            isTypeing = false;
            return;
        }

        if (currentDialog != null && currentDialog.nextId>0)
        {
            DialogSO nextDialog = dialogDatabase.GetDialongById(currentDialog.nextId);
            if(nextDialog !=null)
            {
                currentDialog = nextDialog;
                ShowDialog();
            }
            else
            {
                CloseDialog();
            }
        }
        else
        {
            CloseDialog();
        }
    }

    private IEnumerator TypeText(string text)
    {
        dialogText.text = "";
        foreach (char c in text)
        {
            dialogText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTypeing = false;
    }

    private void stopTypingEffect()
    {
        if(typingCoroutine !=null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }
    }

    //타이핑 효과 함수 
    private void StartTypingEffect(string text)
    {
        isTypeing = true;
        if(typingCoroutine !=null)
        {
            StopCoroutine(typingCoroutine);

        }
        typingCoroutine = StartCoroutine(TypeText(text));
    }

}
