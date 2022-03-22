using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
//using Firebase;
//using Firebase.Firestore;
//using Firebase.Extensions;
using System.Threading.Tasks;

[Serializable]
public class TokenObject
{
    public string rtcToken;

}

namespace agora_utilities
{
    public static class HelperClass
    {

        //토큰서버에 요청하고 응답을 문자열로 callback메서드에 전달
        public static IEnumerator FetchToken(string url, string channel, int userId, Action<string> callback = null)
        {
            UnityWebRequest request = UnityWebRequest.Get(
              string.Format("{0}/rtc/{1}/publisher/uid/{2}/", url, channel, userId)
            );
            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
                callback(null);
                yield break;
            }

            TokenObject tokenInfo = JsonUtility.FromJson<TokenObject>(
              request.downloadHandler.text
            );

            callback(tokenInfo.rtcToken);

            ////생성된 rtc토큰을 파이어베이스DB에 쓰기
            //WriteNewRtc();
            //void WriteNewRtc()
            //{
            //    FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
            //    DocumentReference docRef = db.Collection("RTC").Document();
            //    RTC rtc = new RTC()
            //    {
            //        rtc = tokenInfo.rtcToken


            //    };

            //    docRef.SetAsync(rtc);
            //    print("rtc 토큰 쓰기 완료");
            //}

        }

    }


}



//[FirestoreData]
//public class RTC
//{
//    [FirestoreProperty]
//    public string rtc { get; set; }
//    public string channel { get; set; }

//}

