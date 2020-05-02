using NUnit.Framework;
using static DotLiquid.Extras.Tests.TestUtils;

namespace DotLiquid.Extras.Tests
{
    public class InnerJoinTest
    {
        [Test]
        public void Should_perform_inner_join()
        {
            var data = new {
                Table1 = new [] {
                    new { Id = 1, Field1 = "T1R1" },
                    new { Id = 2, Field1 = "T1R2" },
                    new { Id = 3, Field1 = "T1R3" },
                },
                Table2 = new [] {
                    new { Id = 2, Field2 = "T2R2" },
                    new { Id = 3, Field2 = "T2R3" },
                    new { Id = 4, Field2 = "T1R4" },
                }
            };

            var template = @"
                {% assign joined = Table1 | inner_join:Table2,'Id' -%}
                {% for item in joined -%}
                    {{ item.Field1 }} | {{ item.Field2 }}
                {% endfor -%}
            ";

            var expected = @"
                T1R2 | T2R2
                T1R3 | T2R3
            ";

            AssertRender(data, template, expected);
        }
    }
}
