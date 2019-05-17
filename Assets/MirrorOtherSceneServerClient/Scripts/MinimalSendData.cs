using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace MirrorSample
{
    /// <summary>
    /// サーバとクライアントで通信する独自型、センサーデータの形式を想定します。
    /// JsonUtilityのSerializableよりも型制約が厳しく、コレクション型を許容しない点に注意して下さい。
    /// https://vis2k.github.io/Mirror/Concepts/Communications/NetworkMessages.html
    /// </summary>
    [System.Serializable]
    public class MinimalSendData : MessageBase
    {
        public string name;
        public Vector2 axisData;
    }
}