using metaverse_template;
using System;
using UnityEngine;
using UnityEngine.UI;

public class FirebasePlugDemoCanvas : MonoBehaviour
{
    const string DemoBranch = "FirebasePlugDemoBranch";

    [Header("Panels")]
    [SerializeField] GameObject loadingPanel;
    [SerializeField] GameObject authPanel;
    [SerializeField] GameObject databasePanel;
    [SerializeField] GameObject storagePanel;
    [SerializeField] GameObject authAccountPanel;
    [Space]
    [SerializeField] TMPro.TextMeshProUGUI loadingDebugText;
    [Header("Auth")]
    [SerializeField] TMPro.TMP_InputField emailInput;
    [SerializeField] TMPro.TMP_InputField passInput;
    [SerializeField] TMPro.TextMeshProUGUI authDebugText;
    [SerializeField] Button loginButton;
    [SerializeField] Button signupButton;
    [Header("Auth_Account")]
    [SerializeField] Button changePassWordButton;
    [SerializeField] Button gotoDataBaseAuthButton;
    [SerializeField] Button deleteButton;
    [SerializeField] TMPro.TextMeshProUGUI authAccountDebugText;
    [Header("Database")]
    [SerializeField] GameObject telephoneCardPrefab;
    [SerializeField] TMPro.TMP_InputField nameInput;
    [SerializeField] TMPro.TMP_InputField telephoneInput;
    [SerializeField] TMPro.TextMeshProUGUI databaseDebugText;
    [SerializeField] TMPro.TextMeshProUGUI numberText;
    [SerializeField] Button addButton;
    [SerializeField] Button plusNmbButton;
    [SerializeField] Button minusNmbButton;
    [SerializeField] Button goToStorageBtn;
    [SerializeField] Button goToAuthAccountBtn;
    [SerializeField] RectTransform contentTelephoneCards;
    [Header("Database")]
    [SerializeField] Button uploadButton;
    [SerializeField] Button goToDBBtn;
    [SerializeField] RawImage imageAvatar;
    [SerializeField] TMPro.TextMeshProUGUI storageDebugText;
    [SerializeField] TMPro.TMP_InputField pathImageInput;

    void Start()
    {
        loginButton.onClick.AddListener(LogIn);
        signupButton.onClick.AddListener(SignUp);
        changePassWordButton.onClick.AddListener(ChangePassword);
        deleteButton.onClick.AddListener(RemoveUser);
        addButton.onClick.AddListener(AddTelephone);
        plusNmbButton.onClick.AddListener(PlusNumber);
        minusNmbButton.onClick.AddListener(MinusNumber);
        goToStorageBtn.onClick.AddListener(GotoStorage);
        goToAuthAccountBtn.onClick.AddListener(GotoAuthAccount);
        goToDBBtn.onClick.AddListener(GoToDatabasePanel);
        gotoDataBaseAuthButton.onClick.AddListener(GoToDatabasePanel);
        uploadButton.onClick.AddListener(UploadImage);
        

        GoToLoadingPanel();
        FirebasePlug.instance.StartFirebase(GoToAuthPanel, DebugError);
    }

    void DebugError(string error)
    {
        Debug.LogError(error);
        loadingDebugText.text = error;
        authDebugText.text = error;
        databaseDebugText.text = error;
        storageDebugText.text = error;
        authAccountDebugText.text = error;
    }
    #region auth functions
    void LogIn()
    {
        authDebugText.text = "";
        FirebasePlug.instance.LogIn(emailInput.text, passInput.text, GoToDatabasePanel, DebugError);
    }

    void SignUp()
    {
        authDebugText.text = "";
        FirebasePlug.instance.SignUp(emailInput.text, passInput.text, GoToDatabasePanel, DebugError);
    }

    void ChangePassword()
    {
        if (emailInput.text.Trim() == "")
        {
            DebugError("Write a email in the email imputfield to changes it password");
        }
        else
        {
            FirebasePlug.instance.ChangePassword(() => { authAccountDebugText.text = "A message has been sent to your email address to securely change your password"; }, DebugError);
        }
    }
    void RemoveUser()
    {
        FirebasePlug.instance.RemoveUserAuth(()=> { GoToAuthPanel(); },DebugError);
    }
    #endregion

    #region Goto
    void GoToLoadingPanel()
    {
        loadingPanel.SetActive(true);
        authPanel.SetActive(false);
        databasePanel.SetActive(false);
        storagePanel.SetActive(false);
        authAccountPanel.SetActive(false);
    }

    void GoToAuthPanel()
    {
        loadingPanel.SetActive(false);
        authPanel.SetActive(true);
        databasePanel.SetActive(false);
        storagePanel.SetActive(false);
        authAccountPanel.SetActive(false);
    }

    void GoToDatabasePanel()
    {
        loadingPanel.SetActive(false);
        authPanel.SetActive(false);
        storagePanel.SetActive(false);
        databaseDebugText.text = "";
        ReadTelephones();
        LoadNumber();
        databasePanel.SetActive(true);
        authAccountPanel.SetActive(false);
    }

    void GotoStorage()
    {
        databasePanel.SetActive(false);
        storageDebugText.text = "";
        storagePanel.SetActive(true);
        authAccountPanel.SetActive(false);
        if (imageAvatar.texture == null) LoadImage();
    }

    void GotoAuthAccount()
    {
        databasePanel.SetActive(false);
        storagePanel.SetActive(false);
        authAccountDebugText.text = "";
        authAccountPanel.SetActive(true);

    }
    #endregion

    #region database functions

    void AddTelephone()
    {
        string key = FirebasePlug.instance.CreateUniqueKeyDatabase(DemoBranch + "/Telephones");
        string json = "{ \"name\": \"" + nameInput.text + "\",\"telephone\": \"" + telephoneInput.text + "\"}";
        FirebasePlug.instance.WriteRawJsonDatabase(DemoBranch + "/Telephones/" + key, json,
            () =>
            {
                databaseDebugText.text = "Telephone added successfully";
                ReadTelephones();
            },
            DebugError);
    }

    void ReadTelephones()
    {
        FirebasePlug.instance.ReadValueDatabase(DemoBranch + "/Telephones/", LoadTelephones, DebugError);
    }

    void LoadTelephones(Firebase.Database.DataSnapshot data)
    {
        while (contentTelephoneCards.childCount > 0)
        {
            DestroyImmediate(contentTelephoneCards.GetChild(contentTelephoneCards.childCount - 1).gameObject);
        }

        foreach (Firebase.Database.DataSnapshot ds in data.Children)
        {
            GameObject g = GameObject.Instantiate(telephoneCardPrefab, contentTelephoneCards);
            g.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = ds.Child("name").Value.ToString();
            g.transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = ds.Child("telephone").Value.ToString();
            TelephoneCard tc = g.transform.GetChild(2).GetComponent<TelephoneCard>();
            tc.Key = ds.Key;
            tc.firebasePlugDemoCanvas = this;
        }
        float height = contentTelephoneCards.GetChild(0).GetComponent<RectTransform>().sizeDelta.y;
        contentTelephoneCards.sizeDelta = new Vector2(contentTelephoneCards.sizeDelta.x, height * contentTelephoneCards.childCount);

    }

    public void RemoveTelephone(string keyCard)
    {
        FirebasePlug.instance.RemoveValueDatabase(DemoBranch + "/Telephones/" + keyCard, ReadTelephones, DebugError);
    }

    void LoadNumber()
    {
        FirebasePlug.instance.ReadValueDatabase(DemoBranch + "/Number", UpdateNumberText, DebugError);
    }

    void PlusNumber()
    {
        ModifyNumber(1);
    }

    void MinusNumber()
    {
        ModifyNumber(-1);
    }

    void ModifyNumber(int addnumber)
    {
        FirebasePlug.instance.EditDatabaseValueSecurely(DemoBranch + "/Number", UpdateNumberText,
            DebugError, mutableData =>
            {
                if (mutableData.Value != null)
                {
                    int value = int.Parse(mutableData.Value.ToString());
                    value = value + addnumber;
                    mutableData.Value = value;
                    return Firebase.Database.TransactionResult.Success(mutableData);
                }
                else
                {
                    //To Retry
                    return Firebase.Database.TransactionResult.Success(mutableData);
                }
            });
    }

    void UpdateNumberText(Firebase.Database.DataSnapshot data)
    {
        if (data.Value != null)
        {
            numberText.text = data.Value.ToString();
        }
        else
        {
            FirebasePlug.instance.WriteRawValueDatabase(DemoBranch + "/Number", "0", () => { numberText.text = "0"; }, DebugError);
        }

    }

    #endregion

    #region storage functions

    void LoadImage()
    {
        FirebasePlug.instance.ReadFileStorage("VRTemplate_FirebaseDemoImage.jpg",
            (uri) =>
            {
                FirebasePlug.instance.ReadPictureStorage(uri.AbsoluteUri, SetImage, DebugError);
            },
            (error) => { Debug.Log(error); });
    }

    void SetImage(Texture2D textureImage)
    {
        imageAvatar.texture = textureImage;
    }

    void UploadImage()
    {
        string path = pathImageInput.text;
        if (path != null)
        {
            string[] pathArray = path.Split(".");
            string extension = pathArray[pathArray.Length - 1];
            if (!extension.Equals("jpg"))
            {
                DebugError("In this example you can just upload jpg images");
            }
            else
            {
                Firebase.Storage.MetadataChange newMetadata = new Firebase.Storage.MetadataChange();
                newMetadata.ContentType = "image/jpg";
                FirebasePlug.instance.UploadFileStorage(path, "VRTemplate_FirebaseDemoImage.jpg", (da) => { LoadImage(); }, (error) => { DebugError(error); }, newMetadata);
            }
        }
        else
        {
            DebugError("ERROR GetImage path = Null");
        }
    }
    #endregion
}
