using FluentAssertions;
using FluentValidation;
using NUnit.Framework;
using SnapshotIt.FluentValidation.UnitTests.TestObjects;

namespace SnapshotIt.FluentValidation.UnitTests
{
    [TestFixture]
    public class CaptureValidationExtensionsTests
    {
        private TestProductValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new TestProductValidator();
            Snapshot.Out.Create<TestProduct>(10);
        }

        [Test]
        public void PostWithValidation_ValidObject_ShouldPostSuccessfully()
        {
            // Arrange
            var validProduct = new TestProduct
            {
                Id = 1,
                Name = "Valid Product",
                Price = 99.99m,
                Description = "A valid product"
            };

            // Act
            Action act = () => Snapshot.Out.PostWithValidation(validProduct, _validator);

            // Assert
            act.Should().NotThrow();
            var capturedProduct = Snapshot.Out.Get<TestProduct>(0);
            capturedProduct.Should().NotBeNull();
            capturedProduct.Id.Should().Be(1);
            capturedProduct.Name.Should().Be("Valid Product");
        }

        [Test]
        public void PostWithValidation_InvalidObject_ShouldThrowValidationException()
        {
            // Arrange
            var invalidProduct = new TestProduct
            {
                Id = 0, // Invalid - must be greater than 0
                Name = "", // Invalid - required
                Price = -10m, // Invalid - must be positive
                Description = "Valid description"
            };

            // Act
            Action act = () => Snapshot.Out.PostWithValidation(invalidProduct, _validator);

            // Assert
            act.Should().Throw<ValidationException>()
                .Which.Errors.Should().HaveCount(3);
        }

        [Test]
        public async Task PostWithValidationAsync_ValidObject_ShouldPostSuccessfully()
        {
            // Arrange
            var validProduct = new TestProduct
            {
                Id = 2,
                Name = "Valid Async Product",
                Price = 199.99m,
                Description = "A valid async product"
            };

            // Act
            Func<Task> act = async () => await Snapshot.Out.PostWithValidationAsync(validProduct, _validator);

            // Assert
            await act.Should().NotThrowAsync();
            
            // Check that the product was posted (async methods might post to different collection)
            // Let's try to get it from the first position
            var allProducts = await Snapshot.Out.GetAllAsync<TestProduct>();
            allProducts.Should().NotBeNull();
            allProducts.Should().HaveCountGreaterThan(0);
        }

        [Test]
        public async Task PostWithValidationAsync_InvalidObject_ShouldThrowValidationException()
        {
            // Arrange
            var invalidProduct = new TestProduct
            {
                Id = -1, // Invalid
                Name = new string('a', 101), // Invalid - too long
                Price = 0, // Invalid - must be positive
                Description = "Valid description"
            };

            // Act
            Func<Task> act = async () => await Snapshot.Out.PostWithValidationAsync(invalidProduct, _validator);

            // Assert
            await act.Should().ThrowAsync<ValidationException>()
                .Where(ex => ex.Errors.Count() == 3);
        }

        [Test]
        public async Task PostWithValidationAsync_ValidArrayOfObjects_ShouldPostSuccessfully()
        {
            // Arrange
            var validProducts = new[]
            {
                new TestProduct { Id = 1, Name = "Product 1", Price = 10.00m, Description = "First product" },
                new TestProduct { Id = 2, Name = "Product 2", Price = 20.00m, Description = "Second product" }
            };

            // Act
            Func<Task> act = async () => await Snapshot.Out.PostWithValidationAsync(validProducts, _validator);

            // Assert
            await act.Should().NotThrowAsync();
            var capturedProducts = await Snapshot.Out.GetAllAsync<TestProduct>();
            capturedProducts.Should().HaveCountGreaterOrEqualTo(2);
        }

        [Test]
        public async Task PostWithValidationAsync_ArrayWithInvalidObjects_ShouldThrowValidationException()
        {
            // Arrange
            var mixedProducts = new[]
            {
                new TestProduct { Id = 1, Name = "Valid Product", Price = 10.00m, Description = "Valid" },
                new TestProduct { Id = 0, Name = "", Price = -5.00m, Description = "Invalid" } // Invalid
            };

            // Act
            Func<Task> act = async () => await Snapshot.Out.PostWithValidationAsync(mixedProducts, _validator);

            // Assert
            await act.Should().ThrowAsync<ValidationException>()
                .Where(ex => ex.Errors.Count() == 3); // 3 validation errors from the second product
        }

        [Test]
        public void Validate_ValidObject_ShouldReturnSuccessResult()
        {
            // Arrange
            var validProduct = new TestProduct
            {
                Id = 1,
                Name = "Valid Product",
                Price = 99.99m,
                Description = "A valid product"
            };

            // Act
            var result = Snapshot.Out.Validate(validProduct, _validator);

            // Assert
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Test]
        public void Validate_InvalidObject_ShouldReturnFailureResult()
        {
            // Arrange
            var invalidProduct = new TestProduct
            {
                Id = 0,
                Name = "",
                Price = -10m,
                Description = "Valid description"
            };

            // Act
            var result = Snapshot.Out.Validate(invalidProduct, _validator);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCount(3);
            result.Errors.Should().Contain(e => e.PropertyName == "Id");
            result.Errors.Should().Contain(e => e.PropertyName == "Name");
            result.Errors.Should().Contain(e => e.PropertyName == "Price");
        }

        [Test]
        public async Task ValidateAsync_ValidObject_ShouldReturnSuccessResult()
        {
            // Arrange
            var validProduct = new TestProduct
            {
                Id = 1,
                Name = "Valid Product",
                Price = 99.99m,
                Description = "A valid product"
            };

            // Act
            var result = await Snapshot.Out.ValidateAsync(validProduct, _validator);

            // Assert
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Test]
        public async Task ValidateAsync_InvalidObject_ShouldReturnFailureResult()
        {
            // Arrange
            var invalidProduct = new TestProduct
            {
                Id = 0,
                Name = "",
                Price = -10m,
                Description = "Valid description"
            };

            // Act
            var result = await Snapshot.Out.ValidateAsync(invalidProduct, _validator);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCount(3);
            result.Errors.Should().Contain(e => e.PropertyName == "Id");
            result.Errors.Should().Contain(e => e.PropertyName == "Name");
            result.Errors.Should().Contain(e => e.PropertyName == "Price");
        }

        [Test]
        public void CreateWithValidation_ShouldCreateCollectionWithValidationSupport()
        {
            // Arrange & Act
            Action act = () => Snapshot.Out.CreateWithValidation<TestProduct>(5, _validator);

            // Assert
            act.Should().NotThrow();
        }

        [Test]
        public void GetWithValidation_ValidObject_ShouldReturnValidatedObject()
        {
            // Arrange
            var validProduct = new TestProduct
            {
                Id = 1,
                Name = "Valid Product",
                Price = 99.99m,
                Description = "A valid product"
            };
            Snapshot.Out.Post(validProduct);

            // Act
            var result = Snapshot.Out.GetWithValidation(_validator, 0);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.Name.Should().Be("Valid Product");
        }

        [Test]
        public void GetWithValidation_InvalidObject_ShouldThrowValidationException()
        {
            // Arrange
            var invalidProduct = new TestProduct
            {
                Id = 0,
                Name = "",
                Price = -10m,
                Description = "Valid description"
            };
            Snapshot.Out.Post(invalidProduct);

            // Act
            Action act = () => Snapshot.Out.GetWithValidation(_validator, 0);

            // Assert
            act.Should().Throw<ValidationException>()
                .Which.Errors.Should().HaveCount(3);
        }

        [Test]
        public void GetWithValidation_WithPredicate_ValidObject_ShouldReturnValidatedObject()
        {
            // Arrange
            var validProduct = new TestProduct
            {
                Id = 1,
                Name = "Valid Product",
                Price = 99.99m,
                Description = "A valid product"
            };
            Snapshot.Out.Post(validProduct);

            // Act
            var result = Snapshot.Out.GetWithValidation(p => p.Id == 1, _validator);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.Name.Should().Be("Valid Product");
        }

        [Test]
        public void GetWithValidation_WithPredicate_InvalidObject_ShouldThrowValidationException()
        {
            // Arrange
            var invalidProduct = new TestProduct
            {
                Id = 0,
                Name = "",
                Price = -10m,
                Description = "Valid description"
            };
            Snapshot.Out.Post(invalidProduct);

            // Act
            Action act = () => Snapshot.Out.GetWithValidation(p => p.Id == 0, _validator);

            // Assert
            act.Should().Throw<ValidationException>()
                .Which.Errors.Should().HaveCount(3);
        }

        [Test]
        public async Task GetWithValidationAsync_ValidObject_ShouldReturnValidatedObject()
        {
            // Arrange
            var validProduct = new TestProduct
            {
                Id = 1,
                Name = "Valid Product",
                Price = 99.99m,
                Description = "A valid product"
            };
            await Snapshot.Out.PostAsync(validProduct);

            // Act
            var result = await Snapshot.Out.GetWithValidationAsync(0, _validator);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.Name.Should().Be("Valid Product");
        }

        [Test]
        public async Task GetWithValidationAsync_InvalidObject_ShouldThrowValidationException()
        {
            // Arrange
            var invalidProduct = new TestProduct
            {
                Id = 0,
                Name = "",
                Price = -10m,
                Description = "Valid description"
            };
            await Snapshot.Out.PostAsync(invalidProduct);

            // Act
            Func<Task> act = async () => await Snapshot.Out.GetWithValidationAsync(0, _validator);

            // Assert
            await act.Should().ThrowAsync<ValidationException>()
                .Where(ex => ex.Errors.Count() == 3);
        }

        [Test]
        public async Task GetAllWithValidationAsync_ValidObjects_ShouldReturnValidatedObjects()
        {
            // Arrange
            var validProducts = new[]
            {
                new TestProduct { Id = 1, Name = "Product 1", Price = 10.00m, Description = "First product" },
                new TestProduct { Id = 2, Name = "Product 2", Price = 20.00m, Description = "Second product" }
            };
            await Snapshot.Out.PostAsync(validProducts);

            // Act
            var result = await Snapshot.Out.GetAllWithValidationAsync(_validator);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCountGreaterOrEqualTo(2);
        }

        [Test]
        public async Task GetAllWithValidationAsync_InvalidObjects_ShouldThrowValidationException()
        {
            // Arrange
            var mixedProducts = new[]
            {
                new TestProduct { Id = 1, Name = "Valid Product", Price = 10.00m, Description = "Valid" },
                new TestProduct { Id = 0, Name = "", Price = -5.00m, Description = "Invalid" }
            };
            await Snapshot.Out.PostAsync(mixedProducts);

            // Act
            Func<Task> act = async () => await Snapshot.Out.GetAllWithValidationAsync(_validator);

            // Assert
            await act.Should().ThrowAsync<ValidationException>()
                .Where(ex => ex.Errors.Count() == 3);
        }

        [Test]
        public void GetAsListWithValidation_ValidObjects_ShouldReturnValidatedList()
        {
            // Arrange
            var validProducts = new[]
            {
                new TestProduct { Id = 1, Name = "Product 1", Price = 10.00m, Description = "First product" },
                new TestProduct { Id = 2, Name = "Product 2", Price = 20.00m, Description = "Second product" }
            };
            foreach (var product in validProducts)
            {
                Snapshot.Out.Post(product);
            }

            // Act
            var result = Snapshot.Out.GetAsListWithValidation(_validator);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<List<TestProduct>>();
            result.Should().HaveCountGreaterOrEqualTo(2);
        }

        [Test]
        public void GetAsListWithValidation_InvalidObjects_ShouldThrowValidationException()
        {
            // Arrange
            var invalidProduct = new TestProduct
            {
                Id = 0,
                Name = "",
                Price = -10m,
                Description = "Valid description"
            };
            Snapshot.Out.Post(invalidProduct);

            // Act
            Action act = () => Snapshot.Out.GetAsListWithValidation(_validator);

            // Assert
            act.Should().Throw<ValidationException>()
                .Which.Errors.Should().HaveCount(3);
        }

        [Test]
        public void GetAsListWithValidation_WithSize_ValidObjects_ShouldReturnValidatedList()
        {
            // Arrange
            var validProducts = new[]
            {
                new TestProduct { Id = 1, Name = "Product 1", Price = 10.00m, Description = "First product" },
                new TestProduct { Id = 2, Name = "Product 2", Price = 20.00m, Description = "Second product" }
            };
            foreach (var product in validProducts)
            {
                Snapshot.Out.Post(product);
            }

            // Act
            var result = Snapshot.Out.GetAsListWithValidation(5, _validator);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<List<TestProduct>>();
            result.Should().HaveCountGreaterOrEqualTo(2);
        }

        [Test]
        public void GetAsEnumerableWithValidation_ValidObjects_ShouldReturnValidatedEnumerable()
        {
            // Arrange
            var validProducts = new[]
            {
                new TestProduct { Id = 1, Name = "Product 1", Price = 10.00m, Description = "First product" },
                new TestProduct { Id = 2, Name = "Product 2", Price = 20.00m, Description = "Second product" }
            };
            foreach (var product in validProducts)
            {
                Snapshot.Out.Post(product);
            }

            // Act
            var result = Snapshot.Out.GetAsEnumerableWithValidation(_validator);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCountGreaterOrEqualTo(2);
        }

        [Test]
        public void GetAsEnumerableWithValidation_InvalidObjects_ShouldThrowValidationException()
        {
            // Arrange
            var invalidProduct = new TestProduct
            {
                Id = 0,
                Name = "",
                Price = -10m,
                Description = "Valid description"
            };
            Snapshot.Out.Post(invalidProduct);

            // Act
            Action act = () => Snapshot.Out.GetAsEnumerableWithValidation(_validator).ToList();

            // Assert
            act.Should().Throw<ValidationException>()
                .Which.Errors.Should().HaveCount(3);
        }

        [Test]
        public void GetAsSpanWithValidation_ValidObjects_ShouldReturnValidatedSpan()
        {
            // Arrange
            var validProducts = new[]
            {
                new TestProduct { Id = 1, Name = "Product 1", Price = 10.00m, Description = "First product" },
                new TestProduct { Id = 2, Name = "Product 2", Price = 20.00m, Description = "Second product" }
            };
            foreach (var product in validProducts)
            {
                Snapshot.Out.Post(product);
            }

            // Act
            var result = Snapshot.Out.GetAsSpanWithValidation(_validator);

            // Assert
            result.Length.Should().BeGreaterOrEqualTo(2);
        }

        [Test]
        public void GetAsSpanWithValidation_InvalidObjects_ShouldThrowValidationException()
        {
            // Arrange
            var invalidProduct = new TestProduct
            {
                Id = 0,
                Name = "",
                Price = -10m,
                Description = "Valid description"
            };
            Snapshot.Out.Post(invalidProduct);

            // Act
            Action act = () => Snapshot.Out.GetAsSpanWithValidation(_validator);

            // Assert
            act.Should().Throw<ValidationException>()
                .Which.Errors.Should().HaveCount(3);
        }

        [Test]
        public void GetAsReadonlySpanWithValidation_ValidObjects_ShouldReturnValidatedReadonlySpan()
        {
            // Arrange
            var validProducts = new[]
            {
                new TestProduct { Id = 1, Name = "Product 1", Price = 10.00m, Description = "First product" },
                new TestProduct { Id = 2, Name = "Product 2", Price = 20.00m, Description = "Second product" }
            };
            foreach (var product in validProducts)
            {
                Snapshot.Out.Post(product);
            }

            // Act
            var result = Snapshot.Out.GetAsReadonlySpanWithValidation(_validator);

            // Assert
            result.Length.Should().BeGreaterOrEqualTo(2);
        }

        [Test]
        public void GetAsReadonlySpanWithValidation_InvalidObjects_ShouldThrowValidationException()
        {
            // Arrange
            var invalidProduct = new TestProduct
            {
                Id = 0,
                Name = "",
                Price = -10m,
                Description = "Valid description"
            };
            Snapshot.Out.Post(invalidProduct);

            // Act
            Action act = () => Snapshot.Out.GetAsReadonlySpanWithValidation(_validator);

            // Assert
            act.Should().Throw<ValidationException>()
                .Which.Errors.Should().HaveCount(3);
        }
    }
}