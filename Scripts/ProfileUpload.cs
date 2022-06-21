using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using Firebase;
using Firebase.Extensions;
using Firebase.Storage;
using UnityEditor;

public class ProfileUpload : MonoBehaviour
{
    public RawImage rawImage;
    FirebaseStorage storage;
    StorageReference storageReference;
    StorageReference reference;
    string path;



    //public void OpenFileExplorer()
    //{
    //    path = EditorUtility.OpenFilePanel("show all Images", "", "png");
    //    StartCoroutine(GetTexture());
    //}
    //IEnumerator GetTexture()
    //{
    //    UnityWebRequest www = UnityWebRequestTexture.GetTexture("file:///" + path);
    //    yield return www.SendWebRequest();
    //    if (www.isNetworkError || www.isHttpError)
    //    {
    //        Debug.Log(www.error);
    //    }
    //    else
    //    {
    //        Texture myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
    //        rawImage.texture = myTexture;

    //    }
    //}



}
