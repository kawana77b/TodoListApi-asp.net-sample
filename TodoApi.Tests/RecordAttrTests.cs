#if DEBUG

using TodoApi.Data;
using TodoApi.Models.Helper;

namespace TodoApi.Tests
{
    public class RecordAttrTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test(Description = "The model used for the DB should indicate this by applying RecordAttribute")]
        public void GevenRecotdAttrTest()
        {
            var checkResults = new List<IEnumerable<string>>
            {
                RecordAttributeUtil.CheckRecordAttributeAll<AppDbContext>()
            };
            var attrs = checkResults.SelectMany(x => x);

            if (attrs.Count() > 0)
                Assert.Fail($"The following properties should be attributed [Record]: {string.Join(", ", attrs)}");

            Assert.Pass();
        }
    }
}

#endif