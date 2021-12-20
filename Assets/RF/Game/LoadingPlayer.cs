using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using RF.Photon;
using UnityEngine;

public class LoadingPlayer : MonoBehaviour, IPunObservable, IPunInstantiateMagicCallback
{
    public int index = -1;
    public float loadingProgress = 0;
    public bool isTimeOut = false;
    
    // Start is called before the first frame update
    void Awake()
    {
        PhotonManager.Instance.loadingPlayers.Add(this);
        StartCoroutine("TimeOutTimer");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator TimeOutTimer()
    {
        yield return new WaitForSeconds(180F);
        isTimeOut = true;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(loadingProgress);
            stream.SendNext(isTimeOut);
        }
        else
        {
            loadingProgress = (float)stream.ReceiveNext();
            isTimeOut = (bool) stream.ReceiveNext();
        }
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        info.Sender.TagObject = this.gameObject;
    }
}
