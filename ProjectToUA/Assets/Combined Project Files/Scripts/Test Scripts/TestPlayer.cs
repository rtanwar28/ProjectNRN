using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;


namespace NRN.Tales
{
	// Just for Initial...
	public class TestPlayer : Photon.PunBehaviour //, IPunObservable
	{
		public static GameObject LocalPlayerInstance;

		public void Awake()
		{
			if (photonView.isMine) 
			{
				LocalPlayerInstance = gameObject;
			}

			DontDestroyOnLoad (gameObject);
		}

		public void Start () 
		{
			
		}

		void CalledOnLevelLoaded(int level)
		{
            if (!Physics.Raycast(transform.position, -Vector3.up, 5f))
            {
                transform.position = new Vector3(0f, 5f, 0f);
            }
        }

		public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
		{
            if (stream.isWriting)
            {
                Vector3 pos = transform.position;
                stream.Serialize(ref pos);
            }
            else
            {
                Vector3 pos = Vector3.zero;
                stream.Serialize(ref pos);
            }
        }
	}
}
