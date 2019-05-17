using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

namespace MirrorSample
{
    /// <summary>
    /// クライアント（データ送信）シーンの最小サンプルスクリプト
    /// センサーデータ、として今回は繋がっているゲームコントローラの左スティック、もしくはキーボードの矢印キーの情報をサーバ側に毎フレーム送信します。
    /// </summary>
    public class MinimalClientSample : MonoBehaviour
    {
        [Header("実際にセンサーデータを送信しているスクリプトです")]
        //こうしてインスペクタから値を確認出来るようにしておくとやや便利！
        [SerializeField]
        private Vector2 _inputAxisData = Vector2.zero;

        [SerializeField] private Text _sensorValueShowLabel = null;
        private bool _isSensorValueShowLabelNotNull;

        void Start()
        {
            _isSensorValueShowLabelNotNull = _sensorValueShowLabel != null;
            //ここで、センサーの初期化をする
            //今回はゲームコントローラの左スティックの値を直接使うので何もしない
        }

        void Update()
        {
            //ここで、センサーデータの取得をする
            //今回はゲームコントローラの左スティックの値を直接使うのでInput.GetAxisだけする
            _inputAxisData.x = Input.GetAxis("Horizontal");
            _inputAxisData.y = Input.GetAxis("Vertical");

            if (_isSensorValueShowLabelNotNull)
            {
                _sensorValueShowLabel.text = _inputAxisData.ToString();
            }

            //非接続時はセンサーデータを送らない
            if (NetworkClient.isConnected)
            {
                //本当はここで、変化が無ければ通信をしない、などの間引き処理や、タイマーによって送信頻度を変えると良い

                //今回はニュートラルだったら値を送らない、としています。これによって複数台の動作テストで
                //サーバ側が複数台のクライアント受信を見分けられることを確認しやすいかと思います。
                if (_inputAxisData.magnitude < 0.01f)
                {
                    return;
                }

                //ここで送るデータを作る、センサー情報を差し込む
                MinimalSendData minimalSendData = new MinimalSendData()
                {
                    //名前欄、何も考えずにdeviceNameでいいや…
                    name = SystemInfo.deviceName,
                    //センサーデータは直近のデータをそのまま使う
                    axisData = new Vector2(_inputAxisData.x, _inputAxisData.y)
                };

                //NetworkIdentityとかは無視して、直接サーバに向かって送信する！！！
                NetworkClient.Send(minimalSendData);
            }
        }
    }
}