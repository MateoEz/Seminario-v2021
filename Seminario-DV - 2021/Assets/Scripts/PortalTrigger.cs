using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTrigger : MonoBehaviour
{
    [SerializeField]
    private GameObject exit;

    [SerializeField] private GameObject portalFade;
    private static readonly int FadeOut = Animator.StringToHash("FadeOut");

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PlayerView playerView) && !other.gameObject.GetComponent<DashPlayerFeedback>())
        {
            AudioMaster.Instance.PlayClip("SonidoPortalCorto",0.3f);
            StartCoroutine(OnEnterPortalCoroutine(playerView));

        }
    }

    private IEnumerator OnEnterPortalCoroutine(PlayerView playerView)
    {
        FindObjectOfType<CameraView>().gameObject.AddComponent<AudioListener>();
        playerView.OnEnterPortal();
        portalFade.SetActive(true);
        yield return new WaitForSeconds(1);
        playerView.Transform.position = exit.transform.position;
        portalFade.GetComponent<Animator>().SetTrigger(FadeOut);
        yield return new WaitForSeconds(1);
        Destroy(FindObjectOfType<CameraView>().gameObject.GetComponent<AudioListener>());
        playerView.gameObject.SetActive(true);
        playerView.DashPlayerFeedback.DisablePlayerFeedback();
    }
}

