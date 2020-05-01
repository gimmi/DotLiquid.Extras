using System;
using System.Globalization;
using NUnit.Framework;

namespace DotLiquid.Extras.Tests
{
    public class ExtraFiltersTest
    {
        [Test]
        public void Should_join_tables()
        {
            var template = Template.Parse(@"
{% assign joined = Table1 | inner_join:Table2,'Id' -%}
{% for item in joined -%}
{{ item.Field1 }} | {{ item.Field2 }}
{% endfor -%}
            ");
            var hash = new Hash {
                {
                    "Table1", new[] {
                        new Hash {{"Id", 1}, {"Field1", "T1R1"}},
                        new Hash {{"Id", 2}, {"Field1", "T1R2"}},
                        new Hash {{"Id", 3}, {"Field1", "T1R3"}},
                    }
                }, {
                    "Table2", new[] {
                        new Hash {{"Id", 2}, {"Field2", "T2R2"}},
                        new Hash {{"Id", 3}, {"Field2", "T2R3"}},
                        new Hash {{"Id", 4}, {"Field2", "T1R4"}},
                    }
                }
            };
            var render = template.Render(new RenderParameters(CultureInfo.InvariantCulture) {
                Filters = new[] {typeof(ExtraFilters)},
                LocalVariables = hash
            });

            Assert.That(render, Is.EqualTo(@"
T1R2 | T2R2
T1R3 | T2R3
            "));
        }

        [Test]
        public void Should_wilter_by_clause()
        {
            var template = Template.Parse(@"
{% assign filtered = Table1 | where:'Category','cat' -%}
{% for item in filtered -%}
{{ item.Id }}
{% endfor -%}
            ");
            var hash = new Hash {
                {
                    "Table1", new[] {
                        new Hash {{"Id", 1}, {"Category", "dog"}},
                        new Hash {{"Id", 2}, {"Category", "cat"}},
                        new Hash {{"Id", 3}, {"Category", "pet"}},
                        new Hash {{"Id", 4}, {"Category", "cat"}},
                    }
                }
            };
            var render = template.Render(new RenderParameters(CultureInfo.InvariantCulture) {
                Filters = new[] {typeof(ExtraFilters)},
                LocalVariables = hash
            });

            Assert.That(render, Is.EqualTo(@"
2
4
            "));
        }
    }
}
