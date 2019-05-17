using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace MirrorSample
{
    /// <summary>
    /// サーバ（データ受信）シーンの最小サンプルスクリプト
    /// センサーデータ、として送られてきたデータをActionとして発火します。
    /// クライアントから届いたセンサーデータがリプレイログなのか、ジョイスティックなのか、キーボードの矢印キーなのかは分からない！
    /// </summary>
    public class MinimalServerSample : MonoBehaviour
    {
        
        //以下のActionをStaticにしているのは、サンプルとしてアクセスを簡単にするためです。
        //もちろんStaticじゃなくても大丈夫です。
        
        /// <summary>
        /// 外部からイベントを登録しておく為のAction、IPアドレスを一緒に通知します。切断再登録時の望ましい挙動に合わせてコネクションID使い分けてほしい
        /// stringはIPアドレス
        /// MinimalSendDataは受信した値
        /// </summary>
        public static Action<string, MinimalSendData> OnReceivedSensorDataWithClientIpAddress;

        /// <summary>
        /// 外部からイベントを登録しておく為のAction、コネクションIDを一緒に通知します。切断再登録時の望ましい挙動に合わせてIPアドレスと使い分けてほしい
        /// intは接続ID
        /// MinimalSendDataは受信した値
        /// </summary>
        public static Action<int, MinimalSendData> OnReceivedSensorDataWithClientConnectionID;


        /// <summary>
        /// 外部からイベントを登録しておく為のAction
        /// どこから来た値でも区別しないときはこれを使ってね
        /// MinimalSendDataは受信した値
        /// </summary>
        public static Action<MinimalSendData> OnReceivedSensorData;

        [Header("データ受信時に全てデバッグログに出力するか")] [SerializeField]
        private bool showDebugLog = true;

        void Start()
        {
            //サーバ側でMinimalSendDataをクライアントから受け取った時に何の関数が呼ばれるかを登録しておく
            //Startで指定しておくと親切
            NetworkServer.RegisterHandler<MinimalSendData>(ReceivedInfo);
        }

        /// <summary>
        /// データを受け取った時の挙動です。
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="receivedData"></param>
        private void ReceivedInfo(NetworkConnection conn, MinimalSendData receivedData)
        {
            if (showDebugLog)
            {
                Debug.Log(JsonUtility.ToJson(receivedData));
            }

            //ここでActionを生やしています。
            //メインスレッドから発火されます。IPで区別したかったり、コネクションIDで区別したかったり、区別したくなかったりするので
            //困ったらこういう感じに幾つかの型を定義しておくと、ライブラリを使う側にとって親切だと思います。
            OnReceivedSensorDataWithClientIpAddress?.Invoke(conn.address, receivedData);
            OnReceivedSensorDataWithClientConnectionID?.Invoke(conn.connectionId, receivedData);
            OnReceivedSensorData?.Invoke(receivedData);
        }
    }
}