using System.Collections.Generic;

namespace MagazineAPI
{
    public class PostResult
    {
        public string totalTime { get; set; }
        public bool answerCorrect { get; set; }
        public List<string> shouldBe { get; set; }
    }
}
