using System;
using System.Collections.Generic;
using System.Security.Claims;
using LiveDo.Abstractions.Domain;
using Microsoft.AspNetCore.Identity;

namespace LiveDo.Auth.Domain.Users
{
	/// <summary>
	/// User.
	/// </summary>
	public class User : IdentityUser, IAggregateRoot
	{
	}
}
