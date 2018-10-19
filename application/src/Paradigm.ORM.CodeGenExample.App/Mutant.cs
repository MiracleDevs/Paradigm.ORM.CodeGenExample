namespace Paradigm.ORM.CodeGenExample.App
{
    public class Mutant
    {
        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        public string LastName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Mutant"/> class.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        public Mutant(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }
    }
}