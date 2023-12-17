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
        async public void Init()
        {
            // �ߺ� ȣ�� ����
            Backend.Match.OnMatchRelay -= ReceiveEvent;
            Backend.Match.OnMatchRelay += ReceiveEvent;
        }

        // ������ ������ ��Ŷ ����
        // ���������� �� ��Ŷ�� �޾� ��� Ŭ���̾�Ʈ(��Ŷ ���� Ŭ���̾�Ʈ ����)�� ��ε�ĳ���� ���ش�.
        public void SendDataToInGame<T>(T msg)
        {
            var byteArray = DataParser.DataToJsonData<T>(msg);
            Backend.Match.SendDataToInGameRoom(byteArray);
        }

        private void ReceiveEvent(MatchRelayEventArgs args)
        {
            // ������ �Ľ��� ���� �Ľ� �Ŵ��� �Լ� ȣ��
            BackEndManager.Instance.Parsing.OnRecieve(args);
        }

    }
}