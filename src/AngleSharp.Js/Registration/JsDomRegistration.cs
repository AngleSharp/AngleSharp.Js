namespace AngleSharp.Js
{
    using System;

    /// <summary>
    /// Describes the DOM surface exposed to script, in place of discovering it by reflection.
    /// </summary>
    /// <remarks>
    /// Given one, the engine exposes exactly what it registers: no assembly is scanned and no
    /// attribute is read, which is what makes the surface known to the trimmer and usable under
    /// native AoT. It is invoked once per scripting service, on first use, and the outcome is
    /// shared by every document the service runs scripts of. Several libraries can contribute by
    /// combining their registrations into one, see <see cref="JsDomRegistrations.Combine(JsDomRegistration[])"/>.
    /// </remarks>
    /// <param name="registry">The registry to declare the DOM types on.</param>
    public delegate void JsDomRegistration(IJsDomRegistry registry);

    /// <summary>
    /// Helpers for composing <see cref="JsDomRegistration"/> instances.
    /// </summary>
    public static class JsDomRegistrations
    {
        /// <summary>
        /// Combines several registrations, e.g. the ones of AngleSharp and AngleSharp.Css, into one.
        /// They run in the given order, so a later one adds to or replaces what an earlier one
        /// declared of the same type.
        /// </summary>
        /// <param name="registrations">The registrations to run.</param>
        /// <returns>The combined registration.</returns>
        public static JsDomRegistration Combine(params JsDomRegistration[] registrations)
        {
            registrations = registrations ?? throw new ArgumentNullException(nameof(registrations));

            return registry =>
            {
                for (var i = 0; i < registrations.Length; i++)
                {
                    registrations[i]?.Invoke(registry);
                }
            };
        }
    }
}
