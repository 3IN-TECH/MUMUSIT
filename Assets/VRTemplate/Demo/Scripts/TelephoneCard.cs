using UnityEngine;

public class TelephoneCard : MonoBehaviour
{
    public FirebasePlugDemoCanvas firebasePlugDemoCanvas;
    public string Key;


    public void DeleteThisCard()
    {
        firebasePlugDemoCanvas.RemoveTelephone(Key);
    }
}
