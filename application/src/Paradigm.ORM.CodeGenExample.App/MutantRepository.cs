using System;
using System.Collections.Generic;

namespace Paradigm.ORM.CodeGenExample.App
{
    public class MutantRepository
    {
        /// <summary>
        /// Gets the mutants.
        /// </summary>
        /// <value>
        /// The mutants.
        /// </value>
        private List<Mutant> Mutants { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MutantRepository"/> class.
        /// </summary>
        public MutantRepository()
        {
            this.Mutants = new List<Mutant>
            {
                new Mutant("Jean", "Grey"),
                new Mutant("James", "Howlett"),
                new Mutant("Scott", "Summers"),
                new Mutant("Ororo", "Munroe")
            };
        }

        /// <summary>
        /// Gets the mutant by his/her identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>A mutant.</returns>
        /// <exception cref="ArgumentOutOfRangeException">id</exception>
        public Mutant GetById(int id)
        {
            if (id >= this.Mutants.Count)
                throw new ArgumentOutOfRangeException(nameof(id));

            return this.Mutants[id];
        }
    }
}