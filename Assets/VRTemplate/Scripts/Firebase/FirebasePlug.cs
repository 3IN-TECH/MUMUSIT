using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Storage;
using System;
using System.Collections;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace metaverse_template
{

    public class FirebasePlug : MonoBehaviour
    {
        public static FirebasePlug instance;

        FirebaseAuth auth = null;
        FirebaseUser user;

        const long maxAllowedSize = 1 * 1024 * 1024; //1MB

        void Awake()
        {
            if (FirebasePlug.instance == null)
            {
                FirebasePlug.instance = this;
            }
            else
            {
                if (FirebasePlug.instance != this)
                {
                    Destroy(this.gameObject);
                }
            }
            this.transform.parent = null;
            DontDestroyOnLoad(this.gameObject);
        }

        #region StartFirebase and AutoLogin
        /// <summary>
        /// Starts the connection with firebase, this function must be called before any other function to work correctly.
        /// </summary>
        /// <param name="callback">This action will be called if the successful connection is confirmed.</param>
        /// <param name="fallback">This action will be called if something goes wrong. return a exception text</param>
        public void StartFirebase(Action callback, Action<string> fallback)
        {
            StartCoroutine(Connect(callback, null, fallback, false));
        }

        /// <summary>
        /// Starts the connection with firebase, this function must be called before any other function to work correctly.
        /// Extra: Try to do auto login
        /// </summary>
        /// <param name="callback">This action will be called if the successful connection is confirmed and not be possible auto login.</param>
        /// <param name="autoLoginCallback">This action will be called if the successful connection is confirmed and be possible auto login</param>
        /// <param name="fallback">This action will be called if something goes wrong. return a exception text</param>
        public void StartFirebaseAndTryAutoLogin(Action callback, Action autoLoginCallback, Action<string> fallback)
        {
            StartCoroutine(Connect(callback, autoLoginCallback, fallback, true));
        }

        IEnumerator Connect(Action callback, Action autoLoginCallback, Action<string> fallback, bool autoLogin)
        {
            if (auth == null)
            {
                Task<DependencyStatus> task = FirebaseApp.CheckAndFixDependenciesAsync();
                yield return new WaitUntil(() => task.IsCompleted);

                DependencyStatus dependencyStatus = task.Result;
                if (dependencyStatus == DependencyStatus.Available)
                {
                    auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
                    auth.StateChanged += AuthStateChanged;
                    AuthStateChanged(this, null);
                    if (autoLogin)
                    {
                        yield return new WaitForEndOfFrame();
                        StartCoroutine(CheckForAutoLogin(callback, autoLoginCallback, fallback));
                    }
                    else
                    {
                        callback?.Invoke();
                    }
                }
                else
                {
                    fallback?.Invoke("Could not resolve all Firebase dependencies: " + dependencyStatus);
                    // Firebase Unity SDK is not safe to use here.
                }
            }
            else
            {
                callback?.Invoke();
            }
        }

        void AuthStateChanged(object sender, EventArgs eventArgs)
        {
            if (auth.CurrentUser != user)
            {
                bool signedIn = auth.CurrentUser != null;
                if (!signedIn && user != null)
                {
                    //Sign Out
                }

                user = auth.CurrentUser;

                if (signedIn)
                {
                    //Sign In
                }

            }
        }

        IEnumerator CheckForAutoLogin(Action callback, Action autoLoginCallback, Action<string> fallback)
        {
            if (user != null)
            {
                Task reloadUserTask = user.ReloadAsync();
                yield return new WaitUntil(() => reloadUserTask.IsCompleted);

                //In case its value changes after the waiting period
                if (user != null)
                {
                    //AutoLogin
                    autoLoginCallback?.Invoke();
                }
                else
                {
                    callback?.Invoke();
                }
            }
            else
            {
                fallback?.Invoke("NullException user in CheckForAutoLogin");
            }
        }
        #endregion

        #region FirebaseAuth

        /// <summary>
        /// Authenticate the user by email and password.
        /// </summary>
        /// <param name="callback">This action will be called if the successful authentication.</param>
        /// <param name="fallback">This action will be called if something goes wrong. return a exception text</param>
        public void LogIn(string email, string pass, Action callback, Action<string> fallback)
        {
            if (auth != null)
            {
                if (email != null && email.Trim() != "")
                {
                    if (pass != null && pass.Trim() != "")
                    {
                        StartCoroutine(LogInCoroutine(email, pass, callback, fallback));
                    }
                    else
                    {
                        fallback?.Invoke("Trying LogIn(): pass Empty");
                    }
                }
                else
                {
                    fallback?.Invoke("Trying LogIn(): email Empty");
                }
            }
            else
            {
                fallback?.Invoke("ERROR LogIn: There is no authentication in this operation should not be called.");
            }

        }

        IEnumerator LogInCoroutine(string email, string pass, Action callback, Action<string> fallback)
        {
            if (auth != null)
            {
                Task<AuthResult> task = auth.SignInWithEmailAndPasswordAsync(email, pass);
                yield return new WaitUntil(() => task.IsCompleted);
                if (task.IsFaulted || task.IsCanceled)
                {
                    fallback?.Invoke(task.Exception.ToString());
                }
                else
                {
                    callback?.Invoke();
                }
            }
            else
            {
                fallback?.Invoke("NullException auth in LogIn");
            }
        }

        /// <summary>
        /// Create a new user authentication by email and password and also authenticate the user.
        /// </summary>
        /// <param name="callback">This action will be called if the successful creation and authentication.</param>
        /// <param name="fallback">This action will be called if something goes wrong. return a exception text</param>
        public void SignUp(string email, string pass, Action callback, Action<string> fallback)
        {
            if (auth != null)
            {
                if (email != null && email.Trim() != "")
                {
                    if (pass != null && pass.Trim() != "")
                    {
                        StartCoroutine(SignUpCoroutine(email, pass, callback, fallback));
                    }
                    else
                    {
                        fallback?.Invoke("Trying SignUp(): pass Empty");
                    }
                }
                else
                {
                    fallback?.Invoke("Trying SignUp(): email Empty");
                }
            }
            else
            {
                fallback?.Invoke("ERROR LogIn: There is no authentication in this operation should not be called.");
            }

        }

        IEnumerator SignUpCoroutine(string email, string pass, Action callback, Action<string> fallback)
        {
            if (auth != null)
            {
                Task<AuthResult> task = auth.CreateUserWithEmailAndPasswordAsync(email, pass);
                yield return new WaitUntil(() => task.IsCompleted);
                if (task.IsFaulted || task.IsCanceled)
                {
                    fallback?.Invoke(task.Exception.ToString());
                }
                else
                {
                    callback?.Invoke();
                }
            }
            else
            {
                fallback?.Invoke("NullException auth in SignUp");
            }
        }

        /// <summary>
        /// Sends an email to the currently logged in user's email address where they can change the account password.
        /// </summary>
        /// <param name="callback">This action will be called if the email was successful sended.</param>
        /// <param name="fallback">This action will be called if something goes wrong. return a exception text</param>
        public void ChangePassword(Action callback, Action<string> fallback)
        {
            if (user != null)
            {
                StartCoroutine(ChangePasswordCoroutine(callback, fallback));
            }
            else
            {
                fallback?.Invoke("NullException user in ChangePassword");
            }
        }

        IEnumerator ChangePasswordCoroutine(Action callback, Action<string> fallback)
        {
            if (user != null)
            {
                Task task = auth.SendPasswordResetEmailAsync(user.Email);
                yield return new WaitUntil(() => task.IsCompleted);
                if (task.IsFaulted || task.IsCanceled)
                {
                    fallback?.Invoke(task.Exception.ToString());
                }
                else if (task.IsCompleted)
                {
                    callback?.Invoke();
                }
            }
            else
            {
                fallback?.Invoke("NullException user in ChangePassword");
            }
        }

        /// <summary>
        /// Removes the current user's email address from firebase authentication.
        /// </summary>
        /// <param name="callback">This action will be called if the user was successful removed.</param>
        /// <param name="fallback">This action will be called if something goes wrong. return a exception text</param>
        public void RemoveUserAuth(Action callback, Action<string> fallback)
        {
            if (auth != null)
            {
                StartCoroutine(RemoveUserAuthCoroutine(callback, fallback));
            }
            else
            {
                fallback?.Invoke("ERROR RemoveUserAuth: There is no user logged in this operation should not be called.");
            }
        }


        IEnumerator RemoveUserAuthCoroutine(Action callback, Action<string> fallback)
        {
            if (auth != null)
            {
                Task task = auth.CurrentUser.DeleteAsync();
                yield return new WaitUntil(() => task.IsCompleted);
                if (task.IsFaulted || task.IsCanceled)
                {
                    fallback?.Invoke(task.Exception.ToString());
                }
                else if (task.IsCompleted)
                {
                    callback?.Invoke();
                }
            }
            else
            {
                fallback?.Invoke("NullException currentUser in RemoveUserAuth");
            }

        }

        /// <summary>
        /// Closes the current user session
        /// </summary>
        public void LogOut()
        {
            if (auth != null) auth.SignOut();
            user = null;
            Debug.Log("LOG OUT");
        }

        #endregion

        #region FirebaseDatabase

        /// <summary>
        /// Reads data from a specific branch of the database
        /// </summary>
        /// <param name="path">specific branch of the database</param>
        /// <param name="callback">This action will be called if all goes well, return the data of the database.</param>
        /// <param name="fallback">This action will be called if something goes wrong. return a exception text</param>
        public void ReadValueDatabase(string path, Action<DataSnapshot> callback, Action<string> fallback)
        {
            if (path != null && path.Trim() != "")
            {
                StartCoroutine(ReadValueDatabaseCoroutine(path, callback, fallback));
            }
            else
            {
                fallback?.Invoke("Trying ReadValueDatabase(): path Empty");
            }
        }

        IEnumerator ReadValueDatabaseCoroutine(string path, Action<DataSnapshot> callback, Action<string> fallback)
        {
            Task<DataSnapshot> task = FirebaseDatabase.DefaultInstance.GetReference(path).GetValueAsync();
            yield return new WaitUntil(() => task.IsCompleted);
            if (task.IsFaulted || task.IsCanceled)
            {
                fallback?.Invoke(task.Exception.ToString());
            }
            else if (task.IsCompleted)
            {
                callback?.Invoke(task.Result);
            }
        }

        /// <summary>
        /// Writes data in a specific branch of the database.
        /// The data will be pushed so that it can be used for editing and removing.
        /// </summary>
        /// <param name="path">specific branch of the database</param>
        /// <param name="json"></param>
        /// <param name="callback">This action will be called if data was successful writed in database.</param>
        /// <param name="fallback">This action will be called if something goes wrong. return a exception text</param>
        public void WriteRawJsonDatabase(string path, string json, Action callback, Action<string> fallback)
        {
            if (path != null && path.Trim() != "")
            {
                StartCoroutine(WriteRawJsonDatabaseCoroutine(path, json, callback, fallback));
            }
            else
            {
                fallback?.Invoke("Trying WriteRawJsonDatabase(): path Empty");
            }
        }

        IEnumerator WriteRawJsonDatabaseCoroutine(string path, string json, Action callback, Action<string> fallback)
        {
            Task task = FirebaseDatabase.DefaultInstance.GetReference(path).SetRawJsonValueAsync(json);
            yield return new WaitUntil(() => task.IsCompleted);
            if (task.IsFaulted || task.IsCanceled)
            {
                fallback?.Invoke(task.Exception.ToString());
            }
            else if (task.IsCompleted)
            {
                callback?.Invoke();
            }
        }

        /// <summary>
        /// Writes data in a specific branch of the database.
        /// The data will be pushed so that it can be used for editing and removing.
        /// </summary>
        /// <param name="path">specific branch of the database</param>
        /// <param name="value">new value to write</param>
        /// <param name="callback">This action will be called if data was successful writed in database.</param>
        /// <param name="fallback">This action will be called if something goes wrong. return a exception text</param>
        public void WriteRawValueDatabase(string path, string value, Action callback, Action<string> fallback)
        {
            if (path != null && path.Trim() != "")
            {
                StartCoroutine(WriteRawValueDatabaseCoroutine(path, value, callback, fallback));
            }
            else
            {
                fallback?.Invoke("Trying WriteRawValueDatabase(): path Empty");
            }
        }

        IEnumerator WriteRawValueDatabaseCoroutine(string path, string value, Action callback, Action<string> fallback)
        {
            Task task = FirebaseDatabase.DefaultInstance.GetReference(path).SetValueAsync(value);
            yield return new WaitUntil(() => task.IsCompleted);
            if (task.IsFaulted || task.IsCanceled)
            {
                fallback?.Invoke(task.Exception.ToString());
            }
            else if (task.IsCompleted)
            {
                callback?.Invoke();
            }
        }

        /// <summary>
        /// Removes a specific branch of the database.
        /// </summary>
        /// <param name="path">specific branch of the database</param>
        /// <param name="callback">This action will be called if the brach is successful removed.</param>
        /// <param name="fallback">This action will be called if something goes wrong. return a exception text</param>
        public void RemoveValueDatabase(string path, Action callback, Action<string> fallback)
        {
            if (path != null && path.Trim() != "")
            {
                StartCoroutine(RemoveValueDatabaseCoroutine(path, callback, fallback));
            }
            else
            {
                fallback?.Invoke("Trying RemoveValueDatabase(): path Empty");
            }
        }

        IEnumerator RemoveValueDatabaseCoroutine(string path, Action callback, Action<string> fallback)
        {
            Task task = FirebaseDatabase.DefaultInstance.GetReference(path).RemoveValueAsync();
            yield return new WaitUntil(() => task.IsCompleted);
            if (task.IsFaulted || task.IsCanceled)
            {
                fallback?.Invoke(task.Exception.ToString());
            }
            else if (task.IsCompleted)
            {
                callback?.Invoke();
            }
        }

        /// <summary>
        /// Securely edit a database branch using firebase RunTransaction.
        /// </summary>
        /// <param name="path">specific branch of the database</param>
        /// <param name="callback">This action will be called if the brach is successful edited.</param>
        /// <param name="fallback">This action will be called if something goes wrong. return a exception text</param>
        /// <param name="transaction">Safe mode switching function</param>
        public void EditDatabaseValueSecurely(string path, Action<DataSnapshot> callback, Action<string> fallback, Func<MutableData, TransactionResult> transaction)
        {
            if (path != null && path.Trim() != "")
            {
                StartCoroutine(EditDatabaseValueSecurelyCoroutine(path, callback, fallback, transaction));
            }
            else
            {
                fallback?.Invoke("Trying EditDatabaseValueSecurely(): path Empty");
            }
        }

        IEnumerator EditDatabaseValueSecurelyCoroutine(string path, Action<DataSnapshot> callback, Action<string> fallback, Func<MutableData, TransactionResult> transaction)
        {
            Task<DataSnapshot> task = FirebaseDatabase.DefaultInstance.GetReference(path).RunTransaction(transaction);
            yield return new WaitUntil(() => task.IsCompleted);
            if (task.IsFaulted || task.IsCanceled)
            {
                fallback?.Invoke(task.Exception.ToString());
            }
            else if (task.IsCompleted)
            {
                callback?.Invoke(task.Result);
            }
        }
        /* Transaction example
                    if (mutableData.Value != null)
                    {
                        IDictionary dic = (IDictionary)mutableData.Value;
                        int value = int.Parse(dic["value"].ToString());
                        int maximumValue = int.Parse(dic["maximumValue"].ToString());

                        if (maximumValue >= (value + toSum) && (toSum > 0))
                        {
                            dic["value"] = value + toSum;
                            mutableData.Value = dic;
                            return TransactionResult.Success(mutableData);
                        }
                        else  
                        { 
                            //To not Retry
                            return TransactionResult.Abort();
                        }
                    }
                    else
                    {
                        //To Retry
                        return TransactionResult.Success(mutableData);
                    }
        */

        /// <summary>
        /// Returns a unique key for a specific branch of the database.  
        /// </summary>
        /// <param name="path">specific branch of the database</param>
        public string CreateUniqueKeyDatabase(string path)
        {
            if (path != null) path = "";

            return FirebaseDatabase.DefaultInstance.GetReference(path).Push().Key;

        }
        #endregion



        #region FirebaseStorage

        /// <summary>
        /// Uploads a file into the Firebase Storage and returns its metadata.
        /// </summary>
        /// <param name="filePath">Path where the file to be uploaded is located</param>
        /// <param name="storagePath">specific branch of the storage</param>
        /// <param name="callback">This action will be called if the brach is successful edited, return its metadata</param>
        /// <param name="fallback">This action will be called if something goes wrong. return a exception text</param>
        public void UploadFileStorage(string filePath, string storagePath, Action<StorageMetadata> callback, Action<string> fallback, MetadataChange metadata = null)
        {
            if (filePath != null && filePath.Trim() != "")
            {
                if (File.Exists(filePath))
                {

                    if (storagePath != null && storagePath.Trim() != "")
                    {
                        StartCoroutine(UploadFileStorageCoroutine(File.ReadAllBytes(filePath), metadata, storagePath, callback, fallback));
                    }
                    else
                    {
                        fallback?.Invoke("Trying UploadFileStorage(): storagePath Empty");
                    }
                }
                else
                {
                    fallback?.Invoke("ERROR UploadFileStorage: no file in filePath");
                }
            }
            else
            {
                fallback?.Invoke("Trying UploadFileStorage(): filePath Empty");
            }
        }

        /// <summary>
        /// Uploads a file into the Firebase Storage and returns its metadata.
        /// </summary>
        /// <param name="bytes">the file to be uploaded</param>
        /// <param name="storagePath">specific branch of the storage</param>
        /// <param name="callback">This action will be called if the brach is successful edited, return its metadata</param>
        /// <param name="fallback">This action will be called if something goes wrong. return a exception text</param>
        public void UploadFileStorage(byte[] bytes, string storagePath, Action<StorageMetadata> callback, Action<string> fallback, MetadataChange metadata = null)
        {
            if (bytes != null)
            {
                if (storagePath != null && storagePath.Trim() != "")
                {
                    StartCoroutine(UploadFileStorageCoroutine(bytes, metadata, storagePath, callback, fallback));
                }
                else
                {
                    fallback?.Invoke("Trying UploadFileStorage(): storagePath Empty");
                }
            }
            else
            {
                fallback?.Invoke("Trying UploadFileStorage(): bytes Empty");
            }
        }

        IEnumerator UploadFileStorageCoroutine(byte[] bytes, MetadataChange metadata, string storagePath, Action<StorageMetadata> callback, Action<string> fallback)
        {
            Task<StorageMetadata> task = FirebaseStorage.DefaultInstance.RootReference.Child(storagePath).PutBytesAsync(bytes, metadata);
            yield return new WaitUntil(() => task.IsCompleted);
            if (task.IsFaulted || task.IsCanceled)
            {
                fallback?.Invoke(task.Exception.ToString());
            }
            else if (task.IsCompleted)
            {
                callback?.Invoke(task.Result);
            }
        }

        /// <summary>
        /// Read a file from storage, return its uri
        /// </summary>
        /// <param name="storagePath">specific branch of the storage</param>
        /// <param name="callback">This action will be called if the file is successful readed, return its uri</param>
        /// <param name="fallback">This action will be called if something goes wrong. return a exception text</param>
        public void ReadFileStorage(string storagePath, Action<Uri> callback, Action<string> fallback)
        {
            if (storagePath.Trim() != "" && storagePath != null)
            {
                StartCoroutine(ReadFileStorageCoroutine(storagePath, callback, fallback));
            }
            else
            {
                fallback?.Invoke("Trying ReadFileStorage(): storagePath Empty");
            }
        }

        IEnumerator ReadFileStorageCoroutine(string storagePath, Action<Uri> callback, Action<string> fallback)
        {
            if (storagePath.Trim() != "" && storagePath != null)
            {
                Task<Uri> task = FirebaseStorage.DefaultInstance.RootReference.Child(storagePath).GetDownloadUrlAsync();
                yield return new WaitUntil(() => task.IsCompleted);
                if (task.IsFaulted || task.IsCanceled)
                {
                    fallback?.Invoke(task.Exception.ToString());
                }
                else if (task.IsCompleted)
                {
                    callback?.Invoke(task.Result);
                }
            }
            else
            {
                fallback?.Invoke("Trying ReadFileStorage(): storagePath Empty");
            }
        }

        /// <summary>
        /// Read a picture from storage and returns it as a texture
        /// </summary>
        /// <param name="url">Storage location</param>
        /// <param name="callback">This action will be called if the picture is successful readed, return its texture</param>
        /// <param name="fallback">This action will be called if something goes wrong. return a exception text</param>
        public void ReadPictureStorage(string url, Action<Texture2D> callback, Action<string> fallback)
        {
            if (url.Trim() != "" && url != null)
            {
                StartCoroutine(ReadPictureStorageCoroutine(url, callback, fallback));
            }
            else
            {
                fallback?.Invoke("Trying ReadPictureStorage(): url Empty");
            }
        }

        IEnumerator ReadPictureStorageCoroutine(string url, Action<Texture2D> callback, Action<string> fallback)
        {
            if (url.Trim() != "" && url != null)
            {
                UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
                yield return request.SendWebRequest();
                if (request.result != UnityWebRequest.Result.Success)
                {
                    fallback?.Invoke("Trying GetTexture(): " + request.error.ToString());
                }
                else
                {
                    callback.Invoke(((DownloadHandlerTexture)request.downloadHandler).texture);
                }
            }
            else
            {
                fallback?.Invoke("Trying ReadPictureStorage(): url Empty");
            }
        }

        #endregion

    }
}