using NUnit.Framework;
using static DotLiquid.Extras.Tests.TestUtils;

namespace DotLiquid.Extras.Tests
{
    public class SelectManyTest
    {
        [Test]
        public void Should_filter_string_value()
        {
            var data = new {
                Orders = new [] {
                    new {
                        Id = 1,
                        UserName = "Joe",
                        OrderItems = new [] {
                            new { ItemDescr = "Monitor" },
                            new { ItemDescr = "Mouse" }
                        }
                    },
                    new {
                        Id = 2,
                        UserName = "Jack",
                        OrderItems = new [] {
                            new { ItemDescr = "Keyboard" },
                            new { ItemDescr = "Webcam" }
                        }
                    }
                }
            };

            var template = @"
                {% assign items = Orders | select_many:'OrderItems' %}
                {% for item in items -%}
                    {{ item.UserName }} ordered a {{ item.ItemDescr }}
                {% endfor -%}
            ";

            var expected = @"
                Joe ordered a Monitor
                Joe ordered a Mouse
                Jack ordered a Keyboard
                Jack ordered a Webcam
            ";

            AssertRender(data, template, expected);
        }

        [Test]
        public void Should_keep_outer_value_if_conflict()
        {
            var data = new {
                Outers = new[] {
                    new {
                        MyField = "OuterValue",
                        Inners = new[] {
                            new {MyField = "InnerValue"},
                        }
                    }
                }
            };

            var template = @"
                {% assign items = Outers | select_many:'Inners' %}
                {% for item in items -%}
                    MyField is {{ item.MyField }}
                {% endfor %}
            ";

            var expected = @"
                MyField is OuterValue
            ";

            AssertRender(data, template, expected);
        }
    }
}
