using NUnit.Framework;

namespace DotLiquid.Extras.Tests
{
    public class WhereTest
    {
        [Test]
        public void Should_wilter_by_clause()
        {
            var data = new {
                Table1 = new[] {
                    new { Id = 1, Category = "dog"},
                    new { Id = 2, Category = "cat"},
                    new { Id = 3, Category = "pet"},
                    new { Id = 4, Category = "cat"},
                }
            };

            var template = @"
                {% assign filtered = Table1 | where:'Category','cat' -%}
                {% for item in filtered -%}
                    {{ item.Id }}
                {% endfor -%}
            ";

            var expected = @"
                2
                4
            ";

            TestUtils.AssertRender(data, template, expected);
        }
    }
}