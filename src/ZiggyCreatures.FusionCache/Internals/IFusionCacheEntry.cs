﻿using ZiggyCreatures.Caching.Fusion.Internals.Distributed;
using ZiggyCreatures.Caching.Fusion.Internals.Memory;

namespace ZiggyCreatures.Caching.Fusion.Internals;

/// <summary>
/// Represents a generic entry in <see cref="FusionCache"/>: it can be either a <see cref="FusionCacheMemoryEntry{TValue}"/> or a <see cref="FusionCacheDistributedEntry{TValue}"/>.
/// </summary>
internal interface IFusionCacheEntry
{
	// NOTE: GetValue<TValue>() AND SetValue<TValue(value) ARE NEEDED TO SUPPORT
	// FLEXIBLE TYPE CONVERSIONS, MEANING THAT WE CAN SET, FOR EXAMPLE, A Person
	// VALUE IN THE CACHE AS object AND THEN GET IT BACK AS A Person AGAIN.
	// THIS WOULDN'T WORK WITHOUT THESE METHODS, BECAUSE CASTING AN INSTANCE OF
	// IFusionCacheEntry<object> TO IFusionCacheEntry<Person> WOULD NOT WORK.

	/// <summary>
	/// Get the value inside the entry.
	/// </summary>
	/// <typeparam name="TValue">The typeof the value.</typeparam>
	/// <returns>The value.</returns>
	TValue GetValue<TValue>();

	/// <summary>
	/// Set the value inside the entry.
	/// </summary>
	/// <typeparam name="TValue">The typeof the value.</typeparam>
	/// <param name="value">The value.</param>
	void SetValue<TValue>(TValue value);

	/// <summary>
	/// The timestamp (in ticks) at which the cached value has been originally created: memory cache entries created from distributed cache entries will have the exact same timestamp.
	/// </summary>
	long Timestamp { get; }

	/// <summary>
	/// The intended expiration of the entry as requested from the caller, also used to have "aligned" expirations between different nodes.
	/// <br/>
	/// When fail-safe is enabled the entry is cached with a higher duration (<see cref="FusionCacheEntryOptions.FailSafeMaxDuration"/>) so it may be used as a fallback value in case of problems: when that happens, the LogicalExpiration is used to check if the value is stale, instead of losing it by simply let it expire in the cache.
	/// </summary>
	long LogicalExpirationTimestamp { get; set; }

	/// <summary>
	/// The optional set of tags related to the entry: this may be used to remove/expire multiple entries at once, by tag.
	/// </summary>
	string[]? Tags { get; set; }

	/// <summary>
	/// Metadata about the cache entry.
	/// </summary>
	FusionCacheEntryMetadata? Metadata { get; }
}
