using Checklist_API.Features.Common.Interfaces;
using Checklist_API.Features.Users.DTOs;
using Checklist_API.Features.Users.Entity;
using Checklist_API.Features.Users.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checklist.UnitTests.Mappers;
public class UserRegistrationMapperTests
{
    private readonly IMapper<User, UserRegistrationDTO> _userRegistrationMapper = new UserRegistrationMapper();
}
