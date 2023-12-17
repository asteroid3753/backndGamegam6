using BackEnd;
using BackEnd.Tcp;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace KSY
{
    public class BackEndInGame
    {
        public void Init()
        {
            // 중복 호출 방지
            Backend.Match.OnMatchRelay -= ReceiveEvent;
            Backend.Match.OnMatchRelay += ReceiveEvent;
        }

        // 서버로 데이터 패킷 전송
        // 서버에서는 이 패킷을 받아 모든 클라이언트(패킷 보낸 클라이언트 포함)로 브로드캐스팅 해준다.
        public void SendDataToInGame<T>(T msg)
        {
            Debug.Log("도도ㅗ도도");
            var byteArray = DataParser.DataToJsonData<T>(msg);
            Backend.Match.SendDataToInGameRoom(byteArray);
        }

        private void ReceiveEvent(MatchRelayEventArgs args)
        {

            Debug.Log("나나나나나");
            // 데이터 파싱을 위해 파싱 매니저 함수 호출
            BackEndManager.Instance.Parsing.OnRecieve(args);
        }

    }
}