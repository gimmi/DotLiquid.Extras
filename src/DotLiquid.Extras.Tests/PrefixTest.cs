using NUnit.Framework;

namespace DotLiquid.Extras.Tests
{
    public class SelectPrefixTest
    {
        [Test]
        public void Should_transform_obj_to_array_of_its_properties()
        {
            var data = new {
                Animals = new [] {
                    new { Name = "Poppy", Type = "dog" },
                    new { Name = "Molly", Type = "cat" },
                    new { Name = "Charlie", Type = "pet" },
                    new { Name = "Rosie", Type = "cat" },
                }
            };

            var template = @"
                {% assign prefixeds = Animals | select_prefix:'Animal_' %}
                {% for prefixed in prefixeds -%}
                    {{ prefixed.Animal_Name }} is a {{ prefixed.Animal_Type }}
                {% endfor -%}
            ";

            var expected = @"
                Poppy is a dog
                Molly is a cat
                Charlie is a pet
                Rosie is a cat
            ";

            TestUtils.AssertRender(data, template, expected);
        }
    }
}
