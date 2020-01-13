using System;
using LiveDo.Abstractions.Domain;

namespace LiveDo.Profile.Domain.Interests
{
	public class Profile : Entity<Guid>, IAggregateRoot
	{
		protected Profile()
			: this(Guid.Empty)
		{
			
		}
		
		/// <inheritdoc />
		public Profile(Guid id)
			: base(id)
		{
		}
	}
}
