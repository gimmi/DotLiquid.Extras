using NUnit.Framework;

namespace DotLiquid.Extras.Tests
{
    public class WhereTest
    {
        [Test]
        public void Should_filter_string_value()
        {
            var data = new {
                Animals = new[] {
                    new { Name = "Poppy", Type = "dog"},
                    new { Name = "Molly", Type = "cat"},
                    new { Name = "Charlie", Type = "pet"},
                    new { Name = "Rosie", Type = "cat"},
                }
            };

            var template = @"
                {% assign filtered = Animals | where:'Type','cat' -%}
                {% for item in filtered -%}
                    {{ item.Name }} is a cat
                {% endfor -%}
            ";

            var expected = @"
                Molly is a cat
                Rosie is a cat
            ";

            TestUtils.AssertRender(data, template, expected);
        }

        [Test]
        public void Should_filter_numeric_value()
        {
            var data = new {
                Animals = new[] {
                    new { Name = "Poppy", Age = 1},
                    new { Name = "Molly", Age = 2},
                    new { Name = "Charlie", Age = 1},
                    new { Name = "Rosie", Age = 2},
                }
            };

            var template = @"
                {% assign filtered = Animals | where:'Age',2 -%}
                {% for item in filtered -%}
                    {{ item.Name }} is 2 years old
                {% endfor -%}
            ";

            var expected = @"
                Molly is 2 years old
                Rosie is 2 years old
            ";

            TestUtils.AssertRender(data, template, expected);
        }
    }
}
