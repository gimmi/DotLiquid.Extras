using NUnit.Framework;

namespace DotLiquid.Extras.Tests
{
    public class ValueArrayTest
    {
        [Test]
        public void Should_transform_obj_to_array_of_its_properties()
        {
            var data = new {
                Animals = new {
                    Poppy = new { Type = "dog", Name = "discarded" },
                    Molly = new { Type = "cat", Name = "discarded" },
                    Charlie = new { Type = "pet", Name = "discarded" },
                    Rosie = new { Type = "cat", Name = "discarded" },
                }
            };

            var template = @"
                {% assign animal_ary = Animals | x_value_array:'Name' %}
                {% for animal in animal_ary -%}
                    {{ animal.Name }} is a {{ animal.Type }}
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

        [Test]
        public void Should_discard_key_if_not_specified()
        {
            var data = new {
                Animals = new {
                    Poppy = new { Type = "dog"},
                    Molly = new { Type = "cat"},
                    Charlie = new { Type = "pet"},
                    Rosie = new { Type = "cat"},
                }
            };

            var template = @"
                {% assign animal_ary = Animals | x_value_array %}
                {% for animal in animal_ary -%}
                    There is a {{ animal.Type }}
                {% endfor -%}
            ";

            var expected = @"
                There is a dog
                There is a cat
                There is a pet
                There is a cat
            ";

            TestUtils.AssertRender(data, template, expected);
        }
    }
}
