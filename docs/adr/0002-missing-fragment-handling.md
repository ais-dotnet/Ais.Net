# Missing fragment handling

## Status

Accepted.

## Context

Some AIS data sources include NMEA sentences that are not correctly formed. We sometimes see truncated messages, for example. This causes problems when a corrupt message was a fragment. At some point we need to let go of the fragment because its fellow fragments are never going to arrive.

## Decision

We default to abandoning fragmented messages if 8 other messages have arrived since the first fragment. This is configurable, but we chose 8 because there's a limit of 9 fragmented messages in progress at any one time if you use the AIVDM-level group identifiers. It is configurable because systems that provide tag block group identifiers often use larger group IDs (e.g., 4 digits are common), making it possible for more to be in flight at once. But in practice, most message fragments are adjacent, so a large window is usaully unnecessary.

## Consequences

Missing fragments can now be reported. We can also avoid the problems that can occur when fragment group identifiers are reused, as they inevitably will be in long message streams. By abandoning doomed messages early we avoid the problem where a later reuse of the same id is misidentified as being part of the same message we had earlier.