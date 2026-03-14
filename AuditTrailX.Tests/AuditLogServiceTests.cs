using AuditTrailX.Core.Common;
using AuditTrailX.Core.DTOs.Request;
using AuditTrailX.Core.Entities;
using AuditTrailX.Core.Enums;
using AuditTrailX.Core.Interfaces.Repositories;
using AuditTrailX.Service.Services;
using FluentAssertions;
using Moq;

namespace AuditTrailX.Tests;

public class AuditLogServiceTests
{
    // gerçek repository yerine mock kullanıyorum
    private readonly Mock<IAuditLogRepository> _repoMock;
    private readonly AuditLogService _service;

    public AuditLogServiceTests()
    {
        _repoMock = new Mock<IAuditLogRepository>();
        _service = new AuditLogService(_repoMock.Object);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnResponse_WhenValidRequest()
    {
        // geçerli request geldiğinde doğru response dönüyor mu test ediyorum
        var request = new CreateAuditLogRequest
        {
            ActorId = "user-1",
            ActorName = "Test User",
            ActorType = ActorType.User,
            ActionType = ActionType.Updated,
            EntityName = "UserProfile",
            EntityId = "entity-1"
        };

        var createdLog = new AuditLog
        {
            Id = Guid.NewGuid(),
            ActorId = request.ActorId,
            ActorName = request.ActorName,
            ActorType = request.ActorType,
            ActionType = request.ActionType,
            EntityName = request.EntityName,
            EntityId = request.EntityId,
            CreatedAt = DateTime.UtcNow
        };

        _repoMock
            .Setup(r => r.CreateAsync(It.IsAny<AuditLog>()))
            .ReturnsAsync(createdLog);

        var result = await _service.CreateAsync(request, "127.0.0.1", "TestAgent");

        result.Should().NotBeNull();
        result.ActorId.Should().Be("user-1");
        result.ActionType.Should().Be("Updated");
        _repoMock.Verify(r => r.CreateAsync(It.IsAny<AuditLog>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenLogNotFound()
    {
        // kayıt bulunamazsa null dönüyor mu test ediyorum
        _repoMock
            .Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((AuditLog?)null);

        var result = await _service.GetByIdAsync(Guid.NewGuid());

        result.Should().BeNull();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnPagedResult_WhenLogsExist()
    {
        // sayfalı liste doğru dönüyor mu test ediyorum
        var logs = new List<AuditLog>
        {
            new()
            {
                Id = Guid.NewGuid(),
                ActorId = "user-1",
                ActorName = "Alice",
                ActorType = ActorType.User,
                ActionType = ActionType.Login,
                EntityName = "Session",
                EntityId = "sess-1",
                CreatedAt = DateTime.UtcNow
            }
        };

        _repoMock
            .Setup(r => r.GetAllAsync(It.IsAny<AuditLogQueryParameters>()))
            .ReturnsAsync(new PagedResult<AuditLog>
            {
                Items = logs,
                TotalCount = 1,
                PageNumber = 1,
                PageSize = 20
            });

        var result = await _service.GetAllAsync(new AuditLogQueryParameters());

        result.Should().NotBeNull();
        result.Items.Should().HaveCount(1);
        result.TotalCount.Should().Be(1);
    }
}