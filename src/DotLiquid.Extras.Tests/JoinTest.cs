using NUnit.Framework;
using static DotLiquid.Extras.Tests.TestUtils;

namespace DotLiquid.Extras.Tests
{
    public class JoinTest
    {
        [Test]
        public void Should_perform_inner_join()
        {
            var data = new {
                Orders = new [] {
                    new { Id = 1, UserName = "Joe" },
                    new { Id = 2, UserName = "Jack" },
                    new { Id = 3, UserName = "Roger" },
                },
                OrderItems = new [] {
                    new { OrderId = 1, ItemDescr = "Monitor" },
                    new { OrderId = 2, ItemDescr = "Mouse" },
                    new { OrderId = 3, ItemDescr = "Keyboard" },
                }
            };

            var template = @"
                {% assign joined = Orders | x_join:OrderItems,'Id','OrderId' -%}
                {% for item in joined -%}
                    {{ item.UserName }} ordered a {{ item.ItemDescr }}
                {% endfor -%}
            ";

            var expected = @"
                Joe ordered a Monitor
                Jack ordered a Mouse
                Roger ordered a Keyboard
            ";

            AssertRender(data, template, expected);
        }

        [Test]
        public void Should_use_the_same_key()
        {
            var data = new {
                Orders = new [] {
                    new { Id = 1, UserName = "Joe" },
                    new { Id = 2, UserName = "Jack" },
                    new { Id = 3, UserName = "Roger" },
                },
                OrdersEx = new [] {
                    new { Id = 1, DeliveryDate = "yesterday" },
                    new { Id = 2, DeliveryDate = "today" },
                    new { Id = 3, DeliveryDate = "tomorrow" },
                }
            };

            var template = @"
                {% assign joined = Orders | x_join:OrdersEx,'Id' -%}
                {% for item in joined -%}
                    {{ item.UserName }} order delivered {{ item.DeliveryDate }}
                {% endfor -%}
            ";

            var expected = @"
                Joe order delivered yesterday
                Jack order delivered today
                Roger order delivered tomorrow
            ";

            AssertRender(data, template, expected);
        }

        [Test]
        public void Should_keep_outer_value_if_conflict()
        {
            var data = new {
                Outer = new [] {
                    new { Id = 1, SampleField = "OUTER" },
                },
                Inner = new [] {
                    new { Id = 1, SampleField = "INNER" },
                }
            };

            var template = @"
                {% assign joined = Outer | x_join:Inner,'Id' -%}
                {% for item in joined -%}
                    Value from {{ item.SampleField }} is kept
                {% endfor -%}
            ";

            var expected = @"
                Value from OUTER is kept
            ";

            AssertRender(data, template, expected);
        }
    }
}
