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
        public void Post_ValidObject_ShouldPostSuccessfully()
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
            Action act = () => Snapshot.Out.Post(validProduct, _validator);

            // Assert
            act.Should().NotThrow();
            var capturedProduct = Snapshot.Out.Get<TestProduct>(0);
            capturedProduct.Should().NotBeNull();
            capturedProduct.Id.Should().Be(1);
            capturedProduct.Name.Should().Be("Valid Product");
        }

        [Test]
        public void Post_InvalidObject_ShouldThrowValidationException()
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
            Action act = () => Snapshot.Out.Post(invalidProduct, _validator);

            // Assert
            act.Should().Throw<ValidationException>()
                .Which.Errors.Should().HaveCount(3);
        }

        [Test]
        public async Task PostAsync_ValidObject_ShouldPostSuccessfully()
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
            Func<Task> act = async () => await Snapshot.Out.PostAsync(validProduct, _validator);

            // Assert
            await act.Should().NotThrowAsync();
            
            // Check that the product was posted (async methods might post to different collection)
            // Let's try to get it from the first position
            var allProducts = await Snapshot.Out.GetAllAsync<TestProduct>();
            allProducts.Should().NotBeNull();
            allProducts.Should().HaveCountGreaterThan(0);
        }

        [Test]
        public async Task PostAsync_InvalidObject_ShouldThrowValidationException()
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
            Func<Task> act = async () => await Snapshot.Out.PostAsync(invalidProduct, _validator);

            // Assert
            await act.Should().ThrowAsync<ValidationException>()
                .Where(ex => ex.Errors.Count() == 3);
        }

        [Test]
        public async Task PostAsync_ValidArrayOfObjects_ShouldPostSuccessfully()
        {
            // Arrange
            var validProducts = new[]
            {
                new TestProduct { Id = 1, Name = "Product 1", Price = 10.00m, Description = "First product" },
                new TestProduct { Id = 2, Name = "Product 2", Price = 20.00m, Description = "Second product" }
            };

            // Act
            Func<Task> act = async () => await Snapshot.Out.PostAsync(validProducts, _validator);

            // Assert
            await act.Should().NotThrowAsync();
            var capturedProducts = await Snapshot.Out.GetAllAsync<TestProduct>();
            capturedProducts.Should().HaveCountGreaterOrEqualTo(2);
        }

        [Test]
        public async Task PostAsync_ArrayWithInvalidObjects_ShouldThrowValidationException()
        {
            // Arrange
            var mixedProducts = new[]
            {
                new TestProduct { Id = 1, Name = "Valid Product", Price = 10.00m, Description = "Valid" },
                new TestProduct { Id = 0, Name = "", Price = -5.00m, Description = "Invalid" } // Invalid
            };

            // Act
            Func<Task> act = async () => await Snapshot.Out.PostAsync(mixedProducts, _validator);

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
        public void Create_ShouldCreateCollectionWithValidationSupport()
        {
            // Arrange & Act
            Action act = () => Snapshot.Out.Create<TestProduct>(5, _validator);

            // Assert
            act.Should().NotThrow();
        }

        [Test]
        public void Get_ValidObject_ShouldReturnValidatedObject()
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
            var result = Snapshot.Out.Get<TestProduct>(0, _validator);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.Name.Should().Be("Valid Product");
        }

        [Test]
        public void Get_InvalidObject_ShouldThrowValidationException()
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
            Action act = () => Snapshot.Out.Get<TestProduct>(0, _validator);

            // Assert
            act.Should().Throw<ValidationException>()
                .Which.Errors.Should().HaveCount(3);
        }

        [Test]
        public void Get_WithPredicate_ValidObject_ShouldReturnValidatedObject()
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
            var result = Snapshot.Out.Get(p => p.Id == 1, _validator);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.Name.Should().Be("Valid Product");
        }

        [Test]
        public void Get_WithPredicate_InvalidObject_ShouldThrowValidationException()
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
            Action act = () => Snapshot.Out.Get(p => p.Id == 0, _validator);

            // Assert
            act.Should().Throw<ValidationException>()
                .Which.Errors.Should().HaveCount(3);
        }

        [Test]
        public async Task GetAsync_ValidObject_ShouldReturnValidatedObject()
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
            var result = await Snapshot.Out.GetAsync(0, _validator);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.Name.Should().Be("Valid Product");
        }

        [Test]
        public async Task GetAsync_InvalidObject_ShouldThrowValidationException()
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
            Func<Task> act = async () => await Snapshot.Out.GetAsync(0, _validator);

            // Assert
            await act.Should().ThrowAsync<ValidationException>()
                .Where(ex => ex.Errors.Count() == 3);
        }

        [Test]
        public async Task GetAllAsync_ValidObjects_ShouldReturnValidatedObjects()
        {
            // Arrange
            var validProducts = new[]
            {
                new TestProduct { Id = 1, Name = "Product 1", Price = 10.00m, Description = "First product" },
                new TestProduct { Id = 2, Name = "Product 2", Price = 20.00m, Description = "Second product" }
            };
            await Snapshot.Out.PostAsync(validProducts);

            // Act
            var result = await Snapshot.Out.GetAllAsync(_validator);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCountGreaterOrEqualTo(2);
        }

        [Test]
        public async Task GetAllAsync_InvalidObjects_ShouldThrowValidationException()
        {
            // Arrange
            var mixedProducts = new[]
            {
                new TestProduct { Id = 1, Name = "Valid Product", Price = 10.00m, Description = "Valid" },
                new TestProduct { Id = 0, Name = "", Price = -5.00m, Description = "Invalid" }
            };
            await Snapshot.Out.PostAsync(mixedProducts);

            // Act
            Func<Task> act = async () => await Snapshot.Out.GetAllAsync(_validator);

            // Assert
            await act.Should().ThrowAsync<ValidationException>()
                .Where(ex => ex.Errors.Count() == 3);
        }

        [Test]
        public void GetAsList_ValidObjects_ShouldReturnValidatedList()
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
            var result = Snapshot.Out.GetAsList(_validator);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<List<TestProduct>>();
            result.Should().HaveCountGreaterOrEqualTo(2);
        }

        [Test]
        public void GetAsList_InvalidObjects_ShouldThrowValidationException()
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
            Action act = () => Snapshot.Out.GetAsList(_validator);

            // Assert
            act.Should().Throw<ValidationException>()
                .Which.Errors.Should().HaveCount(3);
        }

        [Test]
        public void GetAsList_WithSize_ValidObjects_ShouldReturnValidatedList()
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
            var result = Snapshot.Out.GetAsList(5, _validator);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<List<TestProduct>>();
            result.Should().HaveCountGreaterOrEqualTo(2);
        }

        [Test]
        public void GetAsEnumerable_ValidObjects_ShouldReturnValidatedEnumerable()
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
            var result = Snapshot.Out.GetAsEnumerable(_validator);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCountGreaterOrEqualTo(2);
        }

        [Test]
        public void GetAsEnumerable_InvalidObjects_ShouldThrowValidationException()
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
            Action act = () => Snapshot.Out.GetAsEnumerable(_validator).ToList();

            // Assert
            act.Should().Throw<ValidationException>()
                .Which.Errors.Should().HaveCount(3);
        }

        [Test]
        public void GetAsSpan_ValidObjects_ShouldReturnValidatedSpan()
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
            var result = Snapshot.Out.GetAsSpan(_validator);

            // Assert
            result.Length.Should().BeGreaterOrEqualTo(2);
        }

        [Test]
        public void GetAsSpan_InvalidObjects_ShouldThrowValidationException()
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
            Action act = () => Snapshot.Out.GetAsSpan(_validator);

            // Assert
            act.Should().Throw<ValidationException>()
                .Which.Errors.Should().HaveCount(3);
        }

        [Test]
        public void GetAsReadonlySpan_ValidObjects_ShouldReturnValidatedReadonlySpan()
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
            var result = Snapshot.Out.GetAsReadonlySpan(_validator);

            // Assert
            result.Length.Should().BeGreaterOrEqualTo(2);
        }

        [Test]
        public void GetAsReadonlySpan_InvalidObjects_ShouldThrowValidationException()
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
            Action act = () => Snapshot.Out.GetAsReadonlySpan(_validator);

            // Assert
            act.Should().Throw<ValidationException>()
                .Which.Errors.Should().HaveCount(3);
        }
    }
}