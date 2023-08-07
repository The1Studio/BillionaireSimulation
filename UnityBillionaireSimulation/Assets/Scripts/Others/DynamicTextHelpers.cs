namespace TheOneStudio.HyperCasual.Others
{
    using Cysharp.Threading.Tasks;
    using TMPro;

    public static class DynamicTextHelpers
    {
        public static async UniTask DialogueText(this TextMeshProUGUI textMeshProUGUI, string content, float delay = 0.05f)
        {
            textMeshProUGUI.text = "";
            foreach (var c in content)
            {
                textMeshProUGUI.text += c;
                await UniTask.Delay((int)(delay * 1000));
            }
        }

        public static async UniTask DialogueTextInTime(this TextMeshProUGUI textMeshProUGUI, string content, float duration = 3f)
        {
            var delay = duration / content.Length;
            await textMeshProUGUI.DialogueText(content, delay);
        }

        public static async UniTask CountingText(this TextMeshProUGUI textMeshProUGUI, long start, long end, float delay = 0.05f)
        {
            textMeshProUGUI.text = start.ToString();
            while (start < end)
            {
                start++;
                textMeshProUGUI.text = start.ToString();
                await UniTask.Delay((int)(delay * 1000));
            }
        }

        public static async UniTask CountingTextInTime(this TextMeshProUGUI textMeshProUGUI, long start, long end, float duration = 3f)
        {
            var delay = duration / (end - start);
            await textMeshProUGUI.CountingText(start, end, delay);
        }
    }
}