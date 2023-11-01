namespace Gravitons.UI.Modal
{
    using UnityEngine.UI;
    using UnityEngine;

    /// <summary>
    /// Manages the UI in the demo scene
    /// </summary>
    public class DemoManager : MonoBehaviour
    {
        public Button button;
        public Button button2;
        public Image image;

        private void Start()
        {
            button.onClick.AddListener(ShowModal);
            button2.onClick.AddListener(ShowModalWithCallback);
        }

        /// <summary>
        /// Show a simple modal
        /// </summary>
        private void ShowModal()
        {
            ModalManager.Show("Did You Know??", "The FitnessGram™ Pacer Test is a multistage aerobic" +
                " capacity test that progressively gets more difficult as it continues. The 20 meter" +
                " pacer test will begin in 30 seconds. Line up at the start. The running speed starts" +
                " slowly, but gets faster each minute after you hear this signal. [beep] A single lap" +
                " should be completed each time you hear this sound. [ding] Remember to run in a straight line," +
                " and run as long as possible. The second time you fail",
                new[] { new ModalButton() { Text = "OK" } });
        }

        /// <summary>
        /// Shows a modal with callback
        /// </summary>
        private void ShowModalWithCallback()
        {
            ModalManager.Show("Change Background Color", "Change background color to a random color",
                new[] { new ModalButton() { Text = "YES", Callback = ChangeColor }, new ModalButton() { Text = "NO" } });
        }

        /// <summary>
        /// Change background color to a random color
        /// </summary>
        private void ChangeColor()
        {
            image.color = new Color(Random.value, Random.value, Random.value);
        }

        private void OnDestroy()
        {
            button.onClick.RemoveListener(ShowModal);
            button2.onClick.RemoveListener(ShowModalWithCallback);
        }
    }
}