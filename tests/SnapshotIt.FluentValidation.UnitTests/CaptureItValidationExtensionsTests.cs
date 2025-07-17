using FluentAssertions;
using FluentValidation;
using NUnit.Framework;
using SnapshotIt.FluentValidation.UnitTests.TestObjects;

namespace SnapshotIt.FluentValidation.UnitTests
{
    public class CaptureItValidationExtensionsTests
    {
        [Test]
        public async Task PostAsync_Should_Throw_When_ValidationFails()
        {
            var product = new TestProduct
            {
                Id = 0, // Invalid Id
                Name = "Test Product",
                Price = 10.0m,
                Description = "A test product"
            };

            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                var validator = new TestProductValidator();
                await Snapshot.Out.PostAsync(product, validator);
            }).Message.Should().Contain("Id must be greater than 0");

        }

        [Test]
        public async Task PostAsync_When_ValidationSucceeds()
        {
            var product = new TestProduct
            {
                Id = 1, // Valid Id
                Name = "Test Product",
                Price = 10.0m,
                Description = "A test product"
            };

            var validator = new TestProductValidator();
            await Snapshot.Out.PostAsync(product, validator);           
        }


        [Test]
        public async Task GetAsync_Should_Throw_When_ValidationFails()
        {
            await Snapshot.Out.PostAsync<TestProduct>(new TestProduct()
            {
                Id = 0, // Invalid Id
                Name = "Test Product",
                Price = 10.0m,
                Description = "A test product"
            });


            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                var validator = new TestProductValidator();
                await Snapshot.Out.GetAsync(0,validator);
            }).Message.Should().Contain("Id must be greater than 0");
        }
    }
}