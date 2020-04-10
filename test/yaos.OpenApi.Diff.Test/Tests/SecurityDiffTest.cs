using System;
using System.Linq;
using openapi_diff;
using Xunit;
using yaos.OpenApi.Diff.Tests._Base;

namespace yaos.OpenApi.Diff.Tests.Tests
{
    public class SecurityDiffTest : BaseTest
    {
        private readonly IOpenAPICompare _openAPICompare;
        private const string OPENAPI_DOC1 = "security_diff_1.yaml";
        private const string OPENAPI_DOC2 = "security_diff_2.yaml";
        private const string OPENAPI_DOC3 = "security_diff_3.yaml";

        public SecurityDiffTest(IOpenAPICompare openAPICompare)
        {3
            _openAPICompare = openAPICompare;
        }
        [Fact]
        public void TestDiffDifferent()
        {
            var changedOpenAPI = _openAPICompare.FromLocations(OPENAPI_DOC1, OPENAPI_DOC2);
            Assert.Equal(3, changedOpenAPI.ChangedOperations.Count);

            var changedOperation1 = changedOpenAPI
                .ChangedOperations
                .FirstOrDefault(x => x.PathUrl.Equals("/pet/{petId}"));
            Assert.NotNull(changedOperation1);
            Assert.False(changedOperation1.isCompatible());

            var changedSecurityRequirements1 = changedOperation1.SecurityRequirements;
            Assert.NotNull(changedSecurityRequirements1);
            Assert.False(changedSecurityRequirements1.isCompatible());
            Assert.Equal(1, changedSecurityRequirements1.getIncreased());
            Assert.Equal(1, changedSecurityRequirements1.getChanged());

            var changedSecurityRequirement1 = changedSecurityRequirements1.getChanged();

            ChangedSecurityRequirement changedSecurityRequirement1 =
                changedSecurityRequirements1.getChanged().get(0);
            assertThat(changedSecurityRequirement1.getChanged()).hasSize(1);
            ChangedSecuritySchemeScopes changedScopes1 =
                changedSecurityRequirement1.getChanged().get(0).getChangedScopes();
            assertThat(changedScopes1)
                .isNotNull()
                .satisfies(
                    stringListDiff->
                        assertThat(stringListDiff.getIncreased())
                            .hasSize(1)
                            .first()
                            .asString()
                            .isEqualTo("read:pets"));

            ChangedOperation changedOperation2 =
                changedOpenApi
                    .getChangedOperations()
                    .stream()
                    .filter(x->x.getPathUrl().equals("/pet3"))
                    .findFirst()
                    .get();
            assertThat(changedOperation2).isNotNull();
            assertThat(changedOperation2.isCompatible()).isFalse();
            ChangedSecurityRequirements changedSecurityRequirements2 =
                changedOperation2.getSecurityRequirements();
            assertThat(changedSecurityRequirements2).isNotNull();
            assertThat(changedSecurityRequirements2.isCompatible()).isFalse();
            assertThat(changedSecurityRequirements2.getChanged()).hasSize(1);
            ChangedSecurityRequirement changedSecurityRequirement2 =
                changedSecurityRequirements2.getChanged().get(0);
            assertThat(changedSecurityRequirement2.getChanged()).hasSize(1);
            ChangedOAuthFlow changedImplicitOAuthFlow2 =
                changedSecurityRequirement2.getChanged().get(0).getOAuthFlows().getImplicitOAuthFlow();
            assertThat(changedImplicitOAuthFlow2).isNotNull();
            assertThat(changedImplicitOAuthFlow2.isAuthorizationUrl()).isTrue();

            ChangedOperation changedOperation3 =
                changedOpenApi
                    .getChangedOperations()
                    .stream()
                    .filter(x->x.getPathUrl().equals("/pet/findByStatus2"))
                    .findFirst()
                    .get();
            assertThat(changedOperation3).isNotNull();
            assertThat(changedOperation3.isCompatible()).isTrue();
            ChangedSecurityRequirements changedSecurityRequirements3 =
                changedOperation3.getSecurityRequirements();
            assertThat(changedSecurityRequirements3).isNotNull();
            assertThat(changedSecurityRequirements3.getIncreased()).hasSize(1);
            SecurityRequirement securityRequirement3 = changedSecurityRequirements3.getIncreased().get(0);
            assertThat(securityRequirement3)
                .hasSize(1)
                .hasEntrySatisfying("petstore_auth", values->assertThat(values).hasSize(2));
        }

        [Fact]
        public void TestWithUnknownSecurityScheme()
        {
            Assert.Throws<ArgumentException>(() => _openAPICompare.FromLocations(OPENAPI_DOC3, OPENAPI_DOC3));
        }
    }
}
