using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using RF.Photon;
using UnityEngine;

public class LoadingPlayer : MonoBehaviour, IPunObservable, IPunInstantiateMagicCallback
{
    public float loadingProgress = 0;
    
    // Start is called before the first frame update
    void Awake()
    {
        PhotonManager.Instance.loadingPlayers.Add(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(loadingProgress);
        }
        else
        {
            loadingProgress = (int)stream.ReceiveNext();
        }
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        info.Sender.TagObject = this.gameObject;
    }
}
