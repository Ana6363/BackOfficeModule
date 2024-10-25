using System;
using System.Collections.Generic;
using BackOffice.Domain.Shared;
using BackOffice.Domain.Staff;
using Moq;
using Xunit;

namespace BackOffice.DomainTests.Staff.Tests
{
    public class StaffTest
    {
        private Mock<IStaffRepository> mockRepository;

        public StaffTest()
        {
            mockRepository = new Mock<IStaffRepository>();
        }

        [Fact]
        public void Constructor_ShouldInitializeProperties_WhenValidArguments()
        {
            // Arrange
            var id = new StaffId(Guid.NewGuid());
            var licenseNumber = new LicenseNumber("12345");
            var specialization = new Specializations("Cardiology");
            var email = new StaffEmail("test@example.com");
            var slots = new List<Slots> { new Slots(DateTime.Now, DateTime.Now.AddHours(1)) };
            var status = new StaffStatus(true);

            // Act
            var staff = new Staff(id, licenseNumber, specialization, email, slots, status);

            // Assert
            Assert.Equal(id, staff.Id);
            Assert.Equal(licenseNumber, staff.LicenseNumber);
            Assert.Equal(specialization, staff.Specialization);
            Assert.Equal(email, staff.Email);
            Assert.Equal(slots, staff.AvailableSlots);
            Assert.Equal(status, staff.Status);
        }

        [Fact]
        public void Constructor_ShouldThrowException_WhenIdIsNull()
        {
            // Arrange
            StaffId id = null;
            var licenseNumber = new LicenseNumber("12345");
            var specialization = new Specializations("Cardiology");
            var email = new StaffEmail("test@example.com");
            var slots = new List<Slots> { new Slots(DateTime.Now, DateTime.Now.AddHours(1)) };
            var status = new StaffStatus(true);

            // Act & Assert
            Assert.Throws<BusinessRuleValidationException>(() => new Staff(id, licenseNumber, specialization, email, slots, status));
        }

        [Fact]
        public void AddSlot_ShouldAddSlot_WhenSlotIsValid()
        {
            // Arrange
            var staff = CreateValidStaff();
            var newSlot = new Slots(DateTime.Now.AddHours(2), DateTime.Now.AddHours(3));

            // Act
            staff.AddSlot(newSlot);

            // Assert
            Assert.Contains(newSlot, staff.AvailableSlots);
        }

        [Fact]
        public void AddSlot_ShouldThrowException_WhenSlotIsNull()
        {
            // Arrange
            var staff = CreateValidStaff();
            Slots newSlot = null;

            // Act & Assert
            Assert.Throws<BusinessRuleValidationException>(() => staff.AddSlot(newSlot));
        }

        [Fact]
        public void AddSlot_ShouldThrowException_WhenSlotConflicts()
        {
            // Arrange
            var staff = CreateValidStaff();
            var conflictingSlot = new Slots(DateTime.Now, DateTime.Now.AddHours(1));

            // Act & Assert
            Assert.Throws<BusinessRuleValidationException>(() => staff.AddSlot(conflictingSlot));
        }

        [Fact]
        public void RemoveSlot_ShouldRemoveSlot_WhenSlotExists()
        {
            // Arrange
            var staff = CreateValidStaff();
            var slotToRemove = staff.AvailableSlots[0];

            // Act
            staff.RemoveSlot(slotToRemove);

            // Assert
            Assert.DoesNotContain(slotToRemove, staff.AvailableSlots);
        }

        [Fact]
        public void Deactivate_ShouldSetStatusToInactive_WhenStatusIsActive()
        {
            // Arrange
            var staff = CreateValidStaff();

            // Act
            staff.Deactivate();

            // Assert
            Assert.False(staff.Status.IsActive);
        }

        [Fact]
        public void Constructor_ShouldThrowException_WhenLicenseNumberIsNull()
        {
            // Arrange
            var id = new StaffId(Guid.NewGuid());
            LicenseNumber licenseNumber = null;
            var specialization = new Specializations("Cardiology");
            var email = new StaffEmail("test@example.com");
            var slots = new List<Slots> { new Slots(DateTime.Now, DateTime.Now.AddHours(1)) };
            var status = new StaffStatus(true);

            // Act & Assert
            Assert.Throws<BusinessRuleValidationException>(() => new Staff(id, licenseNumber, specialization, email, slots, status));
        }

        [Fact]
        public void Constructor_ShouldThrowException_WhenSpecializationIsNull()
        {
            // Arrange
            var id = new StaffId(Guid.NewGuid());
            var licenseNumber = new LicenseNumber("12345");
            Specializations specialization = null;
            var email = new StaffEmail("test@example.com");
            var slots = new List<Slots> { new Slots(DateTime.Now, DateTime.Now.AddHours(1)) };
            var status = new StaffStatus(true);

            // Act & Assert
            Assert.Throws<BusinessRuleValidationException>(() => new Staff(id, licenseNumber, specialization, email, slots, status));
        }

        [Fact]
        public void Constructor_ShouldThrowException_WhenEmailIsNull()
        {
            // Arrange
            var id = new StaffId(Guid.NewGuid());
            var licenseNumber = new LicenseNumber("12345");
            var specialization = new Specializations("Cardiology");
            StaffEmail email = null;
            var slots = new List<Slots> { new Slots(DateTime.Now, DateTime.Now.AddHours(1)) };
            var status = new StaffStatus(true);

            // Act & Assert
            Assert.Throws<BusinessRuleValidationException>(() => new Staff(id, licenseNumber, specialization, email, slots, status));
        }

        [Fact]
        public void Constructor_ShouldThrowException_WhenSlotsIsNull()
        {
            // Arrange
            var id = new StaffId(Guid.NewGuid());
            var licenseNumber = new LicenseNumber("12345");
            var specialization = new Specializations("Cardiology");
            var email = new StaffEmail("test@example.com");
            List<Slots> slots = null;
            var status = new StaffStatus(true);

            // Act & Assert
            Assert.Throws<BusinessRuleValidationException>(() => new Staff(id, licenseNumber, specialization, email, slots, status));
        }

        [Fact]
        public void Constructor_ShouldThrowException_WhenSlotsIsEmpty()
        {
            // Arrange
            var id = new StaffId(Guid.NewGuid());
            var licenseNumber = new LicenseNumber("12345");
            var specialization = new Specializations("Cardiology");
            var email = new StaffEmail("test@example.com");
            var slots = new List<Slots>();
            var status = new StaffStatus(true);

            // Act & Assert
            Assert.Throws<BusinessRuleValidationException>(() => new Staff(id, licenseNumber, specialization, email, slots, status));
        }

        [Fact]
        public void Constructor_ShouldThrowException_WhenStatusIsNull()
        {
            // Arrange
            var id = new StaffId(Guid.NewGuid());
            var licenseNumber = new LicenseNumber("12345");
            var specialization = new Specializations("Cardiology");
            var email = new StaffEmail("test@example.com");
            var slots = new List<Slots> { new Slots(DateTime.Now, DateTime.Now.AddHours(1)) };
            StaffStatus status = null;

            // Act & Assert
            Assert.Throws<BusinessRuleValidationException>(() => new Staff(id, licenseNumber, specialization, email, slots, status));
        }

        private Staff CreateValidStaff()
        {
            var id = new StaffId(Guid.NewGuid());
            var licenseNumber = new LicenseNumber("12345");
            var specialization = new Specializations("Cardiology");
            var email = new StaffEmail("test@example.com");
            var slots = new List<Slots> { new Slots(DateTime.Now, DateTime.Now.AddHours(1)) };
            var status = new StaffStatus(true);

            return new Staff(id, licenseNumber, specialization, email, slots, status);
        }
    }
}
