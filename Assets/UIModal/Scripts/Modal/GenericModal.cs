namespace Gravitons.UI.Modal
{
    using DG.Tweening;
    using System.Collections;
    using UnityEngine;
    using UnityEngine.UI;

    public class GenericModal : Modal
    {
        [Tooltip("Modal title")]
        [SerializeField] protected Text m_Title;
        [Tooltip("Modal body")]
        [SerializeField] protected Text m_Body;
        [Tooltip("Buttons in the modal")]
        [SerializeField] protected Button[] m_Buttons;
        [Tooltip("How long between when each character is displayed")]
        [SerializeField] protected float textDelaySeconds = .1f;
        [Tooltip("Whether or not to display text gradually or all at once")]
        [SerializeField] protected bool displayTextGradually = false;
        [Tooltip("Duration of the opening scale animation")]
        [SerializeField] protected float appearTimeSeconds = .25f;

        [Header("Audio")]
        public bool playAudio = true;
        public AudioSource audSource;
        public AudioClip[] textBlips;
        public AudioClip buttonSound;
        public AudioClip closeWindowSound;

        private bool skipToEndOfText = false;

        /// <summary>
        /// Deactivate buttons in awake
        /// </summary>
        public void Awake()
        {
            for (int i = 0; i < m_Buttons.Length; i++)
            {
                m_Buttons[i].gameObject.SetActive(false);
            }

            StopAllCoroutines();
        }

        public override void Show(ModalContentBase modalContent, ModalButton[] modalButton)
        {
            GenericModalContent content = (GenericModalContent) modalContent;
            if (!displayTextGradually)
            {
                m_Title.text = content.Title;
                m_Body.text = content.Body;
            }
            else
            {
                StartCoroutine(displayTextGradual(m_Title, content.Title, textDelaySeconds));
                StartCoroutine(displayTextGradual(m_Body, content.Body, textDelaySeconds));
            }
            
            //Activate buttons and populate properties
            for (int i = 0; i < modalButton.Length; i++)
            {
                if (i >= m_Buttons.Length)
                {
                    Debug.LogError($"Maximum number of buttons of this modal is {m_Buttons.Length}. But {modalButton.Length} ModalButton was given. To display all buttons increase the size of the button array to at least {modalButton.Length}");
                    return;
                }
                m_Buttons[i].gameObject.SetActive(true);
                m_Buttons[i].GetComponentInChildren<Text>().text = modalButton[i].Text;
                int index = i; //Closure
                m_Buttons[i].onClick.AddListener(() =>
                {
                    if (modalButton[index].Callback != null)
                    {
                        audSource.PlayOneShot(buttonSound);
                        modalButton[index].Callback();
                    }
                    
                    if (modalButton[index].CloseModalOnClick)
                    {
                        LevelManager.Instance.resumeGame();
                        playCloseWindowSound();
                        Close();
                    }
                    m_Buttons[index].onClick.RemoveAllListeners();
                });
            }

            //animate appearance
            float fullYScale = transform.localScale.y;
            transform.localScale = new Vector3(transform.localScale.x, .01f, transform.localScale.z);
            transform.DOScaleY(fullYScale, appearTimeSeconds).SetUpdate(true);

            LevelManager.Instance.pauseGame();
        }

        //custom modfication
        //display text one character at a time asynchronously
        private IEnumerator displayTextGradual(Text textBody, string content, float waitTimeSeconds = .1f, bool replaceExistingText = true)
        {
            char[] contentSplit = content.ToCharArray();

            if (replaceExistingText)
                textBody.text = "";

            for (int i = 0; i < contentSplit.Length; i++)
            {
                //check if player clicked to skip text printout
                if (skipToEndOfText)
                {
                    skipToEndOfText = false;
                    textBody.text = content;
                    break;
                }

                textBody.text += contentSplit[i];

                if (playAudio && audSource != null)
                    audSource.PlayOneShot(textBlips[Mathf.FloorToInt(Random.Range(0, textBlips.Length - .01f))]);

                yield return new WaitForSecondsRealtime(waitTimeSeconds);
            }

            yield return null;
        }

        private void playCloseWindowSound()
        {
            GetComponentInParent<AudioSource>().PlayOneShot(closeWindowSound);
        }

        private void Update()
        {
            //listen for mouse click in case player tries to skip to the end of the text immediately
            if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Space))
            {
                skipToEndOfText = true;
            }
        }
    }
}