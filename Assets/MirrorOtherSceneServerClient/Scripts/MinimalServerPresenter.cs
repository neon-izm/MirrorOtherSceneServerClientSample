using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MirrorSample
{
    /// <summary>
    /// サーバがクライアントから受け取ったセンサーデータをどう扱うかのサンプル
    /// 実際はここでGameObjectを動かしたり、エフェクトの値を変えたりすると思いますが
    /// 何も考えずにCanvasのテキストを書き換えています。
    /// </summary>
    public class MinimalServerPresenter : MonoBehaviour
    {
        [SerializeField] private Text ipAddressLabel = null;
        [SerializeField] private Text receivedSensorDataLabel = null;

        //enable時にイベント追加しておく
        private void OnEnable()
        {
            MinimalServerSample.OnReceivedSensorDataWithClientIpAddress += OnReceivedSensorDataWithClientIpAddress;
        }


        /// <summary>
        /// ここで本来はキャラの動きを制御したりシーン内の何かの値を制御します！
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="data"></param>
        private void OnReceivedSensorDataWithClientIpAddress(string ipAddress, MinimalSendData data)
        {
            ipAddressLabel.text = ipAddress;
            receivedSensorDataLabel.text = data.name + " vec:" + data.axisData;
        }
    }
}